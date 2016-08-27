using System.Collections.ObjectModel;
using System.Threading.Tasks;
using HandyCareCuidador.Model;

namespace HandyCareCuidador.Interface
{
    public interface IFamiliarRestService
    {
        Task<ObservableCollection<Familiar>> RefreshDataAsync(bool syncItems = false);
        Task SaveFamiliarAsync(Familiar familiar, bool isNewItem);
        Task DeleteFamiliarAsync(Familiar familiar);
    }
}