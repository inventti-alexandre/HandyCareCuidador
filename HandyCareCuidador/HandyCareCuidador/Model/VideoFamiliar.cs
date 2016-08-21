namespace HandyCareCuidador.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("VideoFamiliar")]
    public partial class VideoFamiliar
    {
        [Column(Order = 0)]
        public string FamId { get; set; }

        [Column(Order = 1)]
        public string VidId { get; set; }

        public virtual Video Video { get; set; }
    }
}
