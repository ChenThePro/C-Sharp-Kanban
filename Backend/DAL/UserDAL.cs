namespace IntroSE.Kanban.Backend.DAL
{
    internal class UserDAL
    {
        public string Email;
        public string Password;


        public UserDAL(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
