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

namespace HandyCareCuidador.Data
{
    public class MedicamentoAdministradoRestService : IMedicamentoAdministradoRestService
    {
        MobileServiceClient client;
#if OFFLINE_SYNC_ENABLED
        IMobileServiceSyncTable<MedicamentoAdministrado> MedicamentoAdministradoTable;
#else
        IMobileServiceTable<MedicamentoAdministrado> MedicamentoAdministradoTable;
#endif
        public MedicamentoAdministradoRestService()
        {
            client = new MobileServiceClient(Constants.ApplicationURL);
            MedicamentoAdministradoTable = client.GetTable<MedicamentoAdministrado>();
        }

        public async Task DeleteMedicamentoAdministradoAsync(MedicamentoAdministrado medicamentoAdministrado)
        {
            await MedicamentoAdministradoTable.DeleteAsync(medicamentoAdministrado);
        }

        public async Task<ObservableCollection<MedicamentoAdministrado>> RefreshDataAsync(string Id, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif

                IEnumerable<MedicamentoAdministrado> items = await MedicamentoAdministradoTable
                    .Where(m => m.MedAfazer == Id)
                    .ToEnumerableAsync();
                return new ObservableCollection<MedicamentoAdministrado>(items);
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

        public async Task SaveMedicamentoAdministradoAsync(MedicamentoAdministrado item, bool isNewItem)
        {
            try
            {
                if (isNewItem)
                {
                    await MedicamentoAdministradoTable.InsertAsync(item);
                }
                else
                {
                    await MedicamentoAdministradoTable.UpdateAsync(item);
                }

            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
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

                await this.MedicamentoAdministradoTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "allMedicamentoAdministrado",
                    this.MedicamentoAdministradoTable.CreateQuery());
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
