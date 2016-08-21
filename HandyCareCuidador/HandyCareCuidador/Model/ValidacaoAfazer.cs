namespace HandyCareCuidador.Model
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ValidacaoAfazer")]
    public partial class ValidacaoAfazer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ValidacaoAfazer()
        {
            MotivoNaoValidacaoConclusaoAfazer = new HashSet<MotivoNaoValidacaoConclusaoAfazer>();
        }

        public string Id { get; set; }
        public string ValFamId { get; set; }
        public string ValAfazer { get; set; }

        public bool ValValidado { get; set; }

        public virtual ConclusaoAfazer ConclusaoAfazer { get; set; }

        public virtual Familiar Familiar { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MotivoNaoValidacaoConclusaoAfazer> MotivoNaoValidacaoConclusaoAfazer { get; set; }
    }
}
