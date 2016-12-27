using System;

namespace ConsoleApplication
{
    class Program
    {
        // делегат для представления математической функции
        delegate double Function(double x);

        // делегат для определения точки деления исходного отрезка
        delegate double PointIn(Function f, double a, double b);

        // получение середины отрезка
        static double Midpoint(Function f, double a, double b) => (a + b) / 2;

        // получение точки пересечения хорды с осью OX
        static double Chord(Function f, double a, double b) => a - f(a) * (b - a) / (f(b) - f(a));

        // получение точки пересечения касательной с осью OX
        static double Tangentially(Function f, double a, double b)
        {
            // приближенное вычисление производной в точке a
            var fa = (f(a + 0.001) - f(a)) / 0.001;
            if (fa != 0)
            {
                // получение точки пересечения касательной
                // с осью абсцисс
                var c = a - f(a) / fa;
                // если точка c принадлежит отрезку,
                // выбираем ее
                if (a <= c && c <= b)
                    return c;
            }
            // приближенное вычисление производной в точке b
            var fb = (f(b + 0.001) - f(b)) / 0.001;
            if (fb == 0) throw new Exception("Возможно, на этом отрезке корней нет");
            {
                // получение точки пересечения касательной
                // с осью абсцисс
                var c = b - f(b) / fb;
                if (a <= c && c <= b)
                    return c;
            }
            // функция не удовлетворяет тем свойствам,
            // которые гарантируют существование корня
            throw new Exception("Возможно, на этом отрезке корней нет");
        }

        static double RootEquation(Function f, double a, double b, double eps, PointIn methodDelegate)
        {
            // корень гарантировано существует,
            // если на концах отрезка
            // функция принимает значения различных знаков.
            if (f(a) * f(b) > 0)
                throw new Exception("Возможно, на этом отрезке корней нет");
            // цикл метода - вычисления продолжается до тех пор,
            // пока на одном из концов отрезка не будет получено
            // значение функции с заданной точностью
            while (Math.Abs(f(a)) > eps && Math.Abs(f(b)) > eps)
            {
                // поиск точки деления отрезка - вызов делегата
                var c = methodDelegate(f, a, b);
                // если найденная точка - корень, это решение
                if (f(c) == 0)
                    return c;
                // выбор следующего отрезка
                if (!(f(a) * f(c) < 0))
                    a = c;
                else
                    b = c;
            }
            // выбор приближенного значения корня
            return Math.Abs(f(a)) <= eps ? a : b;
        }


        //функция при 1
        public static double F1(double x) => x - 3;

        //функция при 2
        public static double F2(double x) => Math.Exp(x) - x - 2;

        //функция при 3
        public static double F3(double x) => x * x * x + 4 * x - 3;

        static void Main(string[] args)
        {
            Console.WriteLine("Выберите функцию:");
            Console.WriteLine("y = x - 3 нажмите 1");
            Console.WriteLine("y = e^x - x - 2 нажмите 2");
            Console.WriteLine("y = x^3  + 4x - 3 нажмите 3 ");
            var choice1 = int.Parse(Console.ReadLine());
            Function func;

            switch (choice1)
            {
                case 1:
                    func = F1;
                    break;
                case 2:
                    func = F2;
                    break;
                case 3:
                    func = F3;
                    break;
                default:
                    func = F3;
                    break;
            }

            Console.WriteLine("Выберите метод решения:");
            Console.WriteLine("1 - решение методом деления отрезка пополам");
            Console.WriteLine("2 - решение методом хорд");
            Console.WriteLine("3 - решение методом касательных");
            var choice2 = Console.ReadLine();


            Console.WriteLine("Введите отрезок");
            Console.WriteLine("Введите a");
            var a = double.Parse(Console.ReadLine());
            Console.WriteLine("Введите b");
            var b = double.Parse(Console.ReadLine());
            Console.WriteLine("Введите точность решения");
            var eps = double.Parse(Console.ReadLine());

            // делегат для определения точки деления отрезка
            PointIn methodDelegate;

            switch (choice2)
            {
                case "0":
                    // инициализация делегата функцией Midpoint
                    methodDelegate = Midpoint;
                    Console.WriteLine("Выбран метод деления трезка пополам");
                    break;
                case "1":
                    // инициализация делегата функцией Chord
                    methodDelegate = Chord;
                    Console.WriteLine("Выбран метод хорд");
                    break;
                case "2":
                    // инициализация делегата функцией Tangentially
                    methodDelegate = Tangentially;
                    Console.WriteLine("Выбран метод касательных");
                    break;
                default:
                    methodDelegate = Tangentially;
                    break;
            }
            try
            {
                // вызов метода решения уравнения
                Console.WriteLine("Корень = {0}",
                    RootEquation(func, a, b, eps, methodDelegate));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}