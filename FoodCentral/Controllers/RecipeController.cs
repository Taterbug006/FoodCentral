﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodCentral.Controllers;
using FoodCentral.Models;

namespace FoodCentral.Controllers
{
    [Authorize]
    public class RecipeController : Controller
    {
        private readonly FoodCentralDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RecipeController(FoodCentralDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var recipes = await _context.Recipes.ToListAsync();
            return View(recipes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Recipe recipe)
        {
            var user = await _userManager.GetUserAsync(User);
            recipe.UserId = user.Id;
            _context.Add(recipe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if (!isAdmin && recipe.UserId != user.Id)
            {
                return Forbid();
            }

            return View(recipe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Recipe recipe)
        {
            if (id != recipe.Id) return NotFound();

            var dbRecipe = await _context.Recipes.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            if (dbRecipe == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if (!isAdmin && dbRecipe.UserId != user.Id)
            {
                return Forbid();
            }

            recipe.UserId = dbRecipe.UserId;
            _context.Update(recipe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if (!isAdmin && recipe.UserId != user.Id)
            {
                return Forbid();
            }

            return View(recipe);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if (!isAdmin && recipe.UserId != user.Id)
            {
                return Forbid();
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
