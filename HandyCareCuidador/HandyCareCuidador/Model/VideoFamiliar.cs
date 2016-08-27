using System.ComponentModel.DataAnnotations.Schema;

namespace HandyCareCuidador.Model
{
    [Table("VideoFamiliar")]
    public class VideoFamiliar
    {
        [Column(Order = 0)]
        public string FamId { get; set; }

        [Column(Order = 1)]
        public string VidId { get; set; }

        public virtual Video Video { get; set; }
    }
}