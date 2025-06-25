using Frontend.Controllers;
using Frontend.Utils;

namespace Frontend.ViewModel
{
    public class NewPasswordWindowViewModel : NotifiableObject
    {
        private readonly string _email;

        public string NewPassword { get; set; }
        public string Message { get; private set; }
        public string Status { get; private set; }

        private readonly UserController _controller;

        public NewPasswordWindowViewModel(string email)
        {
            _email = email;
            NewPassword = string.Empty;
            Message = string.Empty;
            Status = string.Empty;
            _controller = ControllerFactory.Instance.UserController;
        }

        public bool ResetPassword()
        {
            try
            {
                _controller.ResetPassword(_email, NewPassword);
                Message = "Password reset successfully";
                Status = "Success";
                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Status = "Error";
                return false;
            }
        }
    }
}