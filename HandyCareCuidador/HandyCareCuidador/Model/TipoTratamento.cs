using System.ComponentModel.DataAnnotations.Schema;

namespace HandyCareCuidador.Model
{
    [Table("TipoTratamento")]
    public class TipoTratamento
    {
        public string TipDescricao { get; set; }
        public string TipCuidado { get; set; }

        public virtual MotivoCuidado MotivoCuidado { get; set; }
    }
}