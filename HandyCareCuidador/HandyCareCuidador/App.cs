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
using Xamarin.Forms;

namespace HandyCareCuidador
{
    public class App : Application
    {
        public App()
        {
            Register();
            /*var mainPage = new FreshTabbedNavigationContainer("Handy Care");
            mainPage.AddTab<ListaAfazerPageModel>("Afazeres", null, null);
            mainPage.AddTab<ListaAfazerConcluidoPageModel>("Afazeres concluídos",null,null);
            mainPage.AddTab<ListaMaterialPageModel>("Materiais", null, null);
            mainPage.AddTab<ListaMedicamentoPageModel>("Medicamentos", null, null);*/
            var page = FreshPageModelResolver.ResolvePageModel<MainMenuPageModel>();
            var mainPage = new FreshNavigationContainer(page);
            MainPage = mainPage;
        }
        private void Register()
        {
            FreshIOC.Container.Register<IAfazerRestService, AfazerRestService>();
            FreshIOC.Container.Register<IMaterialRestService, MaterialRestService>();
            FreshIOC.Container.Register<IMedicamentoRestService, MedicamentoRestService>();
            FreshIOC.Container.Register<IMedicamentoAdministradoRestService, MedicamentoAdministradoRestService>();
            FreshIOC.Container.Register<IMaterialUtilizadoRestService, MaterialUtilizadoRestService>();
            FreshIOC.Container.Register<IConclusaoAfazerRestService, ConclusaoAfazerRestService>();
            FreshIOC.Container.Register<IPacienteRestService, PacienteRestService>();
            FreshIOC.Container.Register<ICuidadorPacienteRestService, CuidadorPacienteRestService>();
        }
        public void LoadTabs()
        {
            var mainPage = new FreshTabbedNavigationContainer();
            mainPage.AddTab<ListaAfazerPageModel>("Afazeres", null, null);
            mainPage.AddTab<ListaAfazerConcluidoPageModel>("Concluído", null, null);
            MainPage = mainPage;
        }
        protected override void OnStart()
        {
            // Handle when your app starts
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
