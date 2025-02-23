namespace ET
{
    public class NumericSpwanUIHandler: AEvent<EventType.NumericSpwanUI>
    {
        protected override void Run(EventType.NumericSpwanUI a)
        {
            var dlgMain = a.ZoneScene.GetComponent<UIComponent>().GetDlgLogic<DlgMain>();
            dlgMain?.RefreshUI();

        }
    }
}