using log4net;
using Backend.BuisnessLayer.BoardPackage;

namespace Backend.BuisnessLayer.UserPackage
{
    internal class UserBL
    {
        private bool loggedIn;
        internal string email;
        internal string password;
        internal List<BoardBL> boards;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal UserBL(string email, string password)
        {
            this.email = email;
            this.password = password;
            boards = new List<BoardBL>();
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
            loggedIn = false;
            Log.Info("user logged out");

        }
    }
}
