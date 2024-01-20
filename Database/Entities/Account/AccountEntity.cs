namespace Database.Entities.Account;

using Player;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class AccountEntity : BaseEntity
{
    public const byte MaxChar = 3;
    public const byte MaxAccountCaracteres = 20;
    public const byte MaxEmailCaracteres = Byte.MaxValue;

    [MaxLength(MaxAccountCaracteres)] // Defina o tamanho máximo desejado aqui
    public string Login { get; set; } = string.Empty;

    [MaxLength(MaxAccountCaracteres)] // Defina o tamanho máximo desejado aqui
    public string Password { get; set; } = string.Empty;

    [MaxLength(MaxEmailCaracteres)]
    public string Email { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime LastLoginDate { get; set; } = DateTime.Now;
    public List<PlayerEntity> Players { get; set; } = new List<PlayerEntity>();

    public AccountEntity()
    {   
        for (var i = 0; i < MaxChar; i++)
        {
            Players.Add(new PlayerEntity());
        }
    }
}