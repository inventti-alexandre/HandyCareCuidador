using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace HandyCareCuidador.Model
{
    [Table("ConCelular")]
    public class ConCelular
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ConCelular()
        {
            ContatoEmergencia = new HashSet<ContatoEmergencia>();
        }

        [Column("ConCelular")]
        public int ConCelular1 { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContatoEmergencia> ContatoEmergencia { get; set; }
    }
}