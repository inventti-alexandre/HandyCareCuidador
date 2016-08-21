namespace HandyCareCuidador.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MotivoNaoConclusaoTarefa")]
    public partial class MotivoNaoConclusaoTarefa
    {
        public string MoExplicacao { get; set; }
        public string MoAfazer { get; set; }

        public virtual ConclusaoAfazer ConclusaoAfazer { get; set; }
    }
}
