using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareCuidador.Helper;
using PropertyChanged;

namespace HandyCareCuidador.PageModel
{
    [ImplementPropertyChanged]
    public class CameraPageModel:FreshBasePageModel
    {
        public PageModelHelper PageModelHelper { get; set; }
        public override void Init(object initData)
        {
            base.Init(initData);
            PageModelHelper = new PageModelHelper {BoasVindas = "http://192.168.0.106/mjpg/video.mjpg"};
        }
    }
}
