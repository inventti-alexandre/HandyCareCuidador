using System;
using System.Collections.Generic;
using TK.CustomMap;
using Xamarin.Forms;

namespace HandyCareCuidador.Page
{
    public partial class PinListPage : ContentPage
    {
        private readonly IEnumerable<TKCustomMapPin> _pins;


        public PinListPage(IEnumerable<TKCustomMapPin> pins)
        {
            InitializeComponent();

            _pins = pins;
            BindingContext = _pins;

            lvPins.ItemSelected += (o, e) =>
            {
                if (lvPins.SelectedItem == null) return;

                OnPinSelected((TKCustomMapPin) lvPins.SelectedItem);
            };
        }

        public event EventHandler<PinSelectedEventArgs> PinSelected;

        protected virtual void OnPinSelected(TKCustomMapPin pin)
        {
            PinSelected?.Invoke(this, new PinSelectedEventArgs(pin));
        }
    }

    public class PinSelectedEventArgs : EventArgs
    {
        public PinSelectedEventArgs(TKCustomMapPin pin)
        {
            Pin = pin;
        }

        public TKCustomMapPin Pin { get; private set; }
    }
}