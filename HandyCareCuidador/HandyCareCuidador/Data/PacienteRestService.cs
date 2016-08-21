//#define OFFLINE_SYNC_ENABLED
using HandyCareCuidador.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyCareCuidador.Model;
using System.Collections.ObjectModel;
using Microsoft.WindowsAzure.MobileServices;
using System.Diagnostics;
#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
#endif

namespace HandyCareCuidador.Data
{
    public class PacienteRestService : IPacienteRestService
    {
        static PacienteRestService defaultInstance = new PacienteRestService();
        MobileServiceClient client;
#if OFFLINE_SYNC_ENABLED
        IMobileServiceSyncTable<Paciente> PacienteTable;
#else
        IMobileServiceTable<Paciente> PacienteTable;
#endif
        public PacienteRestService()
        {
            client = new MobileServiceClient(Constants.ApplicationURL);
#if OFFLINE_SYNC_ENABLED
            var store = new MobileServiceSQLiteStore("localstore.db");
            store.DefineTable<Paciente>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            this.client.SyncContext.InitializeAsync(store);

            PacienteTable = client.GetSyncTable<Paciente>();
#else
            PacienteTable = client.GetTable<Paciente>();
#endif
        }
        public async Task DeletePacienteAsync(Paciente paciente)
        {
            await PacienteTable.DeleteAsync(paciente);
        }

        public async Task<ObservableCollection<Paciente>> RefreshDataAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif

                IEnumerable<Paciente> items = await PacienteTable
                    .ToEnumerableAsync();
                return new ObservableCollection<Paciente>(items);
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

        public async Task<ObservableCollection<Paciente>> RefreshDataAsync(string ID, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif

                IEnumerable<Paciente> items = await PacienteTable.Where(item=>item.Id==ID)
                    .ToEnumerableAsync();
                return new ObservableCollection<Paciente>(items);
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

        public Task SavePacienteAsync(Paciente paciente, bool isNewItem)
        {
            throw new NotImplementedException();
        }
    }
}
