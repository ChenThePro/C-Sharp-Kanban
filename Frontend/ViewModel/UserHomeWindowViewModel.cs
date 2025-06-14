using Frontend.Model;

namespace Frontend.ViewModel
{
    internal class UserHomeWindowViewModel
    {
        private UserModel user;

        public UserHomeWindowViewModel(UserModel user)
        {
            this.user = user;
        }
    }
}
