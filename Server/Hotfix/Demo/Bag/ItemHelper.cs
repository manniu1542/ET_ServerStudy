namespace ET
{
    [FriendClassAttribute(typeof (ET.BagComponent))]
    public static class ItemHelper
    {
        /// <summary>
        /// 同步添加item的数据
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="item"></param>
        /// <param name="m2c"></param>
        public static void AsyncAddItemData(Unit unit, Item item, M2C_UpdateSomeOneItem m2c)
        {
            m2c.NetItemOp = (int)NetItemOp.Add;
            m2c.ItemInfo = item.ToMsgData();
            MessageHelper.SendToClient(unit, m2c);
        }
        /// <summary>
        /// 同步移除item的数据
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="item"></param>
        /// <param name="m2c"></param>
        public static void AsyncRemoveItemData(Unit unit, Item item, M2C_UpdateSomeOneItem m2c)
        {
            m2c.NetItemOp = (int)NetItemOp.Remove;
            m2c.ItemInfo = item.ToMsgData();
            MessageHelper.SendToClient(unit, m2c);
        }
        /// <summary>
        /// 同步添加item的数据
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="item"></param>
        /// <param name="m2c"></param>
        public static void AsyncAllBagItemData(Unit unit)
        {
            // 通知客户端重置背包 
            M2C_UpdatePutAllItem m2CUpdatePutAllItem = new M2C_UpdatePutAllItem() { NetItemPut = (int)NetItemPut.Bag, };
            var bagCpt = unit.GetComponent<BagComponent>();
            foreach (var item in bagCpt.dicItems.Values)
            {
                m2CUpdatePutAllItem.ItemInfo.Add(item.ToMsgData());
            }

            MessageHelper.SendToClient(unit, m2CUpdatePutAllItem);
        }
    }
}