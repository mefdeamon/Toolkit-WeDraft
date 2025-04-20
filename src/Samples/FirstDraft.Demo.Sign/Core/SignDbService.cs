using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Demo.Sign.Core
{
    public class SignDbService : BaseSqlService
    {
        public SignDbService() :
            base("laochen",
                new SqlConnectionConfig()
                {
                    Password = "meicreated@2024",
                    Database = "laochen"
                }
            )
        {

        }
    }

    public class base_user
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string password { get; set; }
        public int role { get; set; }
        public DateTime created_at { get; set; }
    }


    public class relation_user_emeter
    {
        public int ur_id { get; set; }
        public int em_id { get; set; }
        public int permission { get; set; }
    }

    public class base_device
    {
        /// <summary>
        /// 设备的唯一标识符，主键，自增
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 设备名称, 用户可以修改
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 注册码，具有唯一性，用于标识设备
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 记录设备的创建时间，也就是第一次设备连入系统时间
        /// </summary>
        public DateTime? created_at { get; set; }

        /// <summary>
        /// 记录设备最后一次上线的时间
        /// </summary>
        public DateTime? last_online_at { get; set; }
    }

}
