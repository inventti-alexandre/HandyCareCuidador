using System.Collections.ObjectModel;
using System.Threading.Tasks;
using HandyCareCuidador.Model;

namespace HandyCareCuidador.Interface
{
    public interface IPeriodoTratamentoRestService
    {
        Task<ObservableCollection<PeriodoTratamento>> RefreshDataAsync(bool syncItems = false);
        Task SavePeriodoTratamentoAsync(PeriodoTratamento periodoTratamento, bool isNewItem);
        Task DeletePeriodoTratamentoAsync(PeriodoTratamento periodoTratamento);
    }
}