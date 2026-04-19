using Microsoft.AspNetCore.Mvc;
using ForgingCalc.Models;
using ForgingCalc.Services;
using System;

namespace ForgingCalc.Controllers
{
    public class ForgingController : Controller
    {
        private readonly IDrawingService _drawingService;

        public ForgingController(IDrawingService drawingService)
        {
            _drawingService = drawingService;
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Calculate(ForgingInput input)
        {
            if (!ModelState.IsValid) return View("Index", input);

            var result = new ForgingResult
            {
                D = input.D,
                dd = input.dd,
                H = input.H,
                x = input.x,
                y = input.y,
                z = input.x - 1,
                Q = input.Q,
                Density = 7.85 
            };

            result.H1 = input.H * input.x + input.y * result.z + input.AllowanceHT;
            result.H2 = input.H * input.x + input.y * result.z + input.Q + input.AllowanceHT;

            var (b1, d2_1) = ToleranceService.GetTolerance(result.H1, input.D);
            result.b_no_probe = b1;
            result.delta2_no_probe = d2_1;

            var (b2, d2_2) = ToleranceService.GetTolerance(result.H2, input.D);
            result.b_probe = b2;
            result.delta2_probe = d2_2;

            double D_part = input.D + input.AllowanceHT;
            double d_part = input.dd - input.AllowanceHT;

            result.D_forg_no_probe = D_part + b1;
            result.dd_forg_no_probe = d_part - b1;
            result.H_forg_no_probe = result.H1 + b1;

            result.D_forg_probe = D_part + b2;
            result.dd_forg_probe = d_part - b2;
            result.H_forg_probe = result.H2 + b2;

            var mNomNP = ToleranceService.CalculateMassDetails(
                result.D_forg_no_probe,
                result.dd_forg_no_probe,
                result.H_forg_no_probe);

            result.V_disk_nom_np = mNomNP.vOut;
            result.V_hole_nom_np = mNomNP.vIn;
            result.MassNominalNoProbe = mNomNP.massOut - mNomNP.massIn;

            double maxD1 = result.D_forg_no_probe + d2_1;
            double minD1 = result.dd_forg_no_probe - (3 * d2_1); 
            double maxH1 = result.H_forg_no_probe + d2_1;

            var mMaxNP = ToleranceService.CalculateMassDetails(maxD1, minD1, maxH1);
            result.V_disk_max_np = mMaxNP.vOut;
            result.V_hole_max_np = mMaxNP.vIn;
            result.MassMaxNoProbe = mMaxNP.massOut - mMaxNP.massIn;

            var mNomP = ToleranceService.CalculateMassDetails(
                result.D_forg_probe,
                result.dd_forg_probe,
                result.H_forg_probe);

            result.V_disk_nom_p = mNomP.vOut;
            result.V_hole_nom_p = mNomP.vIn;
            result.MassNominalProbe = mNomP.massOut - mNomP.massIn;

            double maxD2 = result.D_forg_probe + d2_2;
            double minD2 = result.dd_forg_probe - (3 * d2_2);
            double maxH2 = result.H_forg_probe + d2_2;

            var mMaxP = ToleranceService.CalculateMassDetails(maxD2, minD2, maxH2);
            result.V_disk_max_p = mMaxP.vOut;
            result.V_hole_max_p = mMaxP.vIn;
            result.MassMaxProbe = mMaxP.massOut - mMaxP.massIn;

            var (svgNoProbe, svgProbe) = _drawingService.GenerateDrawings(result);
            result.SvgNoProbe = svgNoProbe;
            result.SvgProbe = svgProbe;

            return View("Result", result);
        }
    }
}