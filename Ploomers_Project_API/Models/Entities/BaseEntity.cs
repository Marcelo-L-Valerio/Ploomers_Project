using System.ComponentModel.DataAnnotations.Schema;

namespace Ploomers_Project_API.Models.Entities
{
    public class BaseEntity
    {
        [Column("id")]
        public Guid Id { get; set; }
    }
}
