using System;
using System.Text;

namespace QLearning.Common
{
    public static class QTableExtensions
    {
        public static double[][] NormalizeMatrix(this double[][] matrix)
        {
            var maximumElement = matrix.GetMaxElement();

            var normalizedMatrix = new double[matrix.Length][];
            for (int i = 0; i < matrix.Length; i++)
            {
                normalizedMatrix[i] = new double[matrix[i].Length];
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    if (matrix[i][j] != 0)
                        normalizedMatrix[i][j] = Math.Round((matrix[i][j] * 100) / maximumElement, 0);
                }
            }
            return normalizedMatrix;
        }

        public static double GetMaxElement(this double[][] matrix)
        {
            double maxElement = 0;
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    if (matrix[i][j] > maxElement)
                        maxElement = matrix[i][j];
                }
            }
            return maxElement;
        }

        public static string Print(this double[][] matrix)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    sb.Append(string.Format("{0}   ", matrix[i][j]));
                }
                sb.AppendLine(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
