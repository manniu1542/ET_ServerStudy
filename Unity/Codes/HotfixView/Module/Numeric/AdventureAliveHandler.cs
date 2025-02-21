namespace ET
{
    public class AdventureAliveHandler: AEvent<EventType.AdventureAlive>
    {
        protected override async void Run(EventType.AdventureAlive a)
        {
            //播放死亡动画

            var curScene = a.ZoneScene.GetComponent<CurrentScenesComponent>().Scene;
            var unitCpt = curScene.GetComponent<UnitComponent>();
            var unit = unitCpt.Get(a.unitId);
            unit.GetComponent<AnimatorComponent>().Play(MotionType.Die);

            await TimerComponent.Instance.WaitAsync(1000);

            if (unit.Type == UnitType.Monster)
                unitCpt.Remove(a.unitId);
        }
    }
}