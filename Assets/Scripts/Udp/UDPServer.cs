using UnityEngine;
using Mx.Util;
using System.Net.Sockets;
using System.Net;
using Mx.Log;
using System.Text;
using System.Threading;
using System;
using Mx.Msg;
using System.Collections;

/// <summary>UDP服务器</summary>
public class UDPServer : MonoSingleton<UDPServer>
{
    #region 数据申明

    private byte[] data;
    private Socket socket;
    private int recv;
    private Thread thread;
    private EndPoint endPoint;

    private Queue msgQueue;

    #endregion

    #region Unity函数

    private void Awake()
    {
        msgQueue = new Queue();
        DontDestroyOnLoad(gameObject);
        InitServer();
    }

    private void OnDestroy()
    {
        Clear();
    }

    private void Update()
    {
        if (msgQueue.Count != 0)
        {
            MessageMgr.SendMessageToUIForm(EnumUIFormType.MainUIForm, Define.ON_ADD_DEBUG_DATA, msgQueue.Dequeue());
        }
    }

    #endregion

    #region 公开函数

    public void InitServer()
    {
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint iep = new IPEndPoint(IPAddress.Any, DebugDefine.UTP_PORT);
            socket.Bind(iep);
            endPoint = (EndPoint)iep;

            thread = new Thread(Receive);
            thread.Start();
        }

        catch (Exception e)
        {
            Debug.LogError("开启服务器失败！ error:" + e.Message);
        }
    }

    #endregion


    #region 私有函数

    private void Receive()
    {
        string msg = string.Empty;
        while (true)
        {
            data = new byte[1024*1024];
            recv = socket.ReceiveFrom(data, ref endPoint);
            msg = Encoding.UTF8.GetString(data, 0, recv);

            msgQueue.Enqueue(msg);
        }
    }

    private void Clear()
    {
        if (socket != null)
        {
            socket.Close();
            socket = null;
        }

        thread.Abort();
    }

    #endregion

}


