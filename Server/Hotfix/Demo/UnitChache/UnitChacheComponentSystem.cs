namespace ET
{
    public class UnitChacheComponentAwakeSystem: AwakeSystem<UnitChacheComponent>
    {
        public override void Awake(UnitChacheComponent self)
        {
        }
    }

    public class UnitChacheComponentDestroySystem: DestroySystem<UnitChacheComponent>
    {
        public override void Destroy(UnitChacheComponent self)
        {
        }
    }

    public static class UnitChacheComponentSystem
    {
    }
}