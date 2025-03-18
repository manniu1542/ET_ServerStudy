using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[ResponseType(nameof(M2C_TestResponse))]
	[Message(OuterOpcode.C2M_TestRequest)]
	[ProtoContract]
	public partial class C2M_TestRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string request { get; set; }

	}

	[Message(OuterOpcode.M2C_TestResponse)]
	[ProtoContract]
	public partial class M2C_TestResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string response { get; set; }

	}

	[ResponseType(nameof(Actor_TransferResponse))]
	[Message(OuterOpcode.Actor_TransferRequest)]
	[ProtoContract]
	public partial class Actor_TransferRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int MapIndex { get; set; }

	}

	[Message(OuterOpcode.Actor_TransferResponse)]
	[ProtoContract]
	public partial class Actor_TransferResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2C_EnterMap))]
	[Message(OuterOpcode.C2G_EnterMap)]
	[ProtoContract]
	public partial class C2G_EnterMap: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_EnterMap)]
	[ProtoContract]
	public partial class G2C_EnterMap: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

// 自己unitId
		[ProtoMember(4)]
		public long MyId { get; set; }

	}

	[Message(OuterOpcode.MoveInfo)]
	[ProtoContract]
	public partial class MoveInfo: Object
	{
		[ProtoMember(1)]
		public List<float> X = new List<float>();

		[ProtoMember(2)]
		public List<float> Y = new List<float>();

		[ProtoMember(3)]
		public List<float> Z = new List<float>();

		[ProtoMember(4)]
		public float A { get; set; }

		[ProtoMember(5)]
		public float B { get; set; }

		[ProtoMember(6)]
		public float C { get; set; }

		[ProtoMember(7)]
		public float W { get; set; }

		[ProtoMember(8)]
		public int TurnSpeed { get; set; }

	}

	[Message(OuterOpcode.UnitInfo)]
	[ProtoContract]
	public partial class UnitInfo: Object
	{
		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public int ConfigId { get; set; }

		[ProtoMember(3)]
		public int Type { get; set; }

		[ProtoMember(4)]
		public float X { get; set; }

		[ProtoMember(5)]
		public float Y { get; set; }

		[ProtoMember(6)]
		public float Z { get; set; }

		[ProtoMember(7)]
		public float ForwardX { get; set; }

		[ProtoMember(8)]
		public float ForwardY { get; set; }

		[ProtoMember(9)]
		public float ForwardZ { get; set; }

		[ProtoMember(10)]
		public List<int> Ks = new List<int>();

		[ProtoMember(11)]
		public List<long> Vs = new List<long>();

		[ProtoMember(12)]
		public MoveInfo MoveInfo { get; set; }

	}

	[Message(OuterOpcode.M2C_CreateUnits)]
	[ProtoContract]
	public partial class M2C_CreateUnits: Object, IActorMessage
	{
		[ProtoMember(2)]
		public List<UnitInfo> Units = new List<UnitInfo>();

	}

	[Message(OuterOpcode.M2C_CreateMyUnit)]
	[ProtoContract]
	public partial class M2C_CreateMyUnit: Object, IActorMessage
	{
		[ProtoMember(1)]
		public UnitInfo Unit { get; set; }

	}

	[Message(OuterOpcode.M2C_StartSceneChange)]
	[ProtoContract]
	public partial class M2C_StartSceneChange: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long SceneInstanceId { get; set; }

		[ProtoMember(2)]
		public string SceneName { get; set; }

	}

	[Message(OuterOpcode.M2C_RemoveUnits)]
	[ProtoContract]
	public partial class M2C_RemoveUnits: Object, IActorMessage
	{
		[ProtoMember(2)]
		public List<long> Units = new List<long>();

	}

	[Message(OuterOpcode.C2M_PathfindingResult)]
	[ProtoContract]
	public partial class C2M_PathfindingResult: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public float X { get; set; }

		[ProtoMember(2)]
		public float Y { get; set; }

		[ProtoMember(3)]
		public float Z { get; set; }

	}

	[Message(OuterOpcode.C2M_Stop)]
	[ProtoContract]
	public partial class C2M_Stop: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_PathfindingResult)]
	[ProtoContract]
	public partial class M2C_PathfindingResult: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public float X { get; set; }

		[ProtoMember(3)]
		public float Y { get; set; }

		[ProtoMember(4)]
		public float Z { get; set; }

		[ProtoMember(5)]
		public List<float> Xs = new List<float>();

		[ProtoMember(6)]
		public List<float> Ys = new List<float>();

		[ProtoMember(7)]
		public List<float> Zs = new List<float>();

	}

	[Message(OuterOpcode.M2C_Stop)]
	[ProtoContract]
	public partial class M2C_Stop: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public long Id { get; set; }

		[ProtoMember(3)]
		public float X { get; set; }

		[ProtoMember(4)]
		public float Y { get; set; }

		[ProtoMember(5)]
		public float Z { get; set; }

		[ProtoMember(6)]
		public float A { get; set; }

		[ProtoMember(7)]
		public float B { get; set; }

		[ProtoMember(8)]
		public float C { get; set; }

		[ProtoMember(9)]
		public float W { get; set; }

	}

	[ResponseType(nameof(G2C_Ping))]
	[Message(OuterOpcode.C2G_Ping)]
	[ProtoContract]
	public partial class C2G_Ping: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_Ping)]
	[ProtoContract]
	public partial class G2C_Ping: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long Time { get; set; }

	}

	[Message(OuterOpcode.G2C_Test)]
	[ProtoContract]
	public partial class G2C_Test: Object, IMessage
	{
	}

	[ResponseType(nameof(M2C_Reload))]
	[Message(OuterOpcode.C2M_Reload)]
	[ProtoContract]
	public partial class C2M_Reload: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.M2C_Reload)]
	[ProtoContract]
	public partial class M2C_Reload: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(R2C_Login))]
	[Message(OuterOpcode.C2R_Login)]
	[ProtoContract]
	public partial class C2R_Login: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.R2C_Login)]
	[ProtoContract]
	public partial class R2C_Login: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string Address { get; set; }

		[ProtoMember(2)]
		public long Key { get; set; }

		[ProtoMember(3)]
		public long GateId { get; set; }

	}

	[ResponseType(nameof(G2C_LoginGate))]
	[Message(OuterOpcode.C2G_LoginGate)]
	[ProtoContract]
	public partial class C2G_LoginGate: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(2)]
		public long GateId { get; set; }

	}

	[Message(OuterOpcode.G2C_LoginGate)]
	[ProtoContract]
	public partial class G2C_LoginGate: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long PlayerId { get; set; }

	}

	[Message(OuterOpcode.G2C_TestHotfixMessage)]
	[ProtoContract]
	public partial class G2C_TestHotfixMessage: Object, IMessage
	{
		[ProtoMember(1)]
		public string Info { get; set; }

	}

	[ResponseType(nameof(M2C_TestRobotCase))]
	[Message(OuterOpcode.C2M_TestRobotCase)]
	[ProtoContract]
	public partial class C2M_TestRobotCase: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int N { get; set; }

	}

	[Message(OuterOpcode.M2C_TestRobotCase)]
	[ProtoContract]
	public partial class M2C_TestRobotCase: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int N { get; set; }

	}

	[ResponseType(nameof(M2C_TransferMap))]
	[Message(OuterOpcode.C2M_TransferMap)]
	[ProtoContract]
	public partial class C2M_TransferMap: Object, IActorLocationRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_TransferMap)]
	[ProtoContract]
	public partial class M2C_TransferMap: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(A2C_LoginAccount))]
	[Message(OuterOpcode.C2A_LoginAccount)]
	[ProtoContract]
	public partial class C2A_LoginAccount: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.A2C_LoginAccount)]
	[ProtoContract]
	public partial class A2C_LoginAccount: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string Token { get; set; }

		[ProtoMember(2)]
		public long AccountId { get; set; }

	}

	[Message(OuterOpcode.A2C_Disconnect)]
	[ProtoContract]
	public partial class A2C_Disconnect: Object, IMessage
	{
		[ProtoMember(1)]
		public int Error { get; set; }

	}

	[Message(OuterOpcode.G2C_ForcePlayerDisconnect)]
	[ProtoContract]
	public partial class G2C_ForcePlayerDisconnect: Object, IMessage
	{
		[ProtoMember(1)]
		public int Error { get; set; }

	}

	[Message(OuterOpcode.MServerInfo)]
	[ProtoContract]
	public partial class MServerInfo: Object
	{
		[ProtoMember(1)]
		public long ServerId { get; set; }

		[ProtoMember(2)]
		public string Name { get; set; }

		[ProtoMember(3)]
		public int State { get; set; }

	}

	[ResponseType(nameof(A2C_GetServerInfo))]
	[Message(OuterOpcode.C2A_GetServerInfo)]
	[ProtoContract]
	public partial class C2A_GetServerInfo: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long AccountId { get; set; }

		[ProtoMember(2)]
		public string Token { get; set; }

	}

	[Message(OuterOpcode.A2C_GetServerInfo)]
	[ProtoContract]
	public partial class A2C_GetServerInfo: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<MServerInfo> ListServerInfo = new List<MServerInfo>();

	}

	[Message(OuterOpcode.MRoleInfo)]
	[ProtoContract]
	public partial class MRoleInfo: Object
	{
		[ProtoMember(1)]
		public long AccountId { get; set; }

		[ProtoMember(2)]
		public int ServerId { get; set; }

		[ProtoMember(3)]
		public string Name { get; set; }

		[ProtoMember(4)]
		public int State { get; set; }

		[ProtoMember(5)]
		public long LastLoginTime { get; set; }

		[ProtoMember(6)]
		public long CreateRoleTime { get; set; }

		[ProtoMember(7)]
		public long RoleId { get; set; }

	}

	[ResponseType(nameof(A2C_CreateRoleInfo))]
	[Message(OuterOpcode.C2A_CreateRoleInfo)]
	[ProtoContract]
	public partial class C2A_CreateRoleInfo: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long AccountId { get; set; }

		[ProtoMember(2)]
		public string Token { get; set; }

		[ProtoMember(3)]
		public string Name { get; set; }

		[ProtoMember(4)]
		public int ServerId { get; set; }

	}

	[Message(OuterOpcode.A2C_CreateRoleInfo)]
	[ProtoContract]
	public partial class A2C_CreateRoleInfo: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public MRoleInfo RoleInfo { get; set; }

	}

	[ResponseType(nameof(A2C_GetAllRoleInfoInServer))]
	[Message(OuterOpcode.C2A_GetAllRoleInfoInServer)]
	[ProtoContract]
	public partial class C2A_GetAllRoleInfoInServer: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long AccountId { get; set; }

		[ProtoMember(2)]
		public string Token { get; set; }

		[ProtoMember(4)]
		public int ServerId { get; set; }

	}

	[Message(OuterOpcode.A2C_GetAllRoleInfoInServer)]
	[ProtoContract]
	public partial class A2C_GetAllRoleInfoInServer: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<MRoleInfo> RoleInfos = new List<MRoleInfo>();

	}

	[ResponseType(nameof(A2C_DeleRoleInfo))]
	[Message(OuterOpcode.C2A_DeleRoleInfo)]
	[ProtoContract]
	public partial class C2A_DeleRoleInfo: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long AccountId { get; set; }

		[ProtoMember(2)]
		public long RoleId { get; set; }

		[ProtoMember(3)]
		public string Token { get; set; }

		[ProtoMember(4)]
		public string Name { get; set; }

		[ProtoMember(5)]
		public int ServerId { get; set; }

	}

	[Message(OuterOpcode.A2C_DeleRoleInfo)]
	[ProtoContract]
	public partial class A2C_DeleRoleInfo: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(A2C_GetRealmGate))]
	[Message(OuterOpcode.C2A_GetRealmGate)]
	[ProtoContract]
	public partial class C2A_GetRealmGate: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long AccountId { get; set; }

		[ProtoMember(2)]
		public string Token { get; set; }

		[ProtoMember(3)]
		public int ServerId { get; set; }

	}

	[Message(OuterOpcode.A2C_GetRealmGate)]
	[ProtoContract]
	public partial class A2C_GetRealmGate: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string KeyRealmGate { get; set; }

		[ProtoMember(2)]
		public string AdressRealmGate { get; set; }

	}

	[ResponseType(nameof(R2C_GetGate))]
	[Message(OuterOpcode.C2R_GetGate)]
	[ProtoContract]
	public partial class C2R_GetGate: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long AccountId { get; set; }

		[ProtoMember(2)]
		public string Token { get; set; }

	}

	[Message(OuterOpcode.R2C_GetGate)]
	[ProtoContract]
	public partial class R2C_GetGate: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string KeyGate { get; set; }

		[ProtoMember(2)]
		public string AdressGate { get; set; }

	}

	[ResponseType(nameof(G2C_LinkGateLogin))]
	[Message(OuterOpcode.C2G_LinkGateLogin)]
	[ProtoContract]
	public partial class C2G_LinkGateLogin: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long AccountId { get; set; }

		[ProtoMember(2)]
		public string SessionKey { get; set; }

		[ProtoMember(3)]
		public long RoleId { get; set; }

	}

	[Message(OuterOpcode.G2C_LinkGateLogin)]
	[ProtoContract]
	public partial class G2C_LinkGateLogin: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long PlayerId { get; set; }

	}

	[ResponseType(nameof(G2C_EnterGame))]
	[Message(OuterOpcode.C2G_EnterGame)]
	[ProtoContract]
	public partial class C2G_EnterGame: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long AccountId { get; set; }

		[ProtoMember(2)]
		public string SessionKey { get; set; }

	}

	[Message(OuterOpcode.G2C_EnterGame)]
	[ProtoContract]
	public partial class G2C_EnterGame: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long UnitID { get; set; }

	}

