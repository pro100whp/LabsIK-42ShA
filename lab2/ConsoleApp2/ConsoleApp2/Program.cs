using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

class Person
{
    string name;
    string age;
    CallConvThiscall.

class Employee : Person
{
    string company;
    public Employee(string name, int age, string company) : base(name, age)
    {
        
        this.company = company;
        Console.WriteLine("Person(name,  age,  company)");
        Console.WriteLine($"{name}, {age}, {company}");
    }
}
class Program
{
    static void Main(string[] args)
    {
        Employee emp = new Employee("bob", 18, "glovo");
        
        
        
    }
}