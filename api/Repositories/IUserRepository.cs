using api.Repositories.Entities;

namespace api.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task AddAsync(User user);
    Task DeleteAsync(int id);
    // Note: 将来の検索（名前やメール）はここに追加していくことになる
}