//获取numCpt组件的刷新（获取人物最新属性）
	[ResponseType(nameof(M2C_NumericCptGet))]
	[Message(OuterOpcode.C2M_NumericCptGet)]
	[ProtoContract]
	public partial class C2M_NumericCptGet: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_NumericCptGet)]
	[ProtoContract]
	public partial class M2C_NumericCptGet: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//通知客户端 数值改变
	[Message(OuterOpcode.M2C_NumbericChange)]
	[ProtoContract]
	public partial class M2C_NumbericChange: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long UnitID { get; set; }

		[ProtoMember(2)]
		public int NumType { get; set; }

		[ProtoMember(3)]
		public long NumValue { get; set; }

	}

//玩家属性加点
	[ResponseType(nameof(M2C_AttributeAddPoint))]
	[Message(OuterOpcode.C2M_AttributeAddPoint)]
	[ProtoContract]
	public partial class C2M_AttributeAddPoint: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int AttributeType { get; set; }

	}

	[Message(OuterOpcode.M2C_AttributeAddPoint)]
	[ProtoContract]
	public partial class M2C_AttributeAddPoint: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//玩家前往冒险
	[ResponseType(nameof(M2C_GoToAdventure))]
	[Message(OuterOpcode.C2M_GoToAdventure)]
	[ProtoContract]
	public partial class C2M_GoToAdventure: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int BattleLevelConfigID { get; set; }

	}

	[Message(OuterOpcode.M2C_GoToAdventure)]
	[ProtoContract]
	public partial class M2C_GoToAdventure: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//冒险结束的反馈
	[ResponseType(nameof(M2C_AdventureEnd))]
	[Message(OuterOpcode.C2M_AdventureEnd)]
	[ProtoContract]
	public partial class C2M_AdventureEnd: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int RoundCount { get; set; }

		[ProtoMember(2)]
		public int AdventureBattleRoundState { get; set; }

	}

	[Message(OuterOpcode.M2C_AdventureEnd)]
	[ProtoContract]
	public partial class M2C_AdventureEnd: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//升级
	[ResponseType(nameof(M2C_NumericUpLevel))]
	[Message(OuterOpcode.C2M_NumericUpLevel)]
	[ProtoContract]
	public partial class C2M_NumericUpLevel: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_NumericUpLevel)]
	[ProtoContract]
	public partial class M2C_NumericUpLevel: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.ItemInfo)]
	[ProtoContract]
	public partial class ItemInfo: Object
	{
		[ProtoMember(1)]
		public long Uid { get; set; }

		[ProtoMember(2)]
		public int ConfigID { get; set; }

		[ProtoMember(3)]
		public int Quality { get; set; }

		[ProtoMember(4)]
		public EquipInfo EquipInfo { get; set; }

	}

