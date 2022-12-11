
namespace Chat.Models;

public class Message
{
    public Message(int id, string text, DateTime sendTime, DateTime readTime, int sender_id, int reciver_id)
    {
        this.id = id;
        this.text = text;
        this.sendTime = sendTime;
        this.readTime = readTime;
        this.sender_id = sender_id;
        this.reciver_id = reciver_id;
    }

    /// <summary>
    /// message id
    /// </summary>
    public int id { get; set; }
    /// <summary>
    /// message text
    /// </summary>
    public string text { get; set; }
    /// <summary>
    /// message sender id
    /// </summary>
    public int sender_id { get; set; }
    /// <summary>
    /// message reciver id
    /// </summary>
    public int reciver_id { get; set; }
    /// <summary>
    /// message sended time
    /// </summary>
    public DateTime sendTime { get; set; }
    /// <summary>
    /// message recived time
    /// </summary>
    public DateTime readTime { get; set; }

}
