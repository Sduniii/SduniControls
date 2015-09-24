using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SduniControls
{
    static class ColorApi
    {
        public static Color LightenBy(this Color color, int percent)
        {
            return ChangeColorBrightness(color, percent / 100.0);
        }

        private static Color ChangeColorBrightness(Color color, double v)
        {
            double red = (255 - color.R) * v + color.R;
            double green = (255 - color.G) * v + color.G;
            double blue = (255 - color.B) * v + color.B;
            Color lighterColor = Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
            return lighterColor;
        }

        public static Color DarkenBy(this Color color, int percent)
        {
            return ChangeColorBrightness(color, -1 * percent / 100.0);
        }
    }
}
