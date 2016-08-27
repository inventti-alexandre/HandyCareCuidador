using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareCuidador.Data;
using HandyCareCuidador.Interface;
using HandyCareCuidador.Model;
using HandyCareCuidador.Page;
using HandyCareCuidador.PageModel;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;

namespace HandyCareCuidador
{
    public class App : Application
    {
        //static ILoginManager loginManager;
        public static bool authenticated;

        public App()
        {
            Register();
            /*var page = FreshPageModelResolver.ResolvePageModel<MainMenuPageModel>();
            var mainPage = new FreshNavigationContainer(page);
            MainPage = mainPage;*/
            var page = FreshPageModelResolver.ResolvePageModel<LoginPageModel>(this);
            var mainPage = new FreshNavigationContainer(page);
            MainPage = mainPage;
        }

        public static IAuthenticate Authenticator { get; private set; }
        public static ObservableCollection<Afazer> Afazeres { get; set; }
        public static ObservableCollection<ConclusaoAfazer> AfazeresConcluidos { get; set; }


        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }

        public static async Task GetAfazeres(bool sync)
        {
            //INSERIR PACID EM MATERIAL E MEDICAMENTO
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
                    if (selection.Count > 0 && AfazeresConcluidos.Count > 0)
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

        public void NewCuidador()
        {
            //var page = FreshPageModelResolver.ResolvePageModel<CuidadorPageModel>();
            //var mainPage = new FreshNavigationContainer(page);
            //MainPage = mainPage;
        }

        private void Register()
        {
            FreshIOC.Container.Register<ICuidadorRestService, CuidadorRestService>();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        public void Logout()
        {
            Properties["IsLoggedIn"] = false; // only gets set to 'true' on the LoginPage
            MainPage = new LoginPage();
        }

        protected override async void OnSleep()
        {
            if (authenticated)
            {
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
        }

        protected override async void OnResume()
        {
            if (authenticated)
            {
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
            // Handle when your app resumes
        }

        public interface IAuthenticate
        {
            Task<bool> Authenticate(MobileServiceAuthenticationProvider provider);
        }
    }
}