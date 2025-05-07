namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class UserSL
    {
        public string Password { get; set; }
        public string Email { get; init; }

         public UserSL(string password, string email)
         {
            Password = password;
            Email = email;
         }
    }
}
