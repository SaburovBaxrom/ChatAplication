using Chat.Service;
using Chat.Models;
namespace Chat.UI;

public class Menu
{
    IService service;
    public Menu()
    {
        service = new Service.Service();
        MainMenu();
    }

    void MainMenu()
    {
        int k = 0;
        TextMenu();
        k = int.Parse(Console.ReadLine()!);
        switch (k)
        {
            case 1:
                service.Registration();
                Console.Clear();
                MainMenu();
                break;
            case 2:
                
                if (service.LoginAsync().Result)
                {
                    Console.WriteLine("Wellcome");
                    Thread.Sleep(1000);
                    Console.Clear();
                    MenuChat();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine(service.myid);
                    Console.WriteLine("Parol yoki login xato");
                    MainMenu();
                }
                break;
            case 3:
                Environment.Exit(0);
                break;
        }

    }

    void MenuChat()
    {
        TextMenuChat();
        int b = int.Parse(Console.ReadLine()!);
        switch (b)
        {
            case 1:
                Console.Clear();
                block();
                break;
            case 2:
                blockGroup();
                break;
            case 3:
                Console.Clear();
                service.CreateGroup().Wait();
                MenuChat();
                break;
            case 4:
                service.JoinGroup().Wait(); 
                Thread.Sleep(1000);
                Console.Clear();
                MenuChat();
                break;
            case 5:
                Console.Clear();
                MainMenu();
                break;
        }
    }

    void block()
    {
        bool check = true;
        Console.Clear();
        service.PrintAllUser();
        Console.WriteLine("Enter 0 to exit");
        int a = int.Parse(Console.ReadLine()!);
        if (a != 0)
        {
            var partner = service.GetAllUserAsync().Result;
            var partnerId = partner[a-1].id;
            Console.WriteLine(partner[a-1].name);
            Console.Clear();
            service.PrintChatMessage(partnerId).Wait();
            Console.WriteLine("Enter your message(enter 0 to exit) : ");
            while (check)
            {
                string mess = Console.ReadLine()!;
                if (mess == "0")
                    check = false;
                else
                    service.SendMessage(partnerId, mess).Wait();
            }
            block();
        }
        else
        {
            Console.Clear();
            MenuChat();
        }
    }

    void blockGroup()
    {
        bool check = true;
        Console.Clear();
        service.PrintGroups().Wait();
        Console.WriteLine("Enter 0 to exit");
        int a = int.Parse(Console.ReadLine()!);
        if (a != 0)
        {
            var partner = service.GetAllGroupsAsync().Result;
            var partnerId = partner[a-1].Id;

            Console.WriteLine(partner[a - 1].Name);
            Console.Clear();
            service.PrintGroupMessage(partnerId).Wait();
            Console.WriteLine("Enter your message(enter 0 to exit) : ");
            while (check)
            {
                string mess = Console.ReadLine()!;
                if (mess == "0")
                    check = false;
                else
                    service.GroupSendMessage(partnerId, mess).Wait();
            }
            blockGroup();
        }
        else
        {
            Console.Clear();
            MenuChat();
        }
    }

    void TextMenuChat()
    {
        Console.WriteLine("1. Chats");
        Console.WriteLine("2. Groups");
        Console.WriteLine("3. Create group");
        Console.WriteLine("4. Join group");
        Console.WriteLine("5. Exit");
        Console.Write("Choose :");

    }

    void TextMenu()
    {
        Console.WriteLine("1. Registration");
        Console.WriteLine("2. Login");
        Console.WriteLine("3. Exit");
        Console.Write("Choose :");
    }
    
}
