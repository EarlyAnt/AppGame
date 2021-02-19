using strange.extensions.signal.impl;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace AppGame.Util
{
    public class TCPConnect
    {
        TcpClient client;

        IPEndPoint ipEndPoint;

        NetworkStream stream;

        bool sendAllSuccess = true;

        public Signal<string> pairTCPSignal;

        List<string> messageList = new List<string>();

        public string SendMessageSuccess = "OK";

        public string SendMessageFalied = "ERROR";

        public bool SendMessageSync(string wifiName, string wifiPassword, string childID)
        {
            client = new TcpClient("192.168.21.1", 8080);

            messageList.Add("ssid:" + wifiName);
            messageList.Add("pwd:" + wifiPassword);
            messageList.Add("child_sn:" + childID);//bind cup .need..
            messageList.Add("tosta:Connect Cup");

            foreach (string message in messageList)
            {
                string allStr = "";

                allStr = message + "\r\n";

                SendMessage(allStr);

                Debug.Log(allStr);
            }

            closeTcp();

            return sendAllSuccess;

        }

        private void SendMessage(string message)
        {
            try
            {
                stream = client.GetStream();
                byte[] messages = Encoding.Default.GetBytes(message);
                stream.Write(messages, 0, messages.Length);

                byte[] bytes = new Byte[1024];
                string data = string.Empty;
                int length = stream.Read(bytes, 0, bytes.Length);
                if (length > 0)
                {
                    data = Encoding.Default.GetString(bytes, 0, length);
                    Debug.Log(data.ToString());
                }
               
            }
            catch (Exception ex)
            {
                sendAllSuccess = false;
                Debug.Log(ex.ToString());
                closeTcp();
            }
        }

        private void closeTcp()
        {
            client.Close();
            stream.Flush();
            stream.Close();
        }

        public void SendMessageAsync(string wifiName, string wifiPassword, string childID){

            messageList.Add("ssid:" + wifiName);
            messageList.Add("pwd:" + wifiPassword);
            messageList.Add("child_sn:" + childID);
            messageList.Add("tosta:Connect Cup");

            Loom.RunAsync(()=>{  
                ipEndPoint = new IPEndPoint(IPAddress.Parse("192.168.21.1"), 8080);
                client = new TcpClient();
                try
                {
                    client.Connect(ipEndPoint);
                }
                catch(Exception e)
                {
                    Debug.Log(e.ToString());
                    Loom.QueueOnMainThread(()=>{  
                        if (pairTCPSignal != null)
                        {
                            pairTCPSignal.Dispatch(SendMessageFalied);
                        }
                    });  

                }
                if (client.Connected)
                {

                    foreach (string message in messageList)
                    {
                        string allStr = "";

                        allStr = message + "\r\n";

                        Debug.Log(allStr);

                        stream = client.GetStream();

                        byte[] messages = Encoding.Default.GetBytes(allStr);
                        stream.Write(messages, 0, messages.Length);

                        byte[] bytes = new Byte[1024];
                        string data = string.Empty;
                        int length = stream.Read(bytes, 0, bytes.Length);
                        if (length > 0)
                        {
                            data = Encoding.Default.GetString(bytes, 0, length);
                            Loom.QueueOnMainThread(() =>
                            {
                                Debug.Log(data.ToString());
                            });
                        }
                    }

                    closeTcp();

                    Loom.QueueOnMainThread(() =>
                    {
                        if (pairTCPSignal != null)
                        {
                            pairTCPSignal.Dispatch(SendMessageSuccess);
                        }

                    });
                }
                else
                {
                    Loom.QueueOnMainThread(()=>{  
                        if (pairTCPSignal != null)
                        {
                            pairTCPSignal.Dispatch(SendMessageFalied);
                        }
               
                    }); 
                }
            }); 
        }
    }
}