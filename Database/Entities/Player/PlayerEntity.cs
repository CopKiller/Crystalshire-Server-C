using Database.Entities.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Database.Entities.Player;

public class PlayerEntity
{
    // General Data
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } = 0;

    public string Name { get; set; } = string.Empty;
    public SexType Sexo { get; set; } = SexType.None;
    public ClassType ClassType { get; set; } = ClassType.None;
    public AccessType AccessType { get; set; } = AccessType.Player;
    public EntidadeType Entidade { get; set; } = EntidadeType.Normal;
    public int Sprite { get; set; } = 1;
    public int Level { get; set; } = 1;
    public int Exp { get; set; } = 1;
    public int Points { get; set; } = 0;
    // Position
    public Position Position { get; set; } = new Position();
    // Atributes
    public Stat Stat { get; set; } = new Stat();
    public Vital Vital { get; set; } = new Vital();

    //Aguardando implementações anteriores

    //public Equipment[] Equipment { get; set;} = new Equipment[Enum.GetValues(typeof(EquipmentType)).Length];

    //public PlayerEntity()
    //{
    //    for (int i = 1; i <= Equipment.Length; i++)
    //    {
    //        Equipment[i] = new Equipment();
    //    }
    //}
}