using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantRecipes.Utilities
{
    public static class Constants
    {
        public const int IA = 16807;
        public const int IM = 2147483647;
        public const double AM = (1.0 / IM);
        public const int IQ = 127773;
        public const int IR = 2836;
        public const int NTAB = 32;
        public const double NDIV = (1 + (IM - 1) / NTAB);
        public const double EPS = 0.00000012;
        public const double RNMX = (1.0 - EPS);
    }
}
