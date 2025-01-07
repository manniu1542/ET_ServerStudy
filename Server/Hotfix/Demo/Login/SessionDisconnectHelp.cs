using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public static class SessionDisconnectHelp
    {
        /// <summary>
        /// 为什么需要等1秒在断开呢，是因为需要给客户端发送一条 ，服务器为什么断开的原因的消息
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static async ETTask Disconnect(this Session session)
        {
            if (session == null || session.IsDisposed)
            {
                return;
            }

            long id = session.InstanceId;
            await TimerComponent.Instance.WaitAsync(1000);

            if (id != session.InstanceId)
            {
                //被别的 session所占用了
            }
            else
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// 让该玩家离开服务器
        /// </summary>
        public static async ETTask LetPlayLeave(Player player, bool isExcept = false)
        {
            if (player == null || player.IsDisposed) return;

            long id = player.InstanceId;
            //防止多个 客户端，同一时刻 请求
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginGate, player.Account.GetHashCode()))
            {
                //调用很多次，进来的账号已经不是同一个 直接return出去
                if (id == 0 || id != player.InstanceId)
                {
                    return;
                }

                if (!isExcept)
                {
                    switch (player.State)
                    {
                        case PlayerState.Disconect:
                            Log.Error("玩家已经下线了，无需再次下线！" + player);

                            return;
                        case PlayerState.Gate:

                            break;
                        case PlayerState.Game:
                            //在游戏中  给map发个消息 移除玩家的unit，保存玩家数据， locatin定位服务器也移除 玩家记录，给定位中心服务器也移除玩家
                            // 请求在map服务器的账号是否有该玩家 找到 Map服务器得到Unit 推送下线通知。
                            var removeUnit = await MessageHelper.CallLocationActor(player.UintId, new G2M_RemoveUnit()) as M2G_RemoveUnit;
                            if (removeUnit.Error == ErrorCode.ERR_Success) return;
                            else
                            {
                                Log.Error("移除失败！" + removeUnit.Error);
                            }

                            int zone = 1;
                            long loginCenterId = StartSceneConfigCategory.Instance.LoginCenters[zone].InstanceId;
                            L2G_RemovePlayer lginfo = await MessageHelper.CallActor(loginCenterId,
                                new G2L_RemovePlayer() { Account = player.Account, ServerId = player.DomainZone() }) as L2G_RemovePlayer;
                            if (lginfo.Error == ErrorCode.ERR_Success)
                            {
                                Log.Error("移除成功！");
                            }
                            else
                            {
                                Log.Error("移除失败！" + lginfo.Error);
                            }

                            break;
                        default:
                            break;
                    }
                }

                player.State = PlayerState.Disconect;
                player.DomainScene().GetComponent<PlayerComponent>()?.Remove(player.Account);

                player.Dispose();
                //一些session 上的异步移除组件 给的等待时间
                await TimerComponent.Instance.WaitAsync(300);
            }
        }
    }
}