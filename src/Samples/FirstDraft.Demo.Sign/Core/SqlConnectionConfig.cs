using FirstDraft.Demo.Sign.Utils;
using System.Reflection;

namespace FirstDraft.Demo.Sign.Core
{
    public class SqlConnectionConfig
    {
        private string server = "127.0.0.1";
        public string Server
        {
            get { return server; }
            set { server = value; }
        }

        private string userid = "root";
        public string Userid
        {
            get { return userid; }
            set { userid = value; }
        }

        private string port = "3306";

        public string Port
        {
            get { return port; }
            set { port = value; }
        }


        private string password = "emeter2024";
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string database = "meter_db";

        public string Database
        {
            get { return database; }
            set { database = value; }
        }


        /// <summary>
        /// 加密
        /// </summary>
        public void Encode()
        {

            // 当前类的属性
            Type thisType = GetType();
            PropertyInfo[] thisPropertys = thisType.GetProperties();

            foreach (var item in thisPropertys)
            {
                var val = item.GetValue(this);
                if (val != null)
                {
                    var valStr = val.ToString();
                    {
                        if (!string.IsNullOrEmpty(valStr))
                        {
                            var infoValueAES = AesService.EncryptStringToBytes_Aes(valStr);
                            item.SetValue(this, ConverterService.GetStringFormByteArray(infoValueAES));
                        }
                    }
                }

            }

        }

        /// <summary>
        /// 解密
        /// </summary>
        public void Decode()
        {
            // 当前类的属性
            Type thisType = GetType();
            PropertyInfo[] thisPropertys = thisType.GetProperties();

            foreach (var item in thisPropertys)
            {
                var val = item.GetValue(this);
                if (val != null)
                {
                    var valStr = val.ToString();
                    {
                        if (!string.IsNullOrEmpty(valStr))
                        {
                            var bytes = ConverterService.GetByteArrayFormString(valStr);
                            item.SetValue(this, AesService.DecryptStringFromBytes_Aes(bytes));
                        }
                    }
                }

            }

        }


        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"server = {server}; port={port}; userid = {userid}; password = {password}; database = {database};Charset=utf8; persistsecurityinfo = True;";
        }

    }
}
