using ET.EventType;

namespace ET
{
    public class ForgeProductionGameTaskEvent: AEvent<EventType.ForgeProductionGameTask>
    {
        protected override void Run(ForgeProductionGameTask a)
        {
            Unit unit = Game.EventSystem.Get(a.UnitInstanceId) as Unit;
            if (unit == null) return;

            var taskCpt = unit.GetComponent<TaskComponent>();
            taskCpt.TrrigerUpdateEligibleGameTask(GameTaskActionType.Forge, a.ProductionID);
        }
    }
}