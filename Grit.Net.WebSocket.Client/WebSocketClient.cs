using SuperSocket.ClientEngine;
using System;
using WebSocket4Net;

namespace Grit.Net.WebSocket1.Client
{
    public class WebSocketClient
    {
        /// <summary>
        /// Open
        /// </summary>
        /// <param name="url">URL,例如ws://localhost:8080</param>
        public void Open(string url)
        {
            WebSocket websocket = new WebSocket(url);
            websocket.Opened += Websocket_Opened;
            websocket.MessageReceived += Websocket_MessageReceived;
            websocket.Closed += Websocket_Closed;
            websocket.Error += Websocket_Error;
            websocket.Open();
        }
        private void Websocket_Opened(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Websocket_Closed(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private void Websocket_Error(object sender, ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }


    }
}
