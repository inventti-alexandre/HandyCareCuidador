//#define OFFLINE_SYNC_ENABLED
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyCareCuidador.Interface;
using HandyCareCuidador.Model;
using Microsoft.WindowsAzure.MobileServices;

#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
#endif

namespace HandyCareCuidador.Data
{
    public class MotivoCuidadoRestService:IMotivoCuidadoRestService
    {
        MobileServiceClient client;
#if OFFLINE_SYNC_ENABLED
        IMobileServiceSyncTable<MotivoCuidado> MotivoCuidadoTable;
#else
            IMobileServiceTable<MotivoCuidado> MotivoCuidadoTable;
#endif

            public MotivoCuidadoRestService()
            {
                this.client = new MobileServiceClient(Constants.ApplicationURL);
#if OFFLINE_SYNC_ENABLED
            var store = new MobileServiceSQLiteStore("MotivoCuidadolocalstore.db");
            store.DefineTable<MotivoCuidado>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            this.client.SyncContext.InitializeAsync(store);

            MotivoCuidadoTable = client.GetSyncTable<MotivoCuidado>();
#else
                MotivoCuidadoTable = client.GetTable<MotivoCuidado>();
#endif
            }

            //public static MotivoCuidadoRestService DefaultManager
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
            //public bool IsOfflineEnabled
            //{
            //    get
            //    {
            //        return MotivoCuidadoTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<MotivoCuidado>;
            //    }
            //}

            public async Task DeleteMotivoCuidadoAsync(MotivoCuidado motivoCuidado)
            {
                await MotivoCuidadoTable.DeleteAsync(motivoCuidado);
            }

            public async Task<ObservableCollection<MotivoCuidado>> RefreshDataAsync(bool syncItems = false)
            {
                try
                {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif
                    IEnumerable<MotivoCuidado> items = await MotivoCuidadoTable
                        .ToEnumerableAsync();
                    var a = items.Count();
                    return new ObservableCollection<MotivoCuidado>(items);
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

            public async Task SaveMotivoCuidadoAsync(MotivoCuidado item, bool isNewItem)
            {
                try
                {
                    if (item.Id == null)
                    {
                        await MotivoCuidadoTable.InsertAsync(item);
                    }
                    else
                    {
                        await MotivoCuidadoTable.UpdateAsync(item);
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

                await this.MotivoCuidadoTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "allMotivoCuidado",
                    this.MotivoCuidadoTable.CreateQuery());
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
