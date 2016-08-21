using HandyCareCuidador.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandyCareCuidador.Interface
{
    public interface IFamiliarRestService
    {
        Task<ObservableCollection<Familiar>> RefreshDataAsync(bool syncItems = false);
        Task SaveFamiliarAsync(Familiar familiar, bool isNewItem);
        Task DeleteFamiliarAsync(Familiar familiar);
    }
}
