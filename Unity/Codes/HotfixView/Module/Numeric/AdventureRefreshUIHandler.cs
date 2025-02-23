namespace ET
{
    public class AdventureRefreshUIHandler: AEvent<EventType.AdventureRefreshUI>
    {
        protected override void Run(EventType.AdventureRefreshUI a)
        {
            var dlgMain = a.ZoneScene.GetComponent<UIComponent>().GetDlgLogic<DlgAdventure>();
            dlgMain?.RefreshUI();
            
        
            
        }
    }
}