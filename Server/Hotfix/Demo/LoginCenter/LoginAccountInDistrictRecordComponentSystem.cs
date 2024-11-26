using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ObjectSystem]
    public class LoginAccountInDistrictRecordComponentAwakeSystem : AwakeSystem<LoginAccountInDistrictRecordComponent>
    {
        public override void Awake(LoginAccountInDistrictRecordComponent self)
        {
            self.dicLoginAccountDistrict = new Dictionary<long, int>();

        }
    }
    [FriendClass(typeof(LoginAccountInDistrictRecordComponent))]
    public static class LoginAccountInDistrictRecordComponentSystem
    {

        public static void Add(this LoginAccountInDistrictRecordComponent self, long accountID, long zone)
        {
            if (self.dicLoginAccountDistrict.ContainsKey(accountID))
            {
                self.dicLoginAccountDistrict[accountID] = (int)zone;
            }
            else
            {
                self.dicLoginAccountDistrict.Add(accountID, (int)zone);
            }

        }

        public static void Remove(this LoginAccountInDistrictRecordComponent self, long accountID)
        {

            self.dicLoginAccountDistrict.Remove(accountID);

        }

        public static bool IsExit(this LoginAccountInDistrictRecordComponent self, long accountID)
        {

            return self.dicLoginAccountDistrict.ContainsKey(accountID);
        }
        public static int Get(this LoginAccountInDistrictRecordComponent self, long accountID)
        {
            if (self.dicLoginAccountDistrict.ContainsKey(accountID))
            {
                return self.dicLoginAccountDistrict[accountID];
            }

            return -1;
        }
    }
}
