
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace Table4_2_Solution;

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Система рівнянь з таблиці 4.2:");

        // Матриця коефіцієнтів
        var A = Matrix<double>.Build.DenseOfArray(new[,]
        {
            { 4.247, 0.275, 0.397, 0.239 },
            { 0.466, 4.235, 0.264, 0.358 },
            { 0.204, 0.501, 3.721, 0.297 },
            { 0.326, 0.421, 0.254, 3.286 }
        });

        // Вектор вільних членів
        var b = Vector<double>.Build.Dense(new[] { 0.721, 0.339, 0.050, 0.486 });

        var x0 = Vector<double>.Build.Dense(4); // Початкове наближення (нульовий вектор)
        const double epsilon = 0.001;

        Console.WriteLine("\nПеревірка умов збіжності:");
        CheckConvergenceConditions(A);

        Console.WriteLine("\nМетод Якобі:");
        var (jacobiSol, jacobiIter, jacobiThird) = SolveByJacobi(A, b, x0, epsilon, 100);
        PrintResults("Якобі", jacobiSol, jacobiIter, A, b, jacobiThird);

        Console.WriteLine("\nМетод Гауса-Зейделя:");
        var (gsSol, gsIter, gsThird) = SolveByGaussSeidel(A, b, x0, epsilon, 100);
        PrintResults("Гауса-Зейделя", gsSol, gsIter, A, b, gsThird);
    }

    private static void CheckConvergenceConditions(Matrix<double> A)
    {
        var isDiagonallyDominant = true;
        for (int i = 0; i < A.RowCount; i++)
        {
            var diag = Math.Abs(A[i, i]);
            var offDiagSum = A.Row(i).L1Norm() - diag;
            Console.WriteLine($"Рядок {i + 1}: |{A[i, i]:F3}| > {offDiagSum:F3} → {diag > offDiagSum}");
            if (diag <= offDiagSum) isDiagonallyDominant = false;
        }

        Console.WriteLine(isDiagonallyDominant
            ? "\nМатриця діагонально домінантна — методи Якобі та Гауса-Зейделя збіжні"
            : "\nУВАГА: Матриця не діагонально домінантна — збіжність не гарантована");
    }
    //якобі
    private static (Vector<double>, int, Vector<double>) SolveByJacobi(Matrix<double> A, Vector<double> b, Vector<double> x0, double epsilon, int maxIterations)
    {
        var x = x0.Clone();
        var xNew = Vector<double>.Build.Dense(x.Count);
        Vector<double> xAtThird = null!;
        int iteration = 0;
        double error;

        do
        {
            for (int i = 0; i < x.Count; i++)
            {
                double sum = A.Row(i).DotProduct(x) - A[i, i] * x[i];
                xNew[i] = (b[i] - sum) / A[i, i];
            }

            error = (xNew - x).InfinityNorm();
            x = xNew.Clone();
            iteration++;

            if (iteration == 3)
                xAtThird = x.Clone();

        } while (error >= epsilon && iteration < maxIterations);

        return (x, iteration, xAtThird);
    }
    //гаус
    private static (Vector<double>, int, Vector<double>) SolveByGaussSeidel(Matrix<double> A, Vector<double> b, Vector<double> x0, double epsilon, int maxIterations)
    {
        var x = x0.Clone();
        Vector<double> xAtThird = null!;
        int iteration = 0;
        double error;

        do
        {
            var xPrev = x.Clone();

            for (int i = 0; i < x.Count; i++)
            {
                double sum = 0;
                for (int j = 0; j < x.Count; j++)
                    if (j != i) sum += A[i, j] * x[j];
                x[i] = (b[i] - sum) / A[i, i];
            }

            error = (x - xPrev).InfinityNorm();
            iteration++;

            if (iteration == 3)
                xAtThird = x.Clone();

        } while (error >= epsilon && iteration < maxIterations);

        return (x, iteration, xAtThird);
    }

    private static void PrintResults(string method, Vector<double> x, int iterations, Matrix<double> A, Vector<double> b, Vector<double> thirdApprox)
    {
        Console.WriteLine($"Ітерацій: {iterations}");
        Console.WriteLine($"Розв'язок: {FormatVector(x)}");
        var residual = A * x - b;
        Console.WriteLine($"Вектор нев'язок r = Ax - b: {FormatVector(residual)}");

        var trueErrorAtThird = (A * thirdApprox - b).L2Norm();
        Console.WriteLine($"Похибка на 3-й ітерації ({method}): {trueErrorAtThird:F6}");
    }

    private static string FormatVector(Vector<double> v)
    {
        return string.Join(", ", v.Select(x => x.ToString("F6")));
    }
}
