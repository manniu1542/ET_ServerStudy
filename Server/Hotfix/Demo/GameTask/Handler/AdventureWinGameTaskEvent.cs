using ET.EventType;

namespace ET
{
    public class AdventureWinGameTaskEvent: AEvent<EventType.AdventureWinGameTask>
    {
        protected override void Run(AdventureWinGameTask a)
        {
            Unit unit = Game.EventSystem.Get(a.UnitInstanceId) as Unit;
            if (unit == null) return;

            var taskCpt = unit.GetComponent<TaskComponent>();

            taskCpt.TrrigerUpdateEligibleGameTask(GameTaskActionType.Adventure, a.adventureConfigId);
        }
    }
}