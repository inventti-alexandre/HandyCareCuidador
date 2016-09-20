using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareCuidador.Data;
using HandyCareCuidador.Helper;
using HandyCareCuidador.Model;
using PropertyChanged;
using Xamarin.Forms;

namespace HandyCareCuidador.PageModel
{
    [ImplementPropertyChanged]
    public class EnviarVideoPageModel : FreshBasePageModel
    {
        public Video Video { get; set; }
        public Familiar Familiar { get; set; }
        public VideoFamiliar VideoFamiliar { get; set; }
        public ImageSource VideoPaciente { get; set; }
        public HorarioViewModel oHorario { get; set; }
        public override void Init(object initData)
        {
            base.Init(initData);
            var x = initData as Tuple<Video, Familiar, Image>;
            Familiar = new Familiar();
            Video = new Video();
            oHorario=new HorarioViewModel();
            VideoPaciente = new StreamImageSource();
            if (x == null) return;
            Familiar = x.Item2;
            Video = x.Item1;
            VideoPaciente = x.Item3.Source;
        }
        public Command EnviarCommand
        {
            get
            {
                return new Command(async () =>
                {
                    oHorario.Visualizar = false;
                    oHorario.ActivityRunning = true;
                    Video.Id = Guid.NewGuid().ToString();
                    VideoFamiliar = new VideoFamiliar
                    {
                        FamId = Familiar.Id,
                        VidId = Video.Id
                    };
                    await CuidadorRestService.DefaultManager.SaveVideoAsync(Video, true);
                    await CuidadorRestService.DefaultManager.SaveVideoFamiliarAsync(VideoFamiliar, true);
                    await CoreMethods.PopPageModel();
                });
            }
        }

    }
}
