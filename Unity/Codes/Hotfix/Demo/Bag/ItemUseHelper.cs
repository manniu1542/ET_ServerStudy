using System;

namespace ET
{
    public static class ItemUseHelper
    {
        public static async ETTask<bool> SellItem(Scene zoneScne, Item bagItem)
        {
            bool isFinish = false;
            try
            {
                C2M_SellItem msg = new C2M_SellItem() { ItemUid = bagItem.Id };

                var gateSession = zoneScne.GetComponent<SessionComponent>().Session;

                M2C_SellItem response = await gateSession.Call(msg) as M2C_SellItem;

                if (response.Error == ErrorCode.ERR_Success)
                {
                    isFinish = true;
                }
                else
                {
                    Log.Error("售卖道具失败：错误码：" + response.Error);
                }
            }
            catch (Exception e)
            {
                Log.Error("售卖道具失败：" + e.ToString());
            }

            return isFinish;
        }

        public static async ETTask<bool> DressUpItem(Scene zoneScne, Item bagItem)
        {
            bool isFinish = false;
            try
            {
                C2M_DressUpItem msg = new C2M_DressUpItem() { ItemBagUid = bagItem.Id };

                var gateSession = zoneScne.GetComponent<SessionComponent>().Session;

                M2C_DressUpItem response = await gateSession.Call(msg) as M2C_DressUpItem;

                if (response.Error == ErrorCode.ERR_Success)
                {
                    isFinish = true;
                }
                else
                {
                    Log.Error("穿戴道具失败：错误码：" + response.Error);
                }
            }
            catch (Exception e)
            {
                Log.Error("穿戴道具失败：" + e.ToString());
            }

            return isFinish;
        }
        
        public static async ETTask<bool> UnloadItem(Scene zoneScne, int roleEquipPos)
        {
            bool isFinish = false;
            try
            {
                C2M_UnloadItem msg = new C2M_UnloadItem() { roleEquipPosition = roleEquipPos };

                var gateSession = zoneScne.GetComponent<SessionComponent>().Session;

                M2C_UnloadItem response = await gateSession.Call(msg) as M2C_UnloadItem;

                if (response.Error == ErrorCode.ERR_Success)
                {
                    isFinish = true;
                }
                else
                {
                    Log.Error("卸下道具失败：错误码：" + response.Error);
                }
            }
            catch (Exception e)
            {
                Log.Error("卸下道具失败：" + e.ToString());
            }

            return isFinish;
        }
    }
}