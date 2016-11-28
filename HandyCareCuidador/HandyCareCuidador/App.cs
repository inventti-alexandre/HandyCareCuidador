using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using FreshMvvm;
using HandyCareCuidador.Data;
using HandyCareCuidador.Interface;
using HandyCareCuidador.Model;
using HandyCareCuidador.PageModel;
using Microsoft.WindowsAzure.MobileServices;
using Octane.Xam.VideoPlayer;
using Rox;
using Syncfusion.SfCalendar.XForms;
using Syncfusion.SfSchedule.XForms;
using TK.CustomMap.Api.Google;

using Xamarin.Forms;
using ZXing.Mobile;

namespace HandyCareCuidador
{
    public class App : Application
    {
        //static ILoginManager loginManager;
        public static bool Authenticated;
        private readonly Image image = new Image();

        public App()
        {
            Register();
            GmsDirection.Init("AIzaSyASYVBniofTez5ZkWBEc1-3EEby_bZeRJk");
            var page = FreshPageModelResolver.ResolvePageModel<LoginPageModel>(this);
            var mainPage = new FreshNavigationContainer(page);
            MainPage = mainPage;


        }

        public static IAuthenticate Authenticator { get; private set; }
        public static ObservableCollection<Afazer> Afazeres { get; set; }
        public static ObservableCollection<ConclusaoAfazer> AfazeresConcluidos { get; set; }
        public event Action TakePicture = () => { };
        public event Action RecordVideo = () => { };

        public void PictureEventHandler()
        {
            TakePictureMethod();
        }

        private void TakePictureMethod()
        {
            var handler = TakePicture;
            handler?.Invoke();
        }

        public void VideoEventHandler()
        {
            RecordVideoMethod();
        }

        private void RecordVideoMethod()
        {
            var handler = RecordVideo;
            handler?.Invoke();
        }

        public void ShowImage(string filepath)
        {
            image.Source = ImageSource.FromFile(filepath);
        }

        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }

        public static async Task GetAfazeres(bool sync)
        {
            try
            {
                await Task.Run(async () =>
                {
                    AfazeresConcluidos =
                        new ObservableCollection<ConclusaoAfazer>(
                            await CuidadorRestService.DefaultManager.RefreshConclusaoAfazerAsync(sync));

                    var selection =
                        new ObservableCollection<Afazer>(
                            await CuidadorRestService.DefaultManager.RefreshAfazerAsync(sync));
                    if ((selection.Count > 0) && (AfazeresConcluidos.Count > 0))
                    {
                        var pacresult =
                            new ObservableCollection<CuidadorPaciente>(
                                    await CuidadorRestService.DefaultManager.RefreshCuidadorPacienteAsync(sync))
                                .AsEnumerable();
                        var result = selection.Where(e => !AfazeresConcluidos.Select(m => m.ConAfazer)
                                .Contains(e.Id))
                            .Where(e => pacresult.Select(m => m.Id).Contains(e.AfaPaciente))
                            .AsEnumerable();
                        Afazeres = new ObservableCollection<Afazer>(result);
                    }
                    else
                    {
                        Afazeres = new ObservableCollection<Afazer>(selection);
                    }
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AbrirMainMenu(Cuidador cuidador)
        {
            var page = FreshPageModelResolver.ResolvePageModel<MainMenuPageModel>(cuidador);
            var mainPage = new FreshNavigationContainer(page);
            MainPage = mainPage;
        }

        public void NewCuidador(Cuidador cuidador, App app)
        {
            var x = new Tuple<Cuidador, App>(cuidador, app);
            var page = FreshPageModelResolver.ResolvePageModel<CuidadorPageModel>(x);
            var mainPage = new FreshNavigationContainer(page);
            MainPage = mainPage;
        }

        private void Register()
        {
            FreshIOC.Container.Register<ICuidadorRestService, CuidadorRestService>();
            FreshIOC.Container.Register<IUserDialogs>(UserDialogs.Instance);
            FreshIOC.Container.Register<MobileBarcodeScanner>(new MobileBarcodeScanner());
            FreshIOC.Container.Register<SfSchedule>(new SfSchedule());
            FreshIOC.Container.Register<SfCalendar>(new SfCalendar());
            //FreshIOC.Container.Register<INfcForms, NfcForms>();

            //FreshIOC.Container.Resolve<VideoView>();
            //FreshIOC.Container.Register<VideoPlayer>(new VideoPlayer());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }


        protected override async void OnSleep()
        {
            if (Authenticated)
                await Task.Run(() =>
                {
                    if (CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId != null)
                        Current.Properties["UserId"] =
                            CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId;
                    if (CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.MobileServiceAuthenticationToken !=
                        null)
                        Current.Properties["Token"] =
                            CuidadorRestService.DefaultManager.CurrentClient.CurrentUser
                                .MobileServiceAuthenticationToken;
                    Debug.WriteLine("OnSleeping");
                });
        }

        protected override async void OnResume()
        {
            if (Authenticated)
                await Task.Run(() =>
                {
                    if (Properties.ContainsKey("UserID"))
                        CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId =
                            (string) Properties["UserId"];
                    if (Properties.ContainsKey("Token"))
                        CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.MobileServiceAuthenticationToken =
                            (string) Properties["Token"];
                    Debug.WriteLine("OnResuming");
                });
        }

        public interface IAuthenticate
        {
            Task<bool> Authenticate(MobileServiceAuthenticationProvider provider);
        }
    }
}