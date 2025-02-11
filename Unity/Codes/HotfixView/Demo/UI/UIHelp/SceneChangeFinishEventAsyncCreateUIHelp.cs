namespace ET
{
    public class SceneChangeFinishEventAsyncCreateUIHelp : AEventAsync<EventType.SceneChangeFinish>
    {
        protected override async ETTask Run(EventType.SceneChangeFinish args)
        {
            args.ZoneScene.GetComponent<UIComponent>().HideWindow(WindowID.WindowID_Loading);
            //UIHelper.Create(args.CurrentScene, UIType.UIHelp, UILayer.Mid).Coroutine();
            await ETTask.CompletedTask;
        }
    }
}
