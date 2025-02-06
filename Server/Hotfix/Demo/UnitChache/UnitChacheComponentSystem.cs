namespace ET
{
    public class UnitChacheComponentAwakeSystem: AwakeSystem<UnitChacheComponent>
    {
        public override void Awake(UnitChacheComponent self)
        {
        }
    }

    public class UnitChacheComponentDestroySystem: DestroySystem<UnitChacheComponent>
    {
        public override void Destroy(UnitChacheComponent self)
        {
        }
    }

    [FriendClass(typeof (UnitChache))]
    [FriendClass(typeof (UnitChacheComponent))]
    public static class UnitChacheComponentSystem
    {
        public static async ETTask AddOrUpdate(this UnitChacheComponent self, long UnitId, ListComponent<Entity> listCpt)
        {
            // using (ListComponent<Entity> list = ListComponent<Entity>.Create())
            // {
            //更新缓存服的数据
            foreach (var cpt in listCpt)
            {
                string type = cpt.GetType().Name;
                if (!self.dicUnitChache.TryGetValue(type, out UnitChache unitChache)) ;
                {
                    unitChache = self.AddChild<UnitChache>();
                    unitChache.key = type;
                    self.dicUnitChache.Add(type, unitChache);
                }
                unitChache.AddOrUpdate(cpt);
            }
            //更新缓存服的数据
            if (listCpt.Count > 0)
                await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Save(UnitId, listCpt);
            // }
        }
    }
}