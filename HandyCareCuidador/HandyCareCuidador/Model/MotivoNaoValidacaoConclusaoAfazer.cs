namespace HandyCareCuidador.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MotivoNaoValidacaoConclusaoAfazer")]
    public partial class MotivoNaoValidacaoConclusaoAfazer
    { 
        public string MoDescricao { get; set; }

        public string MoValidacao { get; set; }

        public virtual ValidacaoAfazer ValidacaoAfazer { get; set; }
    }
}
