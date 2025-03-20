using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [Timer(TimerType.SpawnRankUI)]
    public class SpawnRankUITimer: ATimer<DlgRank>
    {
        public override void Run(DlgRank self)
        {
            try
            {
                //请求最新数据
                self.ReqRefreshUI().Coroutine();
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }

    [FriendClass(typeof (DlgRank))]
    [FriendClassAttribute(typeof (ET.RankInfo))]
    public static class DlgRankSystem
    {
        public static void RegisterUIEvent(this DlgRank self)
        {
            self.RegisterCloseEvent<DlgRank>(self.View.E_CloseButton);
            self.View.E_RankLoopVerticalScrollRect.AddItemRefreshListener(self.OnLoopItemRefreshHandler);
        }

        public static void OnHideWindow(this DlgRank self)
        {
            TimerComponent.Instance.Remove(ref self.SpawnRankUITimerID);
            self.RemoveUIScrollItems(ref self.ScrollItems);
        }

        public static void ShowWindow(this DlgRank self, Entity contextData = null)
        {
            //请求 timer
            self.ReqRefreshUI(true).Coroutine();
        }

        public static void OnLoopItemRefreshHandler(this DlgRank self, Transform transform, int index)
        {
            Scroll_Item_rank scrollItem = self.ScrollItems[index].BindTrans(transform);
            var riCpt = self.ZoneScene().GetComponent<RankInfoComponent>();
            RankInfo ri = riCpt.GetCurRankInfoByIdx(index);

            scrollItem.E_NameText.SetText(ri.name);
            scrollItem.E_RankOrderText.SetText((index + 1).ToString());
            scrollItem.E_LevelText.SetText(ri.count.ToString());
        }

        public static async ETTask ReqRefreshUI(this DlgRank self, bool isOpenUI = false)
        {
            bool isFinish = await self.ReqGetRank();
            if (isFinish)
            {
                self.RefreshUI();
                if (isOpenUI)
                {
                    self.SpawnRankUITimerID = TimerComponent.Instance.NewRepeatedTimer(1000, TimerType.SpawnRankUI, self);
                }
            }
        }

        public static async ETTask<bool> ReqGetRank(this DlgRank self)
        {
            Rank2C_GetCurAllRankInfo m2c_ForgeItem;
            var gateSession = self.ZoneScene().GetComponent<SessionComponent>().Session;
            try
            {
                m2c_ForgeItem = await gateSession.Call(new C2Rank_GetCurAllRankInfo()) as Rank2C_GetCurAllRankInfo;
                if (m2c_ForgeItem.Error != ErrorCode.ERR_Success)
                {
                    Log.Error("请求 制作失败 错误码是：" + m2c_ForgeItem.Error);
                    return false;
                }
                else
                {
                    RankInfoComponent riCpt = self.ZoneScene().GetComponent<RankInfoComponent>();
                    riCpt.ResetRankInfo(m2c_ForgeItem.RankInfos);
                    self.RefreshUI();
                }
            }
            catch (Exception e)
            {
                Log.Error("请求出错：" + e.ToString());
                return false;
            }

            return true;
        }

        public static void RefreshUI(this DlgRank self)
        {
            var riCpt = self.ZoneScene().GetComponent<RankInfoComponent>();
            int count = riCpt.GetCurRankCount();
            self.AddUIScrollItems(ref self.ScrollItems, count);
            self.View.E_RankLoopVerticalScrollRect.SetVisible(true, count);
        }
    }
}