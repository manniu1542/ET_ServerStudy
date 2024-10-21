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

    }
}
