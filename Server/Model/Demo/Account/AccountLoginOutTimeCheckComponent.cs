using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    //登录超时检测（避免玩家在登陆的时候，不进入游戏，且异常退出游戏 例如自动关机，杀死应用后台等，没有跟服务器推送下线的RPC消息，
    //造成服务器存储 没有用的Session，也移除不了，造成资源浪费）
    [ComponentOf(typeof(Session))]
    public class AccountLoginOutTimeCheckComponent : Entity, IAwake<long>, IDestroy
    {
        public long accountId;
        public long timerId;
        public long autoCheckOutTime = 10 * 60 * 1000;//10分钟  10 * 60 * 1000;
    }
}
