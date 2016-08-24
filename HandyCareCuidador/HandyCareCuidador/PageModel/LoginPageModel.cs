using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareCuidador.Data;
using HandyCareCuidador.Helper;
using HandyCareCuidador.Model;
using Microsoft.WindowsAzure.MobileServices;
using PropertyChanged;
using Xamarin.Forms;

namespace HandyCareCuidador.PageModel
{
    [ImplementPropertyChanged]
    public class LoginPageModel : FreshBasePageModel
    {
        private App app;
        private bool authenticated = false;
        public Cuidador Cuidador { get; set; }
        private HorarioViewModel HorarioViewModel { get; set; }
        public override void Init(object initData)
        {
            base.Init(initData);
            ImageSource.FromFile(@"splash.png");
            app = initData as App;
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            Cuidador=new Cuidador();
            HorarioViewModel = new HorarioViewModel {Visualizar = true};
            if (authenticated == true)
            {
                // Hide the Sign-in button.
                HorarioViewModel.Visualizar = false;
            }
        }
        public Command GoogleLoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (App.Authenticator != null)
                            authenticated = await App.Authenticator.Authenticate(MobileServiceAuthenticationProvider.Google);

                        // Set syncItems to true to synchronize the data on startup when offline is enabled.
                        if (authenticated)
                        {
                            Cuidador =
                                await CuidadorRestService.DefaultManager.ProcurarCuidadorAsync(
                                    CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId, MobileServiceAuthenticationProvider.Google);
                            if(Cuidador!=null)
                            app.AbrirMainMenu(Cuidador.Id);
                            else
                            {
                                app.NewCuidador();
                            }
                            //await CoreMethods.PushPageModel<MainMenuPageModel>();

                        }
                    }
                    catch (InvalidOperationException e)
                    {
                         Debug.WriteLine(e.Message);
                        throw;
                    }

                });
            }
        }
        public Command FacebookLoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (App.Authenticator != null)
                            authenticated = await App.Authenticator.Authenticate(MobileServiceAuthenticationProvider.Facebook);

                        // Set syncItems to true to synchronize the data on startup when offline is enabled.
                        if (authenticated)
                        {
                            Cuidador =
                                await CuidadorRestService.DefaultManager.ProcurarCuidadorAsync(
                                    CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId, MobileServiceAuthenticationProvider.Facebook);
                            if (Cuidador != null)
                                app.AbrirMainMenu(Cuidador.Id);
                            else
                            {
                                app.NewCuidador();
                            }
                            //await CoreMethods.PushPageModel<MainMenuPageModel>();

                        }
                    }
                    catch (InvalidOperationException e)
                    {
                        Debug.WriteLine(e.Message);
                        throw;
                    }

                });
            }
        }
        public Command MicrosoftLoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (App.Authenticator != null)
                            authenticated = await App.Authenticator.Authenticate(MobileServiceAuthenticationProvider.MicrosoftAccount);

                        // Set syncItems to true to synchronize the data on startup when offline is enabled.
                        if (authenticated)
                        {
                            Cuidador =
                                await CuidadorRestService.DefaultManager.ProcurarCuidadorAsync(
                                    CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId, MobileServiceAuthenticationProvider.MicrosoftAccount);
                            if (Cuidador != null)
                                app.AbrirMainMenu(Cuidador.Id);
                            else
                            {
                                app.NewCuidador();
                            }
                            //await CoreMethods.PushPageModel<MainMenuPageModel>();

                        }
                    }
                    catch (InvalidOperationException e)
                    {
                        Debug.WriteLine(e.Message);
                        throw;
                    }

                });
            }
        }
    }
}
