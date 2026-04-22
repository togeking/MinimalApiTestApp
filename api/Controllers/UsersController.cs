using Microsoft.AspNetCore.Mvc;
using api.Services;
using api.Services.DTOs;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// 全てのユーザー一覧を取得する
    /// </summary>
    /// <returns>ユーザー情報のリスト</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserResponse>))]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
    {
        return Ok(await _userService.GetUsersAsync());
    }

    /// <summary>
    /// 指定したIDのユーザーを取得する
    /// </summary>
    /// <param name="id">取得したいユーザーのID</param>
    /// <returns>該当するユーザー情報</returns>
    /// <response code="200">ユーザーが見つかった場合</response>
    /// <response code="404">指定されたIDのユーザーが存在しない場合</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponse>> GetUser(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return user == null ? NotFound() : Ok(user);
    }

    /// <summary>
    /// 新しいユーザーを登録する
    /// </summary>
    /// <param name="request">作成するユーザーの名前とメールアドレス</param>
    /// <returns>作成されたユーザー情報</returns>
    /// <response code="201">ユーザーの作成に成功した場合</response>
    /// <response code="400">リクエストの内容に不備がある場合</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponse>> CreateUser([FromBody] UserCreateRequest request)
    {
        var result = await _userService.CreateUserAsync(request);
        return CreatedAtAction(nameof(GetUser), new { id = result.Id }, result);
    }

    /// <summary>
    /// 指定したIDのユーザーを削除する
    /// </summary>
    /// <param name="id">削除したいユーザーのID</param>
    /// <response code="204">削除が成功した（返すデータはない）場合</response>
    /// <response code="404">削除対象のユーザーが存在しない場合</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var success = await _userService.DeleteUserAsync(id);
        return success ? NoContent() : NotFound();
    }
}