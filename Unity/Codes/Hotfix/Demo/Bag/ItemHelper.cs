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