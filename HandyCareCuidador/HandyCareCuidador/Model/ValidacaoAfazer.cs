using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace HandyCareCuidador.Model
{
    [Table("ValidacaoAfazer")]
    public class ValidacaoAfazer
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
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

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MotivoNaoValidacaoConclusaoAfazer> MotivoNaoValidacaoConclusaoAfazer { get; set; }
    }
}