namespace ET
{
    public class UnitChacheAwakeSystem: AwakeSystem<UnitChache>
    {
        public override void Awake(UnitChache self)
        {
        }
    }

    public class UnitChacheDestroySystem: DestroySystem<UnitChache>
    {
        public override void Destroy(UnitChache self)
        {
            foreach (var chacheCpt in self.dicChacheComponent.Values)
            {
                chacheCpt?.Dispose();
            }
            self.dicChacheComponent.Clear();
            self.key = null;
        }
    }

    [FriendClass(typeof (UnitChache))]
    [FriendClass(typeof (UnitChache))]
    public static class UnitChacheSystem
    {
        /// <summary>
        /// 添加更新 该类型下的 组件。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="entity"></param>
        public static void AddOrUpdate(this UnitChache self, Entity entity)
        {
            if (entity == null) return;

            //这个缓存组件已经存在，移除
            if (self.dicChacheComponent.TryGetValue(entity.Id, out Entity oldEntity))
            {
                if (entity != oldEntity)
                {
                    //防止 老的entity没有被释放掉
                    oldEntity.Dispose();
                }

                self.dicChacheComponent.Remove(entity.Id);
            }

            self.dicChacheComponent.Add(entity.Id, entity);
        }

        /// <summary>
        /// 获取缓存的组件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="UnitId"></param>
        /// <returns></returns>
        public static async ETTask<Entity> Get(this UnitChache self, long unitId)
        {
            if (!self.dicChacheComponent.TryGetValue(unitId, out Entity entity))
            {
                //获取他的类型，不要全类型，数据库存储 ET的entity类时，不是全类型存储。
                string type = Game.EventSystem.GetType(self.key).Name;
                entity = await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Query<Entity>(unitId, type);
                if (entity != null)
                    self.AddOrUpdate(entity);
            }

            return entity;
        }
        
        /// <summary>
        /// 获取缓存的组件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public static  void Delete(this UnitChache self, long unitId)
        {
            if (self.dicChacheComponent.TryGetValue(unitId, out Entity entity))
            {
                entity.Dispose();
                self.dicChacheComponent.Remove(unitId);
            }

        }
    }
}