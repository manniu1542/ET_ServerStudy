namespace ET
{
    // 派送到数值修改消息到客户端
    public class NumericChangeEventAsync_NotifyToClient: AEventClass<EventType.NumbericChange>
    {
        protected override void Run(object args)
        {
            EventType.NumbericChange numbericChange = args as EventType.NumbericChange;

            //服务端跟客户端干的事是不一样的。

            numbericChange.Parent?.GetComponent<NumericNoticeComponent>()?.Notify(numbericChange.NumericType, numbericChange.New);
        }
    }
}