namespace ET
{
    public static class ItemFactory
    {
        /// <summary>
        /// 创建Item
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="configId"></param>
        public static Item CreateItem(Entity entity, int configId)
        {
            if ( !ItemConfigCategory.Instance.Contain(configId))
            {
                Log.Error($"当前所创建的物品id 不存在: {configId}");
                return null;
            }
            Item item = entity.AddChild<Item, int>(configId);

            item.RandomItemQuality();
            item.AddItemTypeCpt();
            
            return item;
        }
    }
}