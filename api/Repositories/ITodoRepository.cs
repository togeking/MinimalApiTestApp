using api.Repositories.Entities;

namespace api.Repositories;

public interface ITodoRepository
{
    /// <summary>
    /// 全件取得
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<TodoItem>> GetAllAsync();
    /// <summary>
    /// 指定したIDのタスクを取得
    /// </summary>
    Task<TodoItem?> GetByIdAsync(int id);
    /// <summary>
    /// 指定したUserIdに紐づくタスク一覧を取得
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<IEnumerable<TodoItem>> GetByUserIdAsync(int userId);
    /// <summary>
    /// 新規追加
    /// </summary>
    /// <param name="todoItem"></param>
    /// <returns></returns>
    Task AddAsync(TodoItem todoItem);
    /// <summary>
    /// タスクの更新
    /// </summary>
    /// <param name="todoItem"></param>
    /// <returns></returns>
    Task UpdateAsync(TodoItem todoItem);
    /// <summary>
    /// 指定したIDのタスクを削除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(int id);
}