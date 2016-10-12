using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PropertyChanged;

namespace HandyCareCuidador.Model
{
    [Table("PacienteFamiliar")]
    [ImplementPropertyChanged]
    public class PacienteFamiliar
    {
        public string Id { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(36)]
        public string PacId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(36)]
        public string FamId { get; set; }
    }
}