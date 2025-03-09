namespace ET
{
    [FriendClassAttribute(typeof (ET.ForgeComponent))]
    public static class ForgeHelper
    {
        public static void AsyncForgeProducion(Unit unit)
        {
            ForgeComponent forCpt = unit.GetComponent<ForgeComponent>();
            M2C_UpdateAllForgeProduction m2c = new M2C_UpdateAllForgeProduction();
            foreach (var forCptDicProduction in forCpt.dicProductions)
            {
                m2c.ProductionInfos.Add(forCptDicProduction.Value.ToMsgData());
            }

            MessageHelper.SendToClient(unit, m2c);
        }
    }
}