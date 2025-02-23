
namespace ET
{

    [NumericWatcher(NumericType.AdventureState)]
    public class NumericWatcher_AdventureRefreshUI: INumericWatcher
    {
        public void Run(EventType.NumbericChange args)
        {
            //发送更新ui页面的消息
            Game.EventSystem.Publish(new EventType.AdventureRefreshUI(){ZoneScene = args.Parent.ZoneScene()});
        }
    }
}