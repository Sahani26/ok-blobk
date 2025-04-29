using Blog_April.Data;
using Blog_April.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog_April.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BlogController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var count = await _context.Posts.CountAsync();
            ViewBag.PostCount = count;

            var posts = await _context.Posts.OrderByDescending(p => p.CreatedAt).ToListAsync();
            

            return View(posts);
        }


        public async Task<IActionResult> Details(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
                }
            return View(post);
        }
        [Authorize]
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Post post)
        {
            if (!ModelState.IsValid)
            {
                post.AuthorId = _userManager.GetUserId(User);
                post.CreatedAt= DateTime.Now;
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(post);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null || post.AuthorId != _userManager.GetUserId(User))
            {
                return Unauthorized(); // Not the author
            }
            return View(post);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, Post updatedPost)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null || post.AuthorId != _userManager.GetUserId(User))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                post.Title = updatedPost.Title;
                post.Content = updatedPost.Content;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(post);

        }
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null || post.AuthorId != _userManager.GetUserId(User))
            {
                return Unauthorized();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
