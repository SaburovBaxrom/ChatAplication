
namespace Chat.Models;

public class User
{
    public User(int id, string name, string username, string password)
    {
        this.id = id;
        this.name = name;
        this.username = username;
        this.password = password;
    }

    /// user id
    /// </summary>
    public int id { get; set; }
    /// <summary>
    /// user's name
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// user's username
    /// </summary>
    public string username { get; set; }
    /// <summary>
    /// user's password
    /// </summary>
    public string password { get; set; }

}
