using Chat.Models;
using Chat.Broker.DataBase;
using System.Data.SqlClient;

namespace Chat.Service;

public class Service : IService
{
    IDatabaseBroker broker;
    public int myid { get; set; }

    public Service()
    {
        broker = new DatabaseBroker();
    }

    public async Task<User> GetUserIdbyUserName(string username)
    {
        var userList = await broker.GetAllUserAsync();
        foreach (var item in userList)
        {
            if(item.username == username)
            {
                return item;
            }
        }
        return new User(0,".",".",".");
    }

    public void PrintAllMessage(int myId, int partnerId)
    {
        var messageList = broker.GetMessageAsync(myId, partnerId).Result;
        foreach (var item in messageList)
        {
            Console.WriteLine(item.text);
        }
    }    

    public void PrintAllUser()
    {
        int k = 1;
        var userList = broker.GetAllUserAsync(myid).Result;
        foreach (var item in userList)
        {
            Console.WriteLine(k + ". " + item.name);
            k++;
        }
        Console.Write("Choose :");
    }

    public int GetLastUserId()
    {
        return broker.GetLastUserIdAsync().Result;
    }

    public void Registration()
    {
        Console.Write("Name :");
        string name = Console.ReadLine()!;
        Console.Write("Username :");
        string username = Console.ReadLine()!;
        Console.Write("Password :");
        string password = Console.ReadLine()!;
        int id = GetLastUserId();
        User user = new User(id, name, username, password);
        broker.InsertUserAsync(user);
    }

    public async Task<bool> LoginAsync()
    {
        
        Console.Write("Enter your username :");
        string username = Console.ReadLine()!;
        Console.Write("Enter your password :");
        string password = Console.ReadLine()!;
        var user = await GetUserIdbyUserName(username);
        if(password == user.password )
        {
            myid = user.id;
            return true;
        }
        Console.WriteLine(user.name);
        return false;
    }

    public async Task<List<User>> GetAllUserAsync()
    {
        return await broker.GetAllUserAsync(myid);
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await broker.GetAllUserAsync();
    }

    public async Task CreateGroup()
    {
        Console.WriteLine("---------------- Create group ----------------");
        Console.Write("Enter group name :");

        string name = Console.ReadLine()!;
        int id = await broker.GetLastGroupIdAsync();
        Group group = new Group(id, name, myid);
        GroupChat groupChat = new GroupChat(myid, id);
        var groups = await broker.GetAllGroups();

        bool check = true;
        foreach (var item in groups)
        {
            if (item.Name == name && item.OwnerId == myid) check = false;
        }
        if (check)
        {
            await broker.InsertGroupAsync(group);
            await broker.InsertUserGroup(groupChat);
        }
        else
            Console.WriteLine("Quwanasanba?");

    }

    public async Task PrintGroups()
    {
        Console.WriteLine("---------------- Groups ----------------");
        int k = 1;
        var groups = await broker.GetGroups(myid);
        Console.WriteLine("groups count => " + groups.Count);
        foreach (var item in groups)
        {
            Console.WriteLine(k + ". " + item.Name);
            k++;
        }
        Console.Write("Choose group => ");
    }

    public async Task JoinGroup()
    {
        Console.WriteLine("---------------- Join group ----------------");
        Console.Write("Enter group name :");
        string name = Console.ReadLine()!;
        var group = await broker.GetGrouopByName(name);
        if (group.Id != -1)
        {
            Console.WriteLine($"Siz '{name}' nomli guruhga azo boldingiz");
            GroupChat groupChat = new GroupChat(myid, group.Id);
            await broker.InsertUserGroup(groupChat);
        }
        else
        { 
            Console.WriteLine("Guruh topilmadi!");
        }
    }

    public async Task PrintChatMessage(int partnerId)
    {
        var messages = await broker.GetChatMessageAsync(myid,partnerId);
        messages.AddRange(await broker.GetChatMessageAsync(partnerId, myid));
        messages = messages.OrderBy(x => x.sendTime).ToList();
        if (messages.Count > 0)
            foreach (var item in messages)
            {
                if (item.sender_id == myid)
                {
                    Console.WriteLine($"\n{await broker.GetUserNameByIdAsync(myid)} :\t" + item.text + "\t\t" + item.sendTime);
                }
                else
                {
                    Console.WriteLine($"\n{await broker.GetUserNameByIdAsync(partnerId)} :\t\t\t" + item.text);
                }

            }
        else
            Console.WriteLine("Not found messages");
    }

    public async Task SendMessage(int partnerId, string text)
    {
        using var connection = broker.GetConnectionString();
        var messageId = await broker.GetLastMessageIdAsync();
        
            string query = $"insert into messages(id, senderId, reciverId, text,send_time, read_time) values({messageId},'{myid}','{partnerId}','{text}','{DateTime.Now}','{DateTime.Now}')";
            SqlCommand command = new SqlCommand(query, connection);
            await connection.OpenAsync();
            try
            {
                await command.ExecuteNonQueryAsync();
            }
            catch
            {
                Console.WriteLine("Xatolik");
                throw;
            }     
    }

    public async Task GroupSendMessage(int groupId, string text)
    {
        using var connection = broker.GetConnectionString();
        var messageId = await broker.GetLastGroupMessageIdAsync();

        string query = $"insert into messageGroup(id, senderId, chatId, text,send_time, read_time) values({messageId},'{myid}','{groupId}','{text}','{DateTime.Now}','{DateTime.Now}')";
        SqlCommand command = new SqlCommand(query, connection);
        await connection.OpenAsync();
        try
        {
            await command.ExecuteNonQueryAsync();
        }
        catch
        {
            Console.WriteLine("Xatolik");
            throw;
        }
    }

    public async Task PrintGroupMessage(int groupId)
    {
        var messages = await broker.GetGroupMessageAsync(groupId);
        messages = messages.OrderBy(x => x.sendTime).ToList();
        if (messages.Count > 0)
            foreach (var item in messages)
            {
                if (item.sender_id != myid)
                {
                    Console.WriteLine($"\n{await broker.GetUserNameByIdAsync(item.sender_id)} :\t" + item.text + "\t\t" + item.sendTime);
                }
                else
                {
                    Console.WriteLine($"\n{await broker.GetUserNameByIdAsync(myid)} :\t\t\t" + item.text + "\t\t" + item.sendTime);
                }

            }
        else
            Console.WriteLine("Not found messages");

    }

    public async Task<List<Group>> GetAllGroupsAsync()
    {
        return await broker.GetGroups(myid);
    }
}
