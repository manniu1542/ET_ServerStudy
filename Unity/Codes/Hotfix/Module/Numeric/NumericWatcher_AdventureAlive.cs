
using ET.EventType;

namespace ET
{
    /// <summary>
    /// 监视hp数值变化，改变血条值
    /// </summary>
    [NumericWatcher(NumericType.IsAlive)]
    public class NumericWatcher_AdventureAlive: INumericWatcher
    {
        public void Run(EventType.NumbericChange args)
        {
            //发送更新ui页面的消息
            AdventureAlive alive = new EventType.AdventureAlive();
            alive.ZoneScene = args.Parent.ZoneScene();
            alive.unitId = args.Parent.Id;
            Game.EventSystem.Publish(alive);
        }
    }
}