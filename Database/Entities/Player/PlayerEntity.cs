using Database.Entities.Account;
using Database.Entities.ValueObjects.Player;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities.Player;

public class PlayerEntity : BaseEntity
{
    public const byte MaxNameCaracteres = 20;
    public const byte MaxInventory = 35;
    public const byte MaxSkill = 35;
    public const byte MaxBank = 99;

    public int AccountEntityId { get; set; }
    public virtual AccountEntity? AccountEntity { get; set; }

    public int SlotId { get; set; } = 0;
    [MaxLength(MaxNameCaracteres)]
    public string Name { get; set; } = string.Empty;
    public SexType Sexo { get; set; } = SexType.None;
    public ClassType ClassType { get; set; } = ClassType.None;
    public AccessType AccessType { get; set; } = AccessType.Player;
    public EntidadeType Entidade { get; set; } = EntidadeType.Normal;
    public int Sprite { get; set; } = 0;
    public int Level { get; set; } = 0;
    public int Exp { get; set; } = 0;
    public int Points { get; set; } = 0;
    // Position
    public virtual Position Position { get; set; } = new Position();
    // Atributes
    public virtual Stat Stat { get; set; } = new Stat();
    public virtual Vital Vital { get; set; } = new Vital();

    public virtual List<Equipment> Equipment { get; set; } = new List<Equipment>();
    public virtual List<Inventory> Inventory { get; set; } = new List<Inventory>();
    public virtual List<Bank> Bank { get; set; } = new List<Bank>();
    public virtual List<Skill> Skill { get; set; } = new List<Skill>();
    public virtual Penalty Penalty { get; set; } = new Penalty();

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

        for (var i = 0; i < MaxBank; i++)
        {
            Bank.Add(new Bank());
        }

        for (var i = 0; i < MaxSkill; i++)
        {
            Skill.Add(new Skill());
        }
    }
}