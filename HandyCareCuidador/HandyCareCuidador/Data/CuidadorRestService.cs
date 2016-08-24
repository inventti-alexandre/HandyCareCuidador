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
        IMobileServiceSyncTable<Paciente> PacienteTable;
        IMobileServiceSyncTable<CuidadorPaciente> CuidadorPacienteTable;
#else
        IMobileServiceTable<Cuidador> CuidadorTable;
        IMobileServiceTable<Paciente> PacienteTable;
        IMobileServiceTable<CuidadorPaciente> CuidadorPacienteTable;
        IMobileServiceTable<Afazer> AfazerTable;
        IMobileServiceTable<ConclusaoAfazer> ConclusaoAfazerTable;
        IMobileServiceTable<Material> MaterialTable;
        IMobileServiceTable<MaterialUtilizado> MaterialUtilizadoTable;
        IMobileServiceTable<MedicamentoAdministrado> MedicamentoAdministradoTable;
        IMobileServiceTable<Medicamento> MedicamentoTable;
        IMobileServiceTable<MotivoCuidado> MotivoCuidadoTable;
        IMobileServiceTable<PeriodoTratamento> PeriodoTratamentoTable;

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
            PacienteTable = client.GetSyncTable<Paciente>();
            CuidadorPacienteTable = client.GetSyncTable<CuidadorPaciente>();

