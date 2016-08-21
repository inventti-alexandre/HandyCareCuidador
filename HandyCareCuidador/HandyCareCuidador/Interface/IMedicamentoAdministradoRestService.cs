using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyCareCuidador.Model;

namespace HandyCareCuidador.Interface
{
    public interface IMedicamentoAdministradoRestService
    {
        Task<ObservableCollection<MedicamentoAdministrado>> RefreshDataAsync(string Id, bool syncItems = false);
        Task SaveMedicamentoAdministradoAsync(MedicamentoAdministrado medicamentoAdministrado, bool isNewItem);
        Task DeleteMedicamentoAdministradoAsync(MedicamentoAdministrado medicamentoAdministrado);

    }
}
