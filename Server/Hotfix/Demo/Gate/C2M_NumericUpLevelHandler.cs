using System;

namespace ET
{
    public class C2M_NumericUpLevelHandler: AMActorLocationRpcHandler<Unit, C2M_NumericUpLevel, M2C_NumericUpLevel>
    {
        protected override async ETTask Run(Unit unit, C2M_NumericUpLevel request, M2C_NumericUpLevel response, Action reply)
        {
            //设置玩家的 属性 。（设置的时候就会推送相应消息到服务器）
            var numCpt = unit.GetComponent<NumericComponent>();

            int Level = numCpt.GetAsInt(NumericType.Level);
            var config = PlayerLevelConfigCategory.Instance.Get(Level);

            if (config == null)
            {
                Log.Error("不可以升级，没有该等级在表里：" + Level);
                response.Error = ErrorCode.ERR_NumUpLevelCant;
                reply();
                return;
            }

            long exp = numCpt.GetByKey(NumericType.Exp);
            exp -= config.NeedExp;
            if (exp < 0)
            {
                Log.Error("不可以升级，经验不足：" + config.NeedExp);
                response.Error = ErrorCode.ERR_NumUpLevelCant;
                reply();
                return;
            }

            numCpt.Set(NumericType.Exp, exp);
            numCpt[NumericType.AttributePoint] += 1;
            numCpt[NumericType.Level] += 1;

            //更新排行榜的 玩家等级
            RankHelper.SendUpdateRankOfUnitLevel(unit);

            reply();
            await ETTask.CompletedTask;
        }
    }
}