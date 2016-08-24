using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HandyCareCuidador.Interface;
using HandyCareCuidador.Model;
using Microsoft.WindowsAzure.MobileServices;

namespace HandyCareCuidador.Data
{
    public class PeriodoTratamentoRestService:IPeriodoTratamentoRestService
    {
        static PeriodoTratamentoRestService defaultInstance = new PeriodoTratamentoRestService();
        MobileServiceClient client;
#if OFFLINE_SYNC_ENABLED
        IMobileServiceSyncTable<PeriodoTratamento> PeriodoTratamentoTable;
#else
        IMobileServiceTable<PeriodoTratamento> PeriodoTratamentoTable;
#endif
        public PeriodoTratamentoRestService()
        {
            this.client = new MobileServiceClient(Constants.ApplicationURL);
#if OFFLINE_SYNC_ENABLED
            var store = new MobileServiceSQLiteStore("PeriodoTratamentolocalstore.db");
            store.DefineTable<PeriodoTratamento>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            this.client.SyncContext.InitializeAsync(store);

            PeriodoTratamentoTable = client.GetSyncTable<PeriodoTratamento>();
#else
            PeriodoTratamentoTable = client.GetTable<PeriodoTratamento>();
#endif
        }

        //public static PeriodoTratamentoRestService DefaultManager
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
            get { return PeriodoTratamentoTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<PeriodoTratamento>; }
        }

        public async Task DeletePeriodoTratamentoAsync(PeriodoTratamento periodoTratamento)
        {
            await PeriodoTratamentoTable.DeleteAsync(periodoTratamento);
        }

        public async Task<ObservableCollection<PeriodoTratamento>> RefreshDataAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif
                IEnumerable<PeriodoTratamento> items = await PeriodoTratamentoTable
                    .ToEnumerableAsync();
                var a = items.Count();
                return new ObservableCollection<PeriodoTratamento>(items);
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

        public async Task SavePeriodoTratamentoAsync(PeriodoTratamento item, bool isNewItem)
        {
            try
            {
                if (item.Id == null)
                {
                    await PeriodoTratamentoTable.InsertAsync(item);
                }
                else
                {
                    await PeriodoTratamentoTable.UpdateAsync(item);
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

                await this.PeriodoTratamentoTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "allPeriodoTratamento",
                    this.PeriodoTratamentoTable.CreateQuery());
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