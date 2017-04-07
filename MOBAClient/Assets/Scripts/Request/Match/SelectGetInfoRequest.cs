﻿using System;
using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.Dto;
using Common.OpCode;
using ExitGames.Client.Photon;
using LitJson;

/// <summary>
/// 获取和接收选择数据的请求
/// </summary>
public class SelectGetInfoRequest : BaseRequest
{
    private SelectPanel m_SelectPanel;

    public override void Start()
    {
        base.Start();
        m_SelectPanel = GetComponent<SelectPanel>();
    }

    /// <summary>
    /// 接收选人房间内当前的数据
    /// </summary>
    /// <param name="response"></param>
    public override void OnOperationResponse(OperationResponse response)
    {
        // 获取队伍数据
        SelectModel[] team1 = JsonMapper.ToObject<SelectModel[]>(
          response.Parameters[(byte)ParameterCode.TeamOneData] as string);
        SelectModel[] team2 = JsonMapper.ToObject<SelectModel[]>(
            response.Parameters[(byte)ParameterCode.TeamTwoData] as string);
        
        // 初始化选人数据
        m_SelectPanel.SelectData.InitData(team1, team2);

        // 更新界面
        m_SelectPanel.UpdateView();
        // 初始化选人层
        m_SelectPanel.InitSelectHeroLayer(GameData.player.HeroIds);
    }
}