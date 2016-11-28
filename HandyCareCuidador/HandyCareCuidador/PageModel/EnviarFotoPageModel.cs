using System;
using System.Diagnostics;
using Acr.UserDialogs;
using FreshMvvm;
using HandyCareCuidador.Data;
using HandyCareCuidador.Helper;
using HandyCareCuidador.Model;
using Octane.Xam.VideoPlayer;
using PropertyChanged;
using Xamarin.Forms;

namespace HandyCareCuidador.PageModel
{
    [ImplementPropertyChanged]
    public class EnviarFotoPageModel : FreshBasePageModel
    {
        public Foto Foto { get; set; }
        public Familiar Familiar { get; set; }
        public FotoFamiliar FotoFamiliar { get; set; }
        public ImageSource FotoPaciente { get; set; }
        public PageModelHelper oHorario { get; set; }

        public Command EnviarCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        oHorario.Visualizar = false;
                        oHorario.ActivityRunning = true;
                        Foto.Id = Guid.NewGuid().ToString();
                        Foto.CreatedAt = DateTimeOffset.Now;
                        FotoFamiliar = new FotoFamiliar
                        {
                            FamId = Familiar.Id,
                            FotId = Foto.Id
                        };
                        await CuidadorRestService.DefaultManager.SaveFotoAsync(Foto, true);
                        await CuidadorRestService.DefaultManager.SaveFotoFamiliarAsync(FotoFamiliar, true);

                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString());
                    }
                    finally
                    {
                        await CoreMethods.PopPageModel();
                        UserDialogs.Instance.ShowSuccess("Foto enviada com sucesso", 4000);
                    }

                });
            }
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            var x = initData as Tuple<Foto, Familiar, Image>;
            Familiar = new Familiar();
            Foto = new Foto();
            oHorario = new PageModelHelper();
            if (x == null) return;
            Familiar = x.Item2;
            Foto = x.Item1;
            FotoPaciente = x.Item3.Source;
        }
    }
}