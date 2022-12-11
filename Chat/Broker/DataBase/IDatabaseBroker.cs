using Chat.Models;
using System.Data.SqlClient;

namespace Chat.Broker.DataBase;

public interface IDatabaseBroker
{
    Task InsertUserAsync(User user);
    Task<int> GetLastUserIdAsync();
    Task<int> GetLastMessageIdAsync();
    Task<int> GetLastGroupIdAsync();
    Task<int> GetLastGroupMessageIdAsync();
    Task<List<Message>> GetMessageAsync(int myId, int partnerId);
    Task<List<User>> GetAllUserAsync(int myId);
    Task<List<User>> GetAllUserAsync();

    //Task<List<User>> GetGroupUserAsync(int chatId);
    //Task<List<User>> GetGroupMessageAsync(int chatId);
    Task InsertGroupAsync(Group group);
    Task DeleteUserAsync(int id);
    Task<List<Message>> GetChatMessageAsync(int myId, int PartnerId);
    Task<string> GetUserNameByIdAsync(int id);
    SqlConnection GetConnectionString();
    Task<List<MessageGroup>> GetGroupMessageAsync(int chatId);
    Task<List<Group>> GetGroups(int id);
    Task InsertUserGroup(GroupChat groupChat);
    Task<Group> GetGrouopById(int id);
    Task<List<Group>> GetAllGroups();
    Task<Group> GetGrouopByName(string name);

    //Task JoinToGroup(int myid, int groupId);
}
