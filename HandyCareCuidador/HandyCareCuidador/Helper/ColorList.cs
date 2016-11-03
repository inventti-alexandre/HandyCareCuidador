using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using Xamarin.Forms;

namespace HandyCareCuidador.Helper
{
    [ImplementPropertyChanged]
    public class ColorList
    {
        public string Cor { get; set; }
        public Color Color { get; set; }
    }
}
