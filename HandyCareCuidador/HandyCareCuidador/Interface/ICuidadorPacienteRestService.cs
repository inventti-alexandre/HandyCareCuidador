using System.Collections.ObjectModel;
using System.Threading.Tasks;
using HandyCareCuidador.Model;

namespace HandyCareCuidador.Interface
{
    public interface ICuidadorPacienteRestService
    {
        Task<ObservableCollection<CuidadorPaciente>> RefreshDataAsync(bool syncItems = false);
        Task SaveCuidadorPacienteAsync(CuidadorPaciente cuidadorPaciente, bool isNewItem);
        Task DeleteCuidadorPacienteAsync(CuidadorPaciente cuidadorPaciente);

    }
}