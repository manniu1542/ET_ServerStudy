using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class AccountInfoComponent : Entity, IAwake, IDestroy
    {
        public long AccountId;
        public string Token;


        public string KeyRealmGate;
        public string AdressRealmGate;


        public string KeyGate;
        public string AdressGate;
    }
}
