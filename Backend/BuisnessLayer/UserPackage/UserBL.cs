using log4net;

namespace Backend.BuisnessLayer.UserPackage
{
    internal class UserBL
    {
        internal bool loggedIn;
        internal string email;
        internal string password;
        private static readonly ILog Log = LogManager.GetLogger(typeof(UserBL));

        internal UserBL(string email, string password)
        {
            this.email = email;
            this.password = password;
            loggedIn = true;
        }

        internal UserBL Login(string password)
        {
            if (this.password != password)
                throw new UnauthorizedAccessException("password incorrect");
            loggedIn = true;
            Log.Info("user logged in successfully");
            return this;
        }

        internal void Logout()
        {
            if (!loggedIn)
                throw new InvalidOperationException("user not logged in");
            loggedIn = false;
            Log.Info("user logged out");
        }
    }
}
