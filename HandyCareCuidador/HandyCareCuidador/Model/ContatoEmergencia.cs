namespace HandyCareCuidador.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ContatoEmergencia")]
    public partial class ContatoEmergencia
    {
        [Column(Order = 0)]
        public string Id { get; set; }

        public string ConTelefone { get; set; }

        public string ConCelular { get; set; }

        public string ConEmail { get; set; }

        public int ConDescricao { get; set; }

        [Column(Order = 1)]
        public string ConTipo { get; set; }

        [Column(Order = 2)]
        public string ConPessoa { get; set; }

        public virtual ConCelular ConCelular1 { get; set; }

        public virtual ConEmail ConEmail1 { get; set; }

        public virtual ConTelefone ConTelefone1 { get; set; }

        public virtual Cuidador Cuidador { get; set; }

        public virtual Familiar Familiar { get; set; }

        public virtual TipoContato TipoContato { get; set; }
    }
}
