namespace Frontend.Model
{
    public class UserModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public UserModel(string email, string password) 
        {
            Email = email;
            Password = password;
        }
    }
}
