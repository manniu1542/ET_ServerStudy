namespace ET
{
    [FriendClass(typeof(GateSessionKeyComponent))]
    public static class GateSessionKeyComponentSystem
    {
        public static void Add(this GateSessionKeyComponent self, long key, string sessionKey)
        {
            self.sessionKey.Add(key, sessionKey);
            self.TimeoutRemoveKey(key).Coroutine();
        }

        public static string Get(this GateSessionKeyComponent self, long key)
        {
            string sessionKey = null;
            self.sessionKey.TryGetValue(key, out sessionKey);
            return sessionKey;
        }

        public static void Remove(this GateSessionKeyComponent self, long key)
        {
            self.sessionKey.Remove(key);
        }

        private static async ETTask TimeoutRemoveKey(this GateSessionKeyComponent self, long key)
        {
            await TimerComponent.Instance.WaitAsync(1000 * 20);//20秒
            self.Remove(key);
        }
    }
}