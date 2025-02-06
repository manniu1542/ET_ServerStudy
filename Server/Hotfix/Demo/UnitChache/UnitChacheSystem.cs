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
    }
}