using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ObjectSystem]
    public class EquipmentAffixesAwakeSystem: AwakeSystem<EquipmentAffixes>
    {
        public override void Awake(EquipmentAffixes self)
        {

        }
    }

    [ObjectSystem]
    public class EquipmentAffixesDestroySystem: DestroySystem<EquipmentAffixes>
    {
        public override void Destroy(EquipmentAffixes self)
        {
        }
    }

    [FriendClass(typeof (EquipmentAffixes))]
    [FriendClassAttribute(typeof (ET.EquipInfoComponent))]
    public static class EquipmentAffixesSystem
    {
        
     
        public static EquipmentAffixesInfo ToMsgData(this EquipmentAffixes self)
        {
            EquipmentAffixesInfo info = new EquipmentAffixesInfo();

            info.NumType = self.numType;
            info.NumValue = self.numValue;
            info.EpAffType = (int)self.type;
            
            return info;
        }
        
    }
}