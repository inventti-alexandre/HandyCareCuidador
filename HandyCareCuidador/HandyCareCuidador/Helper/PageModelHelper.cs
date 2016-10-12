using System;
using PropertyChanged;

namespace HandyCareCuidador.Helper
{
    [ImplementPropertyChanged]
    public class PageModelHelper
    {
        public DateTime Data { get; set; }
        public TimeSpan Horario { get; set; }
        public bool deleteVisible { get; set; }
        public bool ActivityRunning { get; set; }
        public bool Visualizar { get; set; }
        public float? Quantidade { get; set; }
        public float? QuantidadeF { get; set; }
        public bool HabilitarMaterial { get; set; }
        public bool HabilitarMedicamento { get; set; }
        public bool NovoCuidador { get; set; }
        public bool NovoPaciente { get; set; }
        public bool DadoPaciente { get; set; }
        public bool FinalizarAfazer { get; set; }
        public bool VisualizarTermino { get; set; }
        public string BoasVindas { get; set; }
        public bool CuidadorExibicao { get; set; }
        public bool NovoCadastro { get; set; }
    }
}