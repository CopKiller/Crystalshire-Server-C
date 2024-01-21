using Database.Entities.ValueObjects.Player;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities.Player;

public class PlayerEntity : BaseEntity
{
    public const byte MaxNameCaracteres = 20;
    public const byte MaxInventory = 35;
    public const byte MaxSkill = 35;

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
    public List<Inventory> Inventory { get; set; } = new List<Inventory>();
    public List<Bank> Bank { get; set; } = new List<Bank>();
    public List<Skill> Skill { get; set; } = new List<Skill>();
    public Penalty Penalty { get; set; } = new Penalty();

    public PlayerEntity()
    {
        for (var i = 1; i <= Enum.GetValues(typeof(EquipmentType)).Length; i++)
        {
            Equipment.Add(new Equipment());
        }

        for (var i = 0; i < MaxInventory; i++)
        {
            Inventory.Add(new Inventory());
        }

        for (var i = 0; i < MaxSkill; i++)
        {
            Skill.Add(new Skill());
        }
    }
}