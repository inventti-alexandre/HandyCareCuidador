using HandyCareCuidador.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandyCareCuidador.Interface
{
    public interface IMaterialRestService
    {
        Task<ObservableCollection<Material>> RefreshDataAsync(bool syncItems = false);
        Task<ObservableCollection<Material>> RefreshDataExistenteAsync(bool syncItems = false);
        Task SaveMaterialAsync(Material material, bool isNewItem);
        Task DeleteMaterialAsync(Material material);
    }
}
