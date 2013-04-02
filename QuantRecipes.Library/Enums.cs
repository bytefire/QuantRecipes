using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantRecipes.Library
{
    public enum OptionExercise
    {
        European,
        American,
    }

    public enum OptionType : int
    {
        Call = 1,
        Put = -1,
    }
}
