using HandyCareCuidador.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandyCareCuidador.Interface
{
    public interface IMaterialUtilizadoRestService
    {
            Task<ObservableCollection<MaterialUtilizado>> RefreshDataAsync(string Id, bool syncItems = false);
            Task SaveMaterialUtilizadoAsync(MaterialUtilizado materialUtilizado, bool isNewItem);
            Task DeleteMaterialUtilizadoAsync(MaterialUtilizado materialUtilizado);
    }
}
