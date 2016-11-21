using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace ConsoleFileEmployee
{
    [Serializable]
    abstract class Employee
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public abstract decimal GetSalary { get; set; }
        protected Employee(string name, int age)
        {
            Name = name;
            Age = age;
        }

        protected abstract decimal Salary();
        public override string ToString()
        {
            return Name + " " + Age + " " + Salary();
        }
    }

    [Serializable]
    sealed class BadEmployee : Employee
    {
        public double Stavka { get; private set; }

        public BadEmployee(string name, int age, double stavka)
            : base(name, age)
        {
            Stavka = stavka;
            GetSalary = Salary();
        }

        public override decimal GetSalary { get; set; }

        protected override decimal Salary()
        {
            return (decimal)(20.8 * 8 * Stavka);
        }
    }

    [Serializable]
    sealed class GoodEmployee : Employee
    {
        public decimal Oklad { get; private set; }
        public GoodEmployee(string name, int age, decimal oklad)
            : base(name, age)
        {
            Oklad = oklad;
            GetSalary = Salary();
        }

        public override decimal GetSalary { get; set; }

        protected override decimal Salary()
        {
            return Oklad;
        }
    }

    class Program
    {
        private static List<Employee> _employee;

        static void Main()
        {
            _employee = new List<Employee>()
            {
                // Имя, Возраст, Ставка/Оклад
                new GoodEmployee("Dmitrenko",22,1000),
                new GoodEmployee("Pipipenko",25,1500),
                new GoodEmployee("Savchenko",14,800),
                new BadEmployee("Filipchuk",20,15),
                new BadEmployee("Konenko",20,10),
                new BadEmployee("Uzun",25,11)
            };
            // Сортировка по убыванию оклада
            _employee = new List<Employee>(_employee.OrderByDescending(a => a.GetSalary));
            var punktA = _employee;

            // Сортировка по убыванию зарплаты
            /*
            _employee.Sort(delegate (Employee x, Employee y)
            {
                if (x.GetSalary == 0 && y.GetSalary == 0)
                    return 0;
                if (x.GetSalary == 0)
                    return 1;
                if (y.GetSalary == 0)
                    return -1;
                return y.GetSalary.CompareTo(x.GetSalary);
            });
            */

            // Вывод на экран данных списка служащих
            foreach (var e in punktA)
            {
                Console.WriteLine(e);
            }
            SeparateLine();

            // Вывод на экран данных первых пяти служащих
            var punktB = punktA.Take(5);
            foreach (var e in punktB)
            {
                Console.WriteLine(e);
            }
            SeparateLine();

            // Вывод на экран имён первых пяти служащих
            foreach (var q in punktA.Take(3))
            {
                Console.WriteLine(q.Name);
            }
            SeparateLine();

            // Вывод на экран имён первых трёх служащих, чьи имена начинаются на "D"
            var punktC = from t in _employee // определяем каждый объект из _employee как t
                         where t.Name.StartsWith("D") // фильтрация по критерию
                         orderby t.Age  // упорядочиваем по возрастанию
                         select t; // выбираем объект
            foreach (var q in punktC.Take(3))
            {
                Console.WriteLine(q.Name);
            }
            SeparateLine();

            // Вывод на экран имён первых трёх служащих, чьи имена начинаются на "D"
            var punktD = _employee.Where(t => t.Name.ToUpper().StartsWith("D")).OrderBy(t => t.Age).Take(3);
            foreach (var q in punktD)
            {
                Console.WriteLine(q.Name);
            }
            SeparateLine();

            // Бинарный контейнер
            var binary = new BinaryFormatter();

            // Сериализация данных служащих и сохранение в файл data.dat
            using (var fs = new FileStream("data.dat", FileMode.Create))
            {
                binary.Serialize(fs, _employee);
                Console.WriteLine("Даные записаны в data.dat");
            }
            SeparateLine();

            // Десериализация данных служащих и чтение из файла data.dat с выводом на экран
            try
            {
                var res = binary.Deserialize(File.Open("data.dat", FileMode.Open)) as List<Employee>;
                if (res != null)
                {
                    foreach (var a in res)
                    {
                        Console.WriteLine(a);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
            Console.ReadKey();
        }

        private static void SeparateLine()
        {
            Console.WriteLine(new string('*', 20));
        }
    }
}
