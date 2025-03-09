using UnityEngine;

namespace ET
{
    [MessageHandler]
    public class M2C_UpdateAllForgeProductionHandler: AMHandler<M2C_UpdateAllForgeProduction>
    {
        protected override void Run(Session session, M2C_UpdateAllForgeProduction message)
        {
            var forgeCpt = session.ZoneScene().GetComponent<ForgeComponent>();
            forgeCpt.ResetFormData(ref message.ProductionInfos);
         
        }
    }
}