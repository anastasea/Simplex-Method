using System;

namespace SimplexMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            LinearProgrammingProblem pr = new LinearProgrammingProblem(true, new double[] { 5,3,-6 }, new double[,]
            {{-2,3,0}, {1,-2,-1}, {0,3,-2}  }, new double[] { -5,4,2 }, new MathSign[] { MathSign.LessThan, MathSign.GreaterThan, MathSign.Equal }, null);

            Simplex sm = new Simplex(pr);
            sm.Calculate();
            Console.ReadKey();
        }
    }
}
