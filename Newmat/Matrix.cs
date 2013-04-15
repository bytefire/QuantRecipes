using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newmat
{
    // OkashTODO: add implementation as you go...
    // OkashTODO: implement this and other matrix classes as generic
    public class Matrix<T>:GeneralMatrix<T>
    {
        public Matrix(int rows, int columns)
            : base(rows, columns)
        {
        }

        public T GetElement(int row, int column)
        {
            return _store[row, column];
        }
    }
}
