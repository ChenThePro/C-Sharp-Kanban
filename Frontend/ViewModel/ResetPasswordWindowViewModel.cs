using Frontend.Controllers;
using Frontend.Utils;

namespace Frontend.ViewModel
{
    public class ResetPasswordWindowViewModel : NotifiableObject
    {
        private string _email;

        public string Email { get => _email; set { _email = value; RaisePropertyChanged(nameof(Email)); } }
        public string Message { get; private set; }
        public string Status { get; private set; }

        private readonly InMemoryTempCodeService _codeService;
        private readonly EmailService _emailService;
        private readonly UserController _controller;

        public ResetPasswordWindowViewModel(InMemoryTempCodeService codeService)
        {
            _email = string.Empty;
            Message = string.Empty;
            Status = string.Empty;
            _codeService = codeService;
            _emailService = new();
            _controller = ControllerFactory.Instance.UserController;
        }

        public bool SendResetCode()
        {
            try
            {
                _controller.AuthenticateUser(Email);
                string code = new Random().Next(100000, 999999).ToString();
                _codeService.Save(Email, code);
                Message = "Code sent successfully";
                Status = "Success";
                return _emailService.SendResetCode(Email, code);
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