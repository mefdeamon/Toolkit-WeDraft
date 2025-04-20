using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FirstDraft.Demo.Sign.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FirstDraft.Demo.Sign.ViewModels.Sign.Base
{
    public abstract class BaseSign : ObservableObject
    {

        protected void Init()
        {
            Email = string.Empty;
            Password = string.Empty;
            Username = string.Empty;
        }

        protected base_user user { get; set; } = new base_user();

        /// <summary>
        /// Is ture U Can Sign
        /// </summary>
        public abstract bool CanSign { get; }


        public string Email
        {
            get { return user.email; }
            set
            {
                SetProperty(user.email, value, user, (u, n) => u.email = n);
                OnPropertyChanged(nameof(CanSign));
            }
        }

        public string Password
        {
            get { return user.password; }
            set
            {
                SetProperty(user.password, value, user, (u, n) => u.password = n);
                OnPropertyChanged(nameof(CanSign));
            }
        }

        public string Username
        {
            get { return user.username; }
            set
            {
                SetProperty(user.username, value, user, (u, n) => u.username = n);
                OnPropertyChanged(nameof(CanSign));
            }
        }
    }


    /// <summary>
    /// The base class of sign in information verification 
    /// </summary>
    public abstract class BaseSignViewModel : BaseSign
    {
        protected readonly SignDbService dbService;
        public BaseSignViewModel(SignDbService service)
        {
            dbService = service;
        }
        #region ERROR NOTIFY

        private bool hasError = false;

        /// <summary>
        /// has a error occured
        /// </summary>
        public bool HasError
        {
            get { return hasError; }
            set { SetProperty(ref hasError, value); }
        }

        private string errorMessage = "";

        /// <summary>
        /// the error message
        /// </summary>
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }

        #endregion

        private bool needRemember;

        /// <summary>
        /// Remember this input and fill in automatically next time
        /// </summary>
        public bool NeedRemember
        {
            get { return needRemember; }
            set { SetProperty(ref needRemember, value); }
        }

        private bool isSigning;

        /// <summary>
        /// Show the status of Sign Button is Running or not
        /// </summary>
        public bool IsSigning
        {
            get { return isSigning; }
            set { SetProperty(ref isSigning, value); }
        }

        #region 异步执行

        protected object mPropertyValueCheckLock = new object();

        protected async Task RunCommandAsync(Expression<Func<bool>> updatingFlag, Func<Task> action)
        {
            // Lock to ensure single access to check
            lock (mPropertyValueCheckLock)
            {
                // Check if the flag property is true (meaning the function is already running)
                if (updatingFlag.GetPropertyValue())
                    return;

                // Set the property flag to true to indicate we are running
                updatingFlag.SetPropertyValue(true);
            }

            try
            {
                // Run the passed in action
                await action();
            }
            finally
            {
                // Set the property flag back to false now it's finished
                updatingFlag.SetPropertyValue(false);
            }
        }

        #endregion

        /// <summary>
        /// Validate the command
        /// </summary>
        public RelayCommand SignCommand => new RelayCommand(async () =>
        {
            await RunCommandAsync(() => IsSigning, async () =>
            {
                await Sign();
            });
        });

        /// <summary>
        /// Skip to other (like SignUp or SignInEmail) page command
        /// </summary>
        public RelayCommand GotoCommand => new RelayCommand(Goto);

        protected abstract void Goto();

        protected abstract Task Sign();

        #region 公共处理方法

        /// <summary>
        /// Wait for a while, then erase the error message display
        /// </summary>
        /// <param name="ms">The number of milliseconds to wait</param>
        protected void WipeErrorAffterMS(int ms = 5000)
        {
            Task.Run(() =>
            {
                System.Threading.Thread.Sleep(ms);
                ErrorMessage = "";
                HasError = false;
            });
        }

        #endregion
    }

    #region 扩展方法

    public static class ExpressionHelpers
    {
        /// <summary>
        /// Compiles an expression and gets the functions return value
        /// </summary>
        /// <typeparam name="T">The type of return value</typeparam>
        /// <param name="lambda">The expression to compile</param>
        /// <returns></returns>
        public static T GetPropertyValue<T>(this Expression<Func<T>> lambda)
        {
            return lambda.Compile().Invoke();
        }

        /// <summary>
        /// Sets the underlying properties value to the given value
        /// from an expression that contains the property
        /// </summary>
        /// <typeparam name="T">The type of value to set</typeparam>
        /// <param name="lambda">The expression</param>
        /// <param name="value">The value to set the property to</param>
        public static void SetPropertyValue<T>(this Expression<Func<T>> lambda, T value)
        {
            // Converts a lambda () => some.Property, to some.Property
            var expression = (lambda as LambdaExpression).Body as MemberExpression;

            // Get the property information so we can set it
            var propertyInfo = (PropertyInfo)expression.Member;
            var target = System.Linq.Expressions.Expression.Lambda(expression.Expression).Compile().DynamicInvoke();

            // Set the property value
            propertyInfo.SetValue(target, value);
        }

    }

    public static class CommonHelper
    {
        /// <summary>
        /// 是邮箱
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsEmail(this string source)
        {
            if(source == null) return false;
            return Regex.IsMatch(source, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", RegexOptions.IgnoreCase);
        }
    }

    public class SHA256Base64
    {
        /// <summary>
        /// Get Hash password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Hashed(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hashedPassword = sha256.ComputeHash(passwordBytes);
                var hashedPasswordString = Convert.ToBase64String(hashedPassword);
                return hashedPasswordString;
            }
        }
    }
    #endregion
}
