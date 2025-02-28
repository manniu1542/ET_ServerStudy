using UnityEngine;

namespace ET
{
    [MessageHandler]
    public class M2C_UpdateSomeOneItemHandler: AMHandler<M2C_UpdateSomeOneItem>
    {
        protected override void Run(Session session, M2C_UpdateSomeOneItem message)
        {
            //添加道具
            if (message.NetItemOp == (int)NetItemOp.Add)
            {
                Item item = ItemFactory.CreateItem(session.ZoneScene(), message.ItemInfo);
                ItemHelper.AddItem(item, (NetItemPut)message.NetItemPut);
            }
            else if (message.NetItemOp == (int)NetItemOp.Remove)
            {
                ItemHelper.RemoveItem(session.ZoneScene(), message.ItemInfo.Uid, (NetItemPut)message.NetItemPut);
                
            }
        }
    }
}