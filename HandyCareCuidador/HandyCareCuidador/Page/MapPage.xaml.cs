using System.Diagnostics;
using HandyCareCuidador.Page.Layout;
using HandyCareCuidador.PageModel;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using TK.CustomMap;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Position = Xamarin.Forms.Maps.Position;

namespace HandyCareCuidador.Page
{
    public partial class MapPage : ContentPage
    {
        public Position CurrentPosition;

        public MapPage()
        {
            InitializeComponent();
            GetLocation();
            CreateView();
            BindingContext = new MapPageModel();
        }

        private async void GetLocation()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                var position = await locator.GetPositionAsync(10000);
                CurrentPosition = new Position(position.Latitude, position.Longitude);
                Debug.WriteLine("Localização atual - Latitude: " + CurrentPosition.Latitude + " " + "Longitude " +
                                CurrentPosition.Longitude);
            }
            catch (GeolocationException e)
            {
                Debug.WriteLine(e.Message);

                throw;
            }
        }

        private void CreateView()
        {
            var autoComplete = new PlacesAutoComplete {ApiToUse = PlacesAutoComplete.PlacesApi.Native};
            autoComplete.SetBinding(PlacesAutoComplete.PlaceSelectedCommandProperty, "PlaceSelectedCommand");
            var mapView = new TKCustomMap(MapSpan.FromCenterAndRadius(CurrentPosition,
                    Distance.FromKilometers(1)))
                //var mapView = new TKCustomMap
                {
                    MapCenter = CurrentPosition,
                    MapType = MapType.Hybrid,
                    IsShowingUser = true,
                    IsRegionChangeAnimated = true
                };
            mapView.SetBinding(TKCustomMap.CustomPinsProperty, "Pins");
            mapView.SetBinding(TKCustomMap.MapClickedCommandProperty, "MapClickedCommand");
            mapView.SetBinding(TKCustomMap.MapLongPressCommandProperty, "MapLongPressCommand");
            mapView.SetBinding(TKCustomMap.MapCenterProperty, "MapCenter");
            mapView.SetBinding(TKCustomMap.PinSelectedCommandProperty, "PinSelectedCommand");
            mapView.SetBinding(TKCustomMap.SelectedPinProperty, "SelectedPin");
            mapView.SetBinding(TKCustomMap.RoutesProperty, "Routes");
            mapView.SetBinding(TKCustomMap.PinDragEndCommandProperty, "DragEndCommand");
            mapView.SetBinding(TKCustomMap.CirclesProperty, "Circles");
            mapView.SetBinding(TKCustomMap.CalloutClickedCommandProperty, "CalloutClickedCommand");
            mapView.SetBinding(TKCustomMap.PolylinesProperty, "Lines");
            mapView.SetBinding(TKCustomMap.PolygonsProperty, "Polygons");
            mapView.SetBinding(TKCustomMap.MapRegionProperty, "MapRegion");
            mapView.SetBinding(TKCustomMap.RouteClickedCommandProperty, "RouteClickedCommand");
            mapView.SetBinding(TKCustomMap.RouteCalculationFinishedCommandProperty, "RouteCalculationFinishedCommand");
            mapView.SetBinding(TKCustomMap.TilesUrlOptionsProperty, "TilesUrlOptions");
            mapView.SetBinding(TKCustomMap.MapFunctionsProperty, "MapFunctions");

            autoComplete.SetBinding(PlacesAutoComplete.BoundsProperty, "MapRegion");


            _baseLayout.Children.Add(
                mapView,
                Constraint.RelativeToView(autoComplete, (r, v) => v.X),
                Constraint.RelativeToView(autoComplete, (r, v) => autoComplete.HeightOfSearchBar),
                heightConstraint: Constraint.RelativeToParent(r => r.Height - autoComplete.HeightOfSearchBar),
                widthConstraint: Constraint.RelativeToView(autoComplete, (r, v) => v.Width));

            _baseLayout.Children.Add(
                autoComplete,
                Constraint.Constant(0),
                Constraint.Constant(0));
        }
    }
}