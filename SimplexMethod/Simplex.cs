using System;
using System.Collections.Generic;
using System.Text;

namespace SimplexMethod
{
    class Simplex
    {
        public LinearProgrammingProblem Prob { get; set; }
        public double[,] SimplexTable { get; set; }
        // Симплексная таблица:
        // 0 строка - коэффициенты при переменных в целевой функции
        // 1-n строки 1-m столбцы - коэффициенты при переменных в соответсвующих ограничениях
        // n+1 строка - оценки
        // n+2 строка - оценки: коэффициент при M
        // 0 столбец - свободные члены
        // m+1 столбец - 
        // Базис хранится отдельно
        public int[] Basis { get; set; }
        public List<int> SlackVarInd { get; set; }
        public List<int> SurplusVarInd { get; set; }
        public List<int> ArtificialVarInd { get; set; }
        private int varInd;
        private const double M = double.PositiveInfinity;

        public Simplex(LinearProgrammingProblem prob)
        {
            Prob = prob;
            SimplexTable = new double[Prob.CountConstraint + 3,
                Prob.CountVariables + CountTotalNewVars() + 2];
            varInd = Prob.CountVariables + 1;
            Basis = new int[Prob.CountConstraint];
            for (int i = 0; i < Prob.CountConstraint + 1; i++)
            {
                for (int j = 0; j < Prob.CountVariables + 1; j++)
                {
                    if (i == 0 && j != 0)
                    {
                        SimplexTable[0, j] = prob.CriteriaCoefficients[j - 1];
                    }
                    else if(i != 0 && j == 0)
                    {
                        SimplexTable[i, 0] = prob.Constants[i - 1];
                    }
                    else if (i == 0 && j == 0)
                    {
                        continue;
                    }
                    else {
                        SimplexTable[i, j] = prob.ConstraintCoefficients[i - 1, j - 1];
                    }
                }
            }

        }
        
        public int CountTotalNewVars()
        {
            int count = 0;
            for(int i = 0; i < Prob.CountConstraint; i++)
            {
                if((Prob.ConstraintSigns[i] == MathSign.LessThan && Prob.Constants[i] >= 0) || 
                    (Prob.ConstraintSigns[i] == MathSign.GreaterThan && Prob.Constants[i] < 0))
                {
                    count++;
                }
                else if((Prob.ConstraintSigns[i] == MathSign.GreaterThan && Prob.Constants[i] >= 0) ||
                    (Prob.ConstraintSigns[i] == MathSign.LessThan && Prob.Constants[i] < 0))
                {
                    count += 2;
                }
                else if(Prob.ConstraintSigns[i] == MathSign.Equal)
                {
                    count++;
                }
            }
            if(Prob.NotNonNegativeVarInd != null)
            {
                count++;
            }
            return count;
        }

        public void ConvertToStandardForm()
        {
            ToMax();
            MakeConstantsPositive();
            MakeAllVarNonNegative();
            AddSlackAndSurplusVariables();
        }

        // Приведение задачи минимизации к задаче максимизации min(F)=max(-F)
        public void ToMax()
        {
            if (Prob.Maximize == false)
            {
                for (int i = 1; i < Prob.CountVariables + 1; i++)
                {
                    SimplexTable[0,i] *= -1;
                }
            }
        }

        // Обеспечение положительности столбца свободных членов 
        public void MakeConstantsPositive()
        {
            for (int i = 0; i < Prob.CountConstraint; i++)
            {
                if (Prob.Constants[i] <= 0)
                {
                    for(int j = 0; j < SimplexTable.GetLength(1) - 1; j++)
                    {
                        SimplexTable[i + 1, j] *= -1;
                    }
                }
            }
        }

        // При присутствии переменных без ограничения на неотрицательность вводятся новые переменные x1 и x2
        // с данным ограничением такие, что x = x1 - x2
        public void MakeAllVarNonNegative()
        {
            if (Prob.NotNonNegativeVarInd != null)
            {
                double coeffCrit = 0;
                double coeffConstr = 0;
                for (int i = 0; i < Prob.CountConstraint + 1; i++)
                {
                    foreach (int ind in Prob.NotNonNegativeVarInd)
                    {
                        if (i == 0)
                        {
                            coeffCrit -= SimplexTable[0, ind + 1];
                        }
                        coeffConstr -= SimplexTable[i, ind + 1];
                    }
                    SimplexTable[i, varInd] = coeffConstr;
                    coeffConstr = 0;
                }
                SimplexTable[0, varInd] = coeffCrit;
                varInd++;
            }
        }
                
        public void AddSlackAndSurplusVariables()
        {
            SlackVarInd = new List<int>();
            SurplusVarInd = new List<int>();
            for (int i = 0; i < Prob.CountConstraint; i++)
            {
                if ((Prob.ConstraintSigns[i] == MathSign.LessThan && Prob.Constants[i] >= 0) ||
                    (Prob.ConstraintSigns[i] == MathSign.GreaterThan && Prob.Constants[i] < 0))
                {
                    SlackVarInd.Add(varInd);
                    SimplexTable[i + 1, varInd] = 1;
                    Basis[i] = varInd;
                    varInd++;
                }
                else if ((Prob.ConstraintSigns[i] == MathSign.GreaterThan && Prob.Constants[i] >= 0) ||
                    (Prob.ConstraintSigns[i] == MathSign.LessThan && Prob.Constants[i] < 0))
                {
                    SurplusVarInd.Add(varInd);
                    SimplexTable[i + 1, varInd] = -1;
                    varInd++;
                }
            }
        }

        public void AddArtificialVariables()
        {
            ArtificialVarInd = new List<int>();
            for (int i = 0; i < Prob.CountConstraint; i++)
            {
                if ((Prob.ConstraintSigns[i] == MathSign.GreaterThan && Prob.Constants[i] >= 0) ||
                    (Prob.ConstraintSigns[i] == MathSign.LessThan && Prob.Constants[i] < 0) ||
                    Prob.ConstraintSigns[i] == MathSign.Equal)
                {
                    ArtificialVarInd.Add(varInd);
                    SimplexTable[i + 1, varInd] = 1;
                    // SimplexTable[0, varInd] = Double.PositiveInfinity;
                    SimplexTable[0, varInd] = -M;
                    Basis[i] = varInd;
                    varInd++;
                }
            }
        }

        private void FindCosts()
        {
            int costMRow = SimplexTable.GetLength(0) - 1;
            int costRow = costMRow - 1;
            for (int j = 0; j < SimplexTable.GetLength(1) - 1; j++)
            {
                SimplexTable[costRow, j] = 0;
                SimplexTable[costMRow, j] = 0;
                for (int i = 1; i < SimplexTable.GetLength(0) - 2; i++)
                {
                    if (SimplexTable[0, Basis[i - 1]] == -M)
                    {
                        SimplexTable[costMRow, j] += (SimplexTable[i, j] * (-1));
                    }
                    else
                    {
                        SimplexTable[costRow, j] += (SimplexTable[i, j] * SimplexTable[0, Basis[i - 1]]);
                    }
                }
                if (SimplexTable[0, j] == -M)
                {
                    SimplexTable[costMRow, j] += 1;
                }
                else
                {
                    SimplexTable[costRow, j] -= SimplexTable[0, j];
                }
            }
        }

        public void Calculate(ref double optObjectiveValue, ref double[] optSolution)
        {
            ConvertToStandardForm();
            AddArtificialVariables();
            FindCosts();
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
