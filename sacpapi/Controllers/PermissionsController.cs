using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using sacpapi.Data;
using sacpapi.Models;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace sacpapi.Controllers
{
    [Route("/[controller]")]

    public class PermissionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PermissionsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("{id}")]
        public IActionResult GetPermissions(int id)
        {
            var menu = _context.Menu
               .Where(item => item.ShowMenu == true)
               .OrderBy(item => item.Index)
               .ToList();

            var menuItemIds = _context.MainMenuPermissions
               .Where(mainMenu => mainMenu.UserId == id)
               .Select(mainMenu => mainMenu.MenuItemId).ToArray();

            var filteredMenu = menu
                .Where(item => menuItemIds.Contains(item.Id))
                .ToList();
            var remainingMenu = menu
                .Where(item => !menuItemIds.Contains(item.Id))
                .ToList();

            var submenu = _context.ProjectMenu
               .Where(item => item.ShowMenu == true)
               .OrderBy(item => item.Index)
               .ToList();

            var submenuItemIds = _context.SubMenuPermissions
                .Where(subMenu => subMenu.UserId == id)
                .Select(subMenu => subMenu.MenuItemId).ToArray();

            var filteredSubMenu = submenu
                .Where(item => submenuItemIds.Contains(item.Id))
                .ToList();

            var remainingSubMenu = submenu
                .Where(item => !submenuItemIds.Contains(item.Id))
                .ToList();

            var commands = _context.luFunctionalities.ToArray();

            var commandIds = _context.CommandLevelPermissions
                .Where(item => item.UserId == id)
                .Select(item => item.CommandId).ToArray();

            var filteredCommands = commands
                .Where(item => commandIds.Contains(item.Id))
                .ToList();

            var remainingCommands = commands
                .Where(item => !commandIds.Contains(item.Id))
                .ToList();


            var result = new
            {
                MenuItemIds = filteredMenu,
                SubMenuItemIds = filteredSubMenu,
                CommandIds = filteredCommands,
                RemainingCommands = remainingCommands,
                RemainingMenu = remainingMenu,
                RemainingSubMenu = remainingSubMenu
            };

            return Json(result);
        }

        [HttpPost("mainmenu/{userid}")]
        public IActionResult SaveMenuPermissions(int userId, [FromBody] List<int> ids)
        {
            try
            {
                if (ids != null && ids.Count > 0)
                {
                    var menuPermissionsToRemove = _context.MainMenuPermissions
                        .Where(menuPermission => menuPermission.UserId == userId);

                    _context.MainMenuPermissions.RemoveRange(menuPermissionsToRemove);
                    _context.SaveChanges();

                    foreach (var id in ids)
                    {
                        var exists = _context.MainMenuPermissions.FirstOrDefault(g => g.UserId == userId && g.MenuItemId == id);

                        if (exists == null)
                        {
                            MainMenuPermission mainMenuPermission = new MainMenuPermission();
                            mainMenuPermission.UserId = userId;
                            mainMenuPermission.MenuItemId = id;

                            _context.MainMenuPermissions.Add(mainMenuPermission);
                        }

                        continue;
                    }

                    _context.SaveChanges();

                    return StatusCode(201);

                }

                return StatusCode(500);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("submenu/{userId}")]
        public IActionResult SaveSubMenuPermissions(int userId, [FromBody] List<int> ids)
        {
            try
            {
                if (ids != null && ids.Count > 0)
                {
                    var PermissionsToRemove = _context.SubMenuPermissions
                        .Where(Permission => Permission.UserId == userId);

                    _context.SubMenuPermissions.RemoveRange(PermissionsToRemove);
                    _context.SaveChanges();
                    foreach (var id in ids)
                    {
                        var exists = _context.SubMenuPermissions.FirstOrDefault(g => g.UserId == userId && g.MenuItemId == id);

                        if (exists == null)
                        {
                            SubMenuPermission subMenuPermission = new SubMenuPermission();
                            subMenuPermission.UserId = userId;
                            subMenuPermission.MenuItemId = id;

                            _context.SubMenuPermissions.Add(subMenuPermission);
                        }

                        continue;
                    }

                    _context.SaveChanges();

                    return StatusCode(201);

                }

                return StatusCode(500);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("command/{userId}")]
        public IActionResult SaveCommandPermissions(int userId, [FromBody] List<int> ids)
        {
            try
            {
                if (ids != null && ids.Count > 0)
                {
                    var PermissionsToRemove = _context.CommandLevelPermissions
                        .Where(Permission => Permission.UserId == userId);

                    _context.CommandLevelPermissions.RemoveRange(PermissionsToRemove);
                    _context.SaveChanges();

                    foreach (var id in ids)
                    {
                        var exists = _context.CommandLevelPermissions.FirstOrDefault(g => g.UserId == userId && g.CommandId == id);

                        if (exists == null)
                        {
                            CommandLevelPermission commandLevelPermission = new CommandLevelPermission();
                            commandLevelPermission.UserId = userId;
                            commandLevelPermission.CommandId = id;

                            _context.CommandLevelPermissions.Add(commandLevelPermission);
                        }

                        continue;
                    }

                    _context.SaveChanges();

                    return StatusCode(201);

                }

                return StatusCode(500);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("mainmenu/{userId}")]
        public IActionResult RemoveMainMenuPermissions(int userId, [FromBody] List<int> ids)
        {
            try
            {
                if (ids != null && ids.Any())
                {
                    var menuPermissionsToRemove = _context.MainMenuPermissions
                        .Where(menuPermission => menuPermission.UserId == userId && ids.Contains(menuPermission.MenuItemId));

                    _context.MainMenuPermissions.RemoveRange(menuPermissionsToRemove);
                    _context.SaveChanges();

                    return Ok(); // Status 200 OK
                }

                return BadRequest("Invalid input. Please provide a non-empty list of permission IDs.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("submenu/{userId}")]
        public IActionResult RemoveSubMenuPermissions(int userId, [FromBody] List<int> ids)
        {
            try
            {
                if (ids != null && ids.Any())
                {
                    var subMenuPermissionsToRemove = _context.SubMenuPermissions
                        .Where(subMenuPermission => subMenuPermission.UserId == userId && ids.Contains(subMenuPermission.MenuItemId));

                    _context.SubMenuPermissions.RemoveRange(subMenuPermissionsToRemove);
                    _context.SaveChanges();

                    return Ok(); // Status 200 OK
                }

                return BadRequest("Invalid input. Please provide a non-empty list of permission IDs.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("command/{userId}")]
        public IActionResult RemoveCommandPermissions(int userId, [FromBody] List<int> ids)
        {
            try
            {
                if (ids != null && ids.Any())
                {
                    var commandPermissionsToRemove = _context.CommandLevelPermissions
                        .Where(commandPermission => commandPermission.UserId == userId && ids.Contains(commandPermission.CommandId));

                    _context.CommandLevelPermissions.RemoveRange(commandPermissionsToRemove);
                    _context.SaveChanges();

                    return Ok();
                }

                return BadRequest("Invalid input. Please provide a non-empty list of permission IDs.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
