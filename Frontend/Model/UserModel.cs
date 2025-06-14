namespace Frontend.Model
{
    public class UserModel : NotifiableModelObject
    {
        private string _email, _password;
        public string Email { get => _email; set { _email = value; RaisePropertyChanged("Email"); } }
        public string Password { get => _password; set { _password = value; RaisePropertyChanged("Password"); } }

        public UserModel(BackendController controller, string email, string password) : base(controller)
        {
            _email = email;
            _password = password;
            Email = _email;
            Password = _password;
        }
    }
}
