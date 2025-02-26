using UnityEngine;

namespace ET
{
    [MessageHandler]
    public class M2C_UpdatePutAllItemHandler: AMHandler<M2C_UpdatePutAllItem>
    {
        protected override void Run(Session session, M2C_UpdatePutAllItem message)
        {
            ItemHelper.Clear(session.ZoneScene(), (NetItemPut)message.NetItemPut);
            foreach (var itemInfo in message.ItemInfo)
            {
                var item = ItemFactory.CreateItem(session.ZoneScene(), itemInfo);
                ItemHelper.AddItem(item, (NetItemPut)message.NetItemPut);
            }
        }
    }
}