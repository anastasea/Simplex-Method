using System;

namespace SimplexMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            // LinearProgrammingProblem pr = new LinearProgrammingProblem(true, new double[] { 5,3,-6 }, new double[,]
            // {{-2,3,0}, {1,-2,-1}, {0,3,-2}  }, new double[] { -5,4,2 }, new MathSign[] { MathSign.LessThan, MathSign.GreaterThan, MathSign.Equal }, null);

            // LinearProgrammingProblem pr = new LinearProgrammingProblem(true, new double[] { 6, 5, 4 }, new double[,]
            // {{1,1,1}, {1,2,0}, {1,1,3}  }, new double[] { 6,4,6 }, new MathSign[] { MathSign.LessThan, MathSign.LessThan, MathSign.LessThan }, null);

            // LinearProgrammingProblem pr = new LinearProgrammingProblem(true, new double[] { -2,-1,2 }, new double[,]
            // {{2,1,1}, {-1,1,-1} }, new double[] { 1,1 }, new MathSign[] { MathSign.Equal, MathSign.Equal }, null);

            // LinearProgrammingProblem pr = new LinearProgrammingProblem(true, new double[] { 1,3,1 }, new double[,]
            // {{1,-1,1}, {1,1,1} }, new double[] { 1, 4 }, new MathSign[] { MathSign.LessThan, MathSign.LessThan }, new int[] { 0,1,2});

            LinearProgrammingProblem pr = new LinearProgrammingProblem(true, new double[] { 2,-3 }, new double[,]
            {{1,-5}, {7,4}, { -7,2} }, new double[] { -5, 28,0 }, new MathSign[] { MathSign.LessThan, MathSign.LessThan, MathSign.GreaterThan }, new int[] { 1});

            Simplex sm = new Simplex(pr);
            sm.Calculate();
            Console.ReadKey();
        }
    }
}
