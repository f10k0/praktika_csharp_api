using Microsoft.EntityFrameworkCore;               // для include и других методов
using ProjectManagement.Data;                       // наш контекст бд
using ProjectManagement.Models;                     // модели

namespace ProjectManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            // используем блок using, чтобы после работы контекст был корректно закрыт
            using (var db = new AppDbContext())
            {
                // ensurecreated создаёт базу данных, если она ещё не существует
                // (это нужно, если мы не применяли миграции, но у нас они есть)
                // здесь он просто гарантирует наличие бд.
                db.Database.EnsureCreated();

                // проверяем, есть ли уже проекты в таблице
                if (!db.Projects.Any())
                {
                    // создаём новый проект
                    var project = new Project { Name = "Изучение EF Core" };
                    db.Projects.Add(project);
                    db.SaveChanges();   // сохраняем изменения, чтобы у проекта появился id

                    // создаём две задачи, привязанные к этому проекту
                    var task1 = new TaskItem
                    {
                        Title = "Прочитать документацию",
                        IsCompleted = false,
                        ProjectId = project.Id
                    };
                    var task2 = new TaskItem
                    {
                        Title = "Создать миграцию",
                        IsCompleted = true,
                        ProjectId = project.Id
                    };

                    // добавляем задачи в контекст и сохраняем
                    db.Tasks.AddRange(task1, task2);
                    db.SaveChanges();
                }

                // выводим все проекты и их задачи в консоль
                Console.WriteLine("Проекты:");

                // include загружает связанные задачи для каждого проекта (жадная загрузка)
                foreach (var p in db.Projects.Include(p => p.Tasks))
                {
                    Console.WriteLine($"- {p.Name} (Id={p.Id})");
                    foreach (var t in p.Tasks)
                    {
                        Console.WriteLine($"   * {t.Title} (Завершена: {t.IsCompleted})");
                    }
                }
            }
        }
    }
}