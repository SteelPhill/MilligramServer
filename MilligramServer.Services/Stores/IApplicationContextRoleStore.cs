using MilligramServer.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace MilligramServer.Services.Stores;

public interface IApplicationContextRoleStore : IQueryableRoleStore<Role>
{
}