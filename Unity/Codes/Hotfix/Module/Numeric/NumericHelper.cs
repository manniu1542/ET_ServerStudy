using System;

namespace ET
{
    public static class NumericHelper
    {
        public static async ETTask<bool> GetNewNumericSpawn(Scene scene)
        {
            var gateSession = scene.ZoneScene().GetComponent<SessionComponent>().Session;
            M2C_NumericCptGet result = null;
            try
            {
                result = await gateSession.Call(new C2M_NumericCptGet()) as M2C_NumericCptGet;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            return result.Error == ErrorCode.ERR_Success;
        }
        public static async ETTask<bool> ReqUpLevel(Scene scene)
        {
            var gateSession = scene.ZoneScene().GetComponent<SessionComponent>().Session;
            M2C_NumericUpLevel result = null;
            try
            {
                result = await gateSession.Call(new C2M_NumericUpLevel()) as M2C_NumericUpLevel;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            return result.Error == ErrorCode.ERR_Success;
        }
    }
}