using System.Collections.ObjectModel;
using System.Threading.Tasks;
using HandyCareCuidador.Model;

namespace HandyCareCuidador.Interface
{
    public interface IMotivoCuidadoRestService
    {
        Task<ObservableCollection<MotivoCuidado>> RefreshDataAsync(bool syncItems = false);
        Task SaveMotivoCuidadoAsync(MotivoCuidado motivoCuidado, bool isNewItem);
        Task DeleteMotivoCuidadoAsync(MotivoCuidado motivoCuidado);

    }
}