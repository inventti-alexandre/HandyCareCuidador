using HandyCareCuidador.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandyCareCuidador.Interface
{
    public interface IConclusaoAfazerRestService
    {
        Task<ObservableCollection<ConclusaoAfazer>> RefreshDataAsync(bool syncItems = false);
        Task SaveConclusaoAfazerAsync(ConclusaoAfazer conclusaoAfazer, bool isNewItem);
        Task DeleteConclusaoAfazerAsync(ConclusaoAfazer conclusaoAfazer);

    }
}
