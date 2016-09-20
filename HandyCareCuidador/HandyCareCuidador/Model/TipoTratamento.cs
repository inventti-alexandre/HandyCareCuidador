using System.ComponentModel.DataAnnotations.Schema;
using PropertyChanged;

namespace HandyCareCuidador.Model
{
    [Table("TipoTratamento")]
    [ImplementPropertyChanged]
    public class TipoTratamento
    {
        public string Id { get; set; }
        public string TipDescricao { get; set; }
        public string TipCuidado { get; set; }

        public virtual MotivoCuidado MotivoCuidado { get; set; }
    }
}