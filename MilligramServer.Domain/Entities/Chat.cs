﻿#pragma warning disable CS8618

namespace MilligramServer.Domain.Entities;

public class Chat
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid? OwnerUserId { get; set; }
    public User? OwnerUser { get; set; }
    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public ICollection<UserChat> UsersChats { get; set; } = new List<UserChat>();
    public bool IsDeleted { get; set; }
}