//装备词条
	[Message(OuterOpcode.EquipmentAffixesInfo)]
	[ProtoContract]
	public partial class EquipmentAffixesInfo: Object
	{
		[ProtoMember(1)]
		public int NumType { get; set; }

		[ProtoMember(2)]
		public long NumValue { get; set; }

		[ProtoMember(3)]
		public int EpAffType { get; set; }

	}

//装备信息
	[Message(OuterOpcode.EquipInfo)]
	[ProtoContract]
	public partial class EquipInfo: Object
	{
		[ProtoMember(1)]
		public bool IsCreateAffixes { get; set; }

		[ProtoMember(2)]
		public int Score { get; set; }

		[ProtoMember(3)]
		public List<EquipmentAffixesInfo> EquipmentAffixesInfos = new List<EquipmentAffixesInfo>();

	}

//通知客户端 道具刷新（可能是背包道具，也可能是人的装备道具）
	[Message(OuterOpcode.M2C_UpdateSomeOneItem)]
	[ProtoContract]
	public partial class M2C_UpdateSomeOneItem: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

//道具操作类型 0 是添加，1是移除，
		[ProtoMember(1)]
		public int NetItemOp { get; set; }

		[ProtoMember(2)]
		public ItemInfo ItemInfo { get; set; }

		[ProtoMember(3)]
		public int NetItemPut { get; set; }

	}

