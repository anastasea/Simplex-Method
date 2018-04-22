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

        public int CountConstraint { get; set; }
        public int CountVariables { get; set; } 
        
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
            ConstraintCoefficients = constraintCoefficients;
            CriteriaCoefficients = criteriaCoefficients;
            CountConstraint = ConstraintCoefficients.GetLength(0);
            CountVariables = ConstraintCoefficients.GetLength(1);
            Constants = constants;
            ConstraintSigns = constraintSigns;
            NotNonNegativeVarInd = notNonNegativeVariables;
        }

        
    }
}

