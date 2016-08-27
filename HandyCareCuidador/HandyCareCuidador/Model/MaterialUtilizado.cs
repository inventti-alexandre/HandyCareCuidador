using System.ComponentModel.DataAnnotations.Schema;
using PropertyChanged;

namespace HandyCareCuidador.Model
{
    [Table("MaterialUtilizado")]
    [ImplementPropertyChanged]
    public class MaterialUtilizado
    {
        [Column(Order = 0)]
        public string MatAfazer { get; set; }

        [Column(Order = 1)]
        public string MatUtilizado { get; set; }

        [Column(Order = 2)]
        public string Id { get; set; }

        public int MatQuantidadeUtilizada { get; set; }

        //public virtual Material Material { get; set; }

        //public virtual Afazer Afazer { get; set; }
    }
}