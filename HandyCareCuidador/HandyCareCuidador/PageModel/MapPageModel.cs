using FreshMvvm;
using PropertyChanged;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace HandyCareCuidador.PageModel
{
    [ImplementPropertyChanged]
    public class MapPageModel:FreshBasePageModel
    {
        public Map HandyCareMap;
        public override void Init(object initData)
        {
            base.Init(initData);
            HandyCareMap = new Map
            {
                MapType = MapType.Hybrid,
                IsShowingUser = true
            };
        }

    }
}