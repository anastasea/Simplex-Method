using System;
using System.Collections.Generic;
using System.Text;

namespace SimplexMethod
{
    class Simplex
    {
        public LinearProgrammingProblem Prob { get; set; }
        public double[,] SimplexTable { get; set; }

        public Simplex(LinearProgrammingProblem prob)
        {
            Prob = prob;
            SimplexTable = new double[Prob.Constants.Length + 1, 
                Prob.ConstraintCoefficients.GetLength(1) + Prob.ArtificialVarInd.Length + Prob.SlackAndSurplusVarInd.Length + 3];
            // for(int  )
            {

            }
        }

        public void Calculate(ref double f, ref double[] optSolution)
        {

        }

        private int FindPivotColumn()
        {
            return 0;
        }

        private int FindPivotRow()
        {
            return 0;
        }
    }
}
