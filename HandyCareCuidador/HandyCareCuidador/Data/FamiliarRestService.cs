using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyCareCuidador.Model;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.ObjectModel;
using System.Diagnostics;
using HandyCareCuidador.Interface;
using HandyCareCuidador;

namespace HandyCareCuidador.Data
{
    public class FamiliarRestService : IFamiliarRestService
    {
        static FamiliarRestService defaultInstance = new FamiliarRestService();
        MobileServiceClient client;
#if OFFLINE_SYNC_ENABLED
        IMobileServiceSyncTable<Familiar> FamiliarTable;
#else
        IMobileServiceTable<Familiar> FamiliarTable;
#endif
        public FamiliarRestService()
        {
            client = new MobileServiceClient(Constants.ApplicationURL);
            FamiliarTable = client.GetTable<Familiar>();
        }

        public static FamiliarRestService DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }

        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }
        public bool IsOfflineEnabled
        {
            get { return FamiliarTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<Familiar>; }
        }

        public async Task DeleteFamiliarAsync(Familiar Familiar)
        {
            await FamiliarTable.DeleteAsync(Familiar);
        }

        public async Task<ObservableCollection<Familiar>> RefreshDataAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif

                IEnumerable<Familiar> items = await FamiliarTable
                    .ToEnumerableAsync();
                return new ObservableCollection<Familiar>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }

        public async Task SaveFamiliarAsync(Familiar item, bool isNewItem)
        {
            if (item.Id == null)
            {
                await FamiliarTable.InsertAsync(item);
            }
            else
            {
                await FamiliarTable.UpdateAsync(item);
            }
        }
    }
#if OFFLINE_SYNC_ENABLED
        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await this.client.SyncContext.PushAsync();

                await this.FamiliarTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "allFamiliar",
                    this.FamiliarTable.CreateQuery());
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }
        }
#endif
}