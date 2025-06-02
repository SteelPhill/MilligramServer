using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilligramServer.Database.Context;
using MilligramServer.Database.Context.Factory;
using MilligramServer.Domain;
using MilligramServer.Extensions.Models;
using MilligramServer.Models.Chats;
using MilligramServer.Services.Managers;

namespace MilligramServer.Controllers;

[Authorize(Roles = RoleTokens.AdminRole)]
public class ChatsController : Controller
{
    private readonly ApplicationContextUserManager _applicationContextUserManager;
    private readonly ApplicationContext _context;
    private readonly ILogger<UsersController> _logger;

    public ChatsController(
        ApplicationContextUserManager applicationContextUserManager,
        IApplicationContextFactory context,
        ILogger<UsersController> logger)
    {
        _applicationContextUserManager = applicationContextUserManager;
        _context = context.Create();
        _logger = logger;
    }

    [Route("Users/{userId:guid}/[controller]")]
    public async Task<IActionResult> Index(
        [FromRoute] Guid userId,
        [FromQuery] string? sortBy,
        [FromQuery] string? searchString,
        [FromQuery] int page = 1,
        CancellationToken cancellationToken = default)
    {
        var user = await _applicationContextUserManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return NotFound();

        var chatsQuery = _applicationContextUserManager.Users
            .Where(u => u.Id == userId)
            .SelectMany(u => u.UsersChats.Select(uc => uc.Chat))
            .Include(c => c.OwnerUser)
            .Include(u => u.UsersChats
                .Where(uc => uc.UserId == userId))
            .ThenInclude(uc => uc.User)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(searchString))
        {
            chatsQuery = chatsQuery.Where(chat =>
                EF.Functions.Like(chat.Id.ToString(), $"%{searchString}%") ||
                EF.Functions.Like(chat.Name, $"%{searchString}%"));
        }

        chatsQuery = sortBy switch
        {
            nameof(ChatModel.Id) => chatsQuery.OrderBy(chat => chat.Id),
            nameof(ChatModel.Id) + Constants.DescSuffix => chatsQuery.OrderByDescending(chat => chat.Id),
            nameof(ChatModel.Name) => chatsQuery.OrderBy(chat => chat.Name),
            nameof(ChatModel.Name) + Constants.DescSuffix => chatsQuery.OrderByDescending(chat => chat.Name),
            nameof(ChatModel.IsDeleted) => chatsQuery.OrderBy(chat => chat.IsDeleted),
            nameof(ChatModel.IsDeleted) + Constants.DescSuffix => chatsQuery.OrderByDescending(chat => chat.IsDeleted),
            _ => chatsQuery.OrderBy(chat => chat.Id)
        };

        var totalCount = await chatsQuery.CountAsync(cancellationToken);
        var chats = await chatsQuery
            .Skip((page - Constants.FirstPage) * Constants.PageSize)
            .Take(Constants.PageSize)
            .ToListAsync(cancellationToken);

        return View(new ChatsIndexModel
        {
            User = await user.ToModelAsync(_applicationContextUserManager),
            UserId = user.Id,
            Chats = chats.ToModels().ToArray(),
            SortBy = sortBy,
            Page = page,
            TotalCount = totalCount,
            SearchString = searchString
        });
    }

    [HttpGet("Users/{userId:guid}/[controller]/[action]/{chatId:guid}")]
    public async Task<IActionResult> Edit(
        [FromRoute] Guid userId,
        [FromRoute] Guid chatId)
    {
        var chat = await _context.Chats
            .Include(c => c.UsersChats)
            .FirstOrDefaultAsync(c => c.Id == chatId);

        if (chat == null) return NotFound();

        return View(new UserChatModel
        {
            UserId = userId,
            ChatId = chatId,
            ChatName = chat.Name,
            Chat = chat.ToModel()
        });
    }

    [HttpPost("Users/{userId:guid}/[controller]/[action]/{chatId:guid}")]
    public async Task<IActionResult> Edit(
        [FromRoute] Guid userId,
        [FromRoute] Guid chatId,
        [FromForm] UserChatModel model)
    {
        if (!ModelState.IsValid)
        {
            var chat = await _context.Chats.FindAsync(chatId);
            if (chat != null) 
                model.Chat = chat.ToModel();
            return View(model);
        }

        var chatToUpdate = await _context.Chats.FindAsync(chatId);
        if (chatToUpdate == null) 
            return NotFound();

        chatToUpdate.Name = model.ChatName;
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Chats", new { userId });
    }

    [HttpPost("Users/{userId:guid}/[controller]/[action]/{chatId:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid userId,
        [FromRoute] Guid chatId)
    {
        var chat = await _context.Chats.FirstOrDefaultAsync(c => c.Id == chatId);
        if (chat == null)
            return NotFound();

        chat.IsDeleted = true;
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Chats", new { userId });
    }

    [HttpPost("Users/{userId:guid}/[controller]/[action]/{chatId:guid}")]
    public async Task<IActionResult> Restore(
        [FromRoute] Guid userId,
        [FromRoute] Guid chatId)
    {
        var chat = await _context.Chats.FirstOrDefaultAsync(c => c.Id == chatId);
        if (chat == null)
            return NotFound();

        chat.IsDeleted = false;
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Chats", new { userId });
    }
}