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
    public class EnviarFotoPageModel:FreshBasePageModel
    {
        public Foto Foto { get; set; }
        public Familiar Familiar { get; set; }
        public FotoFamiliar FotoFamiliar { get; set; }
        public ImageSource FotoPaciente { get; set; }
        public override void Init(object initData)
        {
            base.Init(initData);
            var x = initData as Tuple<Foto,Familiar,Image>;
            Familiar = new Familiar();
            Foto = new Foto();
            if (x != null)
            {
                Familiar = x.Item2;
                Foto = x.Item1;
                FotoPaciente = x.Item3.Source;
            }
        }

        public Command EnviarCommand
        {
            get
            {
                return new Command(async () =>
                {
                    Foto.Id = Guid.NewGuid().ToString();
                    FotoFamiliar = new FotoFamiliar
                    {
                        FamId = Familiar.Id,
                        FotId = Foto.Id
                    };
                    await CuidadorRestService.DefaultManager.SaveFotoAsync(Foto, true);
                    await CuidadorRestService.DefaultManager.SaveFotoFamiliarAsync(FotoFamiliar, true);
                    await CoreMethods.PopPageModel();
                });
            }
        }

    }
}
