using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HandyCareCuidador.Helper
{
    [ImplementPropertyChanged]
    public class HorarioViewModel
    {
        public DateTime Data { get; set; }
        public TimeSpan Horario { get; set; }
        public bool deleteVisible { get; set; }
        public bool ActivityRunning { get; set; }
        public int? Quantidade { get; set; }
        public float? QuantidadeF { get; set; }
        public bool HabilitarMaterial { get; set; }
        public bool HabilitarMedicamento { get; set; }
        public bool FinalizarAfazer { get; set; }
    }
}
