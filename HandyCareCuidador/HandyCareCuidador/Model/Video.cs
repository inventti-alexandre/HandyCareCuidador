namespace HandyCareCuidador.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Video")]
    public partial class Video
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Video()
        {
            VideoFamiliar = new HashSet<VideoFamiliar>();
        }
        public string Id { get; set; }
        public byte[] VidDados { get; set; }

        public string VidDescricao { get; set; }

        public string VidCuidador { get; set; }

        public virtual Cuidador Cuidador { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VideoFamiliar> VideoFamiliar { get; set; }
    }
}
