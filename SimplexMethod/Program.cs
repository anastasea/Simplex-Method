using System;

namespace SimplexMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            LinearProgrammingProblem pr = new LinearProgrammingProblem(true, new double[] { 4, 1, 2 }, new double[,]
            {{1,1,1}, {1,0,-1}}, new double[] { 4, 2 }, new MathSign[] { MathSign.Equal, MathSign.Equal }, new int[] { 2 });

            pr.MakeAllVarNonNegative();
            for(int i = 0; i < pr.CriteriaCoefficients.Length; i++)
            {
                Console.Write("   " + pr.CriteriaCoefficients[i]);
            }
            Console.WriteLine();
            for (int i = 0; i < pr.ConstraintCoefficients.GetLength(0); i++)
            {
                for (int j = 0; j < pr.ConstraintCoefficients.GetLength(1); j++)
                {
                    Console.Write("  " + pr.ConstraintCoefficients[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
