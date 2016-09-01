using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TK.CustomMap;
using Xamarin.Forms;

namespace HandyCareCuidador.Page
{
    public partial class PinListPage : ContentPage
    {
        public event EventHandler<PinSelectedEventArgs> PinSelected;

        private readonly IEnumerable<TKCustomMapPin> _pins;


        public PinListPage(IEnumerable<TKCustomMapPin> pins)
        {
            InitializeComponent();
            
            this._pins = pins;
            this.BindingContext = this._pins;

            this.lvPins.ItemSelected += (o, e) =>
            {
                if (this.lvPins.SelectedItem == null) return;

                this.OnPinSelected((TKCustomMapPin)this.lvPins.SelectedItem);
            };
        }
        protected virtual void OnPinSelected(TKCustomMapPin pin)
        {
            this.PinSelected?.Invoke(this, new PinSelectedEventArgs(pin));
        }
    }
    public class PinSelectedEventArgs : EventArgs
    {
        public TKCustomMapPin Pin { get; private set; }

        public PinSelectedEventArgs(TKCustomMapPin pin)
        {
            this.Pin = pin;
        }
    }
}
