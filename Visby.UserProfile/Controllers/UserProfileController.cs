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
        private readonly AccountService _service_account;

        public UserController(UserProfileContext service, AccountContext service_account)
        {
            _service = new UserProfileService(service);
            _service_account = new AccountService(service_account);
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

        // GET api/user/email/{email}
        [HttpGet("email/{email}")]
        public async Task<IActionResult> Get(string email)
        {
            Result<User> result;
            var user = await _service.GetUserProfileEmail(email);
            if (user == null)
            {
                result = new Result<User>(404, "User not exists", null);
                return NotFound(result);
            }

            result = new Result<User>(200, null, user);
            return Ok(result);
        }

        //GET api/alluserprofilelist
        [HttpGet("alluserprofilelist")]
        public async Task<List<User>> getUserProfileList()
        {
            var result = await _service.GetUserProfileList();
            return result;
        }

        public class UserLoginInfo { public User user; public LoginInfo loginInfo; }

        //添加新用户
        // POST api/user
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]UserLoginInfo info)
        {
            var result = await _service.InsertUserProfile(info.user);
            if (result.Item1 == 200)
            {
                var new_user = await _service.GetUserProfileEmail(info.user.Email);
                info.loginInfo.UserId = new_user.UserId;
                var result2 = await _service_account.insertLoginInfo(info.loginInfo);
                if(result2)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            
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
            var account = await _service_account.GetLoginInfo(id);
            if (user == null)
                return NotFound();
            _service.DeleteUserProfile(user);
            _service_account.deleteLoginInfo(account);
            return NoContent();
        }
    }
}