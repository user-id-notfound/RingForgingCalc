namespace ForgingCalc.Models
{
    public class ForgingInput
    {
        public double D { get; set; }      
        public double dd { get; set; }    
        public double H { get; set; }
        public int x { get; set; }
        public double y { get; set; }
        public int z { get; set; }
        public double Q { get; set; }
        public double AllowanceHT { get; set; }     
    }

    public class ForgingResult
    {
        public double D { get; set; }
        public double dd { get; set; }     
        public double H { get; set; }
        public int x { get; set; }
        public double y { get; set; }
        public int z { get; set; }
        public double Q { get; set; }
        public double Density { get; set; } = 7.85;
        public double H1 { get; set; }
        public double H2 { get; set; }
        public int b_no_probe { get; set; }
        public int delta2_no_probe { get; set; }
        public int b_probe { get; set; }
        public int delta2_probe { get; set; }
        public double D_forg_no_probe { get; set; }
        public double dd_forg_no_probe { get; set; } 
        public double H_forg_no_probe { get; set; }
        public double D_forg_probe { get; set; }
        public double dd_forg_probe { get; set; }    
        public double H_forg_probe { get; set; }
        public double MassNominalNoProbe { get; set; }
        public double MassMaxNoProbe { get; set; }
        public double MassNominalProbe { get; set; }
        public double MassMaxProbe { get; set; }
        public double V_disk_nom_np { get; set; }
        public double V_hole_nom_np { get; set; }
        public double V_disk_max_np { get; set; }
        public double V_hole_max_np { get; set; }
        public double V_disk_nom_p { get; set; }
        public double V_hole_nom_p { get; set; }
        public double V_disk_max_p { get; set; }
        public double V_hole_max_p { get; set; }
        public string SvgNoProbe { get; set; }
        public string SvgProbe { get; set; }
    }
}