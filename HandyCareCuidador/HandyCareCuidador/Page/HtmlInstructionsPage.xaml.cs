using HandyCareCuidador.PageModel;
using TK.CustomMap.Overlays;
using Xamarin.Forms;

namespace HandyCareCuidador.Page
{
    public partial class HtmlInstructionsPage : ContentPage
    {
        public HtmlInstructionsPage(TKRoute route)
        {
            InitializeComponent();
            BindingContext = new HtmlInstructionsPageModel(route);
        }
    }
}