using Android.OS;

namespace HandyCareCuidador.Droid.Services
{
    public class HorarioAfazerBinder : Binder
    {
        protected AfazerService service;

        public HorarioAfazerBinder(AfazerService service)
        {
            this.service = service;
        }

        public AfazerService Service => service;
        public bool IsBound { get; set; }
    }
}