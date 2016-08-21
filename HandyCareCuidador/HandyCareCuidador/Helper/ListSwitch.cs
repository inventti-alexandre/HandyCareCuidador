using HandyCareCuidador.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HandyCareCuidador
{
    public class ListSwitch : Switch
    {
        public static readonly BindableProperty AfazerProperty = BindableProperty.Create(
    propertyName: "Afazer",
    returnType: typeof(Afazer),
    declaringType: typeof(ListSwitch),
    defaultValue: null,
    defaultBindingMode:BindingMode.TwoWay);
        public Afazer Afazer
        {
            get { return (Afazer)GetValue(AfazerProperty); }
            set { SetValue(AfazerProperty, value); }
        }
    }
}
