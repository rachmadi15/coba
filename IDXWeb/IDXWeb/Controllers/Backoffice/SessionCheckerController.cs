using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace IDXWeb.Controllers.Backoffice
{
    public class SessionCheckerController : BaseApiController
    {
        [HttpGet]
        public bool GetAuthStatus()
        {
            var sessionId = Security.GetSessionId();
            var userId = Security.GetUserId();

            Guid sessionGuid;
            if(!Guid.TryParse(sessionId, out sessionGuid)) {
                return false;
            }

            var checker = Services.UserService.ValidateLoginSession(userId, sessionGuid);

            if (!checker) {
                Security.ClearCurrentLogin();
            }

            return checker;
        }
    }
}