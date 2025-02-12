using System;

namespace ET
{
    [FriendClass(typeof (RoleInfo))]
    public class C2M_NumericCptGetHandler: AMActorLocationRpcHandler<Unit, C2M_NumericCptGet, M2C_NumericCptGet>
    {
        protected override async ETTask Run(Unit unit, C2M_NumericCptGet request, M2C_NumericCptGet response, Action reply)
        {
            
            //设置玩家的 属性 。（设置的时候就会推送相应消息到服务器）
            var numCpt = unit.GetComponent<NumericComponent>();
            int exp = numCpt.GetAsInt(NumericType.Exp);
            numCpt.Set(NumericType.Exp, exp + 50);
            int gold = numCpt.GetAsInt(NumericType.Gold);
            numCpt.Set(NumericType.Gold, gold + 100);
            long Position = numCpt.GetByKey(NumericType.Position);
            numCpt.Set(NumericType.Position, Position + 50);
            
            

            reply();
            await ETTask.CompletedTask;
        }
    }
}