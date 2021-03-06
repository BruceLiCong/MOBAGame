﻿using System;
using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.OpCode;
using ExitGames.Client.Photon;
using UnityEngine;

public class PhotonEngine : MonoBehaviour, IPhotonPeerListener
{
    /// <summary>
    /// 客户端连接
    /// </summary>
    private static PhotonPeer m_Peer;

    public static PhotonPeer Peer
    {
        get { return m_Peer; }
    }
    /// <summary>
    /// IP地址 
    /// </summary>
    private string m_ServerAddress = "127.0.0.1:5055";
    /// <summary>
    /// 服务器端应用名称 
    /// </summary>
    private string m_ApplicationName = "MOBA";
    /// <summary>
    /// 使用协议 
    /// </summary>
    private ConnectionProtocol m_Protocol = ConnectionProtocol.Udp;

    /// <summary>
    /// 是否连接
    /// </summary>
    public bool IsConnect = false;

    public static PhotonEngine Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

	void Start () {
		m_Peer = new PhotonPeer(this, m_Protocol);
	    m_Peer.Connect(m_ServerAddress, m_ApplicationName);
	}
	
	void Update () {
	    if (!IsConnect)
	    {
            // 主要是防止中途断开连接时 立即重连
            // TODO 这个地方重连 但没有重新登陆 无法识别客户端
            m_Peer.Connect(m_ServerAddress, m_ApplicationName);
	    }
        m_Peer.Service();
	}

    void OnDestroy()
    {
        m_Peer.Disconnect();
    }

    #region Photon相关接口

    public void DebugReturn(DebugLevel level, string message)
    {
        if (level == DebugLevel.ERROR)
        {
            Log.Error(message);
        }
        else if (level == DebugLevel.WARNING)
        {
            Log.Warning(message);
        }
        else
        {
            Log.Debug(message);
        }
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        BaseRequest request = m_RequestDict.ExTryGet((OperationCode) operationResponse.OperationCode);
        if (request)
        {
            request.OnOperationResponse(operationResponse);
        }
        else
        {
            // 这里有可能是正常的 比如在战斗中的玩家会找不到好友下线的处理
            Log.Warning("找不到响应的对相应处理对象 ："
                + Enum.GetName(typeof(OperationCode), operationResponse.OperationCode));
        }
    }

    public void OnEvent(EventData eventData)
    {
        BaseEvent _event = m_EventDict.ExTryGet((EventCode)eventData.Code);
        if (_event)
        {
            _event.OnEvent(eventData);
        }
        else
        {
            Log.Warning("找不到广播的对相应处理对象 ："
                + Enum.GetName(typeof(OperationCode), eventData.Code));
        }

    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        Log.Debug("当前连接状态： " + statusCode);
        if (statusCode == StatusCode.Connect)
        {
            IsConnect = true;
        }
        else if (statusCode == StatusCode.Disconnect)
        {
            IsConnect = false;
        }
    }

    #endregion


    #region 管理请求，事件

    private Dictionary<OperationCode, BaseRequest> m_RequestDict = new Dictionary<OperationCode, BaseRequest>();
    private Dictionary<EventCode, BaseEvent> m_EventDict = new Dictionary<EventCode, BaseEvent>();

    public void AddRequest(BaseRequest request)
    {
        if (!m_RequestDict.ContainsKey(request.OpCode))
            m_RequestDict.Add(request.OpCode, request);
    }

    public void RemoveRequest(BaseRequest request)
    {
        m_RequestDict.Remove(request.OpCode);
    }

    public void AddEvent(BaseEvent baseEvent)
    {
        if (!m_EventDict.ContainsKey(baseEvent.Code))
            m_EventDict.Add(baseEvent.Code, baseEvent);
    }

    public void RemoveEvent(BaseEvent baseEvent)
    {
        m_EventDict.Remove(baseEvent.Code);
    }

    #endregion
}
