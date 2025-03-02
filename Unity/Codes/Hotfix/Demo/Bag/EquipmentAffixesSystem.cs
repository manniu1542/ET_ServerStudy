namespace ET
{
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