using ET.EventType;

namespace ET
{
    [FriendClassAttribute(typeof (ET.ForgeComponent))]
    public class ForgeRedPointEvent: AEvent<EventType.RefreshForgeRedPoint>
    {
        protected override void Run(RefreshForgeRedPoint a)
        {
            var forgeCpt = a.ZoneScene.GetComponent<ForgeComponent>();

            bool isCanReceive = false;
            foreach (var forgeCptDicProduction in forgeCpt.dicProductions)
            {
                if (forgeCptDicProduction.Value.IsNeedReceive())
                {
                    isCanReceive = true;
                    break;
                }
            }

            if (isCanReceive)
                RedDotHelper.ShowRedDotNode(a.ZoneScene, RedDotType.Forge);
            else
            {
                if (RedDotHelper.IsLogicAlreadyShow(a.ZoneScene, RedDotType.Forge))
                    RedDotHelper.HideRedDotNode(a.ZoneScene, RedDotType.Forge);
            }
        }
    }
}