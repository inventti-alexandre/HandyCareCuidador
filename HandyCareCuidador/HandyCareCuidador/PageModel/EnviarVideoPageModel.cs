using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareCuidador.Data;
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
        public override void Init(object initData)
        {
            base.Init(initData);
            var x = initData as Tuple<Video, Familiar, Image>;
            Familiar = new Familiar();
            Video = new Video();
            VideoPaciente = new StreamImageSource();
            if (x != null)
            {

                Familiar = x.Item2;
                Video = x.Item1;
                VideoPaciente = (ImageSource)x.Item3.Source;
            }
        }
        private bool _AutoPlay = false;
        public bool AutoPlay
        {
            get
            {
                return _AutoPlay;
            }
            set
            {
                _AutoPlay = value;

            }
        }

        private bool _LoopPlay = false;
        public bool LoopPlay
        {
            get
            {
                return _LoopPlay;
            }
            set
            {
                _LoopPlay = value;

            }
        }

        private bool _ShowController = false;
        public bool ShowController
        {
            get
            {
                return _ShowController;
            }
            set
            {
                _ShowController = value;

            }
        }

        private bool _Muted = false;
        public bool Muted
        {
            get
            {
                return _Muted;
            }
            set
            {
                _Muted = value;

            }
        }

        private double _Volume = 1;
        public double Volume
        {
            get
            {
                return _Volume;
            }
            set
            {
                _Volume = value;
            }
        }

        public double SliderVolume
        {
            get
            {
                return _Volume * 100;
            }
            set
            {
                try
                {
                    _Volume = value / 100;
                }
                catch
                {
                    _Volume = 0;
                }
            }
        }

        private string _LabelVideoStatus;
        public string LabelVideoStatus
        {
            get
            {
                return _LabelVideoStatus;
            }
        }
        public ImageSource VideoSource
        {
            get
            {
                ImageSource videoSource = null;
                //try
                //{
                //    var imageSourceConverter = new ImageSourceConverter();
                //    videoSource = (ImageSource)imageSourceConverter.ConvertFromInvariantString(_Source);
                //}
                //catch
                //{

                //}
                return videoSource;
            }
        }
        public Command EnviarCommand
        {
            get
            {
                return new Command(async () =>
                {
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
