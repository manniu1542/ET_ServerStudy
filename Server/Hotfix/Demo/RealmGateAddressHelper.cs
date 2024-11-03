using System.Collections.Generic;


namespace ET
{
    public static class RealmGateAddressHelper
    {
        public static StartSceneConfig GetGate(long accountID, int zone)
        {
            List<StartSceneConfig> zoneGates = StartSceneConfigCategory.Instance.Gates[zone];


            int n = (int)(accountID % zoneGates.Count);// RandomHelper.RandomNumber(0, zoneGates.Count);

            return zoneGates[n];
        }
    }
}
