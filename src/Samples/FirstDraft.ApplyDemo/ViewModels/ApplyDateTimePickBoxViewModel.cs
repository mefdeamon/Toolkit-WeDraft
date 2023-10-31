using CommunityToolkit.Mvvm.ComponentModel;
using FirstDraft.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace FirstDraft.ApplyDemo.ViewModels
{
    public class ApplyDateTimePickBoxViewModel : ObservableObject
    {
        public DateTimePickerDataContext DateTimePicker { get; set; } = new DateTimePickerDataContext();
        public TimePickerDataContext TimePicker { get; set; } = new TimePickerDataContext();

        public ApplyDateTimePickBoxViewModel()
        {
            DateTimePicker.SetDateTime(DateTime.Now.AddYears(-1000));
            TimePicker.SetTime(DateTime.Now.TimeOfDay);

            // 获取时间
            var datetimeText= DateTimePicker.DateTimeText;
            var datetime=DateTimePicker.DateTime;
            var timeText = TimePicker.TimeText;
            var time= TimePicker.Time;
        }
    }
}
