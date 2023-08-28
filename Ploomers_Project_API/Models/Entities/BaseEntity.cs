using System.ComponentModel.DataAnnotations.Schema;

namespace Ploomers_Project_API.Models.Entities
{
    // Just a convention, all entities heir from it the ID field, for a more uniform code
    public class BaseEntity
    {
        [Column("id")]
        public required Guid Id { get; set; }
    }
}
