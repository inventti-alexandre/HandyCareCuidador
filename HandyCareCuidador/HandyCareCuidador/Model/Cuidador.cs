namespace HandyCareCuidador.Model
{
    using Newtonsoft.Json;    //using Microsoft.Azure.Mobile.Server;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Cuidador")]
    //[JsonObject("Cuidador")]
    public partial class Cuidador
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cuidador()
        {
            Avaliacao = new HashSet<Avaliacao>();
            ContatoEmergencia = new HashSet<ContatoEmergencia>();
            CuidadorPaciente = new HashSet<CuidadorPaciente>();
            Foto = new HashSet<Foto>();
            Video = new HashSet<Video>();
        }
        [JsonProperty(PropertyName ="Id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "CuiTipoCuidador")]
        public string CuiTipoCuidador { get; set; }
        [JsonProperty(PropertyName = "CuiValidacaoCuidador")]
        public string CuiValidacaoCuidador { get; set; }
        [JsonProperty(PropertyName = "CuiNome")]
        public string CuiNome { get; set; }
        [JsonProperty(PropertyName = "CuiSobrenome")]
        public string CuiSobrenome { get; set; }
        [JsonProperty(PropertyName = "CuiCidade")]
        public string CuiCidade { get; set; }
        [JsonProperty(PropertyName = "CuiEstado")]
        public string CuiEstado { get; set; }
        public string CuiGoogleId { get; set; }
        public string CuiFacebookId { get; set; }
        [JsonProperty(PropertyName = "CuiMicrosoftId")]
        public string CuiMicrosoftId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Avaliacao> Avaliacao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContatoEmergencia> ContatoEmergencia { get; set; }

        public virtual TipoCuidador TipoCuidador { get; set; }

        public virtual ValidacaoCuidador ValidacaoCuidador { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CuidadorPaciente> CuidadorPaciente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Foto> Foto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Video> Video { get; set; }
    }
}
