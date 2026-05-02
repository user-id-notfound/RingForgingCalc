using System;

namespace ForgingCalc.Services
{
    /// <summary>
    /// Сервис для расчёта допусков и массы поковок
    /// </summary>
    public static class ToleranceService
    {
        private const double DensityKgPerM3 = 7.85;

        private static readonly double[] HBounds = { 150, 200, 250, 315, 400, 500, 630, 800, 1000, 1250, 1400, 1600, 1800, 2000, 2250, 2500 };
        private static readonly double[] DBounds = { 500, 630, 800, 1000, 1250, 1400, 1600, 1800, 2000, 2250, 2500, 2800, 3150, 3500, 4000, 4500, 5000 };

        private static readonly (int b, int d2)[,] Matrix = new (int, int)[16, 17]
        {
            { (24,9), (25,9), (27,10), (0,0), (0,0), (0,0), (0,0), (0,0), (0,0), (0,0), (0,0), (0,0), (0,0), (0,0), (0,0), (0,0), (0,0) },
            { (24,9), (25,9), (27,10), (29,11), (31,11), (35,13), (0,0), (0,0), (0,0), (0,0), (0,0), (0,0), (0,0), (0,0), (0,0), (0,0), (0,0) },
            { (25,9), (26,9), (28,10), (30,11), (32,12), (36,13), (38,14), (40,15), (0,0), (0,0), (0,0), (0,0), (0,0), (0,0), (0,0), (0,0), (0,0) },
            { (27,10), (28,10), (30,11), (32,12), (34,13), (38,14), (41,15), (44,16), (47,18), (51,19), (0,0), (0,0), (0,0), (0,0), (0,0), (0,0), (0,0) },
            { (28,10), (29,11), (31,11), (33,12), (35,13), (40,15), (42,16), (46,17), (49,19), (53,20), (57,21), (61,23), (0,0), (0,0), (0,0), (0,0), (0,0) },
            { (29,11), (30,11), (31,11), (34,13), (36,13), (41,15), (44,16), (48,18), (51,19), (55,21), (59,22), (63,24), (67,25), (71,27), (0,0), (0,0), (0,0) },
            { (30,11), (31,11), (33,12), (35,13), (37,14), (43,16), (46,17), (50,19), (53,20), (57,21), (61,23), (65,25), (69,26), (75,28), (80,30), (95,37), (0,0) },
            { (0,0), (33,12), (36,13), (38,14), (41,15), (46,17), (50,19), (54,20), (57,21), (61,23), (65,25), (71,27), (77,29), (83,31), (89,34), (98,39), (105,43) },
            { (0,0), (0,0), (37,14), (40,15), (43,16), (48,18), (52,20), (56,21), (59,22), (63,24), (68,26), (74,28), (80,30), (86,33), (92,36), (101,41), (108,46) },
            { (0,0), (0,0), (0,0), (44,16), (46,17), (52,20), (56,21), (60,23), (64,24), (68,26), (74,28), (81,30), (86,33), (92,36), (98,39), (105,43), (112,48) },
            { (0,0), (0,0), (0,0), (0,0), (47,18), (54,20), (58,22), (62,23), (67,25), (72,27), (78,29), (83,31), (89,34), (95,37), (101,41), (108,45), (115,50) },
            { (0,0), (0,0), (0,0), (0,0), (48,18), (56,21), (60,23), (65,24), (69,26), (75,28), (81,30), (87,34), (93,36), (99,39), (104,42), (110,47), (119,51) },
            { (0,0), (0,0), (0,0), (0,0), (0,0), (58,22), (63,23), (67,25), (73,27), (79,29), (85,33), (91,35), (97,38), (102,41), (106,43), (113,48), (120,52) },
            { (0,0), (0,0), (0,0), (0,0), (0,0), (0,0), (64,23), (68,25), (75,28), (81,30), (87,34), (92,36), (98,38), (103,41), (107,43), (115,50), (121,52) },
            { (0,0), (0,0), (0,0), (0,0), (0,0), (0,0), (0,0), (69,26), (76,28), (82,30), (88,34), (94,37), (100,40), (104,42), (110,47), (116,50), (122,52) },
            { (0,0), (0,0), (0,0), (0,0), (0,0), (0,0), (0,0), (0,0), (82,30), (85,32), (91,35), (97,38), (100,41), (108,43), (114,49), (119,51), (125,53) }
        };

