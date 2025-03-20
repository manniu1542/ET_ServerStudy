using ET;
using ProtoBuf;
using System.Collections.Generic;

namespace ET
{
    [Message(MongoOpcode.ObjectQueryResponse)]
    [ProtoContract]
    public partial class ObjectQueryResponse: Object, IActorResponse
    {
        [ProtoMember(90)]
        public int RpcId { get; set; }

        [ProtoMember(91)]
        public int Error { get; set; }

        [ProtoMember(92)]
        public string Message { get; set; }

        [ProtoMember(1)]
        public Entity entity { get; set; }
    }

    [ResponseType(nameof (M2M_UnitTransferResponse))]
    [Message(MongoOpcode.M2M_UnitTransferRequest)]
    [ProtoContract]
    public partial class M2M_UnitTransferRequest: Object, IActorRequest
    {
        [ProtoMember(1)]
        public int RpcId { get; set; }

        [ProtoMember(2)]
        public Unit Unit { get; set; }

        [ProtoMember(3)]
        public List<Entity> Entitys = new List<Entity>();
    }

    //获取unit的数据从数据库
    [Message(MongoOpcode.U2G_GetUnitChache)]
    [ProtoContract]
    public partial class U2G_GetUnitChache: Object, IActorResponse
    {
        [ProtoMember(90)]
        public int RpcId { get; set; }

        [ProtoMember(91)]
        public int Error { get; set; }

        [ProtoMember(92)]
        public string Message { get; set; }

        [ProtoMember(2)]
        public List<Entity> EntityType = new List<Entity>();

        [ProtoMember(3)]
        public List<string> listComponentName = new List<string>();
    }

    //从map服务器出发,更新玩家排行榜数据，到数据库
    [Message(MongoOpcode.M2Rank_UpdateRankInfo)]
    [ProtoContract]
    public partial class M2Rank_UpdateRankInfo: Object, IActorRankMessage
    {
        [ProtoMember(90)]
        public int RpcId { get; set; }

        [ProtoMember(1)]
        public MRankInfo RankInfo { get; set; }
    }
}