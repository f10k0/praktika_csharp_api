using System.ComponentModel.DataAnnotations;      // для атрибутов типа [Key], [Required], [StringLength]
using System.ComponentModel.DataAnnotations.Schema; // для [Column], [ForeignKey]

namespace ProjectManagement.Models
{
    // класс проекта - основная сущность "проект"
    public class Project
    {
        // [key] указывает, что это поле - первичный ключ в бд
        [Key]
        public int Id { get; set; }

        // обязательное поле, не может быть null
        [Required]
        // максимальная длина строки - 100 символов
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        // описание проекта может быть пустым, поэтому тип string? (nullable)
        [StringLength(500)]
        public string? Description { get; set; }

        // дата создания, по умолчанию ставим текущее время utc
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // навигационное свойство для связи один-ко-многим: у проекта может быть много задач
        // при инициализации создаём пустую коллекцию, чтобы избежать null
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}