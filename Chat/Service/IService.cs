using Chat.Models;
namespace Chat.Service;

public interface IService
{
    int myid { get; set; }
    void PrintAllUser();
    void PrintAllMessage(int myId, int partnerId);
    Task<User> GetUserIdbyUserName(string username);
    int GetLastUserId();
    void Registration();
    Task<bool> LoginAsync();
    Task<List<User>> GetAllUserAsync();
    Task PrintChatMessage(int partnerId);
    Task SendMessage(int partnerId, string text);
    Task CreateGroup();
    Task<List<User>> GetAllUsersAsync();
    Task PrintGroups();
    Task JoinGroup();
    Task GroupSendMessage(int groupId, string text);
    Task<List<Group>> GetAllGroupsAsync();
    Task PrintGroupMessage(int groupId);

}
