namespace GasDistributionOptimizer.Models
{
    public class BlastFurnace
    {
        /// <summary>
        /// Уникальный идентификатор (номер) доменной печи.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Базовый расход природного газа (ПГ), м³/ч.
        /// </summary>
        public double BaseNaturalGas { get; set; }
        /// <summary>
        /// Минимально допустимый расход природного газа, м³/ч.
        /// </summary>
        public double MinNaturalGas { get; set; }
        /// <summary>
        /// Максимально допустимый расход природного газа, м³/ч.
        /// </summary>
        public double MaxNaturalGas { get; set; }
        /// <summary>
        /// Базовый расход кокса, т/ч.
        /// </summary>
        public double BaseCoke { get; set; }
        /// <summary>
        /// Коэффициент (эквивалент) замены кокса природным газом. 
        /// </summary>
        public double CokeReplacementRatio { get; set; }
        /// <summary>
        /// Базовая производительность печи (выплавка чугуна), т/ч.
        /// </summary>
        public double BaseIronProduction { get; set; }
        /// <summary>
        /// Базовое содержание серы в выплавляемом чугуне (в долях).
        /// </summary>
        public double BaseSulfur { get; set; }
        /// <summary>
        /// Минимально требуемая производительность печи по плану, т/ч.
        /// </summary>
        public double MinIronProduction { get; set; }
        /// <summary>
        /// Максимально допустимый расход кокса (ограничение по экономике или загрузке), т/ч.
        /// </summary>
        public double MaxCoke { get; set; }
        /// <summary>
        /// Максимально допустимое содержание серы в чугуне по технологическим требованиям (в долях).
        /// </summary>
        public double MaxSulfur { get; set; }
        /// <summary>
        /// Коэффициент чувствительности (dP/dV).
        /// Изменение производства чугуна при изменении расхода ПГ на 1 м³/ч.
        /// </summary>
        public double DeltaIronPerGas { get; set; }
        /// <summary>
        /// Коэффициент чувствительности (dP/dK). 
        /// Изменение производства чугуна при изменении расхода кокса на 1 единицу.
        /// </summary>
        public double DeltaIronPerCoke { get; set; }
        /// <summary>
        /// Коэффициент чувствительности (dS/dV). 
        /// Изменение содержания серы при изменении расхода ПГ на 1 м³/ч.
        /// </summary>
        public double DeltaSulfurPerGas { get; set; }
        /// <summary>
        /// Коэффициент чувствительности (dS/dK). 
        /// Изменение содержания серы при изменении расхода кокса на 1 единицу.
        /// </summary>
        public double DeltaSulfurPerCoke { get; set; }
    }
}
