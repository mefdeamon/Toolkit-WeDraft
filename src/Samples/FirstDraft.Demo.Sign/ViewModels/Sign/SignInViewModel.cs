using FirstDraft.Demo.Sign.Core;
using FirstDraft.Demo.Sign.ViewModels.Sign.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Demo.Sign.ViewModels.Sign
{
   


    public class SignInViewModel : BaseSignViewModel
    {
        public SignInViewModel(SignDbService service) : base(service)
        {
        }

        public override bool CanSign => (!string.IsNullOrWhiteSpace(Email)) && (!string.IsNullOrWhiteSpace(Password));

        protected override void Goto()
        {
            App.Current.Services.GetService<SignViewModel>().ViewType = SignViewType.UP;
        }

        protected override async Task Sign()
        {
            // 输入邮箱格式校验
            if (!Email.IsEmail())
            {
                ErrorMessage = "错误！邮箱格式不正确，请重试！";
                HasError = true;
                WipeErrorAffterMS();
                return;
            }
            await Task.Delay(100);

            var sql = "SELECT * FROM base_user WHERE email = @email";
            var user = dbService.QueryFirstOrDefault<base_user>(sql, new { email = Email });
            if (user == null)
            {
                ErrorMessage = "错误！没有找到邮箱，请重试！";
            }
            else
            {
                if (user.password == SHA256Base64.Hashed(Password))
                {
                    App.Current.Dispatcher.Invoke(() => App.Current.Services.GetService<SignViewModel>().ViewType = SignViewType.ED);
                    App.Current.Services.GetService<SignedViewModel>().SetUser(user);
                    App.Current.Services.GetService<SignViewModel>().IsSigned = true;
                    App.Current.Services.GetService<MainWindowModel>()?.GoHomeCommand?.Execute(null);
                }
                else
                {
                    ErrorMessage = "错误！密码不正确，请重试！";
                }
            }


        }
    }
}
