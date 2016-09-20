using System.Collections.ObjectModel;
using System.Threading.Tasks;
using HandyCareCuidador.Model;
using Microsoft.WindowsAzure.MobileServices;

namespace HandyCareCuidador.Interface
{
    public interface ICuidadorRestService
    {
        Task<ObservableCollection<ContatoEmergencia>> RefreshContatoEmergenciaAsync(bool syncItems = false);
        Task<ObservableCollection<ConTelefone>> RefreshConTelefoneAsync(bool syncItems = false);
        Task<ObservableCollection<ConCelular>> RefreshConCelularAsync(bool syncItems = false);

        Task<ObservableCollection<TipoTratamento>> RefreshTipoTratamentoAsync(bool syncItems = false);
        Task SaveTipoTratamentoAsync(TipoTratamento tipoTratamento, bool isNewItem);
        Task DeleteTipoTratamentoAsync(TipoTratamento tipoTratamento);

        Task<ObservableCollection<Video>> RefreshVideoAsync(bool syncItems = false);
        Task SaveVideoAsync(Video video, bool isNewItem);
        Task DeleteVideoAsync(Video video);

        Task<ObservableCollection<VideoFamiliar>> RefreshVideoFamiliarAsync(bool syncItems = false);
        Task SaveVideoFamiliarAsync(VideoFamiliar videoFamiliar, bool isNewItem);
        Task DeleteVideoFamiliarAsync(VideoFamiliar fotoFamiliar);


        Task<ObservableCollection<ViaAdministracaoMedicamento>> RefreshViaAdministracaoMedicamentoAsync(bool syncItems = false);
        Task<ObservableCollection<FormaApresentacaoMedicamento>> RefreshFormaApresentacaoMedicamentoAsync(bool syncItems = false);
        Task<ObservableCollection<FotoFamiliar>> RefreshFotoFamiliarAsync(bool syncItems = false);
        Task SaveFotoFamiliarAsync(FotoFamiliar fotoFamiliar, bool isNewItem);
        Task DeleteFotoFamiliarAsync(FotoFamiliar fotoFamiliar);

        Task<ObservableCollection<Parentesco>> RefreshParentescoAsync(bool syncItems = false);
        Task SaveParentescoAsync(Parentesco parentesco, bool isNewItem);
        Task DeleteParentescoAsync(Parentesco parentesco);

        Task<ObservableCollection<PacienteFamiliar>> RefreshPacienteFamiliarAsync(bool syncItems = false);
        Task SavePacienteFamiliarAsync(PacienteFamiliar pacienteFamiliar, bool isNewItem);
        Task DeletePacienteFamiliarAsync(PacienteFamiliar pacienteFamiliar);

        Task<ObservableCollection<Familiar>> RefreshFamiliarAsync(bool syncItems = false);
        Task SaveFamiliarAsync(Familiar familiar, bool isNewItem);
        Task DeleteFamiliarAsync(Familiar familiar);

        /// <summary>
        ///     Manager de Cuidador
        /// </summary>
        /// <param name="syncItems"></param>
        /// <returns></returns>
        Task<ObservableCollection<Cuidador>> RefreshCuidadorAsync(bool syncItems = false);

        Task<Cuidador> ProcurarCuidadorAsync(string id, MobileServiceAuthenticationProvider provider,
            bool syncItems = false);
        Task SaveCuidadorAsync(Cuidador cuidador, bool isNewItem);
        Task DeleteCuidadorAsync(Cuidador cuidador);


        Task<ObservableCollection<Foto>> RefreshFotoAsync(bool syncItems = false);
        Task<Foto> ProcurarFotoAsync(string id, bool syncItems = false);
        Task SaveFotoAsync(Foto foto, bool isNewItem);
        Task DeleteFotoAsync(Foto foto);

        /// <summary>
        ///     Manager de CuidadorPaciente
        /// </summary>
        /// <param name="syncItems"></param>
        /// <returns></returns>
        Task<ObservableCollection<CuidadorPaciente>> RefreshCuidadorPacienteAsync(bool syncItems = false);

        Task SaveCuidadorPacienteAsync(CuidadorPaciente cuidadorPaciente, bool isNewItem);
        Task DeleteCuidadorPacienteAsync(CuidadorPaciente cuidadorPaciente);

        /// <summary>
        ///     Manager de Paciente
        /// </summary>
        /// <param name="syncItems"></param>
        /// <returns></returns>
        Task<ObservableCollection<Paciente>> RefreshPacienteAsync(bool syncItems = false);

