namespace ET
{
    public static class ItemHelper
    {
        public static void AddItem(Item item, NetItemPut type)
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
                var roleEqpCpt = item.ZoneScene().GetComponent<RoleEquipComponent>();

                if (!roleEqpCpt.DressUpItem(item))
                {
                    item.Dispose();
                }
            }
        }

        public static void RemoveItem(Scene zoneScene, long itemId, NetItemPut type)
        {
            if (type == NetItemPut.Bag)
            {
                var bagCpt = zoneScene.GetComponent<BagComponent>();

                if (!bagCpt.RemoveItem(itemId))
                {
                    Log.Error("移除道具失败，背包 不存在道具：" + itemId);
                }
            }
            else if (type == NetItemPut.Role)
            {
                var roleEqpCpt = zoneScene.GetComponent<RoleEquipComponent>();
                var item = roleEqpCpt.GetItemById(itemId);
                if (!roleEqpCpt.UnloadItem(item.Config.EquipPosition))
                {
                    item.Dispose();
                }
            }
        }

        public static Item GetItem(Scene zoneScene, long itemId, NetItemPut type)
        {
            Item item = null;
            if (type == NetItemPut.Bag)
            {
                var bagCpt = zoneScene.GetComponent<BagComponent>();

                item = bagCpt.GetItem(itemId);
            }
            else if (type == NetItemPut.Role)
            {
                var roleEqpCpt = zoneScene.GetComponent<RoleEquipComponent>();

                item = roleEqpCpt.GetItemById(itemId);
            }

            return item;
        }

        public static void Clear(Scene zoneScene, NetItemPut type)
        {
            if (type == NetItemPut.Bag)
            {
                var bagCpt = zoneScene.GetComponent<BagComponent>();

                bagCpt.Clear();
            }
            else if (type == NetItemPut.Role)
            {
                var roleEqpCpt = zoneScene.GetComponent<RoleEquipComponent>();

                roleEqpCpt.Clear();
            }
        }
    }
}