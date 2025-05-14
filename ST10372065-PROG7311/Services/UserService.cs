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
            _logger.LogInformation("Database path: {Path}", _context.Database.GetDbConnection().DataSource);
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User added: {@User}", user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving user");
                throw;
            }
        }


        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            // Log the query for debugging
            _logger.LogInformation("Validating user with email: {email}", email);

            // Perform a case-insensitive comparison for email
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && u.Password == password);
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.Include(p => p.User).ToListAsync();
        }

        public async Task AddProductAsync(Product product)
        {
            try
            {
                // Log the product details for debugging
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Product added: {@Product}", product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product");
                throw;
            }
        }


        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(int id) => await _context.Users.FindAsync(id);

        public async Task<List<Product>> GetProductsByUserIdAsync(int userId)
        {
            return await _context.Products
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Product>> FilterProductsAsync(int? farmerId = null, string category = null, DateOnly? startDate = null, DateOnly? endDate = null)
        {
            // Start with all products, including User information
            var query = _context.Products.Include(p => p.User).AsQueryable();

            // Filter by farmer if specified
            if (farmerId.HasValue)
            {
                query = query.Where(p => p.UserId == farmerId.Value);
            }

            // Filter by category if specified
            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                query = query.Where(p => p.Category == category);
            }

            // Filter by date range if specified
            if (startDate.HasValue)
            {
                query = query.Where(p => p.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(p => p.Date <= endDate.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<List<User>> GetAllFarmersAsync()
        {
            return await _context.Users
                .Where(u => u.Role == "Farmer")
                .ToListAsync();
        }

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
