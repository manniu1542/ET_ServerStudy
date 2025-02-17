
namespace ET
{
    /// <summary>
    /// 监视hp数值变化，改变血条值
    /// </summary>
    [NumericWatcher(NumericType.Level)]
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