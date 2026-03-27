using Microsoft.EntityFrameworkCore;               // основной функционал ef core
using ProjectManagement.Models;                   // наши модели

namespace ProjectManagement.Data
{
    // контекст базы данных - главный класс, через который мы работаем с бд
    public class AppDbContext : DbContext
    {
        // dbset представляют таблицы в базе данных
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

        // метод настройки подключения к бд
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // используем sqlite, база данных будет лежать в файле projectmanagement.db в папке проекта
            optionsBuilder.UseSqlite("Data Source=projectmanagement.db");
        }

        // метод, где мы задаём дополнительную конфигурацию моделей через fluent api
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // переименовываем таблицы: в бд они будут называться не "projects", а "projectslist"
            modelBuilder.Entity<Project>().ToTable("ProjectsList");
            modelBuilder.Entity<TaskItem>().ToTable("TasksList");

            // для сущности project переименовываем столбец name в projectname
            modelBuilder.Entity<Project>()
                .Property(p => p.Name)
                .HasColumnName("ProjectName");

            // создаём уникальный индекс на поле name (теперь projectname) 
            // это гарантирует, что не будет двух проектов с одинаковым именем
            modelBuilder.Entity<Project>()
                .HasIndex(p => p.Name)
                .IsUnique();

            // настраиваем связь один-ко-многим между проектом и задачами
            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Project)               // у задачи есть один проект
                .WithMany(p => p.Tasks)               // у проекта много задач
                .HasForeignKey(t => t.ProjectId)      // внешний ключ - projectid
                .OnDelete(DeleteBehavior.Cascade);    // при удалении проекта все его задачи удаляются автоматически

            // дополнительно можно задать тип столбца для даты (необязательно, но для ясности)
            modelBuilder.Entity<TaskItem>()
                .Property(t => t.DueDate)
                .HasColumnType("TEXT");
        }
    }
}