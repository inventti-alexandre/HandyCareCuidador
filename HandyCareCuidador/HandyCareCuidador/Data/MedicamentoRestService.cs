//#define OFFLINE_SYNC_ENABLED
using HandyCareCuidador.Interface;
using HandyCareCuidador.Model;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
#endif
namespace HandyCareCuidador.Data
{
    public class MedicamentoRestService: IMedicamentoRestService
    {
        static MedicamentoRestService defaultInstance = new MedicamentoRestService();
        MobileServiceClient client;
#if OFFLINE_SYNC_ENABLED
        IMobileServiceSyncTable<Medicamento> MedicamentoTable;
#else
        IMobileServiceTable<Medicamento> MedicamentoTable;
#endif
        public MedicamentoRestService()
        {
            client = new MobileServiceClient(Constants.ApplicationURL);
#if OFFLINE_SYNC_ENABLED
            var store = new MobileServiceSQLiteStore("localstore.db");
            store.DefineTable<Medicamento>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            this.client.SyncContext.InitializeAsync(store);

            MedicamentoTable = client.GetSyncTable<Medicamento>();
#else
            MedicamentoTable = client.GetTable<Medicamento>();
#endif
        }
        public async Task DeleteMedicamentoAsync(Medicamento medicamento)
        {
            try
            {
                await MedicamentoTable.DeleteAsync(medicamento);
            }
            catch (ArgumentException e)
            {
                Debug.WriteLine("erro filho da puta: {0}", e.Message);
                throw e;
            }
            catch(MobileServiceInvalidOperationException a)
            {
                Debug.WriteLine("Vá à merda, Microsoft: {0}", a.Message);
                throw a;
            }
        }

        public async Task<ObservableCollection<Medicamento>> RefreshDataAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif
          
                IEnumerable<Medicamento> items = await MedicamentoTable
                    .ToEnumerableAsync();
                return new ObservableCollection<Medicamento>(items);
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

        public async Task SaveMedicamentoAsync(Medicamento item, bool isNewItem)
        {
            if (item.Id == null)
            {
                await MedicamentoTable.InsertAsync(item);
            }
            else
            {
                await MedicamentoTable.UpdateAsync(item);
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
