using System.ComponentModel.DataAnnotations.Schema;

namespace HandyCareCuidador.Model
{
    [Table("Avaliacao")]
    public class Avaliacao
    {
        public string Id { get; set; }
        public string AvaCuidador { get; set; }
        public string AvaFamiliar { get; set; }

        public double AvaPontuacao { get; set; }

        public string AvaDescricao { get; set; }

        public virtual Cuidador Cuidador { get; set; }

        public virtual Familiar Familiar { get; set; }
    }
}