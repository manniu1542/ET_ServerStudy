namespace ET
{
    public static class ItemFactory
    {
        public static Item CreateItem(Entity enity, ItemInfo info)
        {
            Item item = enity.AddChildWithId<Item, int>(info.Uid, info.ConfigID);
            item.ResetFormItemInfo(info);

            return item;
        }
        
      
    }
}