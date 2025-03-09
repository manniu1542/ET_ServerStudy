using System;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace ET
{
    [ObjectSystem]
    public class ES_MakeQueueAwakeSystem: AwakeSystem<ES_MakeQueue, Transform>
    {
        public override void Awake(ES_MakeQueue self, Transform transform)
        {
            self.uiTransform = transform;
        }
    }

    [ObjectSystem]
    public class ES_MakeQueueDestroySystem: DestroySystem<ES_MakeQueue>
    {
        public override void Destroy(ES_MakeQueue self)
        {
            self.DestroyWidget();
        }
    }

    [FriendClassAttribute(typeof (ET.Production))]
    public static partial class ES_MakeQueueSystem
    {
        public static void RefreshUI(this ES_MakeQueue self, Production data, SpriteAtlas saIcon)
        {
            self.E_ReceiveButton.AddListenerAsync(() => { return self.ReqReceiveProduction(data.Id); });

            self.ES_EquipItem.RefreshUI(data.Config.ItemConfigId, saIcon);
            self.IntervalRefreshUI(data);
        }

        public static void IntervalRefreshUI(this ES_MakeQueue self, Production data)
        {
            long now = TimeHelper.ServerNow();

            long allMakeTime = data.endTime - data.startTime;
            long nowReminTime =  data.endTime - now;

            bool isMaking = nowReminTime > 0;
       
           

            self.E_ReceiveButton.SetVisible(!isMaking);
            self.E_MakeOverTipText.SetVisible(!isMaking);
            self.E_MakeTimeText.SetVisible(isMaking);
            self.E_MakeTipText.SetVisible(isMaking);

            if (isMaking)
            {
                long minute = nowReminTime / 60 / 1000;
                long second = nowReminTime % (60 * 1000) / 1000;
                second += 1;
                self.E_MakeTimeText.SetText($"{minute}分{second}秒");
                self.E_LeaftTimeSlider.value = (float)nowReminTime / allMakeTime;
            }
            else
            {
                self.E_LeaftTimeSlider.value = 0;
            }
        }

        public static async ETTask ReqReceiveProduction(this ES_MakeQueue self, long productionId)
        {
            //客户端先判断 有没有 位置了(当前 正在打造的 数量，当前等级可以打造的 数量)
            var bagCpt = self.ZoneScene().GetComponent<BagComponent>();
            if (bagCpt.IsMaxCapacity()) return;

            //做请求
            M2C_ReceiveProductionItem m2c_ForgeItem;
            var gateSession = self.ZoneScene().GetComponent<SessionComponent>().Session;
            try
            {
                m2c_ForgeItem =
                        await gateSession.Call(new C2M_ReceiveProductionItem() { ForgeProductionId = productionId }) as M2C_ReceiveProductionItem;
                if (m2c_ForgeItem.Error != ErrorCode.ERR_Success)
                {
                    Log.Error("请求 制作失败 错误码是：" + m2c_ForgeItem.Error);
                    return;
                }
                else
                {
                    self.ZoneScene().GetComponent<ForgeComponent>().RemoveProduction(productionId);

                    self.ZoneScene().GetComponent<UIComponent>().GetDlgLogic<DlgForge>()?.RefreshUI();
                }
            }
            catch (Exception e)
            {
                Log.Error("请求出错：" + e.ToString());
            }
        }
    }
}