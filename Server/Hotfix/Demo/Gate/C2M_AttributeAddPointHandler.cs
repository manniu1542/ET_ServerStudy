using System;

namespace ET
{
    [FriendClass(typeof (GateMapComponent))]
    [FriendClass(typeof (SessionStateComponent))]
    [FriendClass(typeof (SessionPlayerComponent))]
    [FriendClass(typeof (PlayerComponent))]
    public class C2M_AttributeAddPointHandler: AMActorLocationRpcHandler<Unit, C2M_AttributeAddPoint, M2C_AttributeAddPoint>
    {
        protected override async ETTask Run(Unit unit, C2M_AttributeAddPoint request, M2C_AttributeAddPoint response, Action reply)
        {
            // 属性 值修改。保存缓存服以及 数据库（TODO：定时把缓存服数据存储到数据库一次。这样不用反复调用数据库了）

            var configAttribute = PlayerNumericConfigCategory.Instance.Get(request.AttributeType);
            if (configAttribute == null || configAttribute.isAddPoint != 1)
            {
                Log.Error("不可以加点的类型：" + request.AttributeType);
                response.Error = ErrorCode.ERR_AttributeAddPointCant;
                reply();
                return;
            }

            //查看当前可加点数的值 是否 可用。
            var numCpt = unit.GetComponent<NumericComponent>();
            var attributePoint = numCpt.GetAsInt(NumericType.AttributePoint);
            if (attributePoint <= 0)
            {
                Log.Error("当前点数不足，无法加点");
                response.Error = ErrorCode.ERR_AttributeAddPointDontEnough;
                reply();
                return;
            }

            numCpt.Set(NumericType.AttributePoint, attributePoint - 1);

            //对应属性值加+1 (  修改属性时间分发时会对应调整该调整的属性值，NumericChange_AttributeAddPoint)
            numCpt.Set(request.AttributeType, numCpt.GetAsInt(request.AttributeType) + 1);

            await UnitChacheHelper.AddOrUpdateUnitChache(numCpt);

            reply();
            await ETTask.CompletedTask;
        }
    }
}