using ET.EventType;

namespace ET
{
    public class UpdateGameTaskInfoEvent: AEvent<EventType.UpdateGameTaskInfo>
    {
        protected override void Run(UpdateGameTaskInfo a)
        {
            //检查当前是否有任务有没有领取的

            var taskCpt = a.ZoneScene.GetComponent<TaskComponent>();

            bool isShowRed = taskCpt.IsCurCanReceivedAnyTaskRewards();

            if (isShowRed)
            {
                RedDotHelper.ShowRedDotNode(a.ZoneScene, RedDotType.GameTask);
            }
            else
            {
                if (RedDotHelper.IsLogicAlreadyShow(a.ZoneScene, RedDotType.GameTask))
                {
                    RedDotHelper.HideRedDotNode(a.ZoneScene, RedDotType.GameTask);
                }
            }

            a.ZoneScene.GetComponent<UIComponent>().GetDlgLogic<DlgTask>()?.RefreshUI();
        }
    }
}