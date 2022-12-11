namespace Chat.Models;

public class Group
{
    public Group(int id, string name, int ownerId)
    {
        Id = id;
        Name = name;
        OwnerId = ownerId;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public int OwnerId { get; set; }
}
