﻿#pragma warning disable CS8618

using System.ComponentModel;

namespace MilligramServer.Models.Users;

public class UsersIndexModel : SortingPaginationModelBase
{
    [DisplayName("Имя или id")]
    public string? SearchString { get; set; }

    public UserModel[] Users { get; set; }
}