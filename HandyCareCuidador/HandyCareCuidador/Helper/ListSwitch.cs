using HandyCareCuidador.Model;
using Xamarin.Forms;

namespace HandyCareCuidador
{
    public class ListSwitch : Switch
    {
        public static readonly BindableProperty AfazerProperty = BindableProperty.Create(
            "Afazer",
            typeof(Afazer),
            typeof(ListSwitch),
            null,
            BindingMode.TwoWay);

        public Afazer Afazer
        {
            get { return (Afazer) GetValue(AfazerProperty); }
            set { SetValue(AfazerProperty, value); }
        }
    }
}