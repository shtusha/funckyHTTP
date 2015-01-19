using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using FunckyApp.DataAccess;
using FunckyApp.DataAccess.User;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using FunckyApp.Models;
using Microsoft.Owin.Security.Cookies;

namespace FunckyApp.Controllers
{
    [System.Web.Mvc.Authorize]
    [System.Web.Mvc.RoutePrefix("api/Account")]
    public class AccountController : ApiControllerBase
    {
        private const string LocalLoginProvider = "Local";
        private static readonly IUserRepository UserRepository = RepositoryFactory.GetRepository<IUserRepository>();


        public AccountController()
            : this(Startup.OAuthOptions.AccessTokenFormat)
        {
        }

        public AccountController(ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            AccessTokenFormat = accessTokenFormat;
        }


        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [System.Web.Mvc.Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            return new UserInfoViewModel
            {
                UserName = User.Identity.GetUserName(),
                HasRegistered = true,
                LoginProvider = null
            };
        }

        // POST api/Account/Logout
        [System.Web.Mvc.Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [System.Web.Mvc.Route("ManageInfo")]
        public async Task<IHttpActionResult> GetManageInfo()
        {
            var response = await UserRepository.GetAsync(User.Identity.GetUserId());

            if (!response.Success)
            {
                return GetErrorResult(response);
            }

            if (!response.Found)
            {
                ModelState.AddModelError("user.notfound", "User Not Found");
                return NotFound();
            }

            return Ok(new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                UserName = response.Entity.UserName,
                Logins = new List<UserLoginInfoViewModel>
                {
                    new UserLoginInfoViewModel
                    {
                        LoginProvider = LocalLoginProvider,
                        ProviderKey = response.Entity.UserName,
                    }
                }
            });
        }

        // POST api/Account/ChangePassword
        [System.Web.Mvc.Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await UserRepository.GetAsync(User.Identity.GetUserId());

            if (!response.Success)
            {
                return GetErrorResult(response);
            }

            var user = response.Entity;

            if (!response.Found || user.PasswordHash != model.OldPassword.HashPassword(user.HashSalt))
            {
                ModelState.AddModelError("user.notfound", "Invalid user name or password");
                return BadRequest(ModelState);
            }

            user.HashSalt = PasswordUtils.GenerateHashSalt();
            user.PasswordHash = model.NewPassword.HashPassword(user.HashSalt);

            var saveResponse = await UserRepository.SaveAsync(user);

            if (!saveResponse.Success)
            {
                return GetErrorResult(saveResponse);
            }

            return Ok();
        }


        // POST api/Account/Register
        [System.Web.Mvc.AllowAnonymous]
        [System.Web.Mvc.Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await UserRepository.GetAsync(model.UserName);

            if (existingUser.Found)
            {
                ModelState.AddModelError("user.exists", "username is already taken");
                return BadRequest(ModelState);
            }

            var hashSalt = PasswordUtils.GenerateHashSalt();

            var userEntity = new UserEntity
            {
                UserName = model.UserName,
                HashSalt = hashSalt,
                PasswordHash = model.Password.HashPassword(hashSalt)
            };



            var response = await UserRepository.SaveAsync(userEntity);

            if (!response.Success)
            {
                return GetErrorResult(response);
            }

            return Ok();
        }

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }
    }
}
