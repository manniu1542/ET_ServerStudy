using System.Collections.Generic;

namespace ET
{
    public static class RealmGateAddressHelper
    {
        public static StartSceneConfig GetGate(long accountID, int zone)
        {
            //TODO:目前只有一个大区，后续会扩展   （1个大区可能有多个服务器在提供支持，loginAccountZone.Get(acountId) 获取得就是该账号所登录的游戏服务器的号）
             zone = 1;
             
            List<StartSceneConfig> zoneGates = StartSceneConfigCategory.Instance.Gates[zone];

            int n = (int)(accountID % zoneGates.Count); // RandomHelper.RandomNumber(0, zoneGates.Count);

            return zoneGates[n];
        }
    }
}