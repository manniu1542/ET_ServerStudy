namespace ET
{

    [ComponentOf(typeof (Item))]
#if SERVER
    public class EquipInfoComponent: Entity, IAwake, IDestroy, ISerializeToEntity
#else
    public class EquipInfoComponent: Entity, IAwake, IDestroy
#endif

    {

        public int sign;


    }
}