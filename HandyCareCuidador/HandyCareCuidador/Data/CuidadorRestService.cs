// To add offline sync support: add the NuGet package Microsoft.Azure.Mobile.Client.SQLiteStore
// to all projects in the solution and uncomment the symbol definition OFFLINE_SYNC_ENABLED
// For Xamarin.iOS, also edit AppDelegate.cs and uncomment the call to SQLitePCL.CurrentPlatform.Init()
// For more information, see: http://go.microsoft.com/fwlink/?LinkId=620342 

//#define OFFLINE_SYNC_ENABLED
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.System.Threading;
using HandyCareCuidador.Interface;
using HandyCareCuidador.Model;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;

#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;

#endif

namespace HandyCareCuidador.Data
{
    public class CuidadorRestService : ICuidadorRestService
    {
        public static CuidadorRestService DefaultManager { get; private set; } = new CuidadorRestService();
        public MobileServiceClient CurrentClient { get; }

#if OFFLINE_SYNC_ENABLED //165 em diante
        private readonly IMobileServiceSyncTable<Cuidador> CuidadorTable;
        private readonly IMobileServiceSyncTable<Paciente> PacienteTable;
        private readonly IMobileServiceSyncTable<CuidadorPaciente> CuidadorPacienteTable;
        private readonly IMobileServiceSyncTable<Afazer> AfazerTable;
        private readonly IMobileServiceSyncTable<ConclusaoAfazer> ConclusaoAfazerTable;
        private readonly IMobileServiceSyncTable<Material> MaterialTable;
        private readonly IMobileServiceSyncTable<MaterialUtilizado> MaterialUtilizadoTable;
        private readonly IMobileServiceSyncTable<MedicamentoAdministrado> MedicamentoAdministradoTable;
        private readonly IMobileServiceSyncTable<Medicamento> MedicamentoTable;
        private readonly IMobileServiceSyncTable<MotivoCuidado> MotivoCuidadoTable;
        private readonly IMobileServiceSyncTable<PeriodoTratamento> PeriodoTratamentoTable;
        private readonly IMobileServiceSyncTable<TipoCuidador> TipoCuidadorTable;
        private readonly IMobileServiceSyncTable<ValidacaoCuidador> ValidacaoCuidadorTable;

#else
        private readonly IMobileServiceTable<Cuidador> CuidadorTable;
        private readonly IMobileServiceTable<Paciente> PacienteTable;
        private readonly IMobileServiceTable<Familiar> FamiliarTable;
        private readonly IMobileServiceTable<CuidadorPaciente> CuidadorPacienteTable;
        private readonly IMobileServiceTable<Afazer> AfazerTable;
        private readonly IMobileServiceTable<ConclusaoAfazer> ConclusaoAfazerTable;
        private readonly IMobileServiceTable<Material> MaterialTable;
        private readonly IMobileServiceTable<MaterialUtilizado> MaterialUtilizadoTable;
        private readonly IMobileServiceTable<MedicamentoAdministrado> MedicamentoAdministradoTable;
        private readonly IMobileServiceTable<Medicamento> MedicamentoTable;
        private readonly IMobileServiceTable<MotivoCuidado> MotivoCuidadoTable;
        private readonly IMobileServiceTable<PeriodoTratamento> PeriodoTratamentoTable;
        private readonly IMobileServiceTable<TipoCuidador> TipoCuidadorTable;
        private readonly IMobileServiceTable<ValidacaoCuidador> ValidacaoCuidadorTable;
        private readonly IMobileServiceTable<Foto> FotoTable;
        private readonly IMobileServiceTable<FotoFamiliar> FotoFamiliarTable;
        private readonly IMobileServiceTable<Video> VideoTable;
        private readonly IMobileServiceTable<VideoFamiliar> VideoFamiliarTable;
        private readonly IMobileServiceTable<Parentesco> ParentescoTable;
        private readonly IMobileServiceTable<PacienteFamiliar> PacienteFamiliarTable;
        private readonly IMobileServiceTable<ViaAdministracaoMedicamento> ViaAdministracaoMedicamentoTable;
        private readonly IMobileServiceTable<FormaApresentacaoMedicamento> FormaApresentacaoMedicamentoTable;
        private readonly IMobileServiceTable<ContatoEmergencia> ContatoEmergenciaTable;
        private readonly IMobileServiceTable<ConTelefone> ConTelefoneTable;
        private readonly IMobileServiceTable<ConCelular> ConCelularTable;

#endif

