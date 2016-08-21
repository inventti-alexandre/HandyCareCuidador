namespace HandyCareCuidador.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TipoTratamento")]
    public partial class TipoTratamento
    {

        public string TipDescricao { get; set; }
        public string TipCuidado { get; set; }

        public virtual MotivoCuidado MotivoCuidado { get; set; }
    }
}
