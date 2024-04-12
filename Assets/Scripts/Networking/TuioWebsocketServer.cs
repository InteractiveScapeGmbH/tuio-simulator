using System;
using System.Linq;
using System.Net;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace TuioSimulator.Networking
{
    public class TuioWebsocketServer : ITuioServer
    {
        private WebSocketServer _server;

        public void Start(IPAddress address, int port)
        {
            _server = new WebSocketServer(address, port);
            _server.AddWebSocketService<TuioService>("/");
            Debug.Log(_server.WebSocketServices.Hosts.Count());
            _server.Start();
        }

        public void Stop()
        {
            _server.Stop();
        }

        public void Send(string data)
        {
            foreach (var host in _server.WebSocketServices.Hosts)
            {
                host.Sessions.Broadcast(data);
            }
        }

        public void Send(byte[] data)
        {
            foreach (var host in _server.WebSocketServices.Hosts)
            {
                host.Sessions.Broadcast(data);
            }
        }
    }

    public class TuioService : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            Debug.Log("[Server] New client connected.");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            Debug.Log($"[Server] New message from client: {e.Data}");
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Debug.Log($"[Server] Client disconnected. {e.Reason}");
        }
    }
}
