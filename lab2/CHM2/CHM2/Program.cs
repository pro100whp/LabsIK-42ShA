using System;

class NewtonMethod
{
    
    static double Epsilon = 1e-6;

    static double f(double x)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        return Math.Pow(Math.Log10(x), 2) + (5.0 / 3) * Math.Log10(x) - 2.0 / 3;
    }

    static double f_derivative(double x)
    {
        return (2 * Math.Log10(x) / (x * Math.Log(10))) + (5.0 / (3 * x * Math.Log(10)));
    }

    static double g(double x)
    {
        return Math.Pow(Math.Log10(x), 2) - (2.0 / 3) * Math.Log10(x) + 1.0 / 9;
    }

    static double g_derivative(double x)
    {
        return (2 * Math.Log10(x) / (x * Math.Log(10))) - (2.0 / (3 * x * Math.Log(10)));
    }

    static double NewtonMethodSolve(Func<double, double> func, Func<double, double> func_derivative, double x0)
    {
        double x = x0;
        int maxIterations = 100;
        int iter = 0;

        while (Math.Abs(func(x)) > Epsilon && iter < maxIterations)
        {
            x = x - func(x) / func_derivative(x);
            iter++;
        }

        return x;
    }

    static double SimplifiedNewtonSolve(Func<double, double> func, double x0)
    {
        double x = x0;
        double f_prime_x0 = f_derivative(x0);
        int maxIterations = 100;
        int iter = 0;

        while (Math.Abs(func(x)) > Epsilon && iter < maxIterations)
        {
            x = x - func(x) / f_prime_x0;
            iter++;
        }

        return x;
    }

    public static void Main()
    {
        double[] epsilons = { 1e-4, 1e-5, 1e-6 };
        double initialGuess = 0.5;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        foreach (var eps in epsilons)
        {
            Epsilon = eps;
            Console.WriteLine($"Точність: {eps}");
            Console.WriteLine($"Метод Ньютона для f(x): {NewtonMethodSolve(f, f_derivative, initialGuess)}");
            Console.WriteLine($"Спрощений метод Ньютона для f(x): {SimplifiedNewtonSolve(f, initialGuess)}");
            Console.WriteLine($"Метод Ньютона для g(x): {NewtonMethodSolve(g, g_derivative, initialGuess)}");
            Console.WriteLine($"Спрощений метод Ньютона для g(x): {SimplifiedNewtonSolve(g, initialGuess)}");
            Console.WriteLine();
        }
    }
}
