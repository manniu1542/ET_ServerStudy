namespace ET
{
    /// <summary>
    /// 监视hp数值变化，改变血条值
    /// </summary>
    [NumericWatcher(NumericType.Exp)]
    [NumericWatcher(NumericType.AttributePoint)]
    public class NumericWatcher_RoleInfoRefreshUI: INumericWatcher
    {
        public void Run(EventType.NumbericChange args)
        {
            var ZoneScene = args.Parent.ZoneScene();
        
            //判定红点的刷新，是否可以升级，  可加点的数量，

            var numericCpt = UnitHelper.GetMyUnitNumericComponent(ZoneScene.CurrentScene());
            if (args.NumericType == NumericType.Exp)
            {
                int level = (int)numericCpt[NumericType.Level];
                var config = PlayerLevelConfigCategory.Instance.Get(level);
                if (config.NeedExp <= numericCpt[NumericType.Exp])
                {
                    RedDotHelper.ShowRedDotNode(ZoneScene, RedDotType.Role_Level);
                }
                else
                {
                    RedDotHelper.HideRedDotNode(ZoneScene, RedDotType.Role_Level);
                }
            }

            if (args.NumericType == NumericType.AttributePoint)
                if (numericCpt[NumericType.AttributePoint] > 0)
                {
                    RedDotHelper.ShowRedDotNode(ZoneScene, RedDotType.Role_AttributePoint);
                }
                else
                {
                    RedDotHelper.HideRedDotNode(ZoneScene, RedDotType.Role_AttributePoint);
                }
            
            
            var dlgRole = args.Parent.ZoneScene().GetComponent<UIComponent>().GetDlgLogic<DlgRoleInfo>();
            dlgRole?.RefreshUI();

        }
    }
}