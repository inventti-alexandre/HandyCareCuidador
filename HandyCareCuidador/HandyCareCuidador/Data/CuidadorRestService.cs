//#define OFFLINE_SYNC_ENABLED
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
using HandyCareCuidador.Interface;
using System.Collections.ObjectModel;
using System.Diagnostics;
#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
#endif

namespace HandyCareCuidador.Data
{
    public class CuidadorRestService : ICuidadorRestService
    {
        static CuidadorRestService defaultInstance = new CuidadorRestService();
        MobileServiceClient client;
#if OFFLINE_SYNC_ENABLED
        IMobileServiceSyncTable<Cuidador> CuidadorTable;
#else
        IMobileServiceTable<Cuidador> CuidadorTable;
#endif
        public CuidadorRestService()
        {
            client = new MobileServiceClient(Constants.ApplicationURL);
#if OFFLINE_SYNC_ENABLED
            var store = new MobileServiceSQLiteStore("localstore.db");
            store.DefineTable<Cuidador>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            this.client.SyncContext.InitializeAsync(store);

            CuidadorTable = client.GetSyncTable<Cuidador>();
#else
            CuidadorTable = client.GetTable<Cuidador>();
#endif
        }

        public static CuidadorRestService DefaultManager
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
            get { return CuidadorTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<Cuidador>; }
        }

        public async Task DeleteCuidadorAsync(Cuidador cuidador)
        {
            await CuidadorTable.DeleteAsync(cuidador);
        }

        public async Task<ObservableCollection<Cuidador>> RefreshDataAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif

                IEnumerable<Cuidador> items = await CuidadorTable
                    .ToEnumerableAsync();
                return new ObservableCollection<Cuidador>(items);
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

        public async Task SaveCuidadorAsync(Cuidador item, bool isNewItem)
        {
            if (item.Id == null)
            {
                await CuidadorTable.InsertAsync(item);
            }
            else
            {
                await CuidadorTable.UpdateAsync(item);
            }
        }
#if OFFLINE_SYNC_ENABLED
        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await this.client.SyncContext.PushAsync();

                await this.CuidadorTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "allCuidador",
                    this.CuidadorTable.CreateQuery());
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
}