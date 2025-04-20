using FirstDraft.Demo.Sign.Core;
using FirstDraft.Demo.Sign.ViewModels.Sign.Base;
using Microsoft.Extensions.DependencyInjection;

namespace FirstDraft.Demo.Sign.ViewModels.Sign
{
    public class SignedViewModel : BaseSignViewModel
    {
        public SignedViewModel(SignDbService service) : base(service)
        {
        }

        public string Phone
        {
            get { return user.phone; }
            set
            {
                SetProperty(user.phone, value, user, (u, n) => u.phone = n);
            }
        }

        public void SetUser(base_user user)
        {
            Username = user.username;
            Email = user.email;
            Phone = user.phone;
        }

        public override bool CanSign => true;

        protected override void Goto()
        {
            App.Current.Dispatcher.Invoke(() => App.Current.Services.GetService<SignViewModel>().ViewType = SignViewType.IN);
            user = new base_user();
            App.Current.Services.GetService<SignViewModel>().IsSigned = false; ;
        }

        protected override Task Sign()
        {
            throw new NotImplementedException();
        }
    }
}
