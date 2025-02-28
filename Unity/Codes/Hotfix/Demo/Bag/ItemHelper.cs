namespace ET
{
    public static class ItemHelper
    {
      
        public static void AddItem(Item item,NetItemPut type)
        {
            
            if (type == NetItemPut.Bag)
            {
                var bagCpt = item.ZoneScene().GetComponent<BagComponent>();

                if (!bagCpt.AddItem(item))
                {
                    item.Dispose();
                }
            }
            else if (type == NetItemPut.Role)
            {
            }
        }
        public static void RemoveItem(Scene zoneScene,long itemId,NetItemPut type)
        {
            
            if (type == NetItemPut.Bag)
            {
                var bagCpt = zoneScene.GetComponent<BagComponent>();

                if (!bagCpt.RemoveItem(itemId))
                {
                    Log.Error("移除道具失败，背包 不存在道具："+itemId);
                }
            }
            else if (type == NetItemPut.Role)
            {
            }
        }
        public static void Clear(Scene zoneScene,NetItemPut type)
        {
            
            if (type == NetItemPut.Bag)
            {
                var bagCpt = zoneScene.GetComponent<BagComponent>();

                bagCpt.Clear();
            }
            else if (type == NetItemPut.Role)
            {
            }
        }

        
    }
}