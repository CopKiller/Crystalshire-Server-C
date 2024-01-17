namespace Database.Entities.Account;

using Player;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class AccountEntity
{
    public const byte Max_Char = 3;

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } = 0;

    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime LastLoginDate { get; set; } = DateTime.Now;
    public PlayerEntity[] Player { get; set; } = new PlayerEntity[4];

    public AccountEntity()
    {
        // Adicionando elementos nos índices de 1 até o Max_Char
        for (var i = 1; i <= Max_Char; i++) Player[i] = new PlayerEntity();
    }
}