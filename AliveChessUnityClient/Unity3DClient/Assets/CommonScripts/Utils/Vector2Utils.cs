using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.CommonScripts.Utils
{
    public static class Vector2Utils
    {
        public static double Distance(float x1, float y1, float x2, float y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
    }
}
