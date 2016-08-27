using System.ComponentModel.DataAnnotations.Schema;
using PropertyChanged;

namespace HandyCareCuidador.Model
{
    [Table("MedicamentoAdministrado")]
    [ImplementPropertyChanged]
    public class MedicamentoAdministrado
    {
        [Column(Order = 0)]
        public string MedAfazer { get; set; }

        [Column(Order = 1)]
        public string MedAdministrado { get; set; }

        public string Id { get; set; }

        public int MemQuantidadeAdministrada { get; set; }

        public virtual Afazer Afazer { get; set; }

        public virtual Medicamento Medicamento { get; set; }
    }
}