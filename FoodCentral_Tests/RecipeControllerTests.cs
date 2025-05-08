using FoodCentral;
using FoodCentral.Controllers;
using FoodCentral.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Xunit;

namespace FoodCentral_Tests
{
    public class RecipeControllerTests
    {
        private class FakeUserManager : UserManager<IdentityUser>
        {
            private readonly string _userId;
            private readonly bool _isAdmin;

            public FakeUserManager(string userId, bool isAdmin = false)
                : base(new UserStore(), null, null, null, null, null, null, null, null)
            {
                _userId = userId;
                _isAdmin = isAdmin;
            }

            public override Task<IdentityUser> GetUserAsync(ClaimsPrincipal principal)
            {
                return Task.FromResult(new IdentityUser { Id = _userId });
            }

            public override Task<bool> IsInRoleAsync(IdentityUser user, string role)
            {
                return Task.FromResult(_isAdmin && role == "Admin");
            }

            private class UserStore : IUserStore<IdentityUser>
            {
                public Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken) => Task.FromResult(IdentityResult.Success);
                public Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken) => Task.FromResult(IdentityResult.Success);
                public void Dispose() { }
                public Task<IdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken) => Task.FromResult(new IdentityUser { Id = userId });
                public Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken) => Task.FromResult(new IdentityUser());
                public Task<string> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken) => Task.FromResult("");
                public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken) => Task.FromResult(user.Id);
                public Task<string> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken) => Task.FromResult("");
                public Task SetNormalizedUserNameAsync(IdentityUser user, string normalizedName, CancellationToken cancellationToken) => Task.CompletedTask;
                public Task SetUserNameAsync(IdentityUser user, string userName, CancellationToken cancellationToken) => Task.CompletedTask;
                public Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken) => Task.FromResult(IdentityResult.Success);
            }
        }

        private FoodCentralDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<FoodCentralDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new FoodCentralDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        private ClaimsPrincipal CreateUserPrincipal(string userId)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            return new ClaimsPrincipal(identity);
        }

        private RecipeController CreateController(FoodCentralDbContext context, string userId, bool isAdmin = false)
        {
            var userManager = new FakeUserManager(userId, isAdmin);
            var controller = new RecipeController(context, userManager);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = CreateUserPrincipal(userId) }
            };
            return controller;
        }

        [Fact]
        public async Task Index_ReturnsRecipes()
        {
            var context = GetInMemoryDbContext();
            context.Recipes.Add(new Recipe { Name = "Burger" });
            context.SaveChanges();

            var controller = CreateController(context, "user1");
            var result = await controller.Index() as ViewResult;

            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<List<Recipe>>(result.Model);
            Assert.Single(model);
        }

        [Fact]
        public
