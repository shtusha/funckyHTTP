﻿using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using FunckyApp.DataAccess;

namespace FunckyApp
{
    public class ApiControllerBase : ApiController
    {
        protected internal IHttpActionResult GetErrorResult(RepositoryOperationResponseBase result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Success)
            {
                if (result.Errors != null)
                {
                    result.Errors.ForEach(a => ModelState.AddModelError(string.Empty, a));
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        protected bool IsAuthenticated { get { return User.Identity.IsAuthenticated; } }

        protected string UserName { get { return GetUserName(); } }

        private string GetUserName()
        {
            var identity = User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var nameIdentifierClaim = identity.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
                if (nameIdentifierClaim != null)
                {
                    return nameIdentifierClaim.Value;
                }
            }
            return null;
        }
    }
}