using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceLayer.DTOs;
using ServiceLayer.Helpers;
using ServiceLayer.Models;
using ServiceLayer.Services;

namespace SunSD.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UserController(IUserService userService, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]LoginDTO userParam)
        //public IActionResult Authenticate()
        {
            //LoginDTO userParam = new LoginDTO() { Username = "naveed", Password = "naveed" };
            //var user = _userService.Authenticate(userParam.Username, userParam.Password);

            var user = _userService.Authenticate(userParam.Username, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user.Token);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var user = User.Identity.Name;

            var users = _userService.GetAll();
            return Ok(users);
        }

        [AllowAnonymous]
        [HttpGet("CreateTestUser")]
        public IActionResult CreateTestUser()
        {
            User user = new User();
            user.FirstName = "naveed1";
            user.LastName = "naveed1";
            user.Username = "naveed1";
            var userEntity = _userService.Create(user,"pas123");
            var userDto = _mapper.Map<UserDTO>(userEntity);
            return Ok(user);
        }
    }
}