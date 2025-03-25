using ET.EventType;

namespace ET
{
    public class RefreshChatUIHandler: AEvent<EventType.RefreshChatUI>
    {
        protected override void Run(RefreshChatUI a)
        {
            a.ZoneScene.GetComponent<UIComponent>().GetDlgLogic<DlgChat>()?.RefreshUI();
        }
    }
}