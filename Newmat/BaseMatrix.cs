using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newmat
{
    /// <summary>
    /// Base class of all matrices.
    /// </summary>
    public class BaseMatrix<T>
    {
        protected virtual int Search(BaseMatrix<T> baseMatrix)
        {
            throw new NotImplementedException();
        }
    }
}
