using HandyCareCuidador.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandyCareCuidador.Interface
{
    public interface IPacienteRestService
    {
        Task<ObservableCollection<Paciente>> RefreshDataAsync(bool syncItems = false);
        Task<ObservableCollection<Paciente>> RefreshDataAsync(string ID, bool syncItems = false);
        Task SavePacienteAsync(Paciente paciente, bool isNewItem);
        Task DeletePacienteAsync(Paciente paciente);

    }
}
