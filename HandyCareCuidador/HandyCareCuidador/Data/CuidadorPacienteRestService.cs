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
    public class CuidadorPacienteRestService:ICuidadorPacienteRestService
    {
                static CuidadorPacienteRestService defaultInstance = new CuidadorPacienteRestService();
        MobileServiceClient client;
#if OFFLINE_SYNC_ENABLED
        IMobileServiceSyncTable<CuidadorPaciente> CuidadorPacienteTable;
#else
        IMobileServiceTable<CuidadorPaciente> CuidadorPacienteTable;
#endif
        public CuidadorPacienteRestService()
        {
            this.client = new MobileServiceClient(Constants.ApplicationURL);
#if OFFLINE_SYNC_ENABLED
            var store = new MobileServiceSQLiteStore("CuidadorPacientelocalstore.db");
            store.DefineTable<CuidadorPaciente>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            this.client.SyncContext.InitializeAsync(store);

            CuidadorPacienteTable = client.GetSyncTable<CuidadorPaciente>();
#else
            CuidadorPacienteTable = client.GetTable<CuidadorPaciente>();
#endif
        }

        public async Task<ObservableCollection<CuidadorPaciente>> RefreshDataAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif

                IEnumerable<CuidadorPaciente> items = await CuidadorPacienteTable
                    .ToEnumerableAsync();
                return new ObservableCollection<CuidadorPaciente>(items);
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

        public async Task SaveCuidadorPacienteAsync(CuidadorPaciente cuidadorPaciente, bool isNewItem)
        {
            try
            {
                if (isNewItem)
                {
                    await CuidadorPacienteTable.InsertAsync(cuidadorPaciente);
                }
                else
                {
                    await CuidadorPacienteTable.UpdateAsync(cuidadorPaciente);
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
    public async Task DeleteCuidadorPacienteAsync(CuidadorPaciente cuidadorPaciente)
        {
            await CuidadorPacienteTable.DeleteAsync(cuidadorPaciente);
        }
#if OFFLINE_SYNC_ENABLED
        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await this.client.SyncContext.PushAsync();

                await this.ConclusaoAfazerTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "allConclusaoAfazer",
                    this.ConclusaoAfazerTable.CreateQuery());
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
