using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public enum AccountType
    {
        Normal,
        Black,
        Vip
    }
    public class Account:Entity,IAwake,IDestroy
    {
        public string AccountName;

        public string Password;

        public string CreateTime;

        public AccountType Type;




    }
}
