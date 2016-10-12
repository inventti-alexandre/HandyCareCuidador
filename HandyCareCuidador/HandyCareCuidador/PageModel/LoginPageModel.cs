using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private bool authenticated;
        public Cuidador Cuidador { get; set; }
        public PageModelHelper oHorarioViewModel { get; set; }

        public Command GoogleLoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (App.Authenticator != null)
                            authenticated =
                                await App.Authenticator.Authenticate(MobileServiceAuthenticationProvider.Google);
                        if (authenticated)
                        {
                            Application.Current.Properties["UserId"] =
                                CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId;
                            Application.Current.Properties["Token"] =
                                CuidadorRestService.DefaultManager.CurrentClient.CurrentUser
                                    .MobileServiceAuthenticationToken;
                            App.Authenticated = true;
                            oHorarioViewModel.Visualizar = false;
                            oHorarioViewModel.ActivityRunning = true;
                            Cuidador =
                                await
                                    CuidadorRestService.DefaultManager.ProcurarCuidadorAsync(
                                        CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId,
                                        MobileServiceAuthenticationProvider.Google);
                            if (Cuidador != null)
                            {
                                App.Afazeres = new ObservableCollection<Afazer>();
                                app.AbrirMainMenu(Cuidador);
                                await App.GetAfazeres(true);
                            }
                            else
                            {
                                var _cuidador = new Cuidador
                                {
                                    CuiGoogleId = CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId
                                };
                                app.NewCuidador(_cuidador, app);
                            }
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

        public Command LoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (App.Authenticator != null)
                            authenticated =
                                await App.Authenticator.Authenticate(MobileServiceAuthenticationProvider.Google);
                        if (authenticated)
                        {
                            Application.Current.Properties["UserId"] =
                                CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId;
                            Application.Current.Properties["Token"] =
                                CuidadorRestService.DefaultManager.CurrentClient.CurrentUser
                                    .MobileServiceAuthenticationToken;
                            App.Authenticated = true;
                            oHorarioViewModel.Visualizar = false;
                            oHorarioViewModel.ActivityRunning = true;
                            Cuidador =
                                await
                                    CuidadorRestService.DefaultManager.ProcurarCuidadorAsync(
                                        CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId,
                                        MobileServiceAuthenticationProvider.Google);
                            if (Cuidador != null)
                            {
                                App.Afazeres = new ObservableCollection<Afazer>();
                                app.AbrirMainMenu(Cuidador);
                                await App.GetAfazeres(true);
                            }
                            else
                            {
                                var _cuidador = new Cuidador
                                {
                                    CuiMicrosoftId = CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId
                                };
                                app.NewCuidador(_cuidador, app);
                            }
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
                            authenticated =
                                await App.Authenticator.Authenticate(MobileServiceAuthenticationProvider.Facebook);
                        if (authenticated)
                        {
                            Application.Current.Properties["UserId"] =
                                CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId;
                            Application.Current.Properties["Token"] =
                                CuidadorRestService.DefaultManager.CurrentClient.CurrentUser
                                    .MobileServiceAuthenticationToken;
                            App.Authenticated = true;
                            oHorarioViewModel.Visualizar = false;
                            oHorarioViewModel.ActivityRunning = true;
                            Cuidador =
                                await
                                    CuidadorRestService.DefaultManager.ProcurarCuidadorAsync(
                                        CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId,
                                        MobileServiceAuthenticationProvider.Facebook, true);
                            if (Cuidador != null)
                            {
                                App.Afazeres = new ObservableCollection<Afazer>();
                                app.AbrirMainMenu(Cuidador);
                                await App.GetAfazeres(true);
                            }
                            else
                            {
                                var _cuidador = new Cuidador
                                {
                                    CuiFacebookId = CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId
                                };
                                app.NewCuidador(_cuidador, app);
                            }
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
                            authenticated =
                                await
                                    App.Authenticator.Authenticate(MobileServiceAuthenticationProvider.MicrosoftAccount);
                        if (authenticated)
                        {
                            Application.Current.Properties["UserId"] =
                                CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId;
                            Application.Current.Properties["Token"] =
                                CuidadorRestService.DefaultManager.CurrentClient.CurrentUser
                                    .MobileServiceAuthenticationToken;
                            App.Authenticated = true;
                            oHorarioViewModel.Visualizar = false;
                            oHorarioViewModel.ActivityRunning = true;
                            Cuidador =
                                await
                                    CuidadorRestService.DefaultManager.ProcurarCuidadorAsync(
                                        CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId,
                                        MobileServiceAuthenticationProvider.MicrosoftAccount, true);
                            if (Cuidador != null)
                            {
                                App.Afazeres = new ObservableCollection<Afazer>();
                                app.AbrirMainMenu(Cuidador);
                                await App.GetAfazeres(true);
                            }
                            else
                            {
                                var _cuidador = new Cuidador
                                {
                                    CuiMicrosoftId = CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId
                                };
                                app.NewCuidador(_cuidador, app);
                            }
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

        public Command TwitterLoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (App.Authenticator != null)
                            authenticated =
                                await
                                    App.Authenticator.Authenticate(MobileServiceAuthenticationProvider.Twitter);
                        if (authenticated)
                        {
                            Application.Current.Properties["UserId"] =
                                CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId;
                            Application.Current.Properties["Token"] =
                                CuidadorRestService.DefaultManager.CurrentClient.CurrentUser
                                    .MobileServiceAuthenticationToken;
                            App.Authenticated = true;
                            oHorarioViewModel.Visualizar = false;
                            oHorarioViewModel.ActivityRunning = true;
                            Cuidador =
                                await
                                    CuidadorRestService.DefaultManager.ProcurarCuidadorAsync(
                                        CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId,
                                        MobileServiceAuthenticationProvider.Twitter, true);
                            if (Cuidador != null)
                            {
                                App.Afazeres = new ObservableCollection<Afazer>();
                                app.AbrirMainMenu(Cuidador);
                                await App.GetAfazeres(true);
                            }
                            else
                            {
                                var _cuidador = new Cuidador
                                {
                                    CuiTwitterId = CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId
                                };
                                app.NewCuidador(_cuidador, app);
                            }
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

        public Command AzureAdLoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (App.Authenticator != null)
                            authenticated =
                                await
                                    App.Authenticator.Authenticate(
                                        MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);
                        if (authenticated)
                        {
                            Application.Current.Properties["UserId"] =
                                CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId;
                            Application.Current.Properties["Token"] =
                                CuidadorRestService.DefaultManager.CurrentClient.CurrentUser
                                    .MobileServiceAuthenticationToken;
                            App.Authenticated = true;
                            oHorarioViewModel.Visualizar = false;
                            oHorarioViewModel.ActivityRunning = true;
                            Cuidador =
                                await
                                    CuidadorRestService.DefaultManager.ProcurarCuidadorAsync(
                                        CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId,
                                        MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory, true);
                            if (Cuidador != null)
                            {
                                App.Afazeres = new ObservableCollection<Afazer>();
                                app.AbrirMainMenu(Cuidador);
                                await App.GetAfazeres(true);
                            }
                            else
                            {
                                var _cuidador = new Cuidador
                                {
                                    CuiMicrosoftAdId =
                                        CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId
                                };
                                app.NewCuidador(_cuidador, app);
                            }
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

        public override void Init(object initData)
        {
            base.Init(initData);
            ImageSource.FromFile(@"splash.png");
            app = initData as App;
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            Cuidador = new Cuidador();
            oHorarioViewModel = new PageModelHelper {Visualizar = true, ActivityRunning = false};
            if (authenticated)
                oHorarioViewModel.Visualizar = false;
        }
    }
}