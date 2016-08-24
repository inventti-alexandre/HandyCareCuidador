using FreshMvvm;
using HandyCareCuidador.Data;
using HandyCareCuidador.Interface;
using HandyCareCuidador.Page;
using HandyCareCuidador.PageModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;

namespace HandyCareCuidador
{
    public class App : Application
    {
        //static ILoginManager loginManager;
        public interface IAuthenticate
        {
            Task<bool> Authenticate(MobileServiceAuthenticationProvider provider);
        }
        public static IAuthenticate Authenticator { get; private set; }

        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }
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

        public void AbrirMainMenu(string id)
        {
            var page = FreshPageModelResolver.ResolvePageModel<MainMenuPageModel>(id);
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
        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
