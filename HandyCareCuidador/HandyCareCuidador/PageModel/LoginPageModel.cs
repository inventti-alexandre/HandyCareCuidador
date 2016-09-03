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
        public HorarioViewModel oHorarioViewModel { get; set; }

        public Command GoogleLoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (App.Authenticator != null)
                        {
                            authenticated =
                                await App.Authenticator.Authenticate(MobileServiceAuthenticationProvider.Google);
                        }
                        if (authenticated)
                        {
                            Application.Current.Properties["UserId"] =
                                CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId;
                            Application.Current.Properties["Token"] =
                                CuidadorRestService.DefaultManager.CurrentClient.CurrentUser
                                    .MobileServiceAuthenticationToken;
                            App.authenticated = true;
                            oHorarioViewModel.Visualizar = false;
                            oHorarioViewModel.ActivityRunning = true;
                            Cuidador =
                                await CuidadorRestService.DefaultManager.ProcurarCuidadorAsync(CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId, MobileServiceAuthenticationProvider.Google);
                            if (Cuidador != null)
                            {
                                App.Afazeres = new ObservableCollection<Afazer>();
                                app.AbrirMainMenu(Cuidador);
                                await App.GetAfazeres(true);
                            }
                            else
                            {
                                app.NewCuidador();
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
                        {
                            authenticated =
                                await App.Authenticator.Authenticate(MobileServiceAuthenticationProvider.Google);
                        }
                        if (authenticated)
                        {
                            Application.Current.Properties["UserId"] =
                                CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId;
                            Application.Current.Properties["Token"] =
                                CuidadorRestService.DefaultManager.CurrentClient.CurrentUser
                                    .MobileServiceAuthenticationToken;
                            App.authenticated = true;
                            oHorarioViewModel.Visualizar = false;
                            oHorarioViewModel.ActivityRunning = true;
                            Cuidador =
                                await CuidadorRestService.DefaultManager.ProcurarCuidadorAsync(CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId, MobileServiceAuthenticationProvider.Google);
                            if (Cuidador != null)
                            {
                                App.Afazeres = new ObservableCollection<Afazer>();
                                app.AbrirMainMenu(Cuidador);
                                await App.GetAfazeres(true);
                            }
                            else
                            {
                                app.NewCuidador();
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
                            App.authenticated = true;
                            oHorarioViewModel.Visualizar = false;
                            oHorarioViewModel.ActivityRunning = true;
                            Cuidador =
                                await CuidadorRestService.DefaultManager.ProcurarCuidadorAsync(CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId, MobileServiceAuthenticationProvider.Facebook, true);
                            if (Cuidador != null)
                                app.AbrirMainMenu(Cuidador);
                            else
                            {
                                App.Afazeres = new ObservableCollection<Afazer>();
                                app.AbrirMainMenu(Cuidador);
                                await App.GetAfazeres(true);
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
                            App.authenticated = true;
                            oHorarioViewModel.Visualizar = false;
                            oHorarioViewModel.ActivityRunning = true;
                            Cuidador =
                                await CuidadorRestService.DefaultManager.ProcurarCuidadorAsync(CuidadorRestService.DefaultManager.CurrentClient.CurrentUser.UserId, MobileServiceAuthenticationProvider.MicrosoftAccount, true);
                            if (Cuidador != null)
                                app.AbrirMainMenu(Cuidador);
                            else
                            {
                                App.Afazeres = new ObservableCollection<Afazer>();
                                app.AbrirMainMenu(Cuidador);
                                await App.GetAfazeres(true);
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
            oHorarioViewModel = new HorarioViewModel {Visualizar = true, ActivityRunning = false};
            if (authenticated)
            {
                // Hide the Sign-in button.
                oHorarioViewModel.Visualizar = false;
            }
        }
    }
}