        public CuidadorRestService()
        {
            CurrentClient = new MobileServiceClient(Constants.ApplicationURL);
//#if DEBUG
//            CurrentClient = new MobileServiceClient("http://DESKTOP-5TG6LTC/handycareappService/")
//            {
//                AlternateLoginHost = new Uri("https://handycareapp.azurewebsites.net/")
//            };
//            //#else
//            //   MobileService = new MobileServiceClient("https://{servicename}.azurewebsites.net/");  
//#endif
#if OFFLINE_SYNC_ENABLED
            var store = new MobileServiceSQLiteStore("localstore.db");
            store.DefineTable<Cuidador>();
            store.DefineTable<Paciente>();
            store.DefineTable<CuidadorPaciente>();
            store.DefineTable<Afazer>();
            store.DefineTable<ConclusaoAfazer>();
            store.DefineTable<Material>();
            store.DefineTable<MaterialUtilizado>();
            store.DefineTable<MedicamentoAdministrado>();
            store.DefineTable<Medicamento>();
            store.DefineTable<MotivoCuidado>();
            store.DefineTable<PeriodoTratamento>();
            store.DefineTable<TipoCuidador>();
            store.DefineTable<ValidacaoCuidador>();
            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            CurrentClient.SyncContext.InitializeAsync(store);

            CuidadorTable = CurrentClient.GetSyncTable<Cuidador>();
            PacienteTable = CurrentClient.GetSyncTable<Paciente>();
            CuidadorPacienteTable = CurrentClient.GetSyncTable<CuidadorPaciente>();
            AfazerTable = CurrentClient.GetSyncTable<Afazer>();
            ConclusaoAfazerTable = CurrentClient.GetSyncTable<ConclusaoAfazer>();
            MaterialTable = CurrentClient.GetSyncTable<Material>();
            MaterialUtilizadoTable = CurrentClient.GetSyncTable<MaterialUtilizado>();
            MedicamentoAdministradoTable = CurrentClient.GetSyncTable<MedicamentoAdministrado>();
            MedicamentoTable = CurrentClient.GetSyncTable<Medicamento>();
            MotivoCuidadoTable = CurrentClient.GetSyncTable<MotivoCuidado>();
            PeriodoTratamentoTable = CurrentClient.GetSyncTable<PeriodoTratamento>();
            TipoCuidadorTable= CurrentClient.GetSyncTable<TipoCuidador>();
            ValidacaoCuidadorTable = CurrentClient.GetSyncTable<ValidacaoCuidador>();

#else
            CuidadorTable = CurrentClient.GetTable<Cuidador>();
            PacienteTable = CurrentClient.GetTable<Paciente>();
            CuidadorPacienteTable = CurrentClient.GetTable<CuidadorPaciente>();
            AfazerTable = CurrentClient.GetTable<Afazer>();
            ConclusaoAfazerTable = CurrentClient.GetTable<ConclusaoAfazer>();
            MaterialTable = CurrentClient.GetTable<Material>();
            MaterialUtilizadoTable = CurrentClient.GetTable<MaterialUtilizado>();
            MedicamentoAdministradoTable = CurrentClient.GetTable<MedicamentoAdministrado>();
            MedicamentoTable = CurrentClient.GetTable<Medicamento>();
            MotivoCuidadoTable = CurrentClient.GetTable<MotivoCuidado>();
            PeriodoTratamentoTable = CurrentClient.GetTable<PeriodoTratamento>();
            TipoCuidadorTable= CurrentClient.GetTable<TipoCuidador>();
            ValidacaoCuidadorTable = CurrentClient.GetTable<ValidacaoCuidador>();
            FotoTable = CurrentClient.GetTable<Foto>();
            FamiliarTable = CurrentClient.GetTable<Familiar>();
            FotoFamiliarTable = CurrentClient.GetTable<FotoFamiliar>();
            ParentescoTable = CurrentClient.GetTable<Parentesco>();
            PacienteFamiliarTable = CurrentClient.GetTable<PacienteFamiliar>();
            ViaAdministracaoMedicamentoTable = CurrentClient.GetTable<ViaAdministracaoMedicamento>();
            FormaApresentacaoMedicamentoTable = CurrentClient.GetTable<FormaApresentacaoMedicamento>();
            VideoTable = CurrentClient.GetTable<Video>();
            VideoFamiliarTable = CurrentClient.GetTable<VideoFamiliar>();
            ContatoEmergenciaTable = CurrentClient.GetTable<ContatoEmergencia>();
            ConCelularTable = CurrentClient.GetTable<ConCelular>();
            ConTelefoneTable = CurrentClient.GetTable<ConTelefone>();


#endif
        }

