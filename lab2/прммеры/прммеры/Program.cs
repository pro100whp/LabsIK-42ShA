//using System;

//class Animal  // Базовий клас (батьківський)
//{
//    public virtual void animalSound()
//    {
//        Console.WriteLine("Тварина видає звук");
//    }
//}

//class Pig : Animal  // Похідний клас (дочірній)
//{
//    public override void animalSound()
//    {
//        Console.WriteLine("Свиня каже: хрю хрю");
//    }
//}

//class Dog : Animal  // Похідний клас (дочірній)
//{
//    public override void animalSound()
//    {
//        Console.WriteLine("Собака каже: гав гав");
//    }
//}

//class Program
//{
//    static void Main(string[] args)
//    {
//        Console.OutputEncoding = System.Text.Encoding.UTF8;
//        Animal myAnimal = new Animal();  // Створити об’єкт Animal
//        Animal myPig = new Pig();  // Створити об’єкт Pig
//        Animal myDog = new Dog();  // Створити об’єкт Dog

//        myAnimal.animalSound();
//        myPig.animalSound();
//        myDog.animalSound();
//    }
//}
using System;

class Program
{
    static void ShowSum(int a , int b , int c)
    {

        
        Console.WriteLine($"Сума: {a + b + c}");
    }
    static void ShowSum(int a, int b )
    {

        
        
        Console.WriteLine($"Сума: {a + b  }");
    }

    static void Main()
    {
        ShowSum(2, 5, 3);
        ShowSum(2, 5 ); 
    }
}





