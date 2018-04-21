using System;
using System.Collections.Generic;
using System.Text;

namespace SimplexMethod
{
    enum MathSign { GreaterThan, LessThan, Equal }

    class LinearProgrammingProblem
    {
        public bool Maximize { get; set; }
        public double[] CriteriaCoefficients { get; set; }
        public double[,] ConstraintCoefficients { get; set; }
        public double[] Constants { get; set; }
        public MathSign[] ConstraintSigns { get; set; }
        // По умолчанию предполагается, что на остальные переменные наложено условие неотрицательности
        public int[] NotNonNegativeVarInd { get; set; }

        public int[] SlackAndSurplusVarInd { get; set; }
        public int[] ArtificialVarInd { get; set; }
        public int[] NewNonNegativeVarInd { get; set; }

        public LinearProgrammingProblem(bool maximize, double[] criteriaCoefficients, double[,] constraintCoefficients, double[] constants, MathSign[] constraintSigns, int[] notNonNegativeVariables)
        {
            if (criteriaCoefficients.Length != constraintCoefficients.GetLength(1))
            {
                throw new Exception("Несовпадение числа переменных в целевой функции и ограничениях. Необходимо проставить нулевые коэффициенты");
            }
            if (constants.Length != constraintCoefficients.GetLength(0))
            {
                throw new Exception("Несовпадение числа свободных членов и числа ограничений");
            }
            Maximize = maximize;

            if(notNonNegativeVariables != null)
            {
                ConstraintCoefficients = new double[constraintCoefficients.GetLength(0), constraintCoefficients.GetLength(1) + 1];
                CriteriaCoefficients = new double[criteriaCoefficients.Length + 1];
                for (int i = 0; i < constraintCoefficients.GetLength(0); i++)
                {
                    for (int j = 0; j < constraintCoefficients.GetLength(1); j++)
                    {
                        ConstraintCoefficients[i, j] = constraintCoefficients[i, j];
                    }
                }
                for (int i = 0; i < criteriaCoefficients.Length; i++)
                {
                    CriteriaCoefficients[i] = criteriaCoefficients[i];
                }
            }
            else
            {
                ConstraintCoefficients = constraintCoefficients;
                CriteriaCoefficients = criteriaCoefficients;
            }
            Constants = constants;
            ConstraintSigns = constraintSigns;
            NotNonNegativeVarInd = notNonNegativeVariables;
        }

        public void ConvertToStandardForm()
        {
            ToMax();
            MakeConstantsPositive();
            MakeAllVarNonNegative();
        }

        // Приведение задачи минимизации к задаче максимизации min(F)=max(-F)
        private void ToMax()
        {
            if (Maximize == false)
            {
                for (int i = 0; i < CriteriaCoefficients.Length; i++)
                {
                    CriteriaCoefficients[i] *= -1;
                }
            }
        }

        // Обеспечение положительности столбца свободных членов 
        public void MakeConstantsPositive()
        {
            for (int i = 0; i < Constants.Length; i++)
            {
                if (Constants[i] <= 0)
                {
                    Constants[i] *= -1;
                    for (int j = 0; j < ConstraintCoefficients.Length; j++)
                    {
                        ConstraintCoefficients[i, j] *= -1;

                    }
                    if (ConstraintSigns[i] == MathSign.GreaterThan)
                    {
                        ConstraintSigns[i] = MathSign.LessThan;
                    }
                    else if (ConstraintSigns[i] == MathSign.LessThan)
                    {
                        ConstraintSigns[i] = MathSign.GreaterThan;
                    }
                }
            }
        }

        // При присутствии переменных без ограничения на неотрицательность вводятся новые переменные x1 и x2
        // с данным ограничением такие, что x = x1 - x2
        public void MakeAllVarNonNegative()
        {
            if (NotNonNegativeVarInd != null)
            {
                double coeffCrit = 0;
                double coeffConstr = 0;
                for(int i = 0; i < ConstraintCoefficients.GetLength(0); i++)
                {
                    foreach(int ind in NotNonNegativeVarInd)
                    {
                        if(i == 0)
                        {
                            coeffCrit -= CriteriaCoefficients[ind];
                        }
                        coeffConstr -= ConstraintCoefficients[i, ind];
                    }
                    ConstraintCoefficients[i, ConstraintCoefficients.GetLength(1) - 1] = coeffConstr;
                    coeffConstr = 0;
                }
                CriteriaCoefficients[CriteriaCoefficients.Length - 1] = coeffCrit;
            }
        }
    }
}
