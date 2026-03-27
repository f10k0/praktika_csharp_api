using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagement.Models
{
    // класс задачи - вторая сущность
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }

        // название задачи обязательно и не длиннее 200 символов
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        // описание может отсутствовать
        [StringLength(1000)]
        public string? Description { get; set; }

        // статус выполнения: true - выполнена, false - нет
        public bool IsCompleted { get; set; }

        // дата выполнения может быть не задана, поэтому nullable
        public DateTime? DueDate { get; set; }

        // внешний ключ на проект, к которому относится задача
        // через атрибут column задаём имя столбца в таблице (будет "ProjectId")
        [Column("ProjectId")]
        public int ProjectId { get; set; }

        // навигационное свойство для доступа к родительскому проекту
        // foreignkey указывает, что связь идёт по полю projectid
        [ForeignKey("ProjectId")]
        public Project? Project { get; set; }
    }
}