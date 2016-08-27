using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using PropertyChanged;

namespace HandyCareCuidador.Model
{
    [Table("Material")]
    [ImplementPropertyChanged]
    public class Material
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Material()
        {
            MaterialUtilizado = new HashSet<MaterialUtilizado>();
        }

        public string Id { get; set; }

        public string MatDescricao { get; set; }

        public int MatQuantidade { get; set; }
        public string MatPacId { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MaterialUtilizado> MaterialUtilizado { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }
        public bool Deleted { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}