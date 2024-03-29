﻿using System;
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
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace Karenia.Visby.UserProfile.Controllers
{
    public class UserLoginInfo
    {
        public User User { get; set; }
        public LoginInfo LoginInfo { get; set; }
    }

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


        //获取自身身份
        [HttpGet("me")]
        // [Authorize("userProfileApi")]
        public async Task<IActionResult> GetMe()
        {
            // var user_email = User.Claims.Where(claim => claim.Type == "sub").FirstOrDefault().Value;
            // Console.WriteLine(User.Claims);
            var user = await _service.GetUserProfile(5);
            return Ok(user);
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


        //添加新用户
        // POST api/user
        [HttpPost]
        // public async Task<IActionResult> Post([FromBody]User user)
        // {
        //     var result = await _service.InsertUserProfile(user);
        //     if (result.Item1 == 200)
        //     {
        //         var new_user = await _service.GetUserProfileEmail(user.Email);
        //             return Ok(result);
        //     }

        //     return BadRequest(result);
        // }

        public async Task<IActionResult> Post([FromBody]UserLoginInfo info)
        {
            ObjectDump.Write(Console.Out, info);
            var result = await _service.InsertUserProfile(info.User);
            if (result.Item1 == 200)
            {
                var new_user = await _service.GetUserProfileEmail(info.User.Email);
                info.LoginInfo.UserId = new_user.UserId;
                var result2 = await _service_account.insertLoginInfo(info.LoginInfo);
                if (result2)
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
                return BadRequest("No such user");
            _service.DeleteUserProfile(user);
            _service_account.deleteLoginInfo(account);
            return NoContent();
        }

        [HttpPost("{id}/add-favourite")]
        public async Task<IActionResult> AddFavorite(int id, [FromQuery] int paper)
        {
            var result = await _service.AddFavorite(id, paper);
            if (result)
            {
                return NoContent();
            }
            else
            {
                return BadRequest("No such user");
            }
        }

        [HttpPost("{id}/add-download")]
        public async Task<IActionResult> AddDownload(int id, [FromQuery] int paper)
        {
            var result = await _service.AddDownload(id, paper);
            if (result)
            {
                return NoContent();
            }
            else
            {
                return BadRequest("No such user");
            }
        }

    }

    public static class ObjectDump
    {
        public static void Write(TextWriter writer, object obj)
        {
            if (obj == null)
            {
                writer.WriteLine("Object is null");
                return;
            }

            writer.Write("Hash: ");
            writer.WriteLine(obj.GetHashCode());
            writer.Write("Type: ");
            writer.WriteLine(obj.GetType());

            var props = GetProperties(obj);

            if (props.Count > 0)
            {
                writer.WriteLine("-------------------------");
            }

            foreach (var prop in props)
            {
                writer.Write(prop.Key);
                writer.Write(": ");
                writer.WriteLine(prop.Value);
            }
        }

        private static Dictionary<string, string> GetProperties(object obj)
        {
            var props = new Dictionary<string, string>();
            if (obj == null)
                return props;

            var type = obj.GetType();
            foreach (var prop in type.GetProperties())
            {
                var val = prop.GetValue(obj, new object[] { });
                var valStr = val == null ? "" : val.ToString();
                props.Add(prop.Name, valStr);
            }

            return props;
        }
    }

}
