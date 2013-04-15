using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newmat
{
    public class GeneralMatrix<T> : BaseMatrix<T>
    {
        protected T[,] _store;

        public GeneralMatrix(int rows, int columns)
        {
            _store = new T[rows, columns];
        }
    }
}
