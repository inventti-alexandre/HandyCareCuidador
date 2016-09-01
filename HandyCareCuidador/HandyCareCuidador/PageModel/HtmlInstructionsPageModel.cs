using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshMvvm;
using PropertyChanged;
using TK.CustomMap.Overlays;
using Xamarin.Forms;

namespace HandyCareCuidador.PageModel
{
    [ImplementPropertyChanged]
    public class HtmlInstructionsPageModel:FreshBasePageModel
    {
        public HtmlWebViewSource Instructions { get; private set; }


        public HtmlInstructionsPageModel(TKRoute route)
        {
            this.Instructions = new HtmlWebViewSource {Html = @"<html><body>"};
            foreach (var s in route.Steps)
            {
                this.Instructions.Html += $"<b>{s.Distance/1000}km:</b> {s.Instructions}<br /><hr />";
            }
            this.Instructions.Html += @"</body></html>";
        }
    }
}
