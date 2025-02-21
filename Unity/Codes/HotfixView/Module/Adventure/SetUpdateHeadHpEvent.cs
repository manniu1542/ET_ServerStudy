namespace ET
{
    public class SetUpdateHeadHpEvent: AEvent<EventType.SetUpdateHeadHp>
    {
        protected override void Run(EventType.SetUpdateHeadHp a)
        {
            var unitCpt = a.ZoneScene.GetComponent<CurrentScenesComponent>().Scene.GetComponent<UnitComponent>();
            Unit unit = unitCpt.Get(a.unitId);
            if (unit == null || unit.IsDisposed) return;
            var headHp = unit.GetComponent<HeadHpViewComponent>();

            headHp.SetVisible(a.isShow);
            headHp.SetHp();
        }
    }
}