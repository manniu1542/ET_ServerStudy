using UnityEngine;

namespace ET
{
    public class AdventureStartReset_Event: AEventAsync<EventType.AdventureStartReset>
    {
        protected override async ETTask Run(EventType.AdventureStartReset args)
        {
            var unit = UnitHelper.GetMyUnitFromZoneScene(args.ZoneScene);

            unit.GetComponent<AnimatorComponent>().Play(MotionType.Idle);
            
            await ETTask.CompletedTask;
        }
    }
}