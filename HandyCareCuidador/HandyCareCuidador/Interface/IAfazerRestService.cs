using HandyCareCuidador.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandyCareCuidador.Interface
{
    public interface IAfazerRestService
    {
        Task<ObservableCollection<Afazer>> RefreshDataAsync(bool syncItems = false);
        Task SaveAfazerAsync(Afazer afazer, bool isNewItem);
        Task DeleteAfazerAsync(Afazer afazer);
    }
}
