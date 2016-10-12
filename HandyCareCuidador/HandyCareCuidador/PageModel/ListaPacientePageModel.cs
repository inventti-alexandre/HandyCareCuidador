using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareCuidador.Data;
using HandyCareCuidador.Model;
using PropertyChanged;

namespace HandyCareCuidador.PageModel
{
    [ImplementPropertyChanged]
    public class ListaPacientePageModel : FreshBasePageModel
    {
        //IPacienteRestService _restService;
        //private ICuidadorPacienteRestService _cuidadorPacienteRestService;

        public ObservableCollection<Paciente> Pacientes { get; set; }
        public ObservableCollection<CuidadorPaciente> CuidadoresPacientes { get; set; }

        public override async void Init(object initData)
        {
            base.Init(initData);
            await GetMedicamentos();
        }

        public async Task GetMedicamentos()
        {
            try
            {
                CuidadoresPacientes =
                    new ObservableCollection<CuidadorPaciente>(
                        await CuidadorRestService.DefaultManager.RefreshCuidadorPacienteAsync(true));
                var selection =
                    new ObservableCollection<Paciente>(
                        await CuidadorRestService.DefaultManager.RefreshPacienteAsync(true));
                var result = selection.Where(e => CuidadoresPacientes.Select(m => m.PacId)
                    .Contains(e.Id)).AsEnumerable();
                Pacientes = new ObservableCollection<Paciente>(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}