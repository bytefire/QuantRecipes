using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newmat
{
    // OkashTODO: add implementation as you go...
    public class SymmetricMatrix<T>:GeneralMatrix<T>
    {
        public SymmetricMatrix(int rows, int columns)
            : base(rows, columns)
        {
        }
        // OkashTODO: candidate to go in a base class
        public int GetNumberOfRows()
        {
            return _store.GetLength(0);
        }
        // OkashTODO: candidate to go in a base class
        public int GetNumberOfColumns()
        {
            return _store.GetLength(1);
        }

        public DiagonalMatrix<T> GetEigenValues()
        {
            throw new NotImplementedException();
        }
        public Matrix<T> GetEigenValues(DiagonalMatrix<T> diagonalMatrix)
        {
            // OkashTODO: does this need DiagonalMatrix argument?
            throw new NotImplementedException();
        }

        public Matrix<T> Cholesky()
        {
            throw new NotImplementedException();
        }
    }
}