        /// <summary>
        /// Получает допуски на основе высоты и диаметра заготовки
        /// </summary>
        /// <param name="height">Высота заготовки (мм)</param>
        /// <param name="diameter">Диаметр заготовки (мм)</param>
        /// <returns>Кортеж с предельным отклонением размера (b) и отклонением диаметра (d2)</returns>
        public static (int b, int d2) GetTolerance(double height, double diameter)
        {
            var hIdx = FindIndex(height, HBounds);
            var dIdx = FindIndex(diameter, DBounds);

            // Защита от выхода за границы матрицы
            hIdx = Math.Min(hIdx, Matrix.GetLength(0) - 1);
            dIdx = Math.Min(dIdx, Matrix.GetLength(1) - 1);

            return Matrix[hIdx, dIdx];
        }

        /// <summary>
        /// Находит индекс в массиве границ, где значение меньше или равно границе
        /// </summary>
        private static int FindIndex(double value, double[] bounds)
        {
            for (int i = 0; i < bounds.Length; i++)
            {
                if (value <= bounds[i])
                    return i;
            }
            return bounds.Length;
        }

        /// <summary>
        /// Рассчитывает объём и массу цилиндрической заготовки
        /// </summary>
        /// <param name="outerDiameter">Наружный диаметр (мм)</param>
        /// <param name="innerDiameter">Внутренний диаметр (мм)</param>
        /// <param name="length">Длина/высота (мм)</param>
        /// <returns>Детали расчёта массы (объёмы и массы внешнего и внутреннего цилиндров)</returns>
        public static MassDetails CalculateMassDetails(double outerDiameter, double innerDiameter, double length)
        {
            var radii = ConvertToMeters(outerDiameter, innerDiameter, length);

            var volumes = CalculateVolumes(radii.outerRadius, radii.innerRadius, radii.length);
            var masses = CalculateMasses(volumes.outerVolume, volumes.innerVolume);

            return new MassDetails(
                volumes.outerVolume,
                volumes.innerVolume,
                masses.outerMass,
                masses.innerMass);
        }

        private static (double outerRadius, double innerRadius, double length) ConvertToMeters(
            double outerDiameterMm, double innerDiameterMm, double lengthMm)
        {
            return (
                outerRadius: outerDiameterMm / 2000.0,
                innerRadius: innerDiameterMm / 2000.0,
                length: lengthMm / 1000.0);
        }

        private static (double outerVolume, double innerVolume) CalculateVolumes(
            double outerRadius, double innerRadius, double length)
        {
            var outerVolume = Math.PI * outerRadius * outerRadius * length;
            var innerVolume = Math.PI * innerRadius * innerRadius * length;
            return (outerVolume, innerVolume);
        }

        private static (double outerMass, double innerMass) CalculateMasses(
            double outerVolume, double innerVolume)
        {
            return (
                outerMass: outerVolume * DensityKgPerM3,
                innerMass: innerVolume * DensityKgPerM3);
        }
    }

    /// <summary>
    /// Результат расчёта массы детали
    /// </summary>
    /// <param name="vOut">Объём внешнего цилиндра (м³)</param>
    /// <param name="vIn">Объём внутреннего цилиндра (м³)</param>
    /// <param name="massOut">Масса внешнего цилиндра (кг)</param>
    /// <param name="massIn">Масса внутреннего цилиндра (кг)</param>
    public readonly record struct MassDetails(
        double vOut,
        double vIn,
        double massOut,
        double massIn);
}