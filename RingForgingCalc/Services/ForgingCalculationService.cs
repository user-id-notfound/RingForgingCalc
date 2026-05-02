using ForgingCalc.Models;

namespace ForgingCalc.Services
{
    /// <summary>
    /// Интерфейс сервиса для расчёта параметров поковок
    /// </summary>
    public interface IForgingCalculationService
    {
        /// <summary>
        /// Выполняет полный расчёт параметров поковки на основе входных данных
        /// </summary>
        /// <param name="input">Входные данные для расчёта</param>
        /// <returns>Результаты расчёта поковки</returns>
        ForgingResult Calculate(ForgingInput input);
    }

    /// <summary>
    /// Сервис для расчёта параметров поковок колец
    /// </summary>
    public class ForgingCalculationService : IForgingCalculationService
    {
        private const double DensityKgPerM3 = 7.85;

        /// <summary>
        /// Выполняет полный расчёт параметров поковки
        /// </summary>
        public ForgingResult Calculate(ForgingInput input)
        {
            var result = InitializeResult(input);

            // Расчёт высот
            (result.H1, result.H2) = CalculateHeights(input, result);

            // Получение допусков
            (result.b_no_probe, result.delta2_no_probe) = ToleranceService.GetTolerance(result.H1, input.D);
            (result.b_probe, result.delta2_probe) = ToleranceService.GetTolerance(result.H2, input.D);

            // Расчёт размеров поковки
            CalculateForgingDimensions(input, result);

            // Расчёт массы и объёмов
            CalculateMassAndVolumes(result);

            return result;
        }

        private static ForgingResult InitializeResult(ForgingInput input)
        {
            return new ForgingResult
            {
                D = input.D,
                dd = input.dd,
                H = input.H,
                x = input.x,
                y = input.y,
                z = input.x - 1,
                Q = input.Q,
                Density = DensityKgPerM3
            };
        }

        private static (double h1, double h2) CalculateHeights(ForgingInput input, ForgingResult result)
        {
            var h1 = input.H * input.x + input.y * result.z + input.AllowanceHT;
            var h2 = input.H * input.x + input.y * result.z + input.Q + input.AllowanceHT;

            return (h1, h2);
        }

        private static void CalculateForgingDimensions(ForgingInput input, ForgingResult result)
        {
            double partOuterDiameter = input.D + input.AllowanceHT;
            double partInnerDiameter = input.dd - input.AllowanceHT;

            // Размеры без пробы
            result.D_forg_no_probe = partOuterDiameter + result.b_no_probe;
            result.dd_forg_no_probe = partInnerDiameter - result.b_no_probe;
            result.H_forg_no_probe = result.H1 + result.b_no_probe;

            // Размеры с пробой
            result.D_forg_probe = partOuterDiameter + result.b_probe;
            result.dd_forg_probe = partInnerDiameter - result.b_probe;
            result.H_forg_probe = result.H2 + result.b_probe;
        }

        private void CalculateMassAndVolumes(ForgingResult result)
        {
            // Расчёт для варианта без пробы
            var nominalNoProbe = ToleranceService.CalculateMassDetails(
                result.D_forg_no_probe,
                result.dd_forg_no_probe,
                result.H_forg_no_probe);

            result.V_disk_nom_np = nominalNoProbe.vOut;
            result.V_hole_nom_np = nominalNoProbe.vIn;
            result.MassNominalNoProbe = nominalNoProbe.massOut - nominalNoProbe.massIn;

            // Максимальные размеры для варианта без пробы
            double maxOuterDiameterNoProbe = result.D_forg_no_probe + result.delta2_no_probe;
            double minInnerDiameterNoProbe = result.dd_forg_no_probe - (3 * result.delta2_no_probe);
            double maxHeightNoProbe = result.H_forg_no_probe + result.delta2_no_probe;

            var maxNoProbe = ToleranceService.CalculateMassDetails(
                maxOuterDiameterNoProbe,
                minInnerDiameterNoProbe,
                maxHeightNoProbe);

            result.V_disk_max_np = maxNoProbe.vOut;
            result.V_hole_max_np = maxNoProbe.vIn;
            result.MassMaxNoProbe = maxNoProbe.massOut - maxNoProbe.massIn;

            // Расчёт для варианта с пробой
            var nominalProbe = ToleranceService.CalculateMassDetails(
                result.D_forg_probe,
                result.dd_forg_probe,
                result.H_forg_probe);

            result.V_disk_nom_p = nominalProbe.vOut;
            result.V_hole_nom_p = nominalProbe.vIn;
            result.MassNominalProbe = nominalProbe.massOut - nominalProbe.massIn;

            // Максимальные размеры для варианта с пробой
            double maxOuterDiameterProbe = result.D_forg_probe + result.delta2_probe;
            double minInnerDiameterProbe = result.dd_forg_probe - (3 * result.delta2_probe);
            double maxHeightProbe = result.H_forg_probe + result.delta2_probe;

            var maxProbe = ToleranceService.CalculateMassDetails(
                maxOuterDiameterProbe,
                minInnerDiameterProbe,
                maxHeightProbe);

            result.V_disk_max_p = maxProbe.vOut;
            result.V_hole_max_p = maxProbe.vIn;
            result.MassMaxProbe = maxProbe.massOut - maxProbe.massIn;
        }
    }
}
