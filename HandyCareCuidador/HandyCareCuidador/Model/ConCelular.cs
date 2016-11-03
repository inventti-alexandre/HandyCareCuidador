using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using PropertyChanged;

namespace HandyCareCuidador.Model
{
    [Table("ConCelular")]
    [ImplementPropertyChanged]
    public class ConCelular
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ConCelular()
        {
            ContatoEmergencia = new HashSet<ContatoEmergencia>();
        }

        public string Id { get; set; }

        [Column("ConNumCelular")]
        public string ConNumCelular { get; set; }
        public bool Deleted { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContatoEmergencia> ContatoEmergencia { get; set; }
    }
}