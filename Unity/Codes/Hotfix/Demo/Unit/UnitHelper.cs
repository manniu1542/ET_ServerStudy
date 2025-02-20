namespace ET
{
    public static class UnitHelper
    {
        public static Unit GetMyUnitFromZoneScene(Scene zoneScene)
        {
            PlayerComponent playerComponent = zoneScene.GetComponent<PlayerComponent>();
            Scene currentScene = zoneScene.GetComponent<CurrentScenesComponent>().Scene;
            return currentScene.GetComponent<UnitComponent>().Get(playerComponent.MyId);
        }

        public static Unit GetMyUnitFromCurrentScene(Scene currentScene)
        {
            PlayerComponent playerComponent = currentScene.Parent.Parent.GetComponent<PlayerComponent>();
            return currentScene.GetComponent<UnitComponent>().Get(playerComponent.MyId);
        }

        public static NumericComponent GetMyUnitNumericComponent(Scene currentScene)
        {
            var unit = GetMyUnitFromCurrentScene(currentScene);
            return unit?.GetComponent<NumericComponent>();
        }

        /// <summary>
        /// 是否活着
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool IsAlive(this Unit self)
        {
            if (self == null || self.IsDisposed) return false;

            var numCpt = self.GetComponent<NumericComponent>();
            if (numCpt == null) return false;
            
            return numCpt[NumericType.IsAlive] == 0;
        }

        /// <summary>
        /// 设置是否活着
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static void SetAlive(this Unit self, bool isAlive)
        {
            if (self == null || self.IsDisposed) return;

            var numCpt = self.GetComponent<NumericComponent>();
            if (numCpt == null) return;

            numCpt[NumericType.IsAlive] = isAlive? 0 : 1;
        }
    }
}