using FirstDraft.Demo.Sign.Core;
using FirstDraft.Demo.Sign.ViewModels.Sign.Base;
using Microsoft.Extensions.DependencyInjection;

namespace FirstDraft.Demo.Sign.ViewModels.Sign
{
    public class SignUpViewModel : BaseSignViewModel
    {
        public SignUpViewModel(SignDbService service) : base(service)
        {
        }

        public string Phone
        {
            get { return user.phone; }
            set
            {
                SetProperty(user.phone, value, user, (u, n) => u.phone = n);
                OnPropertyChanged(nameof(CanSign));
            }
        }

        private string confirmPassword;
        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set
            {
                SetProperty(ref confirmPassword, value);
                OnPropertyChanged(nameof(CanSign));
            }
        }


        public override bool CanSign => !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Username) && (Password == ConfirmPassword);

        protected override void Goto()
        {
            App.Current.Services.GetService<SignViewModel>().ViewType = SignViewType.IN;
        }

        protected override async Task Sign()
        {
            ErrorMessage = "";

            // 输入邮箱格式校验
            if (!Email.IsEmail())
            {
                ErrorMessage = "错误！邮箱格式不正确，请重试！";
                HasError = true;
                WipeErrorAffterMS();
                return;
            }

            if (Password.Length < 6)
            {
                ErrorMessage = "错误！密码长度不能少于6个符号(字母数字英文符号)，请重试！";
                HasError = true;
                WipeErrorAffterMS();
                return;
            }

            await Task.Delay(100);

            var mysqlPassword = SHA256Base64.Hashed(Password);

            var user = new base_user()
            {
                username = Username,
                phone = Phone,
                email = Email,
                password = mysqlPassword,
                role = 1,
                created_at = DateTime.Now
            };
            string sql = @"INSERT INTO base_user (username, email, phone, password, role, created_at) VALUES (@username, @email,@phone, @password, @role, @created_at);";

            var result = dbService.Insert(sql, user);
            if (result.Success)
            {
                App.Current.Dispatcher.Invoke(() => App.Current.Services.GetService<SignViewModel>().ViewType = SignViewType.ED);
                App.Current.Services.GetService<SignedViewModel>().SetUser(user);
            }
            else
            {
                ErrorMessage = $"错误！{result.Message}，请重试！";
            }
        }
    }
}
