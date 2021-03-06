﻿using System.Collections.Generic;
using Common.Code;
using Common.Dto;
using LitJson;
using UnityEngine;

namespace MOBAClient
{
    public class BattleManager : Singleton<BattleManager>
    {
        #region 请求

        /// <summary>
        /// 完成初始化
        /// </summary>
        private InitCompleteRequest m_InitComplete;
        public void InitComplete()
        {
            m_InitComplete.SendRequest();
        }

        /// <summary>
        /// 移动的请求
        /// </summary>
        private HeroMoveRequest m_HeroMoveRequest;
        /// <summary>
        /// 请求移动
        /// </summary>
        /// <param name="point"></param>
        public void RequestMove(Vector3 point)
        {
            // 发送移动的请求
            m_HeroMoveRequest.SendMove(point);
        }

        /// <summary>
        /// 计算伤害的请求
        /// </summary>
        private DamageRequest m_DamageRequest;
        /// <summary>
        /// 请求计算攻击伤害
        /// </summary>
        /// <param name="attackId">攻击者id</param>
        /// <param name="skillId">技能id</param>
        /// <param name="targetId">目标id</param>
        public void RequestDamage(int attackId, int skillId, int targetId)
        {
            m_DamageRequest.SendDamage(attackId, skillId, targetId);
        }

        /// <summary>
        /// 购买物品的请求
        /// </summary>
        private BuyItemRequest m_BuyRequest;
        /// <summary>
        /// 请求购买物品
        /// </summary>
        /// <param name="itemId"></param>
        public void RequestBuyItem(int itemId)
        {
            m_BuyRequest.SendBuyItem(itemId);
        }

        /// <summary>
        /// 技能升级的请求
        /// </summary>
        private UpgradeSkillRequest m_UpgradeSkillRequest;
        /// <summary>
        /// 请求升级技能
        /// </summary>
        public void RequestUpgradeSkill(int skillId, ItemSkill item)
        {
            m_UpgradeSkillRequest.SendUpgradeSkill(skillId, item);
        }

        /// <summary>
        /// 使用技能的请求
        /// </summary>
        private UseSkillRequest m_UseSkillRequest;
        /// <summary>
        /// 请求使用技能
        /// </summary>
        public void RequestUseSkill(int skillId, int from, int target, int level = 1)
        {
            RequestUseSkill(skillId, from, new int[] { target }, level);
        }
        public void RequestUseSkill(int skillId, int from, int[] target, int level)
        {
            m_UseSkillRequest.SendUseSkill(skillId, level, from, target);            
        }

        /// <summary>
        /// 效果结束的请求
        /// </summary>
        private EffectEndRequest m_EffectEndRequest;
        public void RequestEffectEnd(string effectkey)
        {
            m_EffectEndRequest.SendEffectEnd(effectkey);
        }

        #endregion

        void Start()
        {
            // 释放资源
            UIManager.Instance.ClearAll();
            ResourcesManager.Instance.ReleaseAll();

            // 获取请求
            m_InitComplete = GetComponent<InitCompleteRequest>();
            m_DamageRequest = GetComponent<DamageRequest>();
            m_HeroMoveRequest = GetComponent<HeroMoveRequest>();
            m_BuyRequest = GetComponent<BuyItemRequest>();
            m_UpgradeSkillRequest = GetComponent<UpgradeSkillRequest>();
            m_UseSkillRequest = GetComponent<UseSkillRequest>();
            m_EffectEndRequest = GetComponent<EffectEndRequest>();

            // 加载资源
            ResourcesManager.Instance.Load(Paths.UI_BUY, typeof(AudioClip));

            // 发送进入游戏的请求
            GetComponent<EnterBattleRequest>().SendRequest();
        }
    }
}
