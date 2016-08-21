using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace HandyCareCuidador.Page
{
    public partial class ListaMedicamentoPage : ContentPage
    {
        public ListaMedicamentoPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var lstMedicamento = this.FindByName<ListView>("lstMedicamento");

            if (lstMedicamento != null)
            {
                lstMedicamento.ClearValue(ListView.SelectedItemProperty);
            }
        }

    }
}
