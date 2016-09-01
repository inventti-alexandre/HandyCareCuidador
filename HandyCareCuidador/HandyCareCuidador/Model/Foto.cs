using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using PropertyChanged;

namespace HandyCareCuidador.Model
{
    [Table("Foto")]
    [ImplementPropertyChanged]
    public class Foto
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Foto()
        {
            Familiar = new HashSet<Familiar>();
        }

        public string Id { get; set; }

        [Column(TypeName = "image")]
        public byte[] FotoDados { get; set; }

        public string FotoDescricao { get; set; }

        public string FotCuidador { get; set; }

        public virtual Cuidador Cuidador { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Familiar> Familiar { get; set; }
    }
}