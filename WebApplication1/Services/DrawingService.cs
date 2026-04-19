using ForgingCalc.Models;

namespace ForgingCalc.Services
{
    public interface IDrawingService
    {
        (string withoutSample, string withSample) GenerateDrawings(ForgingResult result);
    }

    public class DrawingService : IDrawingService
    {
        public (string withoutSample, string withSample) GenerateDrawings(ForgingResult result)
        {
            var withoutSampleSvg = GenerateDrawing(result, false);
            var withSampleSvg = GenerateDrawing(result, true);

            return (withoutSampleSvg, withSampleSvg);
        }

        private string GenerateDrawing(ForgingResult result, bool withSample)
        {
            double D_det = result.D;
            double dd_det = result.dd;
            double H_det = result.H;

            double D_f = withSample ? result.D_forg_probe : result.D_forg_no_probe;
            double dd_f = withSample ? result.dd_forg_probe : result.dd_forg_no_probe;
            double H_f = withSample ? result.H_forg_probe : result.H_forg_no_probe;

            int delta2 = withSample ? result.delta2_probe : result.delta2_no_probe;
            int innerTol = 3 * delta2;
            double nomMass = withSample ? result.MassNominalProbe : result.MassNominalNoProbe;
            double maxMass = withSample ? result.MassMaxProbe : result.MassMaxNoProbe;

            const int w = 800;
            const int h = 400;
            const int cX = w / 2;
            const int cY = h / 2;

            int rectW = 500;
            int rectH = 80;
            int holeW = 200;
            int wall = (rectW - holeW) / 2;

            double scaleX = (double)rectW / D_f;
            double scaleY = (double)rectH / H_f;

            double offsetX = (D_f - D_det) / 2 * scaleX;
            double offsetY = (H_f - H_det) / 2 * scaleY;
            double offsetHole = (dd_det - dd_f) / 2 * scaleX;

            var svg = $@"<svg width=""{w}"" height=""{h}"" xmlns=""http://w3.org"" style=""background:#fff; font-family: 'Courier New', monospace;"">
  <defs>
    <marker id=""arrL"" markerWidth=""10"" markerHeight=""7"" refX=""1"" refY=""3.5"" orient=""auto""><polygon points=""10 0, 0 3.5, 10 7""/></marker>
    <marker id=""arrR"" markerWidth=""10"" markerHeight=""7"" refX=""9"" refY=""3.5"" orient=""auto""><polygon points=""0 0, 10 3.5, 0 7""/></marker>
    <pattern id=""hatch"" width=""8"" height=""8"" patternUnits=""userSpaceOnUse"" patternTransform=""rotate(45)"">
      <line x1=""0"" y1=""0"" x2=""0"" y2=""8"" stroke=""#333"" stroke-width=""1""/>
    </pattern>
  </defs>

  <text x=""50"" y=""30"" font-style=""italic"" font-size=""12"">Выше стрелки - размеры детали</text>
  <text x=""50"" y=""45"" font-style=""italic"" font-size=""12"">Ниже стрелки - размеры поковки</text>
  <text x=""550"" y=""30"" font-style=""italic"" font-size=""12"">Масса номинал {nomMass:F3} тонны</text>
  <text x=""550"" y=""45"" font-style=""italic"" font-size=""12"">Масса максимал {maxMass:F3} тонны</text>

  <rect x=""40"" y=""60"" width=""720"" height=""300"" fill=""none"" stroke=""#000"" stroke-width=""1""/>

  <g transform=""translate({cX - rectW / 2}, {cY - rectH / 2})"">
    
    <!-- 1. ПОКОВКА (СИНЯЯ ГРАНИЦА И ШТРИХОВКА) -->
    <!-- Левая заштрихованная часть -->
    <rect x=""0"" y=""0"" width=""{wall}"" height=""{rectH}"" fill=""url(#hatch)"" stroke=""#007bff"" stroke-width=""2""/>
    <!-- Правая заштрихованная часть -->
    <rect x=""{rectW - wall}"" y=""0"" width=""{wall}"" height=""{rectH}"" fill=""url(#hatch)"" stroke=""#007bff"" stroke-width=""2""/>
    <!-- Центральное отверстие поковки -->
    <rect x=""{wall}"" y=""0"" width=""{holeW}"" height=""{rectH}"" fill=""#fff"" stroke=""#007bff"" stroke-width=""1.5""/>

    <!-- 2. ДЕТАЛЬ -->
    // Левая вертикальная стенка детали
double detX1 = offsetX; 
double detX2 = wall - offsetHole;
double detX3 = rectW - wall + offsetHole;
double detX4 = rectW - offsetX;
double detY1 = offsetY;
double detY2 = rectH - offsetY;

svg += $@""
<rect x=""""{{detX1}}"""" y=""""{{detY1}}"""" width=""""{{detX2 - detX1}}"""" height=""""{{detY2 - detY1}}"""" fill=""""none"""" stroke=""""#000"""" stroke-width=""""2""""/>

<!-- Правая часть детали -->
<rect x=""""{{detX3}}"""" y=""""{{detY1}}"""" width=""""{{detX4 - detX3}}"""" height=""""{{detY2 - detY1}}"""" fill=""""none"""" stroke=""""#000"""" stroke-width=""""2""""/>

<!-- Соединительные горизонтальные линии (границы отверстия детали) -->
<line x1=""""{{detX2}}"""" y1=""""{{detY1}}"""" x2=""""{{detX3}}"""" y2=""""{{detY1}}"""" stroke=""""#000"""" stroke-width=""""2""""/>
<line x1=""""{{detX2}}"""" y1=""""{{detY2}}"""" x2=""""{{detX3}}"""" y2=""""{{detY2}}"""" stroke=""""#000"""" stroke-width=""""2""""/>"";

    <!-- 3. ОСЕВАЯ ЛИНИЯ -->
    <line x1=""{rectW / 2}"" y1=""-40"" x2=""{rectW / 2}"" y2=""120"" stroke=""#f60"" stroke-dasharray=""20,5,5,5"" stroke-width=""1""/>

    <!-- 4. РАЗМЕРНЫЕ ЛИНИИ -->
    <line x1=""{wall}"" y1=""0"" x2=""{wall}"" y2=""-30"" stroke=""#000""/>
    <line x1=""{rectW - wall}"" y1=""0"" x2=""{rectW - wall}"" y2=""-30"" stroke=""#000""/>
    <line x1=""{wall}"" y1=""-20"" x2=""{rectW - wall}"" y2=""-20"" stroke=""#000"" marker-start=""url(#arrL)"" marker-end=""url(#arrR)""/>
    <text x=""{rectW / 2}"" y=""-35"" text-anchor=""middle"" font-size=""14"">ø{dd_det:F0}</text>
    <text x=""{rectW / 2}"" y=""-7"" text-anchor=""middle"" font-size=""13"">{dd_f:F0} -{innerTol}</text>

    <line x1=""0"" y1=""{rectH}"" x2=""0"" y2=""{rectH + 40}"" stroke=""#000""/>
    <line x1=""{rectW}"" y1=""{rectH}"" x2=""{rectW}"" y2=""{rectH + 40}"" stroke=""#000""/>
    <line x1=""0"" y1=""{rectH + 30}"" x2=""{rectW}"" y2=""{rectH + 30}"" stroke=""#000"" marker-start=""url(#arrL)"" marker-end=""url(#arrR)""/>
    <text x=""{rectW / 2}"" y=""{rectH + 25}"" text-anchor=""middle"" font-size=""14"">ø{D_det:F0}</text>
    <text x=""{rectW / 2}"" y=""{rectH + 50}"" text-anchor=""middle"" font-size=""13"">{D_f:F0} ±{delta2}</text>

    <line x1=""{rectW}"" y1=""0"" x2=""{rectW + 30}"" y2=""0"" stroke=""#000""/>
    <line x1=""{rectW}"" y1=""{rectH}"" x2=""{rectW + 30}"" y2=""{rectH}"" stroke=""#000""/>
    <line x1=""{rectW + 20}"" y1=""0"" x2=""{rectW + 20}"" y2=""{rectH}"" stroke=""#000"" marker-start=""url(#arrL)"" marker-end=""url(#arrR)""/>
    <text x=""{rectW + 35}"" y=""{rectH / 2 - 5}"" text-anchor=""start"" font-size=""14"">{H_det:F0}</text>
    <text x=""{rectW + 35}"" y=""{rectH / 2 + 15}"" text-anchor=""start"" font-size=""13"">{H_f:F0}</text>
  </g>

  <g transform=""translate({cX}, 340) scale(0.8)"">
    <line x1=""0"" y1=""0"" x2=""40"" y2=""0"" stroke=""#000"" marker-end=""url(#arrR)""/>
    <line x1=""0"" y1=""0"" x2=""0"" y2=""-40"" stroke=""#000"" marker-end=""url(#arrR)""/>
    <text x=""45"" y=""5"" font-size=""12"">X</text>
    <text x=""-5"" y=""-45"" font-size=""12"">Y</text>
  </g>
</svg>";

            return svg;
        }

    }
}