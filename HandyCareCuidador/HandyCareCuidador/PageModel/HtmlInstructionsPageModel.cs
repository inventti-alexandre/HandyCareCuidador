using FreshMvvm;
using PropertyChanged;
using TK.CustomMap.Overlays;
using Xamarin.Forms;

namespace HandyCareCuidador.PageModel
{
    [ImplementPropertyChanged]
    public class HtmlInstructionsPageModel : FreshBasePageModel
    {
        public HtmlInstructionsPageModel(TKRoute route)
        {
            Instructions = new HtmlWebViewSource {Html = @"<html><body>"};
            foreach (var s in route.Steps)
                Instructions.Html += $"<b>{s.Distance/1000}km:</b> {s.Instructions}<br /><hr />";
            Instructions.Html += @"</body></html>";
        }

        public HtmlWebViewSource Instructions { get; }
    }
}