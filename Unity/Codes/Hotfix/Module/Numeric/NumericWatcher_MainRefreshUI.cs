
namespace ET
{
    /// <summary>
    /// 监视hp数值变化，改变血条值
    /// </summary>
    [NumericWatcher(NumericType.Hp)]
    [NumericWatcher(NumericType.Gold)]
    [NumericWatcher(NumericType.Level)]
    public class NumericWatcher_MainRefreshUI: INumericWatcher
    {
        public void Run(EventType.NumbericChange args)
        {
            //发送更新ui页面的消息
            Game.EventSystem.Publish(new EventType.NumericSpwanUI(){ZoneScene = args.Parent.ZoneScene()});
            
            
        }
    }
}