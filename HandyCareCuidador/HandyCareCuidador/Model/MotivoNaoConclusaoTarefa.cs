using System.ComponentModel.DataAnnotations.Schema;

namespace HandyCareCuidador.Model
{
    [Table("MotivoNaoConclusaoTarefa")]
    public class MotivoNaoConclusaoTarefa
    {
        public string MoExplicacao { get; set; }
        public string MoAfazer { get; set; }

        public virtual ConclusaoAfazer ConclusaoAfazer { get; set; }
    }
}