using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyCareCuidador.Page.Layout;
using HandyCareCuidador.PageModel;
using TK.CustomMap;
using TK.CustomMap.Overlays;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace HandyCareCuidador.Page
{
    public partial class AddRoutePage : ContentPage
    {
        public AddRoutePage(ObservableCollection<TKRoute> routes, ObservableCollection<TKCustomMapPin> pins, MapSpan bounds)
        {
            InitializeComponent();

            var searchFrom = new PlacesAutoComplete(false) { ApiToUse = PlacesAutoComplete.PlacesApi.Native, Bounds = bounds, Placeholder = "From" };
            searchFrom.SetBinding(PlacesAutoComplete.PlaceSelectedCommandProperty, "FromSelectedCommand");
            var searchTo = new PlacesAutoComplete(false) { ApiToUse = PlacesAutoComplete.PlacesApi.Native, Bounds = bounds, Placeholder = "To" };
            searchTo.SetBinding(PlacesAutoComplete.PlaceSelectedCommandProperty, "ToSelectedCommand");

            if (Device.OS == TargetPlatform.Android)
            {
                this._baseLayout.Children.Add(
                    null,
                    Constraint.Constant(10),
                    Constraint.RelativeToParent(l => l.Height - 30));
            }

            this._baseLayout.Children.Add(
                searchTo,
                yConstraint: Constraint.RelativeToView(searchFrom, (l, v) => searchFrom.HeightOfSearchBar + 10));

            this._baseLayout.Children.Add(
                searchFrom,
                Constraint.Constant(0),
                Constraint.Constant(10));
            //this.BindingContext = new AddRoutePageModel(routes, pins, bounds);
        }
    }
}
