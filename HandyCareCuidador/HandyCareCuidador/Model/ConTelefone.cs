using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using PropertyChanged;

namespace HandyCareCuidador.Model
{
    [Table("ConTelefone")]
    [ImplementPropertyChanged]
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