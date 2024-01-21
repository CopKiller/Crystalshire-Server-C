using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities.ValueObjects.Player
{
    public class Equipment : BaseEntity
    {
        public int ItemId { get; set; } = 0;
        public byte Bounding { get; set; } = 0;
    }
}
