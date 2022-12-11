using Chat.Models;
using System.Data.SqlClient;
namespace Chat.Broker.DataBase;

public class DatabaseBroker : IDatabaseBroker
{
    public async Task DeleteUserAsync(int id)
    {
        using var connection = GetConnectionString();
        string qurery = $"Delete from userss where id = {id}";
        var command = new SqlCommand(qurery, connection);
        await connection.OpenAsync();
        try
        {
            await command.ExecuteNonQueryAsync();
        }
        catch
        {
            Console.WriteLine("Databasega ulanib bo'lmadi");
            throw;
        }


    }

    public async Task<List<User>> GetAllUserAsync(int myId)
    {
        List<User> users = new List<User>();
        using var connection = GetConnectionString();
        string query = $"Select * from userss";
        var command = new SqlCommand(query,connection);
        await connection.OpenAsync();
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            if ((int)reader["id"] != myId)
            {
                User user = new User((int)reader["id"], reader["name"].ToString()!, reader["username"].ToString()!, reader["password"].ToString()!);
                users.Add(user);
            }
            
        }
        return users;
    }

    public SqlConnection GetConnectionString()
    {
        string connectionString = @"Server=localhost; Database=MyDataBase;User Id=sa; password=Baxram2002;";
        return new SqlConnection(connectionString);
    }

    public async Task<int> GetLastMessageIdAsync()
    {
        int id_ = -1;
        using var connection = GetConnectionString();

        string query = "select top 1 id from messages order by id desc";
        SqlCommand command = new SqlCommand(query, connection);

        await connection.OpenAsync();
        SqlDataReader dataReader = await command.ExecuteReaderAsync();

        while (await dataReader.ReadAsync())
        {
            id_ = int.Parse(dataReader["id"].ToString()!);
        }


        return ++id_;
    }

    public async Task<int> GetLastUserIdAsync()
    {
        int id_ = -1;
        using var connection = GetConnectionString();

        string query = "select top 1 id from userss order by id desc";
        SqlCommand command = new SqlCommand(query, connection);

        await connection.OpenAsync();
        SqlDataReader dataReader = await command.ExecuteReaderAsync();

        while (await dataReader.ReadAsync())
        {
            id_ = int.Parse(dataReader["id"].ToString()!);
        }


        return ++id_;
    }

    public async Task<List<Message>> GetMessageAsync(int myId, int partnerId)
    {
        List<Message> message = new List<Message>();
        using var connection = GetConnectionString();
        
            SqlParameter sqlParameter1 = new SqlParameter("@myId", myId);
            SqlParameter sqlParameter2 = new SqlParameter("@partnerId", partnerId);

            string query = "select * from messages Where senderId = @myId and reciverId = @partnerId";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add(sqlParameter1);
            command.Parameters.Add(sqlParameter2);

            await connection.OpenAsync();
            SqlDataReader dataReader = await command.ExecuteReaderAsync();
            while (await dataReader.ReadAsync())
            {
                Message mes = new Message((int)dataReader["id"],
                    dataReader["text"].ToString()!,
                    DateTime.Parse(dataReader["send_time"].ToString()!),
                    DateTime.Parse(dataReader["read_time"].ToString()!),
                    int.Parse(dataReader["senderId"].ToString()!),
                    int.Parse(dataReader["reciverId"].ToString()!));
                message.Add(mes);
            }
        
        return message;
    }

    public async Task InsertUserAsync(User user)
    {
        using var connection = GetConnectionString();
        
            int id =  GetLastUserIdAsync().Result;
            string query = $"insert into userss(id, username, name, password) " +
                $"values({id},'{user.username}','{user.name}','{user.password}')";
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

    public async Task<List<Message>> GetChatMessageAsync(int myId, int partnerId)
    {
        List<Message> message = new List<Message>();
        using var connection = GetConnectionString();
        
            SqlParameter sqlParameter1 = new SqlParameter("@myId", myId);
            SqlParameter sqlParameter2 = new SqlParameter("@partnerId", partnerId);

            string query = "select * from messages Where senderId = @myId and reciverId = @partnerId";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add(sqlParameter1);
            command.Parameters.Add(sqlParameter2);

            await connection.OpenAsync();
            SqlDataReader dataReader = await command.ExecuteReaderAsync();
            while (await dataReader.ReadAsync())
            {
                Message mes = new Message((int)dataReader["id"],
                    dataReader["text"].ToString()!,
                    DateTime.Parse(dataReader["send_time"].ToString()!),
                    DateTime.Parse(dataReader["read_time"].ToString()!),
                    int.Parse(dataReader["senderId"].ToString()!),
                    int.Parse(dataReader["reciverId"].ToString()!));
                message.Add(mes);
            }
        
        return message;
    }
 
    public async Task<List<MessageGroup>> GetGroupMessageAsync(int chatId)
    {
        List<MessageGroup> message = new List<MessageGroup>();
        using var connection = GetConnectionString();

        SqlParameter sqlParameter2 = new SqlParameter("@groupId", chatId);

        string query = "select * from messageGroup Where chatId = @groupId";
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.Add(sqlParameter2);

        await connection.OpenAsync();
        SqlDataReader dataReader = await command.ExecuteReaderAsync();
        while (await dataReader.ReadAsync())
        {
            MessageGroup mes = new MessageGroup(
                (int)dataReader["id"],
                dataReader["text"].ToString()!,
                (int)dataReader["senderId"],
                (int)dataReader["chatId"],
                DateTime.Parse(dataReader["send_time"].ToString()!),
                DateTime.Parse(dataReader["read_time"].ToString()!)
                );
            message.Add(mes);
        }

        return message;
    }

    public async Task<string> GetUserNameByIdAsync(int id)
    {
        using var connection = GetConnectionString();
        string query = $"Select * from userss where id = {id}";
        var command = new SqlCommand(query, connection);
        await connection.OpenAsync();
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            User user = new User((int)reader["id"], reader["name"].ToString()!, reader["username"].ToString()!, reader["password"].ToString()!);
            return user.name;
        }
        return "";
    }

    public async Task InsertGroupAsync(Group group)
    {
        using var connection = GetConnectionString();

        int id = GetLastGroupIdAsync().Result;
        string query = $"insert into groupChat(id, name, ownerId) " +
            $"values({id},'{group.Name}','{group.OwnerId}')";
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

    public async Task<int> GetLastGroupIdAsync()
    {
        int id_ = -1;
        using var connection = GetConnectionString();

        string query = "select top 1 id from groupChat order by id desc";
        SqlCommand command = new SqlCommand(query, connection);

        await connection.OpenAsync();
        SqlDataReader dataReader = await command.ExecuteReaderAsync();

        while (await dataReader.ReadAsync())
        {
            id_ = int.Parse(dataReader["id"].ToString()!);
        }


        return ++id_;
    }

    public async Task<List<User>> GetAllUserAsync()
    {
        List<User> users = new List<User>();
        using var connection = GetConnectionString();
        string query = $"Select * from userss";
        var command = new SqlCommand(query, connection);
        await connection.OpenAsync();
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
                User user = new User((int)reader["id"], reader["name"].ToString()!, reader["username"].ToString()!, reader["password"].ToString()!);
                users.Add(user);
        }
        return users;
    }
    
    public async Task<List<Group>> GetGroups(int id)
    {
        List<Group> groups = new List<Group>();
        using var connection = GetConnectionString();
        string query = $"Select * from test where userId = {id}";
        var command = new SqlCommand(query, connection);
        await connection.OpenAsync();
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var group = await GetGrouopById((int)reader["groupId"]);
            groups.Add(group);
        }
        return groups;
    }

    public async Task InsertUserGroup(GroupChat groupChat)
    {
        using var connection = GetConnectionString();
        Console.WriteLine("groupChat.groupId = " + groupChat.GroupId);
        Console.WriteLine("groupChat.userId = " + groupChat.UserId);

        string query = $"insert into test(groupId, userId)" +
            $"values({groupChat.GroupId},{groupChat.UserId});";
                
        SqlCommand command = new SqlCommand(query, connection);
        await connection.OpenAsync();
        try
        {
            await command.ExecuteNonQueryAsync();
        }
        catch
        {
            Console.WriteLine("Xatolik test");
            throw;
        }
    }
    
    public async Task<Group> GetGrouopById(int id)
    {
        using var connection = GetConnectionString();
        string query = $"Select * from groupChat where id = {id}";
        var command = new SqlCommand(query, connection);
        await connection.OpenAsync();
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            Group group = new Group((int)reader["id"], reader["name"].ToString()!, (int)reader["ownerId"]);
            return group;
        }
        return new Group(-1,"",-1);

    }
    public async Task<Group> GetGrouopByName(string name)
    {
        using var connection = GetConnectionString();
        string query = $"Select * from groupChat where name = '{name}'";
        var command = new SqlCommand(query, connection);
        await connection.OpenAsync();
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            Group group = new Group((int)reader["id"], reader["name"].ToString()!, (int)reader["ownerId"]);
            return group;
        }
        return new Group(-1, "", -1);

    }

    public async Task<List<Group>> GetAllGroups()
    {
        List<Group> groups = new List<Group>();
        using var connection = GetConnectionString();
        string query = $"Select * from groupChat";
        var command = new SqlCommand(query, connection);
        await connection.OpenAsync();
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var group = await GetGrouopById((int)reader["id"]);
            groups.Add(group);
        }
        return groups;
    }

    public async Task<int> GetLastGroupMessageIdAsync()
    {
        int id_ = -1;
        using var connection = GetConnectionString();

        string query = "select top 1 id from messageGroup order by id desc";
        SqlCommand command = new SqlCommand(query, connection);

        await connection.OpenAsync();
        SqlDataReader dataReader = await command.ExecuteReaderAsync();

        while (await dataReader.ReadAsync())
        {
            id_ = int.Parse(dataReader["id"].ToString()!);
        }


        return ++id_;
    }
}
