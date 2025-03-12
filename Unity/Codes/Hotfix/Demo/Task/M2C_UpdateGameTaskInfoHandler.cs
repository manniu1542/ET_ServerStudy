using UnityEngine;

namespace ET
{
    [MessageHandler]
    public class M2C_UpdateGameTaskInfoHandler: AMHandler<M2C_UpdateGameTaskInfo>
    {
        protected override void Run(Session session, M2C_UpdateGameTaskInfo message)
        {
            var forgeCpt = session.ZoneScene().GetComponent<TaskComponent>();
            forgeCpt.AddOrUpdateGameTask(message.GameTaskInfo);
            
        }
    }
}