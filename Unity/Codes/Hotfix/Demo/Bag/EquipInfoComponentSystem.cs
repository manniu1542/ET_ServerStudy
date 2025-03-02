namespace ET
{
    [ObjectSystem]
    public class EquipInfoComponentAwakeSystem: AwakeSystem<EquipInfoComponent>
    {
        public override void Awake(EquipInfoComponent self)
        {
        }
    }

    [ObjectSystem]
    public class EquipInfoComponentDestroySystem: DestroySystem<EquipInfoComponent>
    {
        public override void Destroy(EquipInfoComponent self)
        {
        }
    }

    [FriendClassAttribute(typeof (ET.EquipInfoComponent))]
    public static class EquipInfoComponentSystem
    {
        public static void ResetDataFormMsg(this EquipInfoComponent self, EquipInfo info)
        {
            self.score = info.Score;
            self.isCreateAffixes = info.IsCreateAffixes;

            self.listAffixes.ForEach(x => x.Dispose());
            self.listAffixes.Clear();

            if (self.isCreateAffixes)
            {
                info.EquipmentAffixesInfos.ForEach(x =>
                {
                    var eqpAff = self.AddChild<EquipmentAffixes>();
                    eqpAff.ResetDataFormMsg(x);
                    self.listAffixes.Add(eqpAff);
                });
            }
        }
    }
}