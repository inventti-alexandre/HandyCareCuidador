using System.ComponentModel.DataAnnotations.Schema;
using PropertyChanged;

namespace HandyCareCuidador.Model
{
    [Table("VideoFamiliar")]
    [ImplementPropertyChanged]
    public class VideoFamiliar
    {
        public string Id { get; set; }
        public string FamId { get; set; }

        public string VidId { get; set; }

        public virtual Video Video { get; set; }

    }
}