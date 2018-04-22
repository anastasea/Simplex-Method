using System;

namespace SimplexMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            LinearProgrammingProblem pr = new LinearProgrammingProblem(true, new double[] { 5,3,-6 }, new double[,]
            {{-2,3,0}, {1,-2,-1}, {0,3,-2}  }, new double[] { 5,4,2 }, new MathSign[] { MathSign.LessThan, MathSign.GreaterThan, MathSign.Equal }, null);

            Simplex sm = new Simplex(pr);
            double d = 0; double[] dd = new double[3];
            sm.Calculate(ref d, ref dd);
            for(int i = 0; i < sm.SimplexTable.GetLength(0); i++)
            {
                for(int j =0; j < sm.SimplexTable.GetLength(1); j++)
                {
                    Console.Write(sm.SimplexTable[i, j] + "   ");
                }
                Console.WriteLine();
            }
            foreach(int i in sm.SlackVarInd)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();
            foreach (int i in sm.SurplusVarInd)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();
            foreach (int i in sm.ArtificialVarInd)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine("basis");
            foreach (int i in sm.Basis)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