#else
            CuidadorTable = client.GetTable<Cuidador>();
            PacienteTable = client.GetTable<Paciente>();
            CuidadorPacienteTable = client.GetTable<CuidadorPaciente>();
            AfazerTable = client.GetTable<Afazer>();
            ConclusaoAfazerTable = client.GetTable<ConclusaoAfazer>();
            MaterialTable = client.GetTable<Material>();
            MaterialUtilizadoTable = client.GetTable<MaterialUtilizado>();
            MedicamentoAdministradoTable = client.GetTable<MedicamentoAdministrado>();
            MedicamentoTable = client.GetTable<Medicamento>();
            MotivoCuidadoTable = client.GetTable<MotivoCuidado>();
            PeriodoTratamentoTable = client.GetTable<PeriodoTratamento>();

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

        public MobileServiceClient CurrentClient => client;

        public bool IsOfflineEnabled => CuidadorTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<Cuidador>;

        public async Task DeleteCuidadorAsync(Cuidador cuidador)
        {
            await CuidadorTable.DeleteAsync(cuidador);

        }
        public async Task DeletePacienteAsync(Paciente paciente)
        {
            await PacienteTable.DeleteAsync(paciente);
        }
        public async Task<Cuidador> ProcurarCuidadorAsync(string id, MobileServiceAuthenticationProvider provider)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif
                var item = new Cuidador();
                IEnumerable<Cuidador> items = await CuidadorTable
                    .ToEnumerableAsync();
                switch (provider)
                {
                    case MobileServiceAuthenticationProvider.Google:
                        item = items.FirstOrDefault(e => e.CuiGoogleId == id);
                        break;
                    case MobileServiceAuthenticationProvider.MicrosoftAccount:
                        item = items.FirstOrDefault(e => e.CuiMicrosoftId == id);
                        break;
                    case MobileServiceAuthenticationProvider.Facebook:
                        item = items.FirstOrDefault(e => e.CuiFacebookId == id);
                        break;
                    default:
                        return null;
                }
                return item;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation at {0}: {1}", CuidadorTable.TableName, msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;

        }
        public async Task<ObservableCollection<Cuidador>> RefreshCuidadorAsync(bool syncItems = false)
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
                Debug.WriteLine(@"Invalid sync operation at {0}: {1}", CuidadorTable.TableName, msioe.Message);
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

        /***********************////////////
        public async Task<ObservableCollection<Paciente>> RefreshPacienteAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif
                var x = client.CurrentUser.MobileServiceAuthenticationToken;
                IEnumerable<Paciente> items = await PacienteTable
                    .ToEnumerableAsync();
                return new ObservableCollection<Paciente>(items);
                
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation at {0}: {1}", PacienteTable.TableName, msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }

        public async Task<ObservableCollection<Paciente>> RefreshPacienteAsync(string ID, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif

                IEnumerable<Paciente> items = await PacienteTable.Where(item => item.Id == ID)
                    .ToEnumerableAsync();
                return new ObservableCollection<Paciente>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation at {0}: {1}", PacienteTable.TableName, msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;

        }
        public Task SavePacienteAsync(Paciente paciente, bool isNewItem)
        {
            throw new NotImplementedException();
        }

        /***************************************************************/
        public async Task<ObservableCollection<CuidadorPaciente>> RefreshCuidadorPacienteAsync(bool syncItems = false)
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
                Debug.WriteLine(@"Invalid sync operation at {0}: {1}", CuidadorPacienteTable.TableName, msioe.Message);
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


        /***************************************/
        public async Task DeleteAfazerAsync(Afazer Afazer)
        {
            await AfazerTable.DeleteAsync(Afazer);
        }

        public async Task<ObservableCollection<Afazer>> RefreshAfazerAsync(bool syncItems = false)
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
                if (item.Id == null)
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
        public async Task DeleteConclusaoAfazerAsync(ConclusaoAfazer conclusaoAfazer)
        {
            await ConclusaoAfazerTable.DeleteAsync(conclusaoAfazer);
        }

        public async Task<ObservableCollection<ConclusaoAfazer>> RefreshConclusaoAfazerAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif

                IEnumerable<ConclusaoAfazer> items = await ConclusaoAfazerTable
                    .ToEnumerableAsync();
                return new ObservableCollection<ConclusaoAfazer>(items);
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

        public async Task SaveConclusaoAfazerAsync(ConclusaoAfazer item, bool isNewItem)
        {
            try
            {
                if (isNewItem)
                {
                    await ConclusaoAfazerTable.InsertAsync(item);
                }
                else
                {
                    await ConclusaoAfazerTable.UpdateAsync(item);
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

        /***********************/
        public async Task DeleteMaterialAsync(Material material)
        {
            try
            {
                await MaterialTable.DeleteAsync(material);
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

        public async Task<ObservableCollection<Material>> RefreshMaterialAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif

                IEnumerable<Material> items = await MaterialTable
                    .ToEnumerableAsync();
                return new ObservableCollection<Material>(items);
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
        public async Task<ObservableCollection<Material>> RefreshMaterialExistenteAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif

                IEnumerable<Material> items = await MaterialTable
                    .Where(m => m.MatQuantidade > 0)
                    .ToEnumerableAsync();
                return new ObservableCollection<Material>(items);
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
        public async Task SaveMaterialAsync(Material item, bool isNewItem)
        {
            try
            {
                if (isNewItem)
                {
                    await MaterialTable.InsertAsync(item);
                }
                else
                {
                    await MaterialTable.UpdateAsync(item);
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


        public async Task DeleteMaterialUtilizadoAsync(MaterialUtilizado MaterialUtilizado)
        {
            await MaterialUtilizadoTable.DeleteAsync(MaterialUtilizado);
        }

        public async Task<ObservableCollection<MaterialUtilizado>> RefreshMaterialUtilizadoAsync(string Id, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif

                IEnumerable<MaterialUtilizado> items = await MaterialUtilizadoTable
                    .Where(m => m.MatAfazer == Id)
                    .ToEnumerableAsync();
                return new ObservableCollection<MaterialUtilizado>(items);
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

        public async Task SaveMaterialUtilizadoAsync(MaterialUtilizado item, bool isNewItem)
        {
            try
            {
                if (isNewItem)
                {
                    await MaterialUtilizadoTable.InsertAsync(item);
                }
                else
                {
                    await MaterialUtilizadoTable.UpdateAsync(item);
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

        public async Task DeleteMedicamentoAdministradoAsync(MedicamentoAdministrado medicamentoAdministrado)
        {
            await MedicamentoAdministradoTable.DeleteAsync(medicamentoAdministrado);
        }

        public async Task<ObservableCollection<MedicamentoAdministrado>> RefreshMedicamentoAdministradoAsync(string Id, bool syncItems = false)
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

        public async Task DeleteMedicamentoAsync(Medicamento medicamento)
        {
            try
            {
                await MedicamentoTable.DeleteAsync(medicamento);
            }
            catch (ArgumentException e)
            {
                Debug.WriteLine("{0}", e.Message);
                throw e;
            }
            catch (MobileServiceInvalidOperationException a)
            {
                Debug.WriteLine("{0}", a.Message);
                throw a;
            }
        }

        public async Task<ObservableCollection<Medicamento>> RefreshMedicamentoAsync(bool syncItems = false)
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

        public async Task DeleteMotivoCuidadoAsync(MotivoCuidado motivoCuidado)
        {
            await MotivoCuidadoTable.DeleteAsync(motivoCuidado);
        }

        public async Task<ObservableCollection<MotivoCuidado>> RefreshMotivoCuidadoAsync(bool syncItems = false)
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

        public async Task DeletePeriodoTratamentoAsync(PeriodoTratamento periodoTratamento)
        {
            await PeriodoTratamentoTable.DeleteAsync(periodoTratamento);
        }

        public async Task<ObservableCollection<PeriodoTratamento>> RefreshPeriodoTratamentoAsync(bool syncItems = false)
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