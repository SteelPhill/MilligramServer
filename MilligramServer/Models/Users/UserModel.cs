#pragma warning disable CS8618
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MilligramServer.Models.Users;

public class UserModel
{
    [DisplayName("Id")]
    public Guid Id { get; set; }

    [DisplayName("Логин")]
    public string Name { get; set; }

    [DisplayName("Имя")]
    public string? Nickname { get; set; }

    [DisplayName("Новый пароль")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Минимальная длина пароля 8 символов")]
    public string? NewPassword { get; set; }

    [DisplayName("Администратор")]
    public bool HasAdminRole { get; set; }

    [DisplayName("Swagger")]
    public bool HasSwaggerRole { get; set; }

    [DisplayName("Удалён")]
    public bool IsDeleted { get; set; }
}