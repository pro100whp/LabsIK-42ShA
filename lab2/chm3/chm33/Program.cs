using System;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace ConditionNumberExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            int N = 5; // Оставляем N = 5, как в исходном коде

            for (int n = 3; n <= 8; n++)
            {
                // Формування матриці A розміром n x n
                var A = Matrix<double>.Build.Dense(n, n, (i, j) =>
                {
                    // Вычисляем c_ij по формуле из исходного кода
                    double c_ij = 0.1 * N * (i + 1) * (j + 1);

                    // Вычисляем a_ij по вашей формуле: a_ij = 150 / (13 * c^3 + 777 * c)
                    double a_ij = 150.0 / (13.0 * Math.Pow(c_ij, 3) + 777.0 * c_ij);
                    return a_ij;
                });

                // Формування вектора b розмірності n, де всі елементи рівні N
                var b = Vector<double>.Build.Dense(n, N);

                // Обчислення Евклідової норми матриці (2-норма – найбільше сингулярне число)
                var svd = A.Svd();
                double matrixNorm = svd.S.Maximum();

                // Обчислення числа обумовленості матриці A
                double condNumber = svd.ConditionNumber;

                // Обчислення Евклідової норми вектора b
                double vectorNorm = b.L2Norm();

                // Виводимо результати
                Console.WriteLine($"n = {n}");
                Console.WriteLine("--------------");
                Console.WriteLine($"Евклідова норма матриці = {matrixNorm}");
                Console.WriteLine($"Евклідова норма вектора = {vectorNorm}");
                Console.WriteLine($"Число обумовленості = {condNumber}");
                Console.WriteLine();
            }
        }
    }
}