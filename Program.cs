using System;
using System.Collections.Generic;

namespace lab4
{
    public class AnalyticHierarchyProcess
    {
        public static string RunAnalyticHierarchyProcess(string[] alternative,
                                                         double[][] A,
                                                         double[][] B,
                                                         double[][] C,
                                                         double[][] D,
                                                         double[][] critery)
        {
            List<List<double>> Apoln = GetPoln(A);
            List<List<double>> Bpoln = GetPoln(B);
            List<List<double>> Cpoln = GetPoln(C);
            List<List<double>> Dpoln = GetPoln(D);
            List<List<double>> critPoln = GetPoln(critery);
            Console.WriteLine("Дополненная матрица А:");
            foreach (List<double> item in Apoln)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("Дополненная матрица B:");
            foreach (List<double> value in Bpoln)
            {
                Console.WriteLine(value);
            }
            Console.WriteLine("Дополненная матрица C:");
            foreach (List<double> list in Cpoln)
            {
                Console.WriteLine(list);
            }
            Console.WriteLine("Дополненная матрица D:");
            foreach (List<double> arrayList in Dpoln)
            {
                Console.WriteLine(arrayList);
            }
            Console.WriteLine("Дополненная матрица критериев:");
            foreach (List<double> doubleArrayList in critPoln)
            {
                Console.WriteLine(doubleArrayList);
            }

            Console.WriteLine("Матрица из столбцов нормализованных сумм: ");
            List<List<double>> matrix = GetMatrix(Apoln, Bpoln, Cpoln, Dpoln);
            foreach (List<double> arrayList in matrix)
            {
                Console.WriteLine(arrayList);
            }

            Console.WriteLine("Матрица нормализованных сумм критериев: ");
            List<double> critMatrix = new List<double>();
            foreach (List<double> doubles in critPoln)
            {
                critMatrix.Add(doubles[doubles.Count - 1]);
            }
            Console.WriteLine(critMatrix);

            List<double> resultVector = MultiplyMatrixAndVector(matrix, critMatrix);
            Console.WriteLine("Результат перемножения матриц: ");
            Console.WriteLine(resultVector);

            return alternative[FindMaxIndex(resultVector)];
        }

        public static int FindMaxIndex(List<double> vector)
        {
            double max = double.NegativeInfinity;
            int maxIndex = -1;
            for (int i = 0; i < vector.Count; i++)
            {
                if (vector[i] > max)
                {
                    max = vector[i];
                    maxIndex = i;
                }
            }
            return maxIndex;
        }

        public static List<double> MultiplyMatrixAndVector(List<List<double>> matrix, List<double> vector)
        {
            int numColsA = matrix[0].Count;
            int numRowsB = vector.Count;
            if (numColsA != numRowsB)
            {
                throw new ArgumentException("Несоответствие размеров матрицы и вектора");
            }
            List<double> result = new List<double>();
            foreach (List<double> doubles in matrix)
            {
                double sum = 0.0;
                for (int j = 0; j < numColsA; j++)
                {
                    sum += doubles[j] * vector[j];
                }
                result.Add(sum);
            }
            return result;
        }

        public static List<List<double>> GetMatrix(List<List<double>> A,
                                                   List<List<double>> B,
                                                   List<List<double>> C,
                                                   List<List<double>> D)
        {
            List<List<double>> res = new List<List<double>>();
            for (int i = 0; i < A.Count; i++)
            {
                List<double> resLine = new List<double>();
                resLine.Add(A[i][A[i].Count - 1]);
                resLine.Add(B[i][B[i].Count - 1]);
                resLine.Add(C[i][C[i].Count - 1]);
                resLine.Add(D[i][D[i].Count - 1]);
                res.Add(resLine);
            }
            return res;
        }

        public static List<List<double>> GetPoln(double[][] matrix)
        {
            List<List<double>> res = new List<List<double>>();
            foreach (double[] row in matrix)
            {
                double sum = 0;
                List<double> resLine = new List<double>();
                foreach (double value in row)
                {
                    resLine.Add(value);
                    sum += value;
                }
                resLine.Add(sum);
                res.Add(resLine);
            }

            foreach (List<double> row in res)
            {
                double normalizedSum = row[row.Count - 1] / SumColumn(res, row.Count - 1);
                row.Add(normalizedSum);
            }
            return res;
        }

        public static double SumColumn(List<List<double>> matrix, int columnIndex)
        {
            double sum = 0;
            foreach (List<double> row in matrix)
            {
                sum += row[columnIndex];
            }
            return sum;
        }

        public static void Main(string[] args)
        {
            double[][] A = {
                new double[] {1, 3, 5, 7},
                new double[] {0.33333, 1, 0.33333, 0.2},
                new double[] {0.2, 3, 1, 0.33333},
                new double[] {0.14268, 5, 3, 1}
            };

            double[][] B = {
                new double[] {1, 7, 1, 0.5},
                new double[] {0.14268, 1, 0.33333, 0.14268},
                new double[] {1, 3, 1, 0.2},
                new double[] {5, 7, 5, 1}
            };

            double[][] C = {
                new double[] {1, 1, 0.2, 0.14268},
                new double[] {1, 1, 0.2, 0.14},
                new double[] {5, 5, 1, 0.33333},
                new double[] {7, 7, 3, 1}
            };

            double[][] D = {
                new double[] {1, 3, 3, 5},
                new double[] {0.33333, 1, 1, 3},
                new double[] {0.33333, 1, 1, 3},
                new double[] {0.2, 0.33333, 0.33333, 1}
            };

            double[][] critery = {
                new double[] {1, 3, 5, 7},
                new double[] {0.33333, 1, 3, 5},
                new double[] {0.2, 0.33333, 1, 3},
                new double[] {0.14286, 0.2, 0.33333, 1}
            };
            string[] alternative = { "МГТС", "Ростелеком", "Акадо", "Qwerty" };
            string result = RunAnalyticHierarchyProcess(alternative, A, B, C, D, critery);
            string GREEN = "\u001B[32m";
            string RESET = "\u001B[0m";
            Console.WriteLine(GREEN + "Результат работы программы." + RESET);
            Console.WriteLine("Лучший выбор: " + result);
        }
    }
}



