namespace ET
{
    public class CreateUnitDamageValueEvent: AEventAsync<EventType.CreateUnitDamageValue>
    {
        protected override async ETTask Run(EventType.CreateUnitDamageValue args)
        {
            var unitCpt = args.ZoneScene.CurrentScene().GetComponent<UnitComponent>();
            var unit = unitCpt.Get(args.unitId);
            unit.GetComponent<HeadHpViewComponent>().SetHp();
           
            args.ZoneScene.GetComponent<FlyDamageValueViewComponent>().SpawnFlyDamage(unit.Position, args.damgeValue).Coroutine();
            
            //最迟1秒钟后这个 角色 会被销毁
            await TimerComponent.Instance.WaitAsync(500);
            if (!unit.IsAlive())
            {
                //这个组件还没被销毁 可以 做个隐藏的延迟
                unit.GetComponent<HeadHpViewComponent>()?.SetVisible(false);         
                
            }
            
            await ETTask.CompletedTask;
        }
    }
}