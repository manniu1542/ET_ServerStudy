using ET.EventType;

namespace ET
{
    [FriendClassAttribute(typeof(ET.EquipmentAffixes))]    // 派送到数值修改消息到客户端
    [FriendClassAttribute(typeof(ET.EquipInfoComponent))]
    public class NumericChangeEventAsync_RoleEquipChang : AEvent<NumCpt_RoleEquipChange>
    {
        protected override void Run(NumCpt_RoleEquipChange args)
        {
            var eqpCpt = args.item.GetComponent<EquipInfoComponent>();

            var numCpt = args.Unit.GetComponent<NumericComponent>();
            switch (args.op)
            {
                case RoleItemOp.DressUp:
                    foreach (var aff in eqpCpt.listAffixes)
                    {
                        int type = aff.numType * 10 + 2;
                        numCpt[type] += aff.numValue;
                    }


                    break;
                case RoleItemOp.Unload:
                    foreach (var aff in eqpCpt.listAffixes)
                    {
                        int type = aff.numType * 10 + 2;
                        numCpt[type] -= aff.numValue;
                    }
                    break;
            }
        }
    }
}