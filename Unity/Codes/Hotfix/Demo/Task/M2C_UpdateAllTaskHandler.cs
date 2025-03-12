using UnityEngine;

namespace ET
{
    [MessageHandler]
    public class M2C_UpdateAllTaskHandler: AMHandler<M2C_UpdateAllTask>
    {
        protected override void Run(Session session, M2C_UpdateAllTask message)
        {
            var forgeCpt = session.ZoneScene().GetComponent<TaskComponent>();
            message.GameTaskInfos.ForEach(x=>forgeCpt.AddOrUpdateGameTask(x));
        }
    }
}