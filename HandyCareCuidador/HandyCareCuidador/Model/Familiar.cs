using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using PropertyChanged;

namespace HandyCareCuidador.Model
{
    [Table("Familiar")]
    [ImplementPropertyChanged]
    public class Familiar
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Familiar()
        {
            Avaliacao = new HashSet<Avaliacao>();
            ContatoEmergencia = new HashSet<ContatoEmergencia>();
            ValidacaoAfazer = new HashSet<ValidacaoAfazer>();
            Foto = new HashSet<Foto>();
            Paciente = new HashSet<Paciente>();
        }

        public string Id { get; set; }
        public string FamParentesco { get; set; }
        public string FamCidade { get; set; }
        public string FamEstado { get; set; }

        public string FamContatoEmergencia { get; set; }

        public string FamNome { get; set; }
        public string FamDescriParentesco { get; set; }
        public string FamSobrenome { get; set; }
        public string FamNomeCompleto => FamNome + " " + FamSobrenome;

        [JsonProperty(PropertyName = "FamGoogleId")]
        public string FamGoogleId { get; set; }

        [JsonProperty(PropertyName = "FamFacebookId")]
        public string FamFacebookId { get; set; }

        [JsonProperty(PropertyName = "FamMicrosoftId")]
        public string FamMicrosoftId { get; set; }

        [JsonProperty(PropertyName = "FamMicrosoftAdId")]
        public string FamMicrosoftAdId { get; set; }

        [JsonProperty(PropertyName = "FamTwitterId")]
        public string FamTwitterId { get; set; }


        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Avaliacao> Avaliacao { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContatoEmergencia> ContatoEmergencia { get; set; }

        public virtual Parentesco Parentesco { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ValidacaoAfazer> ValidacaoAfazer { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Foto> Foto { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Paciente> Paciente { get; set; }
    }
}