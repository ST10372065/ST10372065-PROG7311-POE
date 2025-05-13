using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ST10372065_PROG7311.Data;
using ST10372065_PROG7311.Models;

namespace ST10372065_PROG7311.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserService> _logger;
        public UserService(AppDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<User>> GetAllAsync() => await _context.Users.ToListAsync();

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("User added: {user}", user);
        }

        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            // Check if a user with the given email and password exists
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.Include(p => p.User).ToListAsync();
        }

        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(int id) => await _context.Users.FindAsync(id);

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
