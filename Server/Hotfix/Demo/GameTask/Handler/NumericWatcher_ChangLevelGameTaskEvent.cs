namespace ET
{
    /// <summary>
    /// 等级改变的事件触发
    /// </summary>
    [NumericWatcher(NumericType.Level)]
    public class NumericWatcher_ChangLevelGameTaskEvent: INumericWatcher
    {
        public void Run(EventType.NumbericChange args)
        {
            //发送触发

            Unit unit = args.Parent as Unit;
            if (unit == null) return;

            var taskCpt = unit.GetComponent<TaskComponent>();
            taskCpt.TrrigerUpdateEligibleGameTask(GameTaskActionType.UpdateLevel, targetProgress: (int)args.New);
        }
    }
}