//所有道具放置(背包道具/人的装备道具)
	[Message(OuterOpcode.M2C_UpdatePutAllItem)]
	[ProtoContract]
	public partial class M2C_UpdatePutAllItem: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int NetItemPut { get; set; }

		[ProtoMember(2)]
		public List<ItemInfo> ItemInfo = new List<ItemInfo>();

	}

//所有道具放置(背包道具/人的装备道具)
	[Message(OuterOpcode.M2C_UpdateAllForgeProduction)]
	[ProtoContract]
	public partial class M2C_UpdateAllForgeProduction: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public List<ForgeProductionInfo> ProductionInfos = new List<ForgeProductionInfo>();

	}

	[Message(OuterOpcode.GameTaskInfo)]
	[ProtoContract]
	public partial class GameTaskInfo: Object
	{
		[ProtoMember(1)]
		public int TaskConfigID { get; set; }

		[ProtoMember(2)]
		public int TaskState { get; set; }

		[ProtoMember(3)]
		public int TaskProgress { get; set; }

	}

//所有任务推送
	[Message(OuterOpcode.M2C_UpdateAllTask)]
	[ProtoContract]
	public partial class M2C_UpdateAllTask: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public List<GameTaskInfo> GameTaskInfos = new List<GameTaskInfo>();

	}

