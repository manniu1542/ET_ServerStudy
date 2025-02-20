namespace ET
{
    public class SceneChangeFinishEventAsyncCreateUIHelp: AEventAsync<EventType.SceneChangeFinish>
    {
        protected override async ETTask Run(EventType.SceneChangeFinish args)
        {
            args.ZoneScene.GetComponent<UIComponent>().HideWindow(WindowID.WindowID_Loading);

            var numCpt = UnitHelper.GetMyUnitNumericComponent(args.CurrentScene);
            int battleLevelId = (int)numCpt[NumericType.AdventureState];
            //正在战斗(重连接续战斗)
            if (battleLevelId != 0)
            {
                AdventureComponent adventureComponent = args.CurrentScene.GetComponent<AdventureComponent>();
                await adventureComponent.StartAdventure();
            }

            await ETTask.CompletedTask;
        }
    }
}