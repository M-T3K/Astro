using System;


namespace AstroCalculator.Helpers
{
    public static class AstroMath
    {

        public static double ErrorFunction(double x)
        {
            // constants
            double a1 = 0.254829592;
            double a2 = -0.284496736;
            double a3 = 1.421413741;
            double a4 = -1.453152027;
            double a5 = 1.061405429;
            double p = 0.3275911;

            // Save the sign of x
            int sign = 1;
            if (x < 0)
                sign = -1;
            x = Math.Abs(x);

            // A&S formula 7.1.26
            double t = 1.0 / (1.0 + p * x);
            double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

            return sign * y;
        }



        // Comment copied from original code at http://spiff.rit.edu/richmond/signal.cgi
        // Accessed on 07/12/24
        // ############################################################################
        // # PROCEDURE: fraction_inside
        // # 
        // # DESCRIPTION: figure out what fraction of a star's light falls within
        // #              the aperture.  We assume that the starlight has a circular
        // #              gaussian distribution with FWHM given by the first argument
        // #              (with units of arcsec).  We calculate the fraction of 
        // #              that light which falls within an aperture of radius given
        // #              by second argument (with units of arcsec).
        // #              
        // # RETURNS:     the fraction of light within aperture.

        public static double QuickFraction(double fwhm, double radius)
        {

            double large = 1000.0;

            // calculate how far out the "radius" is in units of "sigmas"

            var sigma = fwhm / 2.35;
            var z = radius / (sigma * 1.414);

            // # now, we assume that a radius of "large" is effectively infinite
            var x1 = AstroMath.ErrorFunction(z);

            var ratio = x1 * x1;
            return ratio;
        }


        // Comment copied from original code at http://spiff.rit.edu/richmond/signal.cgi
        // Accessed on 07/12/24
        //############################################################################
        //# PROCEDURE: fraction_inside_slow
        //# 
        //# DESCRIPTION: figure out what fraction of a star's light falls within
        //# the aperture.  We assume that the starlight has a circular
        //# gaussian distribution with FWHM given by the first argument
        //#              (with units of arcsec).  
        //#               
        //# This function goes to the trouble of calculating how
        //# much of the light falls within fractional pixels defined
        //# by the given radius of a synthetic aperture.  It is slow,
        //# but more accurate than the "fraction_inside" function.
        //#
        //#              3/2/2004  Fixed a bug in addition of light within 
        //# the given aperture; wasn't handling fractional
        //# pixel apertures properly.  Thanks to David Whysong.
        //# MWR
        //#
        //#              8/30/2004 By default, the code now assumes that the star
        //# is centered on a pixel (not on the junction of 
        //# four adjacent pixels).  It now also takes into
        //# account the pixel size.
        //#              
        //# RETURNS:     the fraction of light within aperture.
        //#
        public static double AccurateFraction(double fwhm, double radius, double pixelSize)
        {
            double piece = 20;

            if (pixelSize < 0) throw new NotImplementedException("Pixel Size is smaller than 0, cannot calculate");

            //rescale FWHM and aperture radius into pixels(instead of arcsec)
            fwhm /= pixelSize;
            radius /= pixelSize;

            var maxPixelRadius = 30;

            if (radius >= maxPixelRadius) throw new NotSupportedException($"The radius is greater than the built in limit of {maxPixelRadius}");

            // these values control the placement of the star on the pixel grid:
            //    (0,0) to make the star centered on a junction of four pixels
            //    (0.5, 0.5) to make star centered on one pixel

            var psf_center_x = 0.5;
            var psf_center_y = 0.5;

            var sigma2 = fwhm / 2.35;
            sigma2 = sigma2 * sigma2;

            var radius2 = radius * radius;

            var bit = 1.0 / piece;

            double rad_sum = 0.0;
            double all_sum = 0.0;

            for (double i = 0.0 - maxPixelRadius; i < maxPixelRadius; i++)
            {
                for (double j = 0.0 - maxPixelRadius; j < maxPixelRadius; j++)
                {

                    //now, how much light falls into pixel(i, j)?
                    var pixelSum = 0.0;
                    for (var k = 0.0; k < piece; k++)
                    {

                        var x = (i - psf_center_x) + (k + 0.5) * bit;
                        var fx = Math.Exp( -(x*x) / (2.0*sigma2) );

                        for(var l = 0.0; l < piece; l++)
                        {
                            var y = (j - psf_center_y) + (l - 0.5) * bit;
                            var fy = Math.Exp( -(y*y) / (2.0 * sigma2) );  

                            var inten = fx * fy;
                            var this_bit = inten * bit * bit;
                            pixelSum += this_bit;

                            var rad2 = x*x + y*y;
                            if (rad2 <= radius2)
                            {
                                rad_sum += this_bit;
                            }
                        }

                    }

                    all_sum += pixelSum;
                }

            }

            var ratio = rad_sum / all_sum;

            return ratio;
        }

    }
}
