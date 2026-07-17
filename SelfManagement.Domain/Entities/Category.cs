
namespace SelfManagement.Domain.Entities
{
    public class Category : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        // Navigation Properties
        public ICollection<Project> Projects { get; set; } = new List<Project>();

        public ICollection<Todo> Todos { get; set; } = new List<Todo>();

    }
}