        public async Task<ObservableCollection<Foto>> RefreshFotoAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif

                var items = await FotoTable
                    .ToEnumerableAsync();
                return new ObservableCollection<Foto>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation at {0}: {1}", CuidadorTable.TableName, msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
            }
            return null;
        }

        public async Task SaveFotoAsync(Foto item, bool isNewItem)
        {
            try
            {
                if (isNewItem)
                {
                    await FotoTable.InsertAsync(item);
                }
                else
                {
                    await FotoTable.UpdateAsync(item);
                }

            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation at {0}: {1}", FotoTable.TableName, msioe.Message);
                Debug.WriteLine(msioe.ToString());
            }
        }
        public async Task DeleteFotoAsync(Foto foto)
        {
            await FotoTable.DeleteAsync(foto);
        }


        public bool IsOfflineEnabled
            => CuidadorTable is IMobileServiceSyncTable<Cuidador>;

        public async Task DeleteCuidadorAsync(Cuidador cuidador)
        {
            await CuidadorTable.DeleteAsync(cuidador);
        }

        public async Task DeletePacienteAsync(Paciente paciente)
        {
            await PacienteTable.DeleteAsync(paciente);
        }
        public async Task<Foto> ProcurarFotoAsync(string id, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif
                var items = await FotoTable
                    .ToEnumerableAsync();
                var item = items.FirstOrDefault(e => e.Id == id);
                return item;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation at {0}: {1}", CuidadorTable.TableName, msioe.Message);
                Debug.WriteLine(msioe.ToString());
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine("Nulou {0}", e.Message);
            }

            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
            }
            return null;
        }

        public async Task<Cuidador> ProcurarCuidadorAsync(string id, MobileServiceAuthenticationProvider provider, bool syncItems = false)
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
                var x = CurrentClient.CurrentUser.MobileServiceAuthenticationToken;
                var items = await CuidadorTable
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
                Debug.WriteLine(msioe.ToString());
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine("Nulou {0}", e.Message);
            }

            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
            }
            return null;
        }

        public async Task<ObservableCollection<ContatoEmergencia>> RefreshContatoEmergenciaAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif
                var items = await ContatoEmergenciaTable
                    .ToEnumerableAsync();
                return new ObservableCollection<ContatoEmergencia>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation at {0}: {1}", CuidadorTable.TableName, msioe.Message);
                Debug.WriteLine(msioe.ToString());
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine("Nulou {0}", e.Message);
            }

            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
            }
            return null;
        }

        public async Task<ObservableCollection<ConTelefone>> RefreshConTelefoneAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif
                var items = await ConTelefoneTable
                    .ToEnumerableAsync();
                return new ObservableCollection<ConTelefone>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation at {0}: {1}", CuidadorTable.TableName, msioe.Message);
                Debug.WriteLine(msioe.ToString());
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine("Nulou {0}", e.Message);
            }

            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
            }
            return null;
        }

        public async Task<ObservableCollection<ConCelular>> RefreshConCelularAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif
                var items = await ConCelularTable
                    .ToEnumerableAsync();
                return new ObservableCollection<ConCelular>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation at {0}: {1}", CuidadorTable.TableName, msioe.Message);
                Debug.WriteLine(msioe.ToString());
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine("Nulou {0}", e.Message);
            }

            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
            }
            return null;
        }

        public async Task<ObservableCollection<Video>> RefreshVideoAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif

                var items = await VideoTable
                    .ToEnumerableAsync();
                return new ObservableCollection<Video>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation at {0}: {1}", CuidadorTable.TableName, msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
            }
            return null;
        }

        public async Task SaveVideoAsync(Video video, bool isNewItem)
        {
            try
            {
                if (isNewItem)
                {
                    await VideoTable.InsertAsync(video);
                }
                else
                {
                    await VideoTable.UpdateAsync(video);
                }

            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation at {0}: {1}", FotoFamiliarTable.TableName, msioe.Message);
            }
        }

        public async Task DeleteVideoAsync(Video video)
        {
            await VideoTable.DeleteAsync(video);
        }

        public async Task<ObservableCollection<VideoFamiliar>> RefreshVideoFamiliarAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif

                var items = await VideoFamiliarTable
                    .ToEnumerableAsync();
                return new ObservableCollection<VideoFamiliar>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation at {0}: {1}", CuidadorTable.TableName, msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
            }
            return null;
        }

        public async Task SaveVideoFamiliarAsync(VideoFamiliar videoFamiliar, bool isNewItem)
        {
            try
            {
                if (isNewItem)
                {
                    await VideoFamiliarTable.InsertAsync(videoFamiliar);
                }
                else
                {
                    await VideoFamiliarTable.UpdateAsync(videoFamiliar);
                }

            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation at {0}: {1}", FotoFamiliarTable.TableName, msioe.Message);
            }
        }

        public async Task DeleteVideoFamiliarAsync(VideoFamiliar fotoFamiliar)
        {
            await VideoFamiliarTable.DeleteAsync(fotoFamiliar);
        }

        public async Task<ObservableCollection<ViaAdministracaoMedicamento>> RefreshViaAdministracaoMedicamentoAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif

                var items = await ViaAdministracaoMedicamentoTable
                    .ToEnumerableAsync();
                return new ObservableCollection<ViaAdministracaoMedicamento>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation at {0}: {1}", CuidadorTable.TableName, msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
            }
            return null;
        }

        public async Task<ObservableCollection<FormaApresentacaoMedicamento>> RefreshFormaApresentacaoMedicamentoAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif

                var items = await FormaApresentacaoMedicamentoTable
                    .ToEnumerableAsync();
                return new ObservableCollection<FormaApresentacaoMedicamento>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation at {0}: {1}", CuidadorTable.TableName, msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
            }
            return null;
        }

        public async Task<ObservableCollection<FotoFamiliar>> RefreshFotoFamiliarAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif

                var items = await FotoFamiliarTable
                    .ToEnumerableAsync();
                return new ObservableCollection<FotoFamiliar>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation at {0}: {1}", CuidadorTable.TableName, msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
            }
            return null;
        }

        public async Task SaveFotoFamiliarAsync(FotoFamiliar fotoFamiliar, bool isNewItem)
        {
            try
            {
                if (isNewItem)
                {
                    await FotoFamiliarTable.InsertAsync(fotoFamiliar);
                }
                else
                {
                    await FotoFamiliarTable.UpdateAsync(fotoFamiliar);
                }

            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation at {0}: {1}", FotoFamiliarTable.TableName, msioe.Message);
            }
        }

        public async Task DeleteFotoFamiliarAsync(FotoFamiliar fotoFamiliar)
        {
            await FotoFamiliarTable.DeleteAsync(fotoFamiliar);
        }

        public async Task<ObservableCollection<Parentesco>> RefreshParentescoAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif

                var items = await ParentescoTable
                    .ToEnumerableAsync();
                return new ObservableCollection<Parentesco>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation at {0}: {1}", CuidadorTable.TableName, msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
            }
            return null;
        }

        public async Task SaveParentescoAsync(Parentesco parentesco, bool isNewItem)
        {
            if (parentesco.Id == null)
            {
                await ParentescoTable.InsertAsync(parentesco);
            }
            else
            {
                await ParentescoTable.UpdateAsync(parentesco);
            }
        }

        public async Task DeleteParentescoAsync(Parentesco parentesco)
        {
            await ParentescoTable.DeleteAsync(parentesco);
        }

        public async Task<ObservableCollection<PacienteFamiliar>> RefreshPacienteFamiliarAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif

                var items = await PacienteFamiliarTable
                    .ToEnumerableAsync();
                return new ObservableCollection<PacienteFamiliar>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation at {0}: {1}", CuidadorTable.TableName, msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
            }
            return null;
        }

        public async Task SavePacienteFamiliarAsync(PacienteFamiliar pacienteFamiliar, bool isNewItem)
        {
            if (pacienteFamiliar.Id == null)
            {
                await PacienteFamiliarTable.InsertAsync(pacienteFamiliar);
            }
            else
            {
                await PacienteFamiliarTable.UpdateAsync(pacienteFamiliar);
            }
        }

        public async Task DeletePacienteFamiliarAsync(PacienteFamiliar pacienteFamiliar)
        {
            await PacienteFamiliarTable.DeleteAsync(pacienteFamiliar);
        }

        public async Task<ObservableCollection<Familiar>> RefreshFamiliarAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif

                var items = await FamiliarTable
                    .ToEnumerableAsync();
                return new ObservableCollection<Familiar>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation at {0}: {1}", CuidadorTable.TableName, msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
            }
            return null;
        }

        public async Task SaveFamiliarAsync(Familiar familiar, bool isNewItem)
        {
            if (familiar.Id == null)
            {
                await FamiliarTable.InsertAsync(familiar);
            }
            else
            {
                await FamiliarTable.UpdateAsync(familiar);
            }
        }

        public async Task DeleteFamiliarAsync(Familiar familiar)
        {
            await FamiliarTable.DeleteAsync(familiar);
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

                var items = await CuidadorTable
                    .ToEnumerableAsync();
                return new ObservableCollection<Cuidador>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation at {0}: {1}", CuidadorTable.TableName, msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
            }
            return null;
        }

        public async Task SaveCuidadorAsync(Cuidador item, bool isNewItem)
        {
            if (isNewItem)
            {
                await CuidadorTable.InsertAsync(item);
            }
            else
            {
                await CuidadorTable.UpdateAsync(item);
            }
        }

        /***********************/ ///////////

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
                var items = await PacienteTable
                    .ToEnumerableAsync();
                return new ObservableCollection<Paciente>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation at {0}: {1}", PacienteTable.TableName, msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
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

                var items = await PacienteTable.Where(item => item.Id == ID)
                    .ToEnumerableAsync();
                return new ObservableCollection<Paciente>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation at {0}: {1}", PacienteTable.TableName, msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
            }
            return null;
        }

        public async Task SavePacienteAsync(Paciente paciente, bool isNewItem)
        {
            try
            {
                if (paciente.Id == null)
                {
                    await PacienteTable.InsertAsync(paciente);
                }
                else
                {
                    await PacienteTable.UpdateAsync(paciente);
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

                var items = await CuidadorPacienteTable
                    .ToEnumerableAsync();
                return new ObservableCollection<CuidadorPaciente>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation at {0}: {1}", CuidadorPacienteTable.TableName, msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
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
                var items = await AfazerTable
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
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
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

                var items = await ConclusaoAfazerTable
                    .ToEnumerableAsync();
                return new ObservableCollection<ConclusaoAfazer>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
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
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
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

                var items = await MaterialTable
                    .ToEnumerableAsync();
                return new ObservableCollection<Material>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
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

                var items = await MaterialTable
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
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
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
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
            }
        }


        public async Task DeleteMaterialUtilizadoAsync(MaterialUtilizado MaterialUtilizado)
        {
            await MaterialUtilizadoTable.DeleteAsync(MaterialUtilizado);
        }

        public async Task<ObservableCollection<MaterialUtilizado>> RefreshMaterialUtilizadoAsync(string Id,
            bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif

                var items = await MaterialUtilizadoTable
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
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
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
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
            }
        }

        public async Task DeleteMedicamentoAdministradoAsync(MedicamentoAdministrado medicamentoAdministrado)
        {
            await MedicamentoAdministradoTable.DeleteAsync(medicamentoAdministrado);
        }

        public async Task<ObservableCollection<MedicamentoAdministrado>> RefreshMedicamentoAdministradoAsync(string Id,
            bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif

                var items = await MedicamentoAdministradoTable
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
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
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
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
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

                var items = await MedicamentoTable
                    .ToEnumerableAsync();
                return new ObservableCollection<Medicamento>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
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
                var items = await MotivoCuidadoTable
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
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
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

        public async Task<ObservableCollection<TipoCuidador>> RefreshTipoCuidadorAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif
                var items = await TipoCuidadorTable
                    .ToEnumerableAsync();
                var x = items.Count();
                return new ObservableCollection<TipoCuidador>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine("Ih, rapaz1 {0}", e.Message);
                throw;
            }

            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
            }

            return null;
        }

        public async Task SaveTipoCuidadorAsync(TipoCuidador tipoCuidador, bool isNewItem)
        {
            try
            {
                if (tipoCuidador.Id == null)
                {
                    await TipoCuidadorTable.InsertAsync(tipoCuidador);
                }
                else
                {
                    await TipoCuidadorTable.UpdateAsync(tipoCuidador);
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

        public async Task DeleteTipoCuidadorAsync(TipoCuidador tipoCuidador)
        {
            await TipoCuidadorTable.DeleteAsync(tipoCuidador);
        }

        public async Task<ObservableCollection<ValidacaoCuidador>> RefreshValidacaoCuidadorAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await SyncAsync();
                }
#endif
                var items = await ValidacaoCuidadorTable
                    .ToEnumerableAsync();
                var a = items.Count();
                return new ObservableCollection<ValidacaoCuidador>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
            }
            return null;
        }

        public async Task SaveValidacaoCuidadorAsync(ValidacaoCuidador validacaoCuidador, bool isNewItem)
        {
            try
            {
                if (isNewItem)
                {
                    await ValidacaoCuidadorTable.InsertAsync(validacaoCuidador);
                }
                else
                {
                    await ValidacaoCuidadorTable.UpdateAsync(validacaoCuidador);
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

        public async Task DeleteValidacaoCuidadorAsync(ValidacaoCuidador validacaoCuidador)
        {
            await ValidacaoCuidadorTable.DeleteAsync(validacaoCuidador);
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
                var items = await PeriodoTratamentoTable
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
                Debug.WriteLine(@"Sync error: {0}", e.ToString());
            }
            return null;
        }

        public async Task SavePeriodoTratamentoAsync(PeriodoTratamento item, bool isNewItem)
        {
            try
            {
                if (isNewItem)
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
                await Task.Run(async () =>
                {
                    await CurrentClient.SyncContext.PushAsync();

                    await CuidadorTable.PullAsync(
                        //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                        //Use a different query name for each unique query in your program
                        "allCuidador",
                        CuidadorTable.CreateQuery());

                    await PacienteTable.PullAsync(
                        "allPaciente",
                        PacienteTable.CreateQuery());

                    await MedicamentoTable.PullAsync(
    "allMedicamento",
    MedicamentoTable.CreateQuery());

                    await MaterialUtilizadoTable.PullAsync(
"allMaterialUtilizado",
MaterialUtilizadoTable.CreateQuery());

                    await MedicamentoAdministradoTable.PullAsync(
"allMedicamentoAdministrado",
MedicamentoAdministradoTable.CreateQuery());


                    await CuidadorPacienteTable.PullAsync(
                        "allCuidadorPaciente",
                        CuidadorPacienteTable.CreateQuery());
                });
                await Task.Run(async () =>
                {
                    await ConclusaoAfazerTable.PullAsync(
                        "allConclusaoAfazer",
                        ConclusaoAfazerTable.CreateQuery());

                    await AfazerTable.PullAsync(
                        "allAfazer",
                        AfazerTable.CreateQuery());

                    await TipoCuidadorTable.PullAsync(
    "allTipoCuidador",
    TipoCuidadorTable.CreateQuery());

                    await PeriodoTratamentoTable.PullAsync(
"allPeriodoTratamento",
PeriodoTratamentoTable.CreateQuery());

                    await ValidacaoCuidadorTable.PullAsync(
"allValidacaoCuidador",
ValidacaoCuidadorTable.CreateQuery());

                    await MotivoCuidadoTable.PullAsync(
"allMotivoCuidado",
MotivoCuidadoTable.CreateQuery());
                });
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }
            catch (NullReferenceException e)
            {

                Debug.WriteLine(e.ToString());
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

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.",
                        error.TableName, error.Item["id"]);
                }
            }
        }
#endif
    }
}