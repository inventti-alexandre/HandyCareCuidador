using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshMvvm;
using HandyCareCuidador.Helper;
using HandyCareCuidador.Interface;
using HandyCareCuidador.Model;
using PropertyChanged;
using Xamarin.Forms;

namespace HandyCareCuidador.PageModel
{
    [ImplementPropertyChanged]
    public class ListaAfazerConcluidoPageModel:FreshBasePageModel
    {
        private readonly IConclusaoAfazerRestService _conclusaoRestService;
        private readonly IAfazerRestService _restService;
        private Afazer _selectedAfazer;
        public bool AfazerConcluido;
        public bool DeleteVisible;
        public Afazer AfazerSelecionado { get; set; }
        public ListaAfazerConcluidoPageModel()
        {
            _restService = FreshIOC.Container.Resolve<IAfazerRestService>();
            _conclusaoRestService = FreshIOC.Container.Resolve<IConclusaoAfazerRestService>();
        }
        public ObservableCollection<Afazer> Afazeres { get; set; }
        public ObservableCollection<ConclusaoAfazer> AfazeresConcluidos { get; set; }

        public Afazer SelectedAfazer
        {
            get { return _selectedAfazer; }
            set
            {
                _selectedAfazer = value;
                if (value == null) return;
                AfazerSelected.Execute(value);
                SelectedAfazer = null;
            }
        }
        public override async void Init(object initData)
        {
            base.Init(initData);
            AfazerSelecionado = new Afazer();
            await GetAfazeres();
        }
        public override void ReverseInit(object returndData)
        {
            base.ReverseInit(returndData);
            var newAfazer = returndData as Afazer;
            if (!Afazeres.Contains(newAfazer))
                Afazeres.Add(newAfazer);
            else
                Task.Run(async () => await GetAfazeres());
        }
        public Command<Afazer> AfazerSelected
        {
            get
            {
                return new Command<Afazer>(async afazer =>
                {
                    var afazerConcluido = AfazeresConcluidos.FirstOrDefault(m => m.ConAfazer == afazer.Id);
                    var afazeres = new Tuple<Afazer,ConclusaoAfazer>(afazer, afazerConcluido);
                    await CoreMethods.PushPageModel<ConclusaoAfazerPageModel>(afazer);
                    afazer = null;
                });
            }
        }
        public async Task GetAfazeres()
        {
            try
            {
                await Task.Run(async () =>
                {

                    AfazeresConcluidos = new ObservableCollection<ConclusaoAfazer>(await _conclusaoRestService.RefreshDataAsync());
                    var selection = new ObservableCollection<Afazer>(await _restService.RefreshDataAsync());
                    var result = selection.Where(e => AfazeresConcluidos.Select(m => m.ConAfazer)
                    .Contains(e.Id)).AsEnumerable();
                    Afazeres=new ObservableCollection<Afazer>(result);
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
