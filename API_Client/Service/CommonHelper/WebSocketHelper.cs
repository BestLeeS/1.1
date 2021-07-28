using Fleck;
using Models;
using Models.DemoFunc;
using Models.Sys;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.CommonHelper
{
    public class WebSocketHelper
    {
        public static ConcurrentDictionary<string, IWebSocketConnection> dic_Sockets = new ConcurrentDictionary<string, IWebSocketConnection>();
        public static List<Sys_User> onlineUsers = new List<Sys_User>();
        public static WebSocketServer webSocketServer = null;   
        public static void StartWsServer()
        {
            if (webSocketServer == null)
                webSocketServer = new WebSocketServer($"ws://{SysConfigInfo.WebSocketAddress}");
            webSocketServer.RestartAfterListenError = true;
            webSocketServer.Start(socket =>
            {
                socket.OnOpen = () =>   //连接建立事件
                {
                    string userID = GetWsUserID(socket, "UserID");
                    string userName = GetWsUserID(socket,"UserName");
                    if (!dic_Sockets.ContainsKey(userID))
                    {
                        dic_Sockets.TryAdd(userID.ToUpper(), socket);
                        dic_Sockets.Where(x => x.Key != userID).ToList().ForEach(x=> {
                            IWebSocketConnection tmpTargetSocket = null;
                            dic_Sockets.TryGetValue(x.Key, out tmpTargetSocket);
                            if (tmpTargetSocket != null)
                            {
                                tmpTargetSocket.Send(JsonConvert.SerializeObject(new List<msgContentObj>() { new msgContentObj {
                                        senderID = userID,
                                        senderName = userName,
                                        type = 2
                                    } }));
                            }
                        });
                    }
                    else
                        dic_Sockets.TryUpdate(userID, socket, dic_Sockets[userID]);
                };
                socket.OnClose = () =>  //连接关闭事件
                {
                    string userID = GetWsUserID(socket, "UserID");
                    IWebSocketConnection closeSocket = null;
                    dic_Sockets.TryGetValue(userID, out closeSocket);
                    if (closeSocket != null)
                    {
                        dic_Sockets.Where(x => x.Key != userID).ToList().ForEach(x => {
                            IWebSocketConnection tmpTargetSocket = null;
                            dic_Sockets.TryGetValue(x.Key, out tmpTargetSocket);
                            if (tmpTargetSocket != null)
                            {
                                tmpTargetSocket.Send(JsonConvert.SerializeObject(new List<msgContentObj>() { new msgContentObj {
                                        senderID = userID,
                                        type = 3
                                    } }));
                            }
                        });
                        closeSocket.Close();
                        dic_Sockets.TryRemove(userID, out socket);
                    }
                };
                socket.OnMessage = message =>  //接受客户端网页消息事件
               {
                    WsMsg wsMsg = JsonConvert.DeserializeObject<WsMsg>(message);
                   
                    if (!string.IsNullOrEmpty(wsMsg.receiverID))
                    {
                       wsMsg.receiverID = wsMsg.receiverID.ToUpper();
                       IWebSocketConnection targetSocket = null;
                       dic_Sockets.TryGetValue(wsMsg.receiverID, out targetSocket);
                       if (targetSocket != null)
                        {
                           targetSocket.Send(JsonConvert.SerializeObject(wsMsg.msgContentObjs));
                        }
                    }
                };
            });
        }

        private static string GetWsUserID(IWebSocketConnection webSocketConnection,string paraName)
        {
            return UrlParamsHelper.GetQueryString(webSocketConnection.ConnectionInfo.Origin + webSocketConnection.ConnectionInfo.Path, paraName).ToUpper();
        }
    }
}
