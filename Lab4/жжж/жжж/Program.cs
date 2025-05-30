using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

internal class Program
{
    static void Main(string[] args)
    {
        int[] array = { 1, 2, 3, 22, 33, 17 };

        // Пузырьковая сортировка
        
        
            for (int j = 0; j < array.Length - 1 ; j++) // j++ исправлено
            {
                if (array[j] > array[j + 1])
                {
                    // Обмен значениями
                    int temp = array[j];
                    array[j] = array[j + 1];
                    array[j + 1] = temp;
                }
            }
        

        // Вывод отсортированного массива
        Console.WriteLine(string.Join(", ", array));
    }
}