//售卖道具
	[ResponseType(nameof(M2C_SellItem))]
	[Message(OuterOpcode.C2M_SellItem)]
	[ProtoContract]
	public partial class C2M_SellItem: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long ItemUid { get; set; }

	}

	[Message(OuterOpcode.M2C_SellItem)]
	[ProtoContract]
	public partial class M2C_SellItem: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//穿戴道具
	[ResponseType(nameof(M2C_DressUpItem))]
	[Message(OuterOpcode.C2M_DressUpItem)]
	[ProtoContract]
	public partial class C2M_DressUpItem: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long ItemBagUid { get; set; }

	}

	[Message(OuterOpcode.M2C_DressUpItem)]
	[ProtoContract]
	public partial class M2C_DressUpItem: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//卸下道具
	[ResponseType(nameof(M2C_UnloadItem))]
	[Message(OuterOpcode.C2M_UnloadItem)]
	[ProtoContract]
	public partial class C2M_UnloadItem: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int roleEquipPosition { get; set; }

	}

	[Message(OuterOpcode.M2C_UnloadItem)]
	[ProtoContract]
	public partial class M2C_UnloadItem: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//打造产品的信息
	[Message(OuterOpcode.ForgeProductionInfo)]
	[ProtoContract]
	public partial class ForgeProductionInfo: Object
	{
		[ProtoMember(1)]
		public long StartForgeTime { get; set; }

		[ProtoMember(2)]
		public long EndForgeTime { get; set; }

		[ProtoMember(3)]
		public int ForgeProductionConfigID { get; set; }

		[ProtoMember(4)]
		public int ProductionReceiveState { get; set; }

		[ProtoMember(5)]
		public long ProdictionId { get; set; }

	}

