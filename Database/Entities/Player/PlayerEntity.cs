using Database.Entities.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities.Player;

public class PlayerEntity : BaseEntity
{
    public const byte MaxNameCaracteres = 20;


    [MaxLength(MaxNameCaracteres)]
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

    public List<Equipment> Equipment { get; set; } = new List<Equipment>();

    public PlayerEntity()
    {
        for (var i = 1; i <= Enum.GetValues(typeof(EquipmentType)).Length; i++)
        {
            Equipment.Add(new Equipment());
        }
    }
}