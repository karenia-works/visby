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

namespace Karenia.Visby.UserProfile.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly UserProfileService _service;

        public UserProfileController(UserProfileContext context)
        {
            _service = new UserProfileService(context);
        }
    }
}