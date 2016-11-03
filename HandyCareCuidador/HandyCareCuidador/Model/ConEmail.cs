using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace HandyCareCuidador.Model
{
    [Table("ConEmail")]
    public class ConEmail
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ConEmail()
        {
            ContatoEmergencia = new HashSet<ContatoEmergencia>();
        }

        public string Id { get; set; }

        [Column("ConEnderecoEmail")]
        public string ConEnderecoEmail { get; set; }
        public bool Deleted { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContatoEmergencia> ContatoEmergencia { get; set; }
    }
}