using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareCuidador.CustomPins;
using HandyCareCuidador.Page;
using HandyCareCuidador.Page.Layout;
using Plugin.Geolocator;
using PropertyChanged;
using TK.CustomMap;
using TK.CustomMap.Api;
using TK.CustomMap.Api.Google;
using TK.CustomMap.Api.OSM;
using TK.CustomMap.Interfaces;
using TK.CustomMap.Overlays;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace HandyCareCuidador.PageModel
{
    [ImplementPropertyChanged]
    public class MapPageModel : FreshBasePageModel
    {
        private ObservableCollection<TKCircle> _circles;
        private ObservableCollection<TKPolyline> _lines;
        private Position _mapCenter;

        private MapSpan _mapRegion;
        private ObservableCollection<TKCustomMapPin> _pins;
        private ObservableCollection<TKPolygon> _polygons;
        private ObservableCollection<TKRoute> _routes;
        private TKCustomMapPin _selectedPin;
        private TKTileUrlOptions _tileUrlOptions;

        public MapPageModel()
        {
            GetLocation();

            _pins = new ObservableCollection<TKCustomMapPin>();
            _circles = new ObservableCollection<TKCircle>();
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            //GetPlaces();
        }

        //private void GetPlaces()
        //{
        //    var b = new GmsPlaceResult();
        //    b.
        //    GeoDataApi.GetPlaceTypes();
        //    var pin = new TKCustomMapPin
        //    {
        //        Position = position,
        //        Title = $"Pin {position.Latitude}, {position.Longitude}",
        //        ShowCallout = true,
        //        IsDraggable = true
        //    };
        //    _pins.Add(pin);
        //}

        public TKTileUrlOptions TilesUrlOptions
        {
            get
            {
                return _tileUrlOptions;
                //return new TKTileUrlOptions(
                //    "http://a.basemaps.cartocdn.com/dark_all/{2}/{0}/{1}.png", 256, 256, 0, 18);
                //return new TKTileUrlOptions(
                //    "http://a.tile.openstreetmap.org/{2}/{0}/{1}.png", 256, 256, 0, 18);
            }
            set
            {
                if (_tileUrlOptions != value)
                    _tileUrlOptions = value;
            }
        }

        public IRendererFunctions MapFunctions { get; set; }

        public Command RunSimulationCommand
        {
            get
            {
                return new Command(async _ =>
                {
                    if (!await CoreMethods.DisplayAlert("Start Test?", "Start simulation test?", "Yes", "No"))
                        return;

                    #region PinTest

                    var pin = new TKCustomMapPin
                    {
                        Position = new Position(40.718577, -74.083754)
                    };

                    _pins.Add(pin);
                    await Task.Delay(1000);

                    pin.DefaultPinColor = Color.Purple;
                    await Task.Delay(1000);
                    pin.DefaultPinColor = Color.Green;
                    await Task.Delay(1000);

                    pin.Image = Device.OnPlatform("Icon-Small.png", "icon.png", string.Empty);
                    await Task.Delay(1000);
                    pin.Image = null;
                    await Task.Delay(1000);

                    _pins.Remove(pin);
                    await Task.Delay(1000);
                    _pins.Add(pin);
                    await Task.Delay(1000);
                    _pins.Clear();

                    #endregion

                    #region Circles Test

                    var circle = new TKCircle
                    {
                        Center = new Position(40.659743, -74.049422),
                        Color = Color.Red,
                        Radius = 1000
                    };
                    _circles.Add(circle);
                    await Task.Delay(1000);

                    circle.Color = Color.Green;
                    await Task.Delay(1000);
                    circle.Color = Color.Purple;
                    await Task.Delay(1000);

                    circle.Radius = 2000;
                    await Task.Delay(1000);
                    circle.Radius = 3000;
                    await Task.Delay(1000);

                    circle.Center = new Position(40.718577, -74.083754);
                    await Task.Delay(1000);

                    _circles.Remove(circle);
                    await Task.Delay(1000);
                    _circles.Add(circle);
                    await Task.Delay(1000);
                    _circles.Clear();

                    #endregion

                    #region Lines Test

                    Lines = new ObservableCollection<TKPolyline>();

                    var line = new TKPolyline
                    {
                        Color = Color.Pink,
                        LineWidth = 2f,
                        LineCoordinates = new List<Position>(new[]
                        {
                            new Position(40.647241, -74.081007),
                            new Position(40.702873, -74.016162)
                        })
                    };

                    _lines.Add(line);
                    await Task.Delay(1000);

                    line.Color = Color.Red;
                    await Task.Delay(1000);
                    line.Color = Color.Green;
                    await Task.Delay(1000);

                    line.LineCoordinates = new List<Position>(new[]
                    {
                        new Position(40.647241, -74.081007),
                        new Position(40.702873, -74.016162),
                        new Position(40.690602, -74.017309)
                    });
                    await Task.Delay(1000);
                    _lines.Remove(line);
                    await Task.Delay(1000);
                    _lines.Add(line);
                    await Task.Delay(1000);
                    _lines.Clear();

                    #endregion

                    #region Polygon Test

                    Polygons = new ObservableCollection<TKPolygon>();

                    var poly = new TKPolygon
                    {
                        StrokeColor = Color.Green,
                        StrokeWidth = 2f,
                        Color = Color.Red,
                        Coordinates = new List<Position>(new[]
                        {
                            new Position(40.716901, -74.055969),
                            new Position(40.699878, -73.986296),
                            new Position(40.636811, -74.076240)
                        })
                    };

                    _polygons.Add(poly);
                    await Task.Delay(1000);

                    poly.StrokeColor = Color.Purple;
                    await Task.Delay(1000);
                    poly.StrokeWidth = 5f;
                    await Task.Delay(1000);
                    poly.StrokeWidth = 0;
                    await Task.Delay(1000);
                    poly.StrokeWidth = 2f;
                    await Task.Delay(1000);

                    poly.Color = Color.Yellow;
                    await Task.Delay(1000);

                    _polygons.Remove(poly);
                    await Task.Delay(1000);
                    _polygons.Add(poly);
                    await Task.Delay(1000);
                    _polygons.Clear();

                    #endregion

                    #region Tiles Test

                    TilesUrlOptions = new TKTileUrlOptions(
                        "http://a.basemaps.cartocdn.com/dark_all/{2}/{0}/{1}.png", 256, 256, 0, 18);
                    await Task.Delay(5000);
                    TilesUrlOptions = null;
                    await Task.Delay(5000);
                    TilesUrlOptions = new TKTileUrlOptions(
                        "http://a.tile.openstreetmap.org/{2}/{0}/{1}.png", 256, 256, 0, 18);

                    #endregion
                });
            }
        }

        public Command ShowListCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if ((_pins == null) || !_pins.Any())
                    {
                        await CoreMethods.DisplayAlert("Nothing there!", "No pins to show!", "OK");
                        return;
                    }
                    var listPage = new PinListPage(Pins);
                    listPage.PinSelected += async (o, e) =>
                    {
                        SelectedPin = e.Pin;
                        await Application.Current.MainPage.Navigation.PopAsync();
                    };
                    await Application.Current.MainPage.Navigation.PushAsync(listPage);
                });
            }
        }

        /// <summary>
        ///     Map region bound to <see cref="TKCustomMap" />
        /// </summary>
        public MapSpan MapRegion
        {
            get { return _mapRegion; }
            set
            {
                if (_mapRegion != value)
                    _mapRegion = value;
            }
        }

        /// <summary>
        ///     Pins bound to the <see cref="TKCustomMap" />
        /// </summary>
        public ObservableCollection<TKCustomMapPin> Pins
        {
            get { return _pins; }
            set
            {
                if (_pins != value)
                    _pins = value;
            }
        }

        /// <summary>
        ///     Routes bound to the <see cref="TKCustomMap" />
        /// </summary>
        public ObservableCollection<TKRoute> Routes
        {
            get { return _routes; }
            set
            {
                if (_routes != value)
                    _routes = value;
            }
        }

        /// <summary>
        ///     Circles bound to the <see cref="TKCustomMap" />
        /// </summary>
        public ObservableCollection<TKCircle> Circles
        {
            get { return _circles; }
            set
            {
                if (_circles != value)
                    _circles = value;
            }
        }

        /// <summary>
        ///     Lines bound to the <see cref="TKCustomMap" />
        /// </summary>
        public ObservableCollection<TKPolyline> Lines
        {
            get { return _lines; }
            set
            {
                if (_lines != value)
                    _lines = value;
            }
        }

        /// <summary>
        ///     Polygons bound to the <see cref="TKCustomMap" />
        /// </summary>
        public ObservableCollection<TKPolygon> Polygons
        {
            get { return _polygons; }
            set
            {
                if (_polygons != value)
                    _polygons = value;
            }
        }

        /// <summary>
        ///     Map center bound to the <see cref="TKCustomMap" />
        /// </summary>
        public Position MapCenter
        {
            get { return _mapCenter; }
            set
            {
                if (_mapCenter != value)
                    _mapCenter = value;
            }
        }

        /// <summary>
        ///     Selected pin bound to the <see cref="TKCustomMap" />
        /// </summary>
        public TKCustomMapPin SelectedPin
        {
            get { return _selectedPin; }
            set
            {
                if (_selectedPin != value)
                    _selectedPin = value;
            }
        }

        /// <summary>
        ///     Map Long Press bound to the <see cref="TKCustomMap" />
        /// </summary>
        public Command<Position> MapLongPressCommand
        {
            get
            {
                return new Command<Position>(async position =>
                {
                    var action = await CoreMethods.DisplayActionSheet(
                        "Long Press",
                        "Cancel",
                        null,
                        "Add Pin",
                        "Add Circle");

                    if (action == "Add Pin")
                    {
                        var pin = new TKCustomMapPin
                        {
                            Position = position,
                            Title = $"Pin {position.Latitude}, {position.Longitude}",
                            ShowCallout = true,
                            IsDraggable = true
                        };
                        _pins.Add(pin);
                    }
                    else if (action == "Add Circle")
                    {
                        var circle = new TKCircle
                        {
                            Center = position,
                            Radius = 10000,
                            Color = Color.FromRgba(100, 0, 0, 80)
                        };
                        _circles.Add(circle);
                    }
                });
            }
        }

        /// <summary>
        ///     Map Clicked bound to the <see cref="TKCustomMap" />
        /// </summary>
        public Command<Position> MapClickedCommand
        {
            get
            {
                return new Command<Position>(positon =>
                {
                    SelectedPin = null;

                    // Determine if a point was inside a circle
                    if (
                    (from c in _circles
                        let distanceInMeters = c.Center.DistanceTo(positon)*1000
                        where distanceInMeters <= c.Radius
                        select c).Any())
                        CoreMethods.DisplayAlert("Circle tap", "Circle was tapped", "OK");
                });
            }
        }

        /// <summary>
        ///     Command when a place got selected
        /// </summary>
        public Command<IPlaceResult> PlaceSelectedCommand
        {
            get
            {
                return new Command<IPlaceResult>(async p =>
                {
                    var gmsResult = p as GmsPlacePrediction;
                    if (gmsResult != null)
                    {
                        var details = await GmsPlace.Instance.GetDetails(gmsResult.PlaceId);
                        MapCenter = new Position(details.Item.Geometry.Location.Latitude,
                            details.Item.Geometry.Location.Longitude);
                        return;
                    }
                    var osmResult = p as OsmNominatimResult;
                    if (osmResult != null)
                    {
                        MapCenter = new Position(osmResult.Latitude, osmResult.Longitude);
                        return;
                    }

                    if (Device.OS == TargetPlatform.Android)
                    {
                        var prediction = (TKNativeAndroidPlaceResult) p;

                        var details = await TKNativePlacesApi.Instance.GetDetails(prediction.PlaceId);

                        MapCenter = details.Coordinate;
                    }
                    else if (Device.OS == TargetPlatform.iOS)
                    {
                        var prediction = (TKNativeiOSPlaceResult) p;

                        MapCenter = prediction.Details.Coordinate;
                    }
                });
            }
        }

        /// <summary>
        ///     Pin Selected bound to the <see cref="TKCustomMap" />
        /// </summary>
        public Command PinSelectedCommand
        {
            get
            {
                return new Command(() =>
                {
                    // Chose one

                    // 1. First possibility
                    //this.MapCenter = this.SelectedPin.Position;
                    // 2. Possibility
                    MapRegion = MapSpan.FromCenterAndRadius(SelectedPin.Position, MapRegion.Radius);
                    // 3. Possibility
                    //this.MapFunctions.MoveToMapRegion(
                    //    MapSpan.FromCenterAndRadius(this.SelectedPin.Position, Distance.FromMeters(this.MapRegion.Radius.Meters)),
                    //    true);
                });
            }
        }

        /// <summary>
        ///     Drag End bound to the <see cref="TKCustomMap" />
        /// </summary>
        public Command<TKCustomMapPin> DragEndCommand
        {
            get
            {
                return new Command<TKCustomMapPin>(pin =>
                {
                    var routePin = pin as RoutePin;

                    if (routePin != null)
                        if (routePin.IsSource)
                            routePin.Route.Source = pin.Position;
                        else
                            routePin.Route.Destination = pin.Position;
                });
            }
        }

        /// <summary>
        ///     Route clicked bound to the <see cref="TKCustomMap" />
        /// </summary>
        public Command<TKRoute> RouteClickedCommand
        {
            get
            {
                return new Command<TKRoute>(async r =>
                {
                    var action = await Application.Current.MainPage.DisplayActionSheet(
                        "Route tapped",
                        "Cancel",
                        null,
                        "Show Instructions");

                    if (action == "Show Instructions")
                        await CoreMethods.PushPageModel<HtmlInstructionsPageModel>(r);
                });
            }
        }

        /// <summary>
        ///     Callout clicked bound to the <see cref="TKCustomMap" />
        /// </summary>
        public Command CalloutClickedCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var action = await CoreMethods.DisplayActionSheet(
                        "Callout clicked",
                        "Cancel",
                        "Remove Pin");

                    if (action == "Remove Pin")
                        _pins.Remove(SelectedPin);
                });
            }
        }

        public Command ClearMapCommand
        {
            get
            {
                return new Command(() =>
                {
                    _pins.Clear();
                    _circles.Clear();
                    if (_routes != null)
                        _routes.Clear();
                });
            }
        }

        /// <summary>
        ///     Navigate to a new page to get route source/destination
        /// </summary>
        public Command AddRouteCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (Routes == null) Routes = new ObservableCollection<TKRoute>();

                    //var addRoutePage = new AddRoutePage(this.Routes, this.Pins, this.MapRegion);
                    var x =
                        new Tuple<ObservableCollection<TKRoute>, ObservableCollection<TKCustomMapPin>, MapSpan>(Routes,
                            Pins, MapRegion);
                    await CoreMethods.PushPageModel<AddRoutePageModel>(x);
                    //Application.Current.MainPage.Navigation.PushAsync(addRoutePage);
                });
            }
        }

        private async void GetLocation()
        {
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync(50000);
            _mapCenter = new Position(position.Latitude, position.Longitude);
        }

        //}
        //    }
        //        });
        //            this.MapRegion = r.Bounds;
        //            // move to the bounds of the route
        //        {
        //        return new Command<TKRoute>(r =>
        //    {
        //    get
        //{
        //public Command<TKRoute> RouteCalculationFinishedCommand
        ///// </summary>
        ///// Command when a route calculation finished
        ///// <summary>
    }
}