using HandyCareCuidador.PageModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TK.CustomMap.Overlays;
using Xamarin.Forms;

namespace HandyCareCuidador.Page
{
    public partial class HtmlInstructionsPage : ContentPage
    {
        public HtmlInstructionsPage(TKRoute route)
        {
            InitializeComponent();
            this.BindingContext = new HtmlInstructionsPageModel(route);
        }
    }
}
