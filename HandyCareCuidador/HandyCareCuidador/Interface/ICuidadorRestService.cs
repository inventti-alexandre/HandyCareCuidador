using HandyCareCuidador.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandyCareCuidador.Interface
{
    public interface ICuidadorRestService
    {
        Task<ObservableCollection<Cuidador>> RefreshDataAsync(bool syncItems = false);
        Task SaveCuidadorAsync(Cuidador cuidador, bool isNewItem);
        Task DeleteCuidadorAsync(Cuidador cuidador);

    }
}
