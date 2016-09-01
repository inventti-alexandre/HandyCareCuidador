using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using PropertyChanged;

namespace HandyCareCuidador.Model
{
    [Table("TipoCuidador")]
    [ImplementPropertyChanged]
    public class TipoCuidador
    {
        //[SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public TipoCuidador()
        //{
        //    Cuidador = new HashSet<Cuidador>();
        //}
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "TipDescricao")]
        public string TipDescricao { get; set; }

        //[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Cuidador> Cuidador { get; set; }
    }
}