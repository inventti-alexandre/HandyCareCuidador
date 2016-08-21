using Android.OS;
using FreshMvvm;
using HandyCareCuidador.Interface;
using HandyCareCuidador.Model;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HandyCareCuidador.PageModel
{
    [ImplementPropertyChanged]

    public class ListaMaterialPageModel:FreshBasePageModel
    {
        IMaterialRestService _restService;
        public bool deleteVisible;
        public ListaMaterialPageModel(IMaterialRestService restService)
        {
            _restService = restService;
        }
        public ObservableCollection<Material> Materiais { get; set; }

        public override async void Init(object initData)
        {
            base.Init(initData);
            await GetMateriais();
        }
        public async Task GetMateriais()
        {
            try
            {
                await Task.Run(async () => {
                    Materiais = new ObservableCollection<Material>(await _restService.RefreshDataAsync());
                });
            }
            catch (Exception)
            {

                throw;
            }
        }


        public override async void ReverseInit(object returndData)
        {
            base.ReverseInit(returndData);
            var newMaterial = returndData as Material;
            if (!Materiais.Contains(newMaterial))
            {
                Materiais.Add(newMaterial);
            }
            else
            {
                await GetMateriais();
            }
        }
        public Command AddMaterial
        {
            get
            {
                return new Command(async () =>
                {
                    deleteVisible = false;
                    await CoreMethods.PushPageModel<MaterialPageModel>();
                });
            }
        }
        Material _selectedMaterial;
        public Material SelectedMaterial
        {
            get
            {
                return _selectedMaterial;
            }
            set
            {
                _selectedMaterial = value;
                if (value != null)
                {
                    MaterialSelected.Execute(value);
                    SelectedMaterial = null;
                }
            }
        }
        public Command<Material> MaterialSelected
        {
            get
            {
                return new Command<Material>(async (material) =>
                {
                    deleteVisible = true;
                    RaisePropertyChanged("IsVisible");
                    await CoreMethods.PushPageModel<MaterialPageModel>(material);
                    material = null;
                });
            }
        }

    }
}
