using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace AstroCalculator.DataModels
{
    public class SNRData
    {
        [Required]
        [DefaultValue("None")]
        public string? FilterValue = "None";

        [Required]
        [DefaultValue(0.0)]
        public double LowerMagnitudeLimit = 0.0;

        [Required]
        [DefaultValue(14.0)]
        public double UpperMagnitudeLimit = 14.0;

        [Required]
        [DefaultValue(1.0)]
        public double MagInterval = 1.0;

        [Required]
        [DefaultValue(36.0)]
        public double TelescopeDiameterCM = 36.0;

        [Required]
        [DefaultValue(0.8)]
        [Range(0.0, 1.0, ErrorMessage = "QE Must be between 0.0 and 1.0")]
        public double OverallQE = 0.8; // In the original code there is a dropdown option depending on CCD which substitutes the value used at the right.

        [Required]
        [DefaultValue(0.24)]
        public double PixelSizeArcsecPerPixel = 0.24;
        
        [Required]
        [DefaultValue(3.5)]
        public double CCDReadoutNoise = 3.5;

        [Required]
        [DefaultValue(17)]
        public double SkyBrightnessMagPerSqArcsec = 17; // In the original code there is a dropdown option depending on the location (suburbs, city, etc)

        [Required]
        [DefaultValue(1.2)]
        public double AirMass = 1.2;

        [Required]
        [DefaultValue(300.0)]
        public double ExposureTimeInSeconds = 300;

        [Required]
        [DefaultValue(1.5)]
        public double FWHMArcsec = 1.5;

        [Required]
        [DefaultValue(6.0)]
        public double RadiusForPhotometry = 6.0; // Area over which signal is measured (arcsec)

    }
}
