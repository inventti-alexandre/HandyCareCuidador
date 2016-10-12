using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using PropertyChanged;

namespace HandyCareCuidador.Model
{
    [Table("Parentesco")]
    [ImplementPropertyChanged]
    public class Parentesco
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Parentesco()
        {
            Familiar = new HashSet<Familiar>();
        }

        public string Id { get; set; }
        public string ParDescricao { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Familiar> Familiar { get; set; }
    }
}