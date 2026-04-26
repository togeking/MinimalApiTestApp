using Microsoft.AspNetCore.Mvc;
using api.Services;
using api.Services.DTOs;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly ITodoService _todoService;

    // 司令塔（Service）をDIで注入するぜ
    public TodosController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    /// <summary>
    /// 全てのタスク一覧を取得する
    /// </summary>
    /// <returns>タスク情報のリスト</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TodoResponse>))]
    public async Task<ActionResult<IEnumerable<TodoResponse>>> GetTodos()
    {
        return Ok(await _todoService.GetTodosAsync());
    }

    /// <summary>
    /// 指定したIDのタスクを1件取得する
    /// </summary>
    /// <param name="id">取得したいタスクのID</param>
    /// <returns>該当するタスク情報</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TodoResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TodoResponse>> GetTodo(int id)
    {
        var todo = await _todoService.GetTodoByIdAsync(id);
        return todo == null ? NotFound() : Ok(todo);
    }

    /// <summary>
    /// 特定のユーザーに紐づくタスク一覧を取得する
    /// </summary>
    /// <param name="userId">ユーザーID</param>
    /// <returns>該当ユーザーのタスクリスト</returns>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TodoResponse>))]
    public async Task<ActionResult<IEnumerable<TodoResponse>>> GetTodosByUser(int userId)
    {
        return Ok(await _todoService.GetTodosByUserIdAsync(userId));
    }

    /// <summary>
    /// 新しいタスクを作成する
    /// </summary>
    /// <param name="request">作成するタスクの内容</param>
    /// <returns>作成されたタスク情報</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TodoResponse))]
    public async Task<ActionResult<TodoResponse>> CreateTodo([FromBody] TodoCreateRequest request)
    {
        var result = await _todoService.CreateTodoAsync(request);
        // CreatedAtActionを使って、詳細取得用URLをLocationヘッダーにセットするぜ
        return CreatedAtAction(nameof(GetTodo), new { id = result.Id }, result);
    }

    /// <summary>
    /// 既存のタスクを更新する
    /// </summary>
    /// <param name="id">更新対象のID</param>
    /// <param name="request">更新内容</param>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTodo(int id, [FromBody] TodoUpdateRequest request)
    {
        // URLのIDとボディのIDが食い違ってたら弾くのがお作法だ
        if (id != request.Id) return BadRequest("ID mismatch");

        var success = await _todoService.UpdateTodoAsync(request);
        return success ? NoContent() : NotFound();
    }

    /// <summary>
    /// 指定したIDのタスクを削除する
    /// </summary>
    /// <param name="id">削除したいタスクのID</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        var success = await _todoService.DeleteTodoAsync(id);
        return success ? NoContent() : NotFound();
    }
}