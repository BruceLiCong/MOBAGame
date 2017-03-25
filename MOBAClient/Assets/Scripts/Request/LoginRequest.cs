﻿using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.OpCode;
using ExitGames.Client.Photon;
using UnityEngine;

public class LoginRequest : BaseRequest
{
    [HideInInspector]
    public string Username;
    [HideInInspector]
    public string Password;

    private LoginPanel m_LoginPanel;

    void Start()
    {
        this.OpCode = OperationCode.Login;
        base.Start();
        
        m_LoginPanel = GetComponent<LoginPanel>();
    }

    public override void DefalutRequest()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.Username, Username);
        data.Add((byte)ParameterCode.Password, Password);
        PhotonEngine.Peer.OpCustom((byte) OpCode, data, true);
    }

    public override void OnOperationResponse(OperationResponse response)
    {
        m_LoginPanel.OnLoginResponse((ReturnCode)response.ReturnCode);
    }
}