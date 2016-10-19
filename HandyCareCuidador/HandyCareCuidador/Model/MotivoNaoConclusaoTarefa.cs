using System.ComponentModel.DataAnnotations.Schema;
using PropertyChanged;

namespace HandyCareCuidador.Model
{
    [Table("MotivoNaoConclusaoTarefa")]
    [ImplementPropertyChanged]
    public class MotivoNaoConclusaoTarefa
    {
        public string Id { get; set; }
        public string MoExplicacao { get; set; }
        public string MoAfazer { get; set; }

        public virtual ConclusaoAfazer ConclusaoAfazer { get; set; }
    }
}