using Microsoft.AspNetCore.Mvc;
using ForgingCalc.Models;
using ForgingCalc.Services;
using Microsoft.Extensions.Logging;

namespace ForgingCalc.Controllers
{
    /// <summary>
    /// Контроллер для расчёта параметров поковок колец
    /// </summary>
    public class ForgingController : Controller
    {
        private readonly IDrawingService _drawingService;
        private readonly IForgingCalculationService _calculationService;
        private readonly ILogger<ForgingController> _logger;

        public ForgingController(
            IDrawingService drawingService,
            IForgingCalculationService calculationService,
            ILogger<ForgingController> logger)
        {
            _drawingService = drawingService ?? throw new ArgumentNullException(nameof(drawingService));
            _calculationService = calculationService ?? throw new ArgumentNullException(nameof(calculationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Calculate(ForgingInput input)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Некорректные входные данные: {@Input}", input);
                return View("Index", input);
            }

            try
            {
                var result = _calculationService.Calculate(input);

                var (svgNoProbe, svgProbe) = _drawingService.GenerateDrawings(result);
                result.SvgNoProbe = svgNoProbe;
                result.SvgProbe = svgProbe;

                _logger.LogInformation("Успешный расчёт поковки. D={D}, dd={dd}, H={H}", input.D, input.dd, input.H);

                return View("Result", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при расчёте поковки");
                TempData["ErrorMessage"] = "Произошла ошибка при расчёте. Пожалуйста, проверьте входные данные.";
                return View("Index", input);
            }
        }
    }
}