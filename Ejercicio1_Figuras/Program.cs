// Parcial #3 - Ejercicio 1: Clase abstracta, arreglo de tipo padre y polimorfismo
using System;

namespace Ejercicio1_Figuras
{

    public abstract class Figura
    {

        public abstract double CalcularArea();
        public abstract double CalcularPerimetro();

        public string Nombre => GetType().Name;
    }

    public class Rectangulo : Figura
    {
        public double Base { get; }
        public double Altura { get; }

        public Rectangulo(double baseRect, double altura)
        {
            Base = baseRect;
            Altura = altura;
        }

        public override double CalcularArea() => Base * Altura;

        public override double CalcularPerimetro() => 2 * (Base + Altura);
    }

    public class Cuadrado : Figura
    {
        public double Lado { get; }

        public Cuadrado(double lado)
        {
            Lado = lado;
        }

        public override double CalcularArea() => Lado * Lado;

        public override double CalcularPerimetro() => 4 * Lado;
    }

    public class Triangulo : Figura
    {
        public double LadoA { get; }
        public double LadoB { get; }
        public double LadoC { get; }

        public Triangulo(double ladoA, double ladoB, double ladoC)
        {
            if (ladoA + ladoB <= ladoC || ladoA + ladoC <= ladoB || ladoB + ladoC <= ladoA)
                throw new ArgumentException("Los lados no forman un triángulo válido.");

            LadoA = ladoA;
            LadoB = ladoB;
            LadoC = ladoC;
        }

        // Área con la fórmula de Herón.
        public override double CalcularArea()
        {
            double s = CalcularPerimetro() / 2;
            return Math.Sqrt(s * (s - LadoA) * (s - LadoB) * (s - LadoC));
        }

        public override double CalcularPerimetro() => LadoA + LadoB + LadoC;
    }

    internal class Program
    {
        static void Main(string[] args)
        {

            Figura[] figuras = new Figura[]
            {
                new Rectangulo(8, 5),
                new Cuadrado(4),
                new Triangulo(3, 4, 5),
                new Rectangulo(2.5, 10),
                new Triangulo(6, 6, 6)
            };

            Console.WriteLine("=== Parcial #3 - Figuras geométricas (Polimorfismo) ===\n");
            Console.WriteLine($"{"Figura",-12}{"Área",12}{"Perímetro",14}");
            Console.WriteLine(new string('-', 38));


            foreach (Figura figura in figuras)
            {
                Console.WriteLine($"{figura.Nombre,-12}{figura.CalcularArea(),12:F2}{figura.CalcularPerimetro(),14:F2}");
            }
        }
    }
}
