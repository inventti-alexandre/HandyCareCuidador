using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using PropertyChanged;

namespace HandyCareCuidador.Model
{
    [Table("Video")]
    [ImplementPropertyChanged]
    public class Video
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Video()
        {
            VideoFamiliar = new HashSet<VideoFamiliar>();
        }

        public string Id { get; set; }
        public byte[] VidDados { get; set; }

        public string VidDescricao { get; set; }

        public string VidCuidador { get; set; }

        public virtual Cuidador Cuidador { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VideoFamiliar> VideoFamiliar { get; set; }
    }
}