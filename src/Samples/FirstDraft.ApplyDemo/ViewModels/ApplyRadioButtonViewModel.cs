using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace FirstDraft.ApplyDemo.ViewModels
{
    public class ApplyRadioButtonViewModel : ObservableObject
    {



        public ApplyRadioButtonViewModel()
        {
            Privacys = new List<PrivacyDataModel>();
            foreach (var item in Enum.GetNames(typeof(PrivacyMode)))
            {
                PrivacyMode mode = (PrivacyMode)Enum.Parse(typeof(PrivacyMode), item);
                var p = new PrivacyDataModel(mode);
                Privacys.Add(p);
            }

            Privacy = Privacys[0];
            Privacy.IsChecked = true;
        }

        private PrivacyMode privacyMode;

        public PrivacyMode PrivacyMode
        {
            get { return privacyMode; }
            set { SetProperty(ref privacyMode, value); }
        }


        public List<PrivacyDataModel> Privacys { get; set; }

        private PrivacyDataModel privacy;

        public PrivacyDataModel Privacy
        {
            get { return privacy; }
            set { SetProperty(ref privacy, value); }
        }

        public RelayCommand ChooseCommand => new RelayCommand(() =>
        {
            Privacy = Privacys.Where(t => t.IsChecked).FirstOrDefault();
        });

    }

    public enum PrivacyMode
    {
        [Description("公开")]
        Public = 0,
        [Description("粉丝可见")]
        Partial = 1,
        [Description("关注可见")]
        Protected = 2,
        [Description("私密")]
        Private = 3
    }

    public class PrivacyModeToBoolConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value == null ? false : value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null && value.Equals(true) ? parameter : Binding.DoNothing;
        }
    }
    public class PrivacyDataModel : ObservableObject
    {
        static int id = 0;
        public PrivacyDataModel(PrivacyMode pcyMode)
        {
            Id = id;
            Mode = pcyMode;
            Name = Mode.ToDescription();
            id++;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public PrivacyMode Mode { get; set; }

        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set { SetProperty(ref isChecked, value); }
        }

    }


    public static class EnumExtension
    {
        public static string ToDescription(this Enum val)
        {
            var type = val.GetType();

            var memberInfo = type.GetMember(val.ToString());

            var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes == null || attributes.Length != 1)
            {
                //如果没有定义描述，就把当前枚举值的对应名称返回
                return val.ToString();
            }

            return (attributes.Single() as DescriptionAttribute).Description;
        }
    }
}
