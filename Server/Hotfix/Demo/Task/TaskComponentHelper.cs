namespace ET
{
    [FriendClassAttribute(typeof (ET.TaskComponent))]
    public static class TaskComponentHelper
    {
        public static void AsyncGameTask(Unit unit)
        {
            TaskComponent taskCpt = unit.GetComponent<TaskComponent>();
            M2C_UpdateAllTask m2c = new M2C_UpdateAllTask();
            foreach (var forCptDicProduction in taskCpt.sdicItems)
            {
                m2c.GameTaskInfos.Add(forCptDicProduction.Value.ToMsgData());
            }

            MessageHelper.SendToClient(unit, m2c);
        }
    }
}