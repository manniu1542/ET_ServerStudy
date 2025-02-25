namespace ET
{
    public static class ItemHelper
    {
        /// <summary>
        /// 同步添加item的数据
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="item"></param>
        /// <param name="m2c"></param>
        public static void AsyncAddItemData(Unit unit,Item item,M2C_BagUpdateItem m2c)
        {
            m2c.NetItemOp = (int)NetItemOp.Add;
            m2c.ItemInfo = item.ToMsgData();
            MessageHelper.SendToClient(unit,m2c);
        }
        
        
    }
}