//发送打造item（装备等道具）请求
	[ResponseType(nameof(M2C_ForgeItem))]
	[Message(OuterOpcode.C2M_ForgeItem)]
	[ProtoContract]
	public partial class C2M_ForgeItem: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int ForgeProductionConfigId { get; set; }

	}

	[Message(OuterOpcode.M2C_ForgeItem)]
	[ProtoContract]
	public partial class M2C_ForgeItem: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public ForgeProductionInfo ForgeProInfo { get; set; }

	}

//发送领取打造item（装备等道具）请求
	[ResponseType(nameof(M2C_ReceiveProductionItem))]
	[Message(OuterOpcode.C2M_ReceiveProductionItem)]
	[ProtoContract]
	public partial class C2M_ReceiveProductionItem: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long ForgeProductionId { get; set; }

	}

	[Message(OuterOpcode.M2C_ReceiveProductionItem)]
	[ProtoContract]
	public partial class M2C_ReceiveProductionItem: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//推送任务进度的更新
	[Message(OuterOpcode.M2C_UpdateGameTaskInfo)]
	[ProtoContract]
	public partial class M2C_UpdateGameTaskInfo: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public GameTaskInfo GameTaskInfo { get; set; }

	}

//领取任务奖励的请求
	[ResponseType(nameof(M2C_ReceiveGameTaskReward))]
	[Message(OuterOpcode.C2M_ReceiveGameTaskReward)]
	[ProtoContract]
	public partial class C2M_ReceiveGameTaskReward: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int TaskConfigId { get; set; }

	}

	[Message(OuterOpcode.M2C_ReceiveGameTaskReward)]
	[ProtoContract]
	public partial class M2C_ReceiveGameTaskReward: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//排行榜信息
	[Message(OuterOpcode.MRankInfo)]
	[ProtoContract]
	public partial class MRankInfo: Object
	{
		[ProtoMember(1)]
		public long UnitID { get; set; }

		[ProtoMember(2)]
		public long Count { get; set; }

		[ProtoMember(3)]
		public string Name { get; set; }

	}

//获取排行榜信息
	[ResponseType(nameof(Rank2C_GetCurAllRankInfo))]
	[Message(OuterOpcode.C2Rank_GetCurAllRankInfo)]
	[ProtoContract]
	public partial class C2Rank_GetCurAllRankInfo: Object, IActorRankRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.Rank2C_GetCurAllRankInfo)]
	[ProtoContract]
	public partial class Rank2C_GetCurAllRankInfo: Object, IActorRankResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<MRankInfo> RankInfos = new List<MRankInfo>();

	}

}
