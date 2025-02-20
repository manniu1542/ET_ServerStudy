using System;

namespace ET
{
    public static class AdventureHelper
    {
        public static async ETTask<bool> OnStartGameLevelClickHandler(Scene zoneScene, int levelId)
        {
            Session gateSession = zoneScene.GetComponent<SessionComponent>().Session;
            M2C_GoToAdventure enterGame = null;

            try
            {
                enterGame = await gateSession.Call(new C2M_GoToAdventure() { BattleLevelConfigID = levelId }) as M2C_GoToAdventure;
            }
            catch (Exception e)
            {
                Log.Error(e);
                // ErrorCode.ERR_NetReqTimeOut;
                return false;
            }

            if (enterGame.Error != ErrorCode.ERR_Success)
            {
                Log.Error("前往冒险网络请求失败！错误码：" + enterGame.Error);
            }

            return enterGame.Error == ErrorCode.ERR_Success;
        }
        public static async ETTask<bool> OnEndGameCheck(Scene zoneScene, int roundCount,AdventureBattleRoundState state)
        {
            Session gateSession = zoneScene.GetComponent<SessionComponent>().Session;
            M2C_AdventureEnd enterGame = null;

            try
            {
                enterGame = await gateSession.Call(new C2M_AdventureEnd() { RoundCount = roundCount ,AdventureBattleRoundState = (int)state}) as M2C_AdventureEnd;
            }
            catch (Exception e)
            {
                Log.Error(e);
                // ErrorCode.ERR_NetReqTimeOut;
                return false;
            }

            if (enterGame.Error != ErrorCode.ERR_Success)
            {
                Log.Error("冒险结束网络请求失败！错误码：" + enterGame.Error);
            }

            return enterGame.Error == ErrorCode.ERR_Success;
        }
    }
}