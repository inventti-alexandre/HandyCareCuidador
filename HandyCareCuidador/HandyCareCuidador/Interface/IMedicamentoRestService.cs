using HandyCareCuidador.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandyCareCuidador.Interface
{
    public interface IMedicamentoRestService
    {
        Task<ObservableCollection<Medicamento>> RefreshDataAsync(bool syncItems = false);
        Task SaveMedicamentoAsync(Medicamento medicamento, bool isNewItem);
        Task DeleteMedicamentoAsync(Medicamento medicamento);

    }
}
