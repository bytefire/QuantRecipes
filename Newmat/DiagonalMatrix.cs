using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newmat
{
    // OkashTODO: add implementation as you go...
    public class DiagonalMatrix<T>:GeneralMatrix<T>
    {
        public DiagonalMatrix(int rows)
            : base(rows, rows)
        {
        }

        // NOTE: for some reason the GetElement metod is not in the base GeneralMatrix class in the original Newmat library.
        //       so in keeping with the original, the GetElement methods have been written separately in 
        //       each of the derived classes.
        public T GetElement(int row, int column)
        {
            return _store[row, column];
        }
    }
}
