using System;

namespace ET
{
    public static class ES_AttributeItemSystem
    {
        //注册btn时间， 刷新ui  ， 点击事件

        public static void RegisterUIEvent(this ES_AttributeItem self, int type)
        {
            EUIHelper.AddListener(self.E_AddButton, async () =>
            {
                bool isFinish = await self.ReqAddAttribute(type);
                if (isFinish) self.ZoneScene().GetComponent<UIComponent>().GetDlgLogic<DlgRoleInfo>()?.RefreshUI();
            });
        }

        public static async ETTask<bool> ReqAddAttribute(this ES_AttributeItem self, int type)
        {
            Session gateSession = self.ZoneScene().GetComponent<SessionComponent>().Session;
            M2C_AttributeAddPoint enterGame = null;

            try
            {
                enterGame = await gateSession.Call(new C2M_AttributeAddPoint() { AttributeType = type }) as M2C_AttributeAddPoint;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return false;
            }

            if (gateSession.Error != ErrorCode.ERR_Success)
            {
                Log.Error("网关服务器连接失败！错误码：" + gateSession.Error);
            }

            return gateSession.Error == ErrorCode.ERR_Success;
        }

        public static void RefreshUI(this ES_AttributeItem self, int type)
        {
            var unit = UnitHelper.GetMyUnitFromCurrentScene(self.ZoneScene().CurrentScene());

            self.EAttributeValueText.text = unit.GetComponent<NumericComponent>().GetByKey(type).ToString();
        }
    }
}