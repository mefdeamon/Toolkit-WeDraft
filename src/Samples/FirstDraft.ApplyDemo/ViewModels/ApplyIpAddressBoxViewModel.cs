using CommunityToolkit.Mvvm.ComponentModel;
using FirstDraft.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace FirstDraft.ApplyDemo.ViewModels
{
    public class ApplyIpAddressBoxViewModel : ObservableObject
    {
        public IpAddressDataContext IpAddress { get; set; }
        public IpAddressDataContext Mask { get; set; }
        public IpAddressDataContext Gateway { get; set; }
        public IpAddressDataContext Server { get; set; }
        public ApplyIpAddressBoxViewModel()
        {
            IpAddress = new IpAddressDataContext();
            IpAddress.SetAddress("192.168.13.58");

            Mask = new IpAddressDataContext();
            Mask.SetAddress("255.255.255.0");

            Gateway = new IpAddressDataContext();
            Gateway.SetAddress("192.168.13.1");

            Server = new IpAddressDataContext();
            Server.SetAddress("192.168.13.168");
        }

        private int port = 5542;
        public int Port
        {
            get { return port; }
            set { SetProperty(ref port, value); }
        }

    }
}
