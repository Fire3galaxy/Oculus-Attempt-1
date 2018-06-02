// This is the client DLL class code to use for the sockServer
// include this DLL in your Plugins folder under Assets
// using it is very simple
// Look at LinkSyncSCR.cs


using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Collections;

namespace SharpConnect {
    public class Connector {
        const int READ_BUFFER_SIZE = 921601; // Size of a 640x480 RGB image + 1
        const int PORT_NUM = 10000;

        private TcpClient client;
        private byte[] readBuffer = new byte[READ_BUFFER_SIZE];
        public string strMessage = string.Empty;
        public string res = String.Empty;
        public bool isConnected = false;
        public byte[] messageBuffer = new byte[READ_BUFFER_SIZE];

        public Connector() { }

        public string fnConnectResult(string sNetIP, int iPORT_NUM) {
            try {
                // The TcpClient is a subclass of Socket, providing higher level 
                // functionality like streaming.
                client = new TcpClient(sNetIP, PORT_NUM);
                // Start an asynchronous read invoking DoRead to avoid lagging the user
                // interface.
                client.GetStream().BeginRead(readBuffer, 0, READ_BUFFER_SIZE, new AsyncCallback(DoRead), null);
                isConnected = true;

                return "Connection Succeeded";
            } catch (Exception ex) {
                isConnected = false;
                return "Server is not active.  Please start server and try again.      " + ex.ToString();
            }
        }

        public void fnPacketTest(string sInfo) {
            SendData(sInfo);
        }

        public void fnDisconnect() {
            SendData("DISCONNECT");
            isConnected = false;
        }

        //public void fnListUsers() {
        //    SendData("REQUESTUSERS");
        //}

        // Function that actually constantly receives stream from server in background
        private void DoRead(IAsyncResult ar) {
            int BytesRead;
            try {
                // Finish asynchronous read into readBuffer and return number of bytes read.
                BytesRead = client.GetStream().EndRead(ar);
                if (BytesRead < 1) {
                    // if no bytes were read server has close.  
                    res = "Disconnected";
                    return;
                }
                messageBuffer = readBuffer;
                // Convert the byte array the message was saved into
                strMessage = Encoding.ASCII.GetString(readBuffer, 0, BytesRead);
                ProcessCommands(strMessage);

                // Start a new asynchronous read into readBuffer.
                client.GetStream().BeginRead(readBuffer, 0, READ_BUFFER_SIZE, new AsyncCallback(DoRead), null);
            } catch {
                res = "Disconnected";
            }
        }
        
        private void ProcessCommands(string strMessage) {
        }

        //// Process the command received from the server, and take appropriate action.
        //private void ProcessCommands(string strMessage) {
        //    string[] dataArray;

        //    // Message parts are divided by "|"  Break the string into an array accordingly.
        //    dataArray = strMessage.Split((char)124);
        //    // dataArray(0) is the command.
        //    switch (dataArray[0]) {
        //        case "JOIN":
        //            // Server acknowledged login.
        //            res = "You have joined the chat";
        //            break;
        //        case "CHAT":
        //            // Received chat message, display it.
        //            res = dataArray[1].ToString();
        //            break;
        //        case "REFUSE":
        //            // Server refused login with this user name, try to log in with another.
        //            AttemptLogin(pUserName);
        //            res = "Attempted Re-Login";
        //            break;
        //        case "LISTUSERS":
        //            // Server sent a list of users.
        //            ListUsers(dataArray);
        //            break;
        //        case "BROAD":
        //            // Server sent a broadcast message
        //            res = "ServerMessage: " + dataArray[1].ToString();
        //            break;
        //    }
        //}

        // Use a StreamWriter to send a message to server.
        private void SendData(string data) {
            StreamWriter writer = new StreamWriter(client.GetStream());
            writer.Write(data + (char)10);
            writer.Flush();
        }
    }
}