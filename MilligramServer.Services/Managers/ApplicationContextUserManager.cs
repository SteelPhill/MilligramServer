﻿using MilligramServer.Domain.Entities;
using MilligramServer.Services.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace MilligramServer.Services.Managers;

public class ApplicationContextUserManager : UserManager<User>
{
    public ApplicationContextUserManager(
        IApplicationContextUserStore store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<User> passwordHasher,
        IEnumerable<IUserValidator<User>> userValidators,
        IEnumerable<IPasswordValidator<User>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<ApplicationContextUserManager> logger)
        : base(
            store,
            optionsAccessor,
            passwordHasher,
            userValidators,
            passwordValidators,
            keyNormalizer,
            errors,
            services,
            logger)
    {
    }

    public Task<User?> GetUserAndLoadRolesAsync(ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);

        var id = GetUserId(principal);
        return id == null ? Task.FromResult<User?>(null) : FindByIdAndLoadRolesAsync(id);
    }

    public async Task<User?> FindByIdAndLoadRolesAsync(string userId)
    {
        ThrowIfDisposed();

        return await ((IApplicationContextUserStore) Store)
            .FindByIdAndLoadRolesAsync(userId, CancellationToken).ConfigureAwait(false);
    }

    public async Task<User?> FindByNameAndLoadRolesAsync(string userName)
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(userName);

        userName = NormalizeName(userName);
        return await ((IApplicationContextUserStore) Store)
            .FindByNameAndLoadRolesAsync(userName, CancellationToken).ConfigureAwait(false);
    }

    public async Task Restore(string userName)
    {
        var applicationContextUserStore = (IApplicationContextUserStore)Store;

        var user = applicationContextUserStore.FindByNameAsync(userName, CancellationToken).Result;
        if (user != null) 
            await applicationContextUserStore.RestoreAsync(user, CancellationToken).ConfigureAwait(false);
    }
}