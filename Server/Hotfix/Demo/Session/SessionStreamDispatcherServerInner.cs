using System;
using System.IO;

namespace ET
{
    [SessionStreamDispatcher(SessionStreamDispatcherType.SessionStreamDispatcherServerInner)]
    public class SessionStreamDispatcherServerInner: ISessionStreamDispatcher
    {
        public void Dispatch(Session session, MemoryStream memoryStream)
        {
            ushort opcode = 0;
            try
            {
                long actorId = BitConverter.ToInt64(memoryStream.GetBuffer(), Packet.ActorIdIndex);
                opcode = BitConverter.ToUInt16(memoryStream.GetBuffer(), Packet.OpcodeIndex);
                Type type = null;
                object message = null;
#if SERVER

                // 内网收到外网消息，有可能是gateUnit消息，还有可能是gate广播消息
                if (OpcodeTypeComponent.Instance.IsOutrActorMessage(opcode))
                {
                    InstanceIdStruct instanceIdStruct = new InstanceIdStruct(actorId);
                    instanceIdStruct.Process = Game.Options.Process;
                    long realActorId = instanceIdStruct.ToLong();

                    Entity entity = Game.EventSystem.Get(realActorId);
                    if (entity == null)
                    {
                        type = OpcodeTypeComponent.Instance.GetType(opcode);
                        message = MessageSerializeHelper.DeserializeFrom(opcode, type, memoryStream);
                        Log.Error($"not found actor: {session.DomainScene().Name}  {opcode} {realActorId} {message}");
                        return;
                    }

                    if (entity is Session gateSession)
                    {
                        // 发送给客户端
                        memoryStream.Seek(Packet.OpcodeIndex, SeekOrigin.Begin);
                        gateSession.Send(0, memoryStream);
                        return;
                    }

                    //问题：做个 处理，如果收到遇到顶号 自己的二次登录时候（游戏过程中导致玩家 自己下线 很短时间 ，服务器还没有把用户资料释放的时候。用户又再次登录）。
                    //     玩家的 session被释放掉了 导致 出错  代码执行到=》   if (entity == null) ，  Log.Error($"not found actor:
                    //解决：使用Play这个组件。Play永远不会释放。(C2G_LinkGateLoginHandler) 从Play拿到更早更换的Gate网关连接玩家的Session。来与玩家互通消息，
                    //      否则需要等到 C2G_EnterGameHandler 才会 得到 非空的session的id。

                    if (entity is Player player)
                    {
                        if (player != null && !player.IsDisposed)
                        {
                            Session sessionOther = Game.EventSystem.Get(player.SessionInstanceId) as Session;
                            if (sessionOther != null && !sessionOther.IsDisposed)
                            {
                                
                                //派发 网关消息
                                memoryStream.Seek(Packet.OpcodeIndex, SeekOrigin.Begin);
                                sessionOther.Send(0, memoryStream);
                                return;
                            }
                        }
                    }
                }

#endif
                type = OpcodeTypeComponent.Instance.GetType(opcode);
                message = MessageSerializeHelper.DeserializeFrom(opcode, type, memoryStream);

                if (message is IResponse iResponse && !(message is IActorResponse))
                {
                    session.OnRead(opcode, iResponse);
                    return;
                }

                OpcodeHelper.LogMsg(session.DomainZone(), opcode, message);

                // 收到actor消息,放入actor队列
                switch (message)
                {
                    case IActorRequest iActorRequest:
                    {
                        InstanceIdStruct instanceIdStruct = new InstanceIdStruct(actorId);
                        int fromProcess = instanceIdStruct.Process;
                        instanceIdStruct.Process = Game.Options.Process;
                        long realActorId = instanceIdStruct.ToLong();

                        void Reply(IActorResponse response)
                        {
                            Session replySession = NetInnerComponent.Instance.Get(fromProcess);
                            // 发回真实的actorId 做查问题使用
                            replySession.Send(realActorId, response);
                        }

                        InnerMessageDispatcherHelper.HandleIActorRequest(opcode, realActorId, iActorRequest, Reply);
                        return;
                    }
                    case IActorResponse iActorResponse:
                    {
                        InstanceIdStruct instanceIdStruct = new InstanceIdStruct(actorId);
                        instanceIdStruct.Process = Game.Options.Process;
                        long realActorId = instanceIdStruct.ToLong();
                        InnerMessageDispatcherHelper.HandleIActorResponse(opcode, realActorId, iActorResponse);
                        return;
                    }
                    case IActorMessage iactorMessage:
                    {
                        InstanceIdStruct instanceIdStruct = new InstanceIdStruct(actorId);
                        instanceIdStruct.Process = Game.Options.Process;
                        long realActorId = instanceIdStruct.ToLong();
                        InnerMessageDispatcherHelper.HandleIActorMessage(opcode, realActorId, iactorMessage);
                        return;
                    }
                    default:
                    {
                        MessageDispatcherComponent.Instance.Handle(session, opcode, message);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"InnerMessageDispatcher error: {opcode}\n{e}");
            }
        }
    }
}