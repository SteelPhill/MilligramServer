#pragma warning disable CS8618

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MilligramServer.Models.Chats;

public class UserChatModel
{
    public Guid UserId { get; set; }
    public Guid ChatId { get; set; }

    [Required(ErrorMessage = "Не указано название")]
    [DisplayName("Название чата")]
    public string ChatName { get; set; }

    public ChatModel Chat { get; set; }
}