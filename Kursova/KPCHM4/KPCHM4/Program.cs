using System;
using System.Text;
using MathNet.Numerics.LinearAlgebra;



internal class Program
{
    private static void Main()
    {
        Console.OutputEncoding = Encoding.Unicode;

        var C = Matrix<double>.Build.DenseOfArray(new[,]
        {
            { 0.01, 0.02, -0.62, 0.08 },
            { 0.03, 0.28,  0.33, -0.07 },
            { 0.09, 0.13,  0.42,  0.28 },
            { 0.19, -0.23, 0.08,  0.37 }
        });

        var d = Vector<double>.Build.Dense(new[]
        {
            -1.3,
            1.1,
            -1.7,
            1.5
        });

        const double epsilon = 0.001;

        Console.WriteLine("1. Перевірка умов збіжності:");
        var norm = C.InfinityNorm();
        Console.WriteLine($"   Норма матриці C: ||C||∞ = {norm:F6}");

        if (norm >= 1)
        {
            Console.WriteLine("   УВАГА: ||C||∞ ≥ 1 - метод може не збігатися!");
            return;
        }

        Console.WriteLine("   Умова збіжності ||C||∞ < 1 виконується.\n");

        Console.WriteLine("2. Обчислення необхідної кількості ітерацій:");
        var x0 = d.Clone();
        var x1 = C * x0 + d;
        var initialError = (x1 - x0).InfinityNorm();
        var q = norm;
        //обчислення теоритичної кількості ітерації
        var estimatedIterations = (int)Math.Ceiling(Math.Log(epsilon * (1 - q) / initialError) / Math.Log(q));
        Console.WriteLine($"   Початкова похибка: {initialError:F6}");
        Console.WriteLine($"   Теоретична кількість ітерацій: {estimatedIterations}\n");

        Console.WriteLine("3. Ітераційний процес:");
        var x = x0.Clone();
        var iteration = 0;
        double error;

        Console.WriteLine("   Початкове наближення:");
        PrintVector(x);
        //ітераційний цикл
        do
        {
            var xPrev = x.Clone();
            //формула ітерації
            x = C * xPrev + d;
            iteration++;
            error = (x - xPrev).InfinityNorm();

            Console.WriteLine($"\n   Ітерація {iteration}:");
            PrintVector(x);
            Console.WriteLine($"   Похибка: {error:F6}");

            if (iteration > 100)
            {
                Console.WriteLine("   Досягнуто максимальну кількість ітерацій (100).");
                break;
            }

        } while (error >= epsilon);

        Console.WriteLine("\n4. Результати:");
        Console.WriteLine($"   Виконано ітерацій: {iteration}");
        Console.WriteLine($"   Досягнута похибка: {error:F6}");
        Console.WriteLine("   Фінальний розв'язок:");
        PrintVector(x);
    }

    private static void PrintVector(Vector<double> vector)
    {
        Console.WriteLine($"   x1 = {vector[0]:F6}");
        Console.WriteLine($"   x2 = {vector[1]:F6}");
        Console.WriteLine($"   x3 = {vector[2]:F6}");
        Console.WriteLine($"   x4 = {vector[3]:F6}");
    }
}
