using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareCuidador.Interface;
using HandyCareCuidador.Model;
using PropertyChanged;

namespace HandyCareCuidador.PageModel
{
    [ImplementPropertyChanged]
    public class ListaPacientePageModel:FreshBasePageModel
    {
        IPacienteRestService _restService;
        private ICuidadorPacienteRestService _cuidadorPacienteRestService;
        public ListaPacientePageModel()
        {
            _restService = FreshIOC.Container.Resolve<IPacienteRestService>();
            _cuidadorPacienteRestService = FreshIOC.Container.Resolve<ICuidadorPacienteRestService>();
        }

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
                CuidadoresPacientes = new ObservableCollection<CuidadorPaciente>(await _cuidadorPacienteRestService.RefreshDataAsync());
                var selection = new ObservableCollection<Paciente>(await _restService.RefreshDataAsync());
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
