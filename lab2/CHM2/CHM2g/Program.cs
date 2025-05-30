using System;

class SimpleIterationMethod
{
    static double Epsilon = 1e-6;

   
    static double p_iteration(double x)
    {
        return (0.5 - Math.Cos(x)) / 2;
    }

   
    static double g_iteration(double x)
    {
        double value = 1.2 + 0.2 * x * x - 0.3 * x;
        return Math.Pow(Math.Abs(value), 1.0 / 3.0) * Math.Sign(value);
    }

    
    static double SolveBySimpleIteration(Func<double, double> iterationFunc, double x0, out int iterations)
    {
        double x = x0;
        double xPrev;
        int maxIterations = 1000;
        iterations = 0;

        do
        {
            xPrev = x;
            x = iterationFunc(x);
            iterations++;

            // Перевірка на розбіжність
            if (double.IsNaN(x) || double.IsInfinity(x))
            {
                Console.WriteLine("Метод розбігається!");
                return xPrev;
            }

            if (iterations >= maxIterations)
            {
                Console.WriteLine($"Метод не зійшовся за {maxIterations} ітерацій!");
                return x;
            }
        } while (Math.Abs(x - xPrev) >= Epsilon);

        return x;
    }

    // Перевірка збіжності (похідна повинна бути < 1)
    static bool CheckConvergence(Func<double, double> func, double x)
    {
        double h = 1e-6;
        double derivative = (func(x + h) - func(x - h)) / (2 * h);
        return Math.Abs(derivative) < 1;
    }

    public static void Main()
    {
        double[] epsilons = { 1e-4, 1e-5, 1e-6 };
        double initialGuessP = -0.4; // Оптимізоване початкове наближення для p(x)
        double initialGuessG = 1.3;  // Початкове наближення для g(x)
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        foreach (var eps in epsilons)
        {
            Epsilon = eps;
            Console.WriteLine($"Точність: {eps}");

            // Для p(x)
            Console.WriteLine($"Перевірка збіжності для p(x) при x0 = {initialGuessP}: " +
                            CheckConvergence(p_iteration, initialGuessP));
            int iterationsP;
            double resultP = SolveBySimpleIteration(p_iteration, initialGuessP, out iterationsP);
            Console.WriteLine($"Метод простої ітерації для p(x): {resultP:F8}, " +
                            $"кількість ітерацій: {iterationsP}");

            // Для g(x)
            Console.WriteLine($"Перевірка збіжності для g(x) при x0 = {initialGuessG}: " +
                            CheckConvergence(g_iteration, initialGuessG));
            int iterationsG;
            double resultG = SolveBySimpleIteration(g_iteration, initialGuessG, out iterationsG);
            Console.WriteLine($"Метод простої ітерації для g(x): {resultG:F8}, " +
                            $"кількість ітерацій: {iterationsG}");

            Console.WriteLine();
        }
    }
}