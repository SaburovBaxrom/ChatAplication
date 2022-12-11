namespace Chat.Models;

public class MessageGroup
{
    public MessageGroup(int id, string text, int sender_id, int chat_id, DateTime sendTime, DateTime readTime)
    {
        this.id = id;
        this.text = text;
        this.sender_id = sender_id;
        this.chat_id = chat_id;
        this.sendTime = sendTime;
        this.readTime = readTime;
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
    public int chat_id { get; set; }
    /// <summary>
    /// message sended time
    /// </summary>
    public DateTime sendTime { get; set; }
    /// <summary>
    /// message recived time
    /// </summary>
    public DateTime readTime { get; set; }

}
