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
            self.numType = 0;
            self.type = EquipmentAffixesType.Normal;
            self.numValue = 0;
        }
    }
    
    [FriendClassAttribute(typeof (ET.EquipmentAffixes))]
    public static class EquipmentAffixesSystem
    {
        public static void ResetDataFormMsg(this EquipmentAffixes self, EquipmentAffixesInfo info)
        {
            self.type = (EquipmentAffixesType)info.EpAffType;
            self.numType = info.NumType;
            self.numValue = info.NumValue;
        }
    }
}