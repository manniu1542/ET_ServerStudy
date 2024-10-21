using OfficeOpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{

    [ObjectSystem]
    public class TokenComponentAwakeSystem : AwakeSystem<TokenComponent>
    {
        public override void Awake(TokenComponent self)
        {
            self.dicToken = new Dictionary<long, string>();

        }
    }
    [FriendClass(typeof(TokenComponent))]
    public static class TokenComponentSystem
    {

        public static void Add(this TokenComponent self, long id, string token)
        {
            if (!self.dicToken.ContainsKey(id))
            {

                self.dicToken.Add(id, token);

                self.OutTimeRemoveToken(id, token).Coroutine();

            }



        }
        public static string Get(this TokenComponent self, long id)
        {
            self.dicToken.TryGetValue(id, out string value);
            return value;
        }
        public static void Remove(this TokenComponent self, long id)
        {
            self.dicToken.Remove(id);
        }

        //令牌超时处理
        public static async ETTask OutTimeRemoveToken(this TokenComponent self, long id, string token)
        {
            //10分钟没有 操作就下线处理
            int minite = 10;
            long time = minite * 60 * 1000;
            await TimerComponent.Instance.WaitAsync(time);
            string onlineToken = self.Get(id);
            if (!string.IsNullOrEmpty(token) && onlineToken == token)
            {
                self.Remove(id);
            }


        }

    }
}
