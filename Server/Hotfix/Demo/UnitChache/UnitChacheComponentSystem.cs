namespace ET
{
   
    [FriendClass(typeof (UnitChache))]
    public class UnitChacheComponentAwakeSystem: AwakeSystem<UnitChacheComponent>
    {
        public override void Awake(UnitChacheComponent self)
        {
            //获取 项目中所有类型，找出所有继承了IUnitChache的类型 
            foreach (var type in Game.EventSystem.GetTypes().Values)
            {
                //IsAssignableFrom .该type的类型是否有继承IUnitChache 类型
                if (type != typeof (IUnitChache) && typeof (IUnitChache).IsAssignableFrom(type))
                {
                    self.listUnitChacheKey.Add(type.FullName);
                }
            }
            
            self.listUnitChacheKey.Add(typeof(Unit).FullName);
            
            foreach (var iunitChacheType in self.listUnitChacheKey)
            {
                UnitChache uc = self.AddChild<UnitChache>();
                uc.key = iunitChacheType;
                self.dicUnitChache.Add(iunitChacheType,uc);
            }
        }
    }

    public class UnitChacheComponentDestroySystem: DestroySystem<UnitChacheComponent>
    {
        public override void Destroy(UnitChacheComponent self)
        {
            foreach (var unitChache in   self.dicUnitChache)
            {
                unitChache.Value?.Dispose();
            }
            self.dicUnitChache.Clear();
            self.listUnitChacheKey.Clear();
            
        }
    }

    [FriendClass(typeof (UnitChache))]
    [FriendClass(typeof (UnitChacheComponent))]
    public static class UnitChacheComponentSystem
    {
        public static async ETTask AddOrUpdate(this UnitChacheComponent self, long UnitId, ListComponent<Entity> listCpt)
        {
        
            //更新缓存服的数据
            foreach (var cpt in listCpt)
            {
                string type = cpt.GetType().FullName;
                if (!self.dicUnitChache.TryGetValue(type, out UnitChache unitChache)) 
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
           
        }

        /// <summary>
        /// 获取缓存组件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="UnitId"></param>
        /// <param name="keyCpt"></param>
        /// <returns></returns>
        public static async ETTask<Entity> Get(this UnitChacheComponent self, long unitId, string keyCpt)
        {
            //判断缓存服是否有该数据。有了直接返回把。
            if (!self.dicUnitChache.TryGetValue(keyCpt, out UnitChache unitChache)) 
            {
                unitChache = self.AddChild<UnitChache>();
                unitChache.key = keyCpt;
                self.dicUnitChache.Add(keyCpt, unitChache);
            }
            return await unitChache.Get(unitId);

        }
        /// <summary>
        /// 获取缓存组件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="UnitId"></param>
        /// <param name="keyCpt"></param>
        /// <returns></returns>
        public static void Delete(this UnitChacheComponent self, long unitId)
        {
            //判断缓存服是否有该数据。有了直接返回把。
            foreach (var item in self.dicUnitChache)
            {
                item.Value.Delete(unitId);   
                
            }
       

        }
        
    }
}