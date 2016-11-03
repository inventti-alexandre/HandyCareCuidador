using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Syncfusion.SfCalendar.XForms;
using Xamarin.Forms;

namespace HandyCareCuidador.Helper
{
    public class MonthInlineAppointmentBehavior:Behavior<SfCalendar>
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(MonthInlineAppointmentBehavior), null);
        public static readonly BindableProperty InputConverterProperty =
                BindableProperty.Create("Converter", typeof(IValueConverter), typeof(MonthInlineAppointmentBehavior), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public IValueConverter Converter
        {
            get { return (IValueConverter)GetValue(InputConverterProperty); }
            set { SetValue(InputConverterProperty, value); }
        }
        public SfCalendar AssociatedObject { get; private set; }

        protected override void OnAttachedTo(SfCalendar bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;
            bindable.BindingContextChanged += OnBindingContextChanged;
            bindable.InlineToggled += BindableOnInlineToggled;
        }

        private void BindableOnInlineToggled(object sender, InlineToggledEventArgs e)
        {
            if (Command == null)
            {
                return;
            }

            object parameter = Converter.Convert(e, typeof(object), null, null);
            if (Command.CanExecute(parameter))
            {
                Command.Execute(parameter);
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }
        private void OnBindingContextChanged(object sender, EventArgs e)
        {
           OnBindingContextChanged();
        }

        protected override void OnDetachingFrom(SfCalendar bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.BindingContextChanged -= OnBindingContextChanged;
            bindable.InlineToggled -= BindableOnInlineToggled;
            AssociatedObject = null;
        }
    }
}
