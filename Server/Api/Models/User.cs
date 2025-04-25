namespace Api.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public bool IsOnline { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    //nav
    public List<Message> Messages = [];

}