        Task<ObservableCollection<Paciente>> RefreshPacienteAsync(string ID, bool syncItems = false);
        Task SavePacienteAsync(Paciente paciente, bool isNewItem);
        Task DeletePacienteAsync(Paciente paciente);

        /// <summary>
        ///     Manager de Afazer
        /// </summary>
        /// <param name="syncItems"></param>
        /// <returns></returns>
        Task<ObservableCollection<Afazer>> RefreshAfazerAsync(bool syncItems = false);

        Task SaveAfazerAsync(Afazer afazer, bool isNewItem);
        Task DeleteAfazerAsync(Afazer afazer);

        /// <summary>
        ///     Manager de ConclusaoAfazer
        /// </summary>
        /// <param name="syncItems"></param>
        /// <returns></returns>
        Task<ObservableCollection<ConclusaoAfazer>> RefreshConclusaoAfazerAsync(bool syncItems = false);

        Task SaveConclusaoAfazerAsync(ConclusaoAfazer conclusaoAfazer, bool isNewItem);
        Task DeleteConclusaoAfazerAsync(ConclusaoAfazer conclusaoAfazer);

        /// <summary>
        /// </summary>
        /// <param name="syncItems"></param>
        /// <returns></returns>
        Task<ObservableCollection<Material>> RefreshMaterialAsync(bool syncItems = false);

        Task<ObservableCollection<Material>> RefreshMaterialExistenteAsync(bool syncItems = false);
        Task SaveMaterialAsync(Material material, bool isNewItem);
        Task DeleteMaterialAsync(Material material);

        /// <summary>
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="syncItems"></param>
        /// <returns></returns>
        Task<ObservableCollection<MaterialUtilizado>> RefreshMaterialUtilizadoAsync(string Id, bool syncItems = false);

        Task SaveMaterialUtilizadoAsync(MaterialUtilizado materialUtilizado, bool isNewItem);
        Task DeleteMaterialUtilizadoAsync(MaterialUtilizado materialUtilizado);

        Task<ObservableCollection<MedicamentoAdministrado>> RefreshMedicamentoAdministradoAsync(string Id,
            bool syncItems = false);

        Task SaveMedicamentoAdministradoAsync(MedicamentoAdministrado medicamentoAdministrado, bool isNewItem);
        Task DeleteMedicamentoAdministradoAsync(MedicamentoAdministrado medicamentoAdministrado);

        /// <summary>
        /// </summary>
        /// <param name="syncItems"></param>
        /// <returns></returns>
        Task<ObservableCollection<Medicamento>> RefreshMedicamentoAsync(bool syncItems = false);

        Task SaveMedicamentoAsync(Medicamento medicamento, bool isNewItem);
        Task DeleteMedicamentoAsync(Medicamento medicamento);

        /// <summary>
        /// </summary>
        /// <param name="syncItems"></param>
        /// <returns></returns>
        Task<ObservableCollection<MotivoCuidado>> RefreshMotivoCuidadoAsync(bool syncItems = false);

        Task SaveMotivoCuidadoAsync(MotivoCuidado motivoCuidado, bool isNewItem);
        Task DeleteMotivoCuidadoAsync(MotivoCuidado motivoCuidado);

        /// <summary>
        /// </summary>
        /// <param name="syncItems"></param>
        /// <returns></returns>
        Task<ObservableCollection<PeriodoTratamento>> RefreshPeriodoTratamentoAsync(bool syncItems = false);
        Task SavePeriodoTratamentoAsync(PeriodoTratamento periodoTratamento, bool isNewItem);
        Task DeletePeriodoTratamentoAsync(PeriodoTratamento periodoTratamento);
        Task<ObservableCollection<TipoCuidador>> RefreshTipoCuidadorAsync(bool syncItems = false);
        Task SaveTipoCuidadorAsync(TipoCuidador tipoCuidador, bool isNewItem);
        Task DeleteTipoCuidadorAsync(TipoCuidador tipoCuidador);
        Task<ObservableCollection<ValidacaoCuidador>> RefreshValidacaoCuidadorAsync(bool syncItems = false);
        Task SaveValidacaoCuidadorAsync(ValidacaoCuidador validacaoCuidador, bool isNewItem);
        Task DeleteValidacaoCuidadorAsync(ValidacaoCuidador validacaoCuidador);


    }
}