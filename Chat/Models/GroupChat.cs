
namespace Chat.Models;

public class GroupChat
{
    public GroupChat(int userId, int groupId)
    {
        UserId = userId;
        GroupId = groupId;
    }

    public int UserId { get; set; }
    public int GroupId { get; set; }
}
