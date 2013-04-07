using Newmat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuantRecipes.Utilities;

namespace QuantRecipes.Library
{
    // OkashTODO: add implementation as you go...
    public class MatrixUtil
    {
        private int iset = 0;
        private double gset = 0.0;
        /// <summary>
        /// Computes four correlated deviates for Monte Carlo simulation.
        /// </summary>
        /// <param name="R">Correlation matrix (which is always symmetric).</param>
        /// <param name="dt">Time step.</param>
        /// <param name="z">Array to store correlated deviates.</param>
        /// <returns></returns>
        public double[] GenerateCorrelatedDeviates(SymmetricMatrix R, double dt, double[] z)
        {
            double sum =  0.0;
            // standard normal deviate
            double deviate = 0.0;
            // number of rows in correlation matrix.
            int m = R.GetNumberOfRows();
            // list of correlated deviates
            //List<double> dz;
            // list of eigenvalues
            List<double> eigenValues = new List<double>();
            // array of eigenvectors
            List<double>[] eigenVectors = new List<double>[4];
            // stores eigenvalues of correlation matrix R
            double[] lambda = new double[] { 0.0, 0.0, 0.0, 0.0 };
            // stores correlated deviates
            double[] dw = new double[] { 0.0, 0.0, 0.0, 0.0 };
            
            DiagonalMatrix D = R.GetEigenValues();
            Matrix V = GenerateEigenVectors(R);

            // store eigen values
            for (int i = 0; i < m; i++)
            {
                eigenValues.Add(D.GetElement(i, i));
                lambda[i] = D.GetElement(i, i);
            }

            // stores rows of eigenvectors so that we can compute
            // dz[i] = v[i][1]*sqrt(eigenvalue[1])*dw1 + v[i][2]*sqrt(eigenvalue[2])*dw2 + ...
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    eigenVectors[i].Add(V.GetElement(i, j));
                }
            }

            Random randomNumberGenerator = new Random();
            long seed = randomNumberGenerator.Next() % 100;
            
            // generate uncorrelated deviates
            for (int i = 0; i < m; i++)
            {
                deviate = NormalDeviate(ref seed);
                dw[i] = deviate * Math.Sqrt(dt);
            }

            // generate correlated deviates
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    sum += eigenVectors[i][j] * Math.Sqrt(eigenValues[j]) * dw[j];
                }
                z[i] = sum;
            }
            return z;
        }
        /// <summary>
        /// Computes correlated deviates from Cholesky decomposition.
        /// </summary>
        /// <param name="R">correlation matrix</param>
        /// <param name="dt">step size</param>
        /// <param name="z">correlated deviates array to be returned.</param>
        /// <returns>array of correlated normal deviates</returns>
        /// <remarks>
        /// OkashTODO: do we need the parameter double[] z?? Dont think so. 
        ///            Same question goes to the method GenerateCorrelatedDeviates above.
        /// </remarks>
        public double[] GenerateCorrelatedDeviatesCholesky(SymmetricMatrix R, double dt, double[] z)
        {
            int m = R.GetNumberOfRows();
            int n = R.GetNumberOfColumns();
            StatUtility util = new StatUtility();
            // standard normal deviate
            double deviate = 0.0;
            // stores deviate * sqrt(dt)
            double[] dw = new double[4] { 0.0, 0.0, 0.0, 0.0 };
            double sum = 0.0;
            // lower-banded (lb) matrix
            Matrix lb = R.Cholesky();

            Random randomNumberGenerator = new Random();
            long seed = randomNumberGenerator.Next() % 100;

            // generate uncorrelated deviates
            for (int i = 0; i < m; i++)
            {
                deviate = util.gasdev(ref seed);
                dw[i] = deviate * Math.Sqrt(dt);
            }
            // generate correlated deviates
            for (int i = 0; i < m; i++)
            {
                sum = 0;
                for (int j = 0; j < n; j++)
                {
                    sum += lb.GetElement(i, j) * dw[j];
                }
                z[i] = sum;
            }
            return z;
        }

        private Matrix GenerateEigenVectors(SymmetricMatrix symmetricMatrix)
        {
            // OkashTODO: why do we need the diagonal matrix here??
            DiagonalMatrix D = new DiagonalMatrix(symmetricMatrix.GetNumberOfRows());
            Matrix eigenVectors = symmetricMatrix.GetEigenValues(D);
            return eigenVectors;
        }

        private double NormalDeviate(ref long idum)
        {
            double fac, rsq, v1, v2;
            if (iset == 0)
            {
                do
                {
                    v1 = 2.0 * ran(ref idum) - 1.0;
                    v2 = 2.0 * ran(ref idum) - 1.0;
                    rsq = v1 * v1 + v2 * v2;
                } while (rsq >= 1.0 || rsq == 0.0);
                fac = Math.Sqrt(-2.0 * Math.Log(rsq) / rsq);

                gset = v1 * fac;
                iset = 1;
                return v2 * fac;
            }
            else
            {
                iset = 0;
                return gset;
            }
        }
        private double ran(ref long idum)
        {
            int j;
            long k;
            long iy = 0;
            long[] iv = new long[Constants.NTAB];
            double temp;

            if (idum <= 0 || iy != 0)
            {
                if (-idum < 1)
                {
                    idum = 1;
                }
                else
                {
                    idum = -idum;
                }
                for (j = Constants.NTAB + 7; j >= 0; j--)
                {
                    k = idum / Constants.IQ;
                    idum = Constants.IA * (idum - k * Constants.IQ) - Constants.IR * k;
                    if (idum < 0)
                        idum += Constants.IM;
                    if (j < Constants.NTAB)
                        iv[j] = idum;
                }
                iy = iv[0];
            }

            k = idum / Constants.IQ;
            idum = Constants.IA * (idum - k * Constants.IQ) - Constants.IR * k;
            if (idum < 0)
            {
                idum += Constants.IM;
            }
            j = (int)(iy / Constants.NDIV);
            iy = iv[j];
            iv[j] = idum;
            if ((temp = Constants.AM * iy) > Constants.RNMX)
                return Constants.RNMX;
            else
                return temp;
        }
    }
}
