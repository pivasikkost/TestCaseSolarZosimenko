using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCase.Model;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace TestCaseSolarZosimenkoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new TestCaseDbDataContext(); //инициализация контекста
            int n; //переменная-ключ
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Введите номер функции:");
                Console.WriteLine("1 - Просмотр списка задач;");
                Console.WriteLine("2 - Добавление задачи;");
                Console.WriteLine("3 - Редактирование задачи;");
                Console.WriteLine("4 - Удаление задачи;");
                Console.WriteLine("0 - Выход.");
                Console.WriteLine();
                Console.Write("Ваш выбор: ");
                n = int.Parse(Console.ReadLine());    //считывание пункта меню
                Console.WriteLine();

                switch (n)
                {
                    case 1:
                        ViewTasks(context);
                        break;
                    case 2:
                        AddTask(context);
                        break;
                    case 3:
                        EditTask(context);
                        break;
                    case 4:
                        DeleteTask(context);
                        break;
                    case 0:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Некорректный ввод!");
                        break;
                }
                Console.ReadLine(); //задержка информации на экране
            }
        
        }
        
        //вывод данных
        public static void ViewTasks(TestCaseDbDataContext context)
        {
            var tasks = context.Tasks.ToList();

            Console.WriteLine("|ID| |Дата|             |Описание|");

            foreach (var task in tasks)
            {
                Console.WriteLine(" {0}   {1} {2}", task.ID, task.Date, task.Description);
            }
        }

        //добавление новой задачи в таблицу
        public static void AddTask(TestCaseDbDataContext context)
        {
            string date, description;
            Console.WriteLine("Введите дату исполнения задачи");
            date = Console.ReadLine();
            Console.WriteLine("Введите описание задачи");
            description = Console.ReadLine();

            var newTask = new Tasks
            {
                Date = Convert.ToDateTime(date),
                Description = description
            };
            context.Tasks.InsertOnSubmit(newTask);
            context.Tasks.Context.SubmitChanges();
            Console.WriteLine();
            Console.WriteLine("Ввод окончен");
        }

        //удаление задачи из таблицы
        public static void DeleteTask(TestCaseDbDataContext context)
        {
            int id;
            Console.WriteLine("Введите идентификатор задачи");
            id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();

            var delTask = context.Tasks.Where(p => p.ID == id).FirstOrDefault();
            if (delTask != null)
            {
                context.Tasks.DeleteOnSubmit(delTask);
                context.Tasks.Context.SubmitChanges();

                Console.WriteLine("Задача с ID = {0} удалена", id);
            }
            else Console.WriteLine("Задачи с введённым ID нет в списке!");
        }

        //редактирование задачи
        public static void EditTask(TestCaseDbDataContext context)
        {
            int id;
            string date, description;
            Console.WriteLine("Введите идентификатор задачи");
            id = Convert.ToInt32(Console.ReadLine());

            var editTask = context.Tasks.Where(p => p.ID == id).FirstOrDefault();
            if (editTask != null)
            {
                Console.WriteLine("Введите дату исполнения задачи");
                date = Console.ReadLine();
                Console.WriteLine("Введите описание задачи");
                description = Console.ReadLine();

                editTask.Date = Convert.ToDateTime(date);
                editTask.Description = description;
                context.Tasks.Context.SubmitChanges();

                Console.WriteLine();
                Console.WriteLine("Редактирование завершено", id);
            }
            else Console.WriteLine("Задачи с введённым ID нет в списке!");
            
        }
    }
}