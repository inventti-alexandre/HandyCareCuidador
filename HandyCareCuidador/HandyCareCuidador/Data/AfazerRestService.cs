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
    public class AfazerRestService: IAfazerRestService
    {
        static AfazerRestService defaultInstance = new AfazerRestService();
        MobileServiceClient client;
#if OFFLINE_SYNC_ENABLED
        IMobileServiceSyncTable<Afazer> AfazerTable;
#else
        IMobileServiceTable<Afazer> AfazerTable;
#endif
        public AfazerRestService()
        {
            this.client = new MobileServiceClient(Constants.ApplicationURL);
#if OFFLINE_SYNC_ENABLED
            var store = new MobileServiceSQLiteStore("afazerlocalstore.db");
            store.DefineTable<Afazer>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            this.client.SyncContext.InitializeAsync(store);

            AfazerTable = client.GetSyncTable<Afazer>();
#else
            AfazerTable = client.GetTable<Afazer>();
#endif
        }

        //public static AfazerRestService DefaultManager
        //{
        //    get
        //    {
        //        return defaultInstance;
        //    }
        //    private set
        //    {
        //        defaultInstance = value;
        //    }
        //}

        //public MobileServiceClient CurrentClient
        //{
        //    get { return client; }
        //}
        public bool IsOfflineEnabled
        {
            get { return AfazerTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<Afazer>; }
        }

        public async Task DeleteAfazerAsync(Afazer Afazer)
        {
            await AfazerTable.DeleteAsync(Afazer);
        }

        public async Task<ObservableCollection<Afazer>> RefreshDataAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif
                IEnumerable<Afazer> items = await AfazerTable
                    .ToEnumerableAsync();
                var a = items.Count();
                return new ObservableCollection<Afazer>(items);
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

        public async Task SaveAfazerAsync(Afazer item, bool isNewItem)
        {
            try
            {
                if (item.Id==null)
                {
                    await AfazerTable.InsertAsync(item);
                }
                else
                {
                    await AfazerTable.UpdateAsync(item);
                }
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

        }

#if OFFLINE_SYNC_ENABLED
        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await this.client.SyncContext.PushAsync();

                await this.AfazerTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "allAfazer",
                    this.AfazerTable.CreateQuery());
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
