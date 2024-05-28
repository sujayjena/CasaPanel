using CasaAPI.Models;
using Microsoft.AspNetCore.Http;

namespace CasaAPI.Helpers
{
    public class SessionManager
    {
        private static long _loggedInUserId;
        public static long LoggedInUserId { set { _loggedInUserId = value; } get { return _loggedInUserId; } }

        public SessionManager()
        {
            UsersLoginSessionData? sessionData = (UsersLoginSessionData?)new HttpContextAccessor().HttpContext.Items["SessionData"]!;
            if (sessionData != null)
            {
                LoggedInUserId = sessionData.UserId;
            }
        }

        //public static void InitializesSessionData()
        //{ 

        //}
    }
}
