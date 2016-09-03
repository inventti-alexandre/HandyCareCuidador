using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace HandyCareCuidador.Model
{
    [Table("ConTelefone")]
    public class ConTelefone
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ConTelefone()
        {
            ContatoEmergencia = new HashSet<ContatoEmergencia>();
        }
        public string Id { get; set; }
        [Column("ConNumTelefone")]
        public string ConNumTelefone { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContatoEmergencia> ContatoEmergencia { get; set; }
    }
}