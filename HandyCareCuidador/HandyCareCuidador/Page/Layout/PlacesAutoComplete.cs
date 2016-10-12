using System;
using System.Collections.Generic;
using System.Linq;
using TK.CustomMap.Api;
using TK.CustomMap.Api.Google;
using TK.CustomMap.Api.OSM;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using static Xamarin.Forms.BindableProperty;

namespace HandyCareCuidador.Page.Layout
{
    public class PlacesAutoComplete : RelativeLayout
    {
        // TODO: SUMMARIES
        public enum PlacesApi
        {
            Google,
            Osm,
            Native
        }

        public static readonly BindableProperty BoundsProperty = Create<PlacesAutoComplete, MapSpan>(
            p => p.Bounds,
            default(MapSpan));

        public static readonly BindableProperty PlaceSelectedCommandProperty =
            Create<PlacesAutoComplete, Command<IPlaceResult>>(
                p => p.PlaceSelectedCommand,
                null);

        private readonly bool _useSearchBar;
        private ListView _autoCompleteListView;
        private Entry _entry;

        private IEnumerable<IPlaceResult> _predictions;

        private SearchBar _searchBar;

        private bool _textChangeItemSelected;

        public PlacesAutoComplete(bool useSearchBar)
        {
            _useSearchBar = useSearchBar;
            Init();
        }

        public PlacesAutoComplete()
        {
            _useSearchBar = true;
            Init();
        }

        public PlacesApi ApiToUse { get; set; }

        public Command<IPlaceResult> PlaceSelectedCommand
        {
            get { return (Command<IPlaceResult>) GetValue(PlaceSelectedCommandProperty); }
            set { SetValue(PlaceSelectedCommandProperty, value); }
        }

        public double HeightOfSearchBar
        {
            get { return _useSearchBar ? _searchBar.Height : _entry.Height; }
        }

        private string SearchText
        {
            get { return _useSearchBar ? _searchBar.Text : _entry.Text; }
            set
            {
                if (_useSearchBar)
                    _searchBar.Text = value;
                else
                    _entry.Text = value;
            }
        }

        public MapSpan Bounds
        {
            get { return (MapSpan) GetValue(BoundsProperty); }
            set { SetValue(BoundsProperty, value); }
        }

        public string Placeholder
        {
            get { return _useSearchBar ? _searchBar.Placeholder : _entry.Placeholder; }
            set
            {
                if (_useSearchBar)
                    _searchBar.Placeholder = value;
                else
                    _entry.Placeholder = value;
            }
        }

        private void Init()
        {
            OsmNominatim.Instance.CountryCodes.Add("de");

            _autoCompleteListView = new ListView
            {
                IsVisible = false,
                RowHeight = 40,
                HeightRequest = 0,
                BackgroundColor = Color.White
            };
            _autoCompleteListView.ItemTemplate = new DataTemplate(() =>
            {
                var cell = new TextCell();
                cell.SetBinding(TextCell.TextProperty, "Description");

                return cell;
            });

            View searchView;
            if (_useSearchBar)
            {
                _searchBar = new SearchBar
                {
                    Placeholder = "Search for address..."
                };
                _searchBar.TextChanged += SearchTextChanged;
                _searchBar.SearchButtonPressed += SearchButtonPressed;

                searchView = _searchBar;
            }
            else
            {
                _entry = new Entry
                {
                    Placeholder = "Sarch for address"
                };
                _entry.TextChanged += SearchTextChanged;

                searchView = _entry;
            }
            Children.Add(searchView,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent(l => l.Width));

            Children.Add(
                _autoCompleteListView,
                Constraint.Constant(0),
                Constraint.RelativeToView(searchView, (r, v) => v.Y + v.Height));

            _autoCompleteListView.ItemSelected += ItemSelected;

            _textChangeItemSelected = false;
        }

        private void SearchButtonPressed(object sender, EventArgs e)
        {
            if ((_predictions != null) && _predictions.Any())
                HandleItemSelected(_predictions.First());
            else
                Reset();
        }

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_textChangeItemSelected)
            {
                _textChangeItemSelected = false;
                return;
            }

            SearchPlaces();
        }

        private async void SearchPlaces()
        {
            try
            {
                if (string.IsNullOrEmpty(SearchText))
                {
                    _autoCompleteListView.ItemsSource = null;
                    _autoCompleteListView.IsVisible = false;
                    _autoCompleteListView.HeightRequest = 0;
                    return;
                }

                IEnumerable<IPlaceResult> result = null;

                if (ApiToUse == PlacesApi.Google)
                {
                    var apiResult = await GmsPlace.Instance.GetPredictions(SearchText);

                    if (apiResult != null)
                        result = apiResult.Predictions;
                }
                else if (ApiToUse == PlacesApi.Native)
                {
                    result = await TKNativePlacesApi.Instance.GetPredictions(SearchText, Bounds);
                }
                else
                {
                    result = await OsmNominatim.Instance.GetPredictions(SearchText);
                }

                if ((result != null) && result.Any())
                {
                    _predictions = result;

                    _autoCompleteListView.HeightRequest = result.Count()*40;
                    _autoCompleteListView.IsVisible = true;
                    _autoCompleteListView.ItemsSource = _predictions;
                }
                else
                {
                    _autoCompleteListView.HeightRequest = 0;
                    _autoCompleteListView.IsVisible = false;
                }
            }
            catch
            {
                // TODO
            }
        }

        private void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;
            var prediction = (IPlaceResult) e.SelectedItem;

            HandleItemSelected(prediction);
        }

        private void HandleItemSelected(IPlaceResult prediction)
        {
            if ((PlaceSelectedCommand != null) && PlaceSelectedCommand.CanExecute(this))
                PlaceSelectedCommand.Execute(prediction);

            _textChangeItemSelected = true;

            SearchText = prediction.Description;
            _autoCompleteListView.SelectedItem = null;

            Reset();
        }

        private void Reset()
        {
            _autoCompleteListView.ItemsSource = null;
            _autoCompleteListView.IsVisible = false;
            _autoCompleteListView.HeightRequest = 0;

            if (_useSearchBar)
                _searchBar.Unfocus();
            else
                _entry.Unfocus();
        }
    }
}