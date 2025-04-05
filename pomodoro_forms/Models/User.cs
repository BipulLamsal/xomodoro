using SQLite;

public class User
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }        
    public string Nickname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}