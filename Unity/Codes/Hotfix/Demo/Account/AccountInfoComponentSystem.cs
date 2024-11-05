using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ObjectSystem]
    public class AccountInfoComponentAwakeSystem : AwakeSystem<AccountInfoComponent>
    {
        public override void Awake(AccountInfoComponent self)
        {
            
        }
    }

    [ObjectSystem]
    public class AccountInfoComponentDestroySystem : DestroySystem<AccountInfoComponent>
    {
        public override void Destroy(AccountInfoComponent self)
        {
          
        }
    }

    [FriendClass(typeof(AccountInfoComponent))]
    public static class AccountInfoComponentSystem
    {

        public static void ReadAccountInfo(this AccountInfoComponent self,long accountId,string token)
        {

            self.AccountId = accountId;
            self.Token = token;


        }



    }
}
