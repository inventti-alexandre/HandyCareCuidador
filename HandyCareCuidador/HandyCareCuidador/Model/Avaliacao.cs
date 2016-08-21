namespace HandyCareCuidador.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Avaliacao")]
    public partial class Avaliacao
    {
        public string Id { get; set; }
        public string AvaCuidador { get; set; }
        public string AvaFamiliar { get; set; }

        public double AvaPontuacao { get; set; }

        public string AvaDescricao { get; set; }

        public virtual Cuidador Cuidador { get; set; }

        public virtual Familiar Familiar { get; set; }
    }
}
