using UnityEngine;

namespace ET
{
    [MessageHandler]
    public class M2C_NumbericChangeHandler: AMHandler<M2C_NumbericChange>
    {
        protected override void Run(Session session, M2C_NumbericChange message)
        {
            var curScene = session.DomainScene().GetComponent<CurrentScenesComponent>().Scene;
            var unit = curScene?.GetComponent<UnitComponent>()?.Get(message.UnitID);
           
            var numericComponent = unit?.GetComponent<NumericComponent>();
   
            numericComponent?.Set(message.NumType, message.NumValue);
        }
    }
}