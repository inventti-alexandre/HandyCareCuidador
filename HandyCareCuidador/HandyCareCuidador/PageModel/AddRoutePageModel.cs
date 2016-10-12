using System;
using System.Collections.ObjectModel;
using FreshMvvm;
using HandyCareCuidador.CustomPins;
using PropertyChanged;
using TK.CustomMap;
using TK.CustomMap.Api;
using TK.CustomMap.Overlays;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace HandyCareCuidador.PageModel
{
    [ImplementPropertyChanged]
    public class AddRoutePageModel : FreshBasePageModel
    {
        private Position _from, _to;

        //public AddRoutePageModel(ObservableCollection<TKRoute> routes, ObservableCollection<TKCustomMapPin> pins, MapSpan bounds)
        //{
        //    this.Routes = routes;
        //    this.Pins = pins;
        //    this.Bounds = bounds;
        //}

        private IPlaceResult _fromPlace, _toPlace;

        public ObservableCollection<TKCustomMapPin> Pins { get; private set; }
        public ObservableCollection<TKRoute> Routes { get; private set; }
        public MapSpan Bounds { get; private set; }

        public Command<IPlaceResult> FromSelectedCommand
        {
            get
            {
                return new Command<IPlaceResult>(async p =>
                {
                    if (Device.OS == TargetPlatform.iOS)
                    {
                        var placeResult = (TKNativeiOSPlaceResult) p;
                        _fromPlace = placeResult;
                        _from = placeResult.Details.Coordinate;
                    }
                    else
                    {
                        var placeResult = (TKNativeAndroidPlaceResult) p;
                        _fromPlace = placeResult;
                        var details = await TKNativePlacesApi.Instance.GetDetails(placeResult.PlaceId);

                        _from = details.Coordinate;
                    }
                });
            }
        }

        public Command<IPlaceResult> ToSelectedCommand
        {
            get
            {
                return new Command<IPlaceResult>(async p =>
                {
                    if (Device.OS == TargetPlatform.iOS)
                    {
                        var placeResult = (TKNativeiOSPlaceResult) p;
                        _toPlace = placeResult;
                        _to = placeResult.Details.Coordinate;
                    }
                    else
                    {
                        var placeResult = (TKNativeAndroidPlaceResult) p;
                        _toPlace = placeResult;
                        var details = await TKNativePlacesApi.Instance.GetDetails(placeResult.PlaceId);

                        _to = details.Coordinate;
                    }
                });
            }
        }

        public Command AddRouteCommand
        {
            get
            {
                return new Command(() =>
                {
                    if ((_toPlace == null) || (_fromPlace == null)) return;

                    var route = new TKRoute
                    {
                        TravelMode = TKRouteTravelMode.Driving,
                        Source = _from,
                        Destination = _to,
                        Color = Color.Blue
                    };

                    Pins.Add(new RoutePin
                    {
                        Route = route,
                        IsSource = true,
                        IsDraggable = true,
                        Position = _from,
                        Title = _fromPlace.Description,
                        ShowCallout = true,
                        DefaultPinColor = Color.Green
                    });
                    Pins.Add(new RoutePin
                    {
                        Route = route,
                        IsSource = false,
                        IsDraggable = true,
                        Position = _to,
                        Title = _toPlace.Description,
                        ShowCallout = true,
                        DefaultPinColor = Color.Red
                    });

                    Routes.Add(route);

                    Application.Current.MainPage.Navigation.PopAsync();
                });
            }
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            var x = initData as Tuple<ObservableCollection<TKRoute>, ObservableCollection<TKCustomMapPin>, MapSpan>;
            if (x != null)
            {
                Routes = x.Item1;
                Pins = x.Item2;
                Bounds = x.Item3;
            }
        }
    }
}