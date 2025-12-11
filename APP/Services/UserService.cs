using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using CORE.APP.Services.Authentication.MVC;
using Microsoft.EntityFrameworkCore;

namespace APP.Services
{
    public class UserService : Service<User>, IService<UserRequest, UserResponse>
    {
        private readonly ICookieAuthService _cookieAuthService;

        public UserService(DbContext db, ICookieAuthService cookieAuthService) : base(db)
        {
            _cookieAuthService = cookieAuthService;
        }

        protected override IQueryable<User> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking).Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .OrderByDescending(u => u.IsActive).ThenBy(u => u.UserName);
        }

        public CommandResponse Create(UserRequest request)
        {
            if (Query().Any(u => u.UserName == request.UserName))
                return Error("User with the same user name exists!");
            var entity = new User
            {
                UserName = request.UserName,
                Password = request.Password,
                IsActive = request.IsActive,
                RoleIds = request.RoleIds
            };
            Create(entity);
            return Success("User created successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false).SingleOrDefault(u => u.Id == id);
            if (entity is null)
                return Error("User not found!");
            Delete(entity.UserRoles);
            Delete(entity);
            return Success("User deleted successfully.", entity.Id);
        }

        public UserRequest Edit(int id)
        {
            var entity = Query().SingleOrDefault(u => u.Id == id);
            if (entity is null)
                return null;
            return new UserRequest
            {
                Id = entity.Id,
                UserName = entity.UserName,
                Password = entity.Password,
                IsActive = entity.IsActive,
                RoleIds = entity.RoleIds
            };
        }

        public UserResponse Item(int id)
        {
            var entity = Query().SingleOrDefault(u => u.Id == id);
            if (entity is null)
                return null;
            return new UserResponse
            {
                Id = entity.Id,
                Guid = entity.Guid,
                UserName = entity.UserName,
                Password = entity.Password,
                IsActive = entity.IsActive,
                IsActiveF = entity.IsActive ? "Active" : "Inactive",
                RoleIds = entity.RoleIds,
                Roles = string.Join("<br>", entity.UserRoles.Select(ur => ur.Role.Name).ToList())
            };
        }

        public List<UserResponse> List()
        {
            return Query().Select(u => new UserResponse
            {
                Id = u.Id,
                Guid = u.Guid,
                UserName = u.UserName,
                Password = u.Password,
                IsActive = u.IsActive,
                IsActiveF = u.IsActive ? "Active" : "Inactive",
                RoleIds = u.RoleIds,
                Roles = string.Join("<br>", u.UserRoles.Select(ur => ur.Role.Name).ToList())
            }).ToList();
        }

        public CommandResponse Update(UserRequest request)
        {
            if (Query().Any(u => u.Id != request.Id && u.UserName == request.UserName))
                return Error("User with the same user name exists!");
            var entity = Query(false).SingleOrDefault(u => u.Id == request.Id);
            if (entity is null)
                return Error("User not found!");
            Delete(entity.UserRoles);
            entity.UserName = request.UserName;
            entity.Password = request.Password;
            entity.IsActive = request.IsActive;
            entity.RoleIds = request.RoleIds;
            Update(entity);
            return Success("User updated successfully.", entity.Id);
        }

        public async Task Logout() => await _cookieAuthService.SignOut();

        public async Task<CommandResponse> Login(UserLoginRequest request)
        {
            var entity = Query().SingleOrDefault(u => u.UserName == request.UserName && u.Password == request.Password && u.IsActive);

            if (entity is null)
                return Error("Invalid user name or password!");

            await _cookieAuthService.SignIn(entity.Id, entity.UserName, entity.UserRoles.Select(ur => ur.Role.Name).ToArray());

            return Success("User logged in successfully.", entity.Id);
        }
    }
}
