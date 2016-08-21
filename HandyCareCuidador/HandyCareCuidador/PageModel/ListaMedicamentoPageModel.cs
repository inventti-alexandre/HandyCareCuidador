using FreshMvvm;
using HandyCareCuidador.Interface;
using HandyCareCuidador.Model;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HandyCareCuidador.PageModel
{
    [ImplementPropertyChanged]

    public class ListaMedicamentoPageModel: FreshBasePageModel
    {
            IMedicamentoRestService _restService;
            public bool deleteVisible;
            public ListaMedicamentoPageModel(IMedicamentoRestService restService)
            {
                _restService = restService;
            }
            public ObservableCollection<Medicamento> Medicamentos { get; set; }

            public override async void Init(object initData)
            {
                base.Init(initData);
                await GetMedicamentos();
            }
            public async Task GetMedicamentos()
            {
                try
                {
                    await Task.Run(async () => {
                        Medicamentos = new ObservableCollection<Medicamento>(await _restService.RefreshDataAsync());
                        });
                }
                catch (Exception)
                {
                    throw;
                }
            }

        public override void ReverseInit(object returndData)
            {
                base.ReverseInit(returndData);
                var newMedicamento = returndData as Medicamento;
                if (!Medicamentos.Contains(newMedicamento))
                {
                Medicamentos.Add(newMedicamento);
                }
            }
            public Command AddMedicamento
            {
                get
                {
                    return new Command(async () =>
                    {
                        deleteVisible = false;
                        await CoreMethods.PushPageModel<MedicamentoPageModel>();
                    });
                }
            }
            Medicamento _selectedMedicamento;
            public Medicamento SelectedMedicamento
            {
                get
                {
                    return _selectedMedicamento;
                }
                set
                {
                    _selectedMedicamento = value;
                    if (value != null)
                    {
                        MedicamentoSelected.Execute(value);
                        SelectedMedicamento = null;
                    }
                }
            }
            public Command<Medicamento> MedicamentoSelected
            {
                get
                {
                    return new Command<Medicamento>(async (medicamento) =>
                    {
                        deleteVisible = true;
                        RaisePropertyChanged("IsVisible");
                        await CoreMethods.PushPageModel<MedicamentoPageModel>(medicamento);
                        medicamento = null;
                    });
                }
            }
    }
}
