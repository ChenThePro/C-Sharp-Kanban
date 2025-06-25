using Frontend.Utils;

namespace Frontend.ViewModel
{
    public class VerificationWindowViewModel : NotifiableObject
    {
        private string _inputCode;

        public string Email { get; init; }
        public string InputCode { get => _inputCode; set { _inputCode = value; RaisePropertyChanged(nameof(InputCode)); } }
        public string Message { get; private set; }
        public string Status { get; private set; }

        private readonly InMemoryTempCodeService _codeService;

        public VerificationWindowViewModel(string email, InMemoryTempCodeService codeService)
        {
            Email = email;
            _inputCode = string.Empty;
            Message = string.Empty;
            Status = string.Empty;
            _codeService = codeService;
        }

        public bool Verify()
        {
            if (_codeService.TryGetCode(Email, out var code) && code == InputCode)
            {
                _codeService.Remove(Email);
                Message = "Verification successful! You can now reset your password";
                Status = "Success";
                return true;
            }
            Message = "Invalid code";
            Status = "Error";
            return false;
        }
    }
}