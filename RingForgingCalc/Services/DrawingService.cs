using ForgingCalc.Models;

namespace ForgingCalc.Services
{
    /// <summary>
    /// Сервис для генерации SVG-чертежей поковок
    /// </summary>
    public interface IDrawingService
    {
        /// <summary>
        /// Генерирует пару чертежей (с пробой и без)
        /// </summary>
        /// <param name="result">Результаты расчёта поковки</param>
        /// <returns>Кортеж с SVG для варианта без пробы и с пробой</returns>
        (string withoutSample, string withSample) GenerateDrawings(ForgingResult result);
    }

    /// <summary>
    /// Реализация сервиса генерации SVG-чертежей
    /// </summary>
    public class DrawingService : IDrawingService
    {
        private const int SvgWidth = 800;
        private const int SvgHeight = 400;
        private const int CenterX = SvgWidth / 2;
        private const int CenterY = SvgHeight / 2;

        public (string withoutSample, string withSample) GenerateDrawings(ForgingResult result)
        {
            var withoutSampleSvg = GenerateDrawing(result, withSample: false);
            var withSampleSvg = GenerateDrawing(result, withSample: true);

            return (withoutSampleSvg, withSampleSvg);
        }

        private string GenerateDrawing(ForgingResult result, bool withSample)
        {
            var dimensions = GetDimensions(result, withSample);
            var tolerances = GetTolerances(result, withSample);
            var masses = GetMasses(result, withSample);

            const int rectWidth = 500;
            const int rectHeight = 80;
            const int holeWidth = 200;
            const int wallThickness = 150;

            return $@"<svg width=""{SvgWidth}"" height=""{SvgHeight}"" xmlns=""http://www.w3.org/2000/svg"" style=""background:#fff; font-family: 'Courier New', monospace;"">
  <defs>
    <marker id=""arrL"" markerWidth=""10"" markerHeight=""7"" refX=""1"" refY=""3.5"" orient=""auto""><polygon points=""10 0, 0 3.5, 10 7""/></marker>
    <marker id=""arrR"" markerWidth=""10"" markerHeight=""7"" refX=""9"" refY=""3.5"" orient=""auto""><polygon points=""0 0, 10 3.5, 0 7""/></marker>
    <pattern id=""hatch"" width=""8"" height=""8"" patternUnits=""userSpaceOnUse"" patternTransform=""rotate(45)"">
      <line x1=""0"" y1=""0"" x2=""0"" y2=""8"" stroke=""#333"" stroke-width=""1""/>
    </pattern>
  </defs>

  <text x=""50"" y=""30"" font-style=""italic"" font-size=""12"">Выше стрелки - размеры детали</text>
  <text x=""50"" y=""45"" font-style=""italic"" font-size=""12"">Ниже стрелки - размеры поковки</text>
  <text x=""550"" y=""30"" font-style=""italic"" font-size=""12"">Масса номинал {masses.nominal:F3} тонны</text>
  <text x=""550"" y=""45"" font-style=""italic"" font-size=""12"">Масса максимал {masses.max:F3} тонны</text>

  <rect x=""40"" y=""60"" width=""720"" height=""300"" fill=""none"" stroke=""#000"" stroke-width=""1""/>

  <g transform=""translate({CenterX - rectWidth / 2}, {CenterY - rectHeight / 2})"">
    
    <!-- Левая заштрихованная часть поковки -->
    <rect x=""0"" y=""0"" width=""{wallThickness}"" height=""{rectHeight}"" fill=""url(#hatch)"" stroke=""#007bff"" stroke-width=""2""/>
    <!-- Правая заштрихованная часть поковки -->
    <rect x=""{rectWidth - wallThickness}"" y=""0"" width=""{wallThickness}"" height=""{rectHeight}"" fill=""url(#hatch)"" stroke=""#007bff"" stroke-width=""2""/>
    <!-- Центральное отверстие поковки -->
    <rect x=""{wallThickness}"" y=""0"" width=""{holeWidth}"" height=""{rectHeight}"" fill=""#fff"" stroke=""#007bff"" stroke-width=""1.5""/>

    <!-- Левая вертикальная стенка детали -->
    <rect x=""7"" y=""7"" width=""136"" height=""66"" fill=""none"" stroke=""#000"" stroke-width=""2""/>
    <!-- Правая часть детали -->
    <rect x=""357"" y=""7"" width=""136"" height=""66"" fill=""none"" stroke=""#000"" stroke-width=""2""/>
    <!-- Соединительные горизонтальные линии (границы отверстия детали) -->
    <line x1=""143"" y1=""7"" x2=""357"" y2=""7"" stroke=""#000"" stroke-width=""2""/>
    <line x1=""143"" y1=""73"" x2=""357"" y2=""73"" stroke=""#000"" stroke-width=""2""/>

    <line x1=""{rectWidth / 2}"" y1=""-40"" x2=""{rectWidth / 2}"" y2=""120"" stroke=""#f60"" stroke-dasharray=""20,5,5,5"" stroke-width=""1""/>

    <line x1=""{wallThickness}"" y1=""0"" x2=""{wallThickness}"" y2=""-30"" stroke=""#000""/>
    <line x1=""{rectWidth - wallThickness}"" y1=""0"" x2=""{rectWidth - wallThickness}"" y2=""-30"" stroke=""#000""/>
    <line x1=""{wallThickness}"" y1=""-20"" x2=""{rectWidth - wallThickness}"" y2=""-20"" stroke=""#000"" marker-start=""url(#arrL)"" marker-end=""url(#arrR)""/>
    <text x=""{rectWidth / 2}"" y=""-35"" text-anchor=""middle"" font-size=""14"">ø{dimensions.partInnerDiameter:F0}</text>
    <text x=""{rectWidth / 2}"" y=""-7"" text-anchor=""middle"" font-size=""13"">{dimensions.forgInnerDiameter:F0} -{tolerances.innerTolerance}</text>

    <line x1=""0"" y1=""{rectHeight}"" x2=""0"" y2=""{rectHeight + 40}"" stroke=""#000""/>
    <line x1=""{rectWidth}"" y1=""{rectHeight}"" x2=""{rectWidth}"" y2=""{rectHeight + 40}"" stroke=""#000""/>
    <line x1=""0"" y1=""{rectHeight + 30}"" x2=""{rectWidth}"" y2=""{rectHeight + 30}"" stroke=""#000"" marker-start=""url(#arrL)"" marker-end=""url(#arrR)""/>
    <text x=""{rectWidth / 2}"" y=""{rectHeight + 25}"" text-anchor=""middle"" font-size=""14"">ø{dimensions.partOuterDiameter:F0}</text>
    <text x=""{rectWidth / 2}"" y=""{rectHeight + 50}"" text-anchor=""middle"" font-size=""13"">{dimensions.forgOuterDiameter:F0} ±{tolerances.outerTolerance}</text>

    <line x1=""{rectWidth}"" y1=""0"" x2=""{rectWidth + 30}"" y2=""0"" stroke=""#000""/>
    <line x1=""{rectWidth}"" y1=""{rectHeight}"" x2=""{rectWidth + 30}"" y2=""{rectHeight}"" stroke=""#000""/>
    <line x1=""{rectWidth + 20}"" y1=""0"" x2=""{rectWidth + 20}"" y2=""{rectHeight}"" stroke=""#000"" marker-start=""url(#arrL)"" marker-end=""url(#arrR)""/>
    <text x=""{rectWidth + 35}"" y=""{rectHeight / 2 - 5}"" text-anchor=""start"" font-size=""14"">{dimensions.partHeight:F0}</text>
    <text x=""{rectWidth + 35}"" y=""{rectHeight / 2 + 15}"" text-anchor=""start"" font-size=""13"">{dimensions.forgHeight:F0}</text>
  </g>

  <g transform=""translate({CenterX}, 340) scale(0.8)"">
    <line x1=""0"" y1=""0"" x2=""40"" y2=""0"" stroke=""#000"" marker-end=""url(#arrR)""/>
    <line x1=""0"" y1=""0"" x2=""0"" y2=""-40"" stroke=""#000"" marker-end=""url(#arrR)""/>
    <text x=""45"" y=""5"" font-size=""12"">X</text>
    <text x=""-5"" y=""-45"" font-size=""12"">Y</text>
  </g>
</svg>";
        }

        private static (double partOuterDiameter, double partInnerDiameter, double partHeight, 
                        double forgOuterDiameter, double forgInnerDiameter, double forgHeight) 
            GetDimensions(ForgingResult result, bool withSample)
        {
            return (
                partOuterDiameter: result.D,
                partInnerDiameter: result.dd,
                partHeight: result.H,
                forgOuterDiameter: withSample ? result.D_forg_probe : result.D_forg_no_probe,
                forgInnerDiameter: withSample ? result.dd_forg_probe : result.dd_forg_no_probe,
                forgHeight: withSample ? result.H_forg_probe : result.H_forg_no_probe);
        }

        private static (int outerTolerance, int innerTolerance) GetTolerances(ForgingResult result, bool withSample)
        {
            int delta2 = withSample ? result.delta2_probe : result.delta2_no_probe;
            int innerTolerance = 3 * delta2;
            return (delta2, innerTolerance);
        }

        private static (double nominal, double max) GetMasses(ForgingResult result, bool withSample)
        {
            return (
                nominal: withSample ? result.MassNominalProbe : result.MassNominalNoProbe,
                max: withSample ? result.MassMaxProbe : result.MassMaxNoProbe);
        }
    }
}