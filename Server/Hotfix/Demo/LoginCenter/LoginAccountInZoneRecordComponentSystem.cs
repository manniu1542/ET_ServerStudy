using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ObjectSystem]
    public class LoginAccountInZoneRecordComponentAwakeSystem : AwakeSystem<LoginAccountInZoneRecordComponent>
    {
        public override void Awake(LoginAccountInZoneRecordComponent self)
        {
            self.dicLoginAccountZone = new Dictionary<long, int>();

        }
    }
    [FriendClass(typeof(LoginAccountInZoneRecordComponent))]
    public static class LoginAccountInZoneRecordComponentSystem
    {

        public static void Add(this LoginAccountInZoneRecordComponent self, long accountID, int zone)
        {


        }

        public static void Remove(this LoginAccountInZoneRecordComponent self, long accountID)
        {



        }

        public static bool IsExit(this LoginAccountInZoneRecordComponent self, long accountID)
        {
    
            return self.dicLoginAccountZone.ContainsKey(accountID);
        }
        public static int Get(this LoginAccountInZoneRecordComponent self, long accountID)
        {
            if (self.dicLoginAccountZone.ContainsKey(accountID))
            {
              return  self.dicLoginAccountZone[accountID];
            }
        
            return -1;
        }
    }
}
