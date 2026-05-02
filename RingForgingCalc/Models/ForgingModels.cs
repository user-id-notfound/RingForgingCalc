namespace ForgingCalc.Models
{
    /// <summary>
    /// Входные данные для расчёта поколки кольца
    /// </summary>
    public class ForgingInput
    {
        /// <summary>
        /// Наружный диаметр детали (мм)
        /// </summary>
        public double D { get; set; }

        /// <summary>
        /// Внутренний диаметр детали (мм)
        /// </summary>
        public double dd { get; set; }

        /// <summary>
        /// Высота детали (мм)
        /// </summary>
        public double H { get; set; }

        /// <summary>
        /// Количество проходов по высоте
        /// </summary>
        public int x { get; set; }

        /// <summary>
        /// Прибавка на каждый проход (мм)
        /// </summary>
        public double y { get; set; }

        /// <summary>
        /// Резервное поле (не используется в расчётах)
        /// </summary>
        public int z { get; set; }

        /// <summary>
        /// Дополнительный припуск (мм)
        /// </summary>
        public double Q { get; set; }

        /// <summary>
        /// Припуск на горячую правку (мм)
        /// </summary>
        public double AllowanceHT { get; set; }
    }

    /// <summary>
    /// Результаты расчёта поколки кольца
    /// </summary>
    public class ForgingResult
    {
        // Входные параметры
        public double D { get; set; }
        public double dd { get; set; }
        public double H { get; set; }
        public int x { get; set; }
        public double y { get; set; }
        public int z { get; set; }
        public double Q { get; set; }

        /// <summary>
        /// Плотность материала (кг/м³)
        /// </summary>
        public double Density { get; set; } = 7.85;

        /// <summary>
        /// Расчётная высота без отбора проб
        /// </summary>
        public double H1 { get; set; }

        /// <summary>
        /// Расчётная высота с отбором проб
        /// </summary>
        public double H2 { get; set; }

        /// <summary>
        /// Предельное отклонение размера поковки (без пробы)
        /// </summary>
        public int b_no_probe { get; set; }

        /// <summary>
        /// Предельное отклонение диаметра (без пробы)
        /// </summary>
        public int delta2_no_probe { get; set; }

        /// <summary>
        /// Предельное отклонение размера поковки (с пробой)
        /// </summary>
        public int b_probe { get; set; }

        /// <summary>
        /// Предельное отклонение диаметра (с пробой)
        /// </summary>
        public int delta2_probe { get; set; }

        /// <summary>
        /// Наружный диаметр поковки без пробы
        /// </summary>
        public double D_forg_no_probe { get; set; }

        /// <summary>
        /// Внутренний диаметр поковки без пробы
        /// </summary>
        public double dd_forg_no_probe { get; set; }

        /// <summary>
        /// Высота поковки без пробы
        /// </summary>
        public double H_forg_no_probe { get; set; }

        /// <summary>
        /// Наружный диаметр поковки с пробой
        /// </summary>
        public double D_forg_probe { get; set; }

        /// <summary>
        /// Внутренний диаметр поковки с пробой
        /// </summary>
        public double dd_forg_probe { get; set; }

        /// <summary>
        /// Высота поковки с пробой
        /// </summary>
        public double H_forg_probe { get; set; }

        /// <summary>
        /// Номинальная масса без пробы (кг)
        /// </summary>
        public double MassNominalNoProbe { get; set; }

        /// <summary>
        /// Максимальная масса без пробы (кг)
        /// </summary>
        public double MassMaxNoProbe { get; set; }

        /// <summary>
        /// Номинальная масса с пробой (кг)
        /// </summary>
        public double MassNominalProbe { get; set; }

        /// <summary>
        /// Максимальная масса с пробой (кг)
        /// </summary>
        public double MassMaxProbe { get; set; }

        /// <summary>
        /// Объём наружного цилиндра без пробы (м³)
        /// </summary>
        public double V_disk_nom_np { get; set; }

        /// <summary>
        /// Объём внутреннего цилиндра без пробы (м³)
        /// </summary>
        public double V_hole_nom_np { get; set; }

        /// <summary>
        /// Объём наружного цилиндра (макс.) без пробы (м³)
        /// </summary>
        public double V_disk_max_np { get; set; }

        /// <summary>
        /// Объём внутреннего цилиндра (макс.) без пробы (м³)
        /// </summary>
        public double V_hole_max_np { get; set; }

        /// <summary>
        /// Объём наружного цилиндра с пробой (м³)
        /// </summary>
        public double V_disk_nom_p { get; set; }

        /// <summary>
        /// Объём внутреннего цилиндра с пробой (м³)
        /// </summary>
        public double V_hole_nom_p { get; set; }

        /// <summary>
        /// Объём наружного цилиндра (макс.) с пробой (м³)
        /// </summary>
        public double V_disk_max_p { get; set; }

        /// <summary>
        /// Объём внутреннего цилиндра (макс.) с пробой (м³)
        /// </summary>
        public double V_hole_max_p { get; set; }

        /// <summary>
        /// SVG-чертёж без пробы
        /// </summary>
        public string SvgNoProbe { get; set; } = string.Empty;

        /// <summary>
        /// SVG-чертёж с пробой
        /// </summary>
        public string SvgProbe { get; set; } = string.Empty;
    }
}