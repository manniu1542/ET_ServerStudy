namespace ET
{
    [MessageHandler]
    public class M2C_BagUpdateItemHandler: AMHandler<M2C_BagUpdateItem>
    {
        protected override void Run(Session session, M2C_BagUpdateItem message)
        {
            var bagCpt = session.ZoneScene().GetComponent<BagComponent>();

            if (message.NetItemOp == (int)NetItemOp.Add)
            {
                ItemInfo itemInfo = message.ItemInfo;
                
                bagCpt.AddItem(itemInfo);
            }
            else if (message.NetItemOp == (int)NetItemOp.Remove)
            {
            }
        }
    }
}