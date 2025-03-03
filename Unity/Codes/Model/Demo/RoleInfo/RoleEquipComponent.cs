using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public enum RoleEuipPosType
    {
        None = 0, //不可装备
        Head = 1, //头盔
        Clothes = 2, //衣服
        Shoes = 3, //鞋子
        Ring = 4, //戒指
        Weapon = 5, //武器
        Shield = 6, //盾牌
        Count
    }

    public enum RoleItemOp
    {
        DressUp = 1,
        Unload = 2,
    }

    /// <summary>
    /// 账号角色信息组件
    /// </summary>
    /// 
    [ComponentOf(typeof (Unit))]
    [ChildType(typeof (Item))]
#if SERVER
    public class RoleEquipComponent: Entity, IAwake, IDestroy, ITransfer, IUnitChache, IDeserialize
#else
    public class RoleEquipComponent: Entity, IAwake, IDestroy
#endif

    {
#if SERVER
        [BsonIgnore]
#endif
        public Dictionary<int, Item> dicEquips;

#if SERVER
        [BsonIgnore]
#endif
        public M2C_UpdateSomeOneItem m2c_roleEqpItem = new M2C_UpdateSomeOneItem() { NetItemPut = (int)NetItemPut.Role };
    }
}