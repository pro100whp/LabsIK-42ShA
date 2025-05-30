using System;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace LUExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            // Створюємо матрицю коефіцієнтів A (4x4)
            var A = DenseMatrix.OfArray(new double[,]
            {
                { 0.17, -0.18, 0.19, -5.74 },
                { 0.11, -0.43, 0.15, -0.17 },
                { 0.12, 0.14, 0.16, 0.18 },
                { 0.71, -0.13, -0.41, 0.52 }
            });

            // Вектор правих частин b
            var b = DenseVector.OfArray(new double[] { 1.00, 1.9, 2.00, 1.00 });

            // Знаходимо LU-розклад
            var lu = A.LU();
            var L = lu.L; 
            var U = lu.U; 

            
            double detA = lu.Determinant;

          
            var A_inv = lu.Inverse();

            
            var x = lu.Solve(b);

            
            Console.WriteLine("Матриця L:");
            Console.WriteLine(L.ToMatrixString());
            Console.WriteLine("Матриця U:");
            Console.WriteLine(U.ToMatrixString());
            Console.WriteLine($"Визначник матриці A = {detA}");
            Console.WriteLine("Обернена матриця A^(-1):");
            Console.WriteLine(A_inv.ToMatrixString());
            Console.WriteLine("Розв’язок системи (x):");
            for (int i = 0; i < x.Count; i++)
            {
                Console.WriteLine($"x[{i}] = {x[i]}");
            }
        }
    }
}