using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using PropertyChanged;

namespace HandyCareCuidador.Model
{
    [Table("ValidacaoCuidador")]
    [ImplementPropertyChanged]
    public class ValidacaoCuidador
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ValidacaoCuidador()
        {
            Cuidador = new HashSet<Cuidador>();
        }

        public string Id { get; set; }

        [Column(TypeName = "image")]
        public byte[] ValDocumento { get; set; }

        public bool ValValidado { get; set; }
        public string CorenEnfermeiro { get; set; }

        public virtual CorenEnfermeiro CorenEnfermeiro1 { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cuidador> Cuidador { get; set; }
    }
}