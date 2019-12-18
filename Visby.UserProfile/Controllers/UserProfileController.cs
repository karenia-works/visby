using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Karenia.Visby.UserProfile.Models;
using Karenia.Visby.UserProfile.Services;
using Karenia.Visby.Result;
using Microsoft.AspNetCore.JsonPatch;

namespace Karenia.Visby.UserProfile.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserProfileService _service;

        public UserController(UserProfileContext service)
        {
            _service = new UserProfileService(service);
        }

        //根据ID返回唯一用户
        // GET api/user/{user id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int user_id)
        {
            Result<User> result;
            var user = await _service.GetUserProfile(user_id);
            if (user == null)
            {
                result = new Result<User>(404, "User not exists", null);
                return NotFound(result);
            }

            result = new Result<User>(200, null, user);
            return Ok(result);
        }

        //添加新用户
        // POST api/user
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            var result = await _service.InsertUserProfile(user);
            if (result.Item1 == 200)
                return Ok(result);
            return BadRequest(result);
        }

        //更改用户信息
        // PATCH api/user/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<User> user)
        {
            User user_result = await _service.GetUserProfile(id);
            if (user_result == null)
                return BadRequest();
            user.ApplyTo(user_result);
            return Ok();
        }

        //删除用户信息
        // DELETE api/professor?id={professor id}
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery(Name = "id")] int id)
        {
            var user = await _service.GetUserProfile(id);
            if (user == null)
                return NotFound();
            _service.DeleteUserProfile(user);
            return NoContent();
        }
    }
}