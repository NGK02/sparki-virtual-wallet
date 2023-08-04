using VirtualWallet.DataAccess.Enums;
using VirtualWallet.Web.Helper.Contracts;

namespace VirtualWallet.Web.Helper
{
    public class AuthManagerMVC : IAuthManagerMVC
    {
        public const string notAthorized = "You are not Authorized to do this!";

        private readonly IHttpContextAccessor contextAccessor;

        public AuthManagerMVC(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }
        /// <summary>
        /// Check if there is logged user.
        /// <param key="LoggedUser">Key paraim is LoggedUser from session.</param>
        /// </summary>
        public bool isLogged(string key)
        {
            if (!this.contextAccessor.HttpContext.Session.Keys.Contains(key))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Check if session logged user is admin.
        /// <param key="roleId">Key paraim is roleId from session.</param>
        /// </summary>
        public bool isAdmin(string key)
        {
            if (this.contextAccessor.HttpContext.Session.GetInt32(key) != (int)RoleName.Admin)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Check if logged user ID matches the content creator ID.
        /// <param key="userId">Key paraim is userId from session.</param>
        /// </summary>
        public bool isContentCreator(string key, int contentCreatorId)
        {
            if (this.contextAccessor.HttpContext.Session.GetInt32(key) != contentCreatorId)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Check if logged user is blocked.
        /// <param key="roleId">Key paraim is roleId from session.</param>
        /// </summary>
        public bool isBlocked(string key)
        {
            if (this.contextAccessor.HttpContext.Session.GetInt32(key) == (int)RoleName.Blocked)
            {
                return true;
            }
            return false;
        }
    }
}
