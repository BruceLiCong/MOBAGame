﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestKeyCtrl : MonoBehaviour
{
    public AIBaseCtrl Ctrl;

    void Update()
    {
        #region 空格

        if (Input.GetKey(KeyCode.Space))
        {
            // 焦距到自己的英雄
            Camera.main.GetComponent<CameraCtrl>().FocusOn(Ctrl.transform);
        }

        #endregion

        #region 鼠标左键

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouse = Input.mousePosition;
            // 屏幕坐标转换为射线
            Ray ray = Camera.main.ScreenPointToRay(mouse);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            for (int i = hits.Length - 1; i >= 0; i--)
            {
                if (hits[i].collider.gameObject.tag == "Shop")
                {
                    UIManager.Instance.ShopPanel(UIPanelType.Shop);
                    break;
                }
            }
        }

        #endregion

        #region 鼠标右键

        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mouse = Input.mousePosition;
            // 屏幕坐标转换为射线
            Ray ray = Camera.main.ScreenPointToRay(mouse);

            HitState state = HitState.INVALID;
            RaycastHit hit = new RaycastHit();
            RaycastHit[] hits = Physics.RaycastAll(ray);
            for (int i = hits.Length - 1; i >= 0; i--)
            {
                // 投射到敌人则攻击
                if (hits[i].collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    hit = hits[i];
                    state = HitState.ATTACK;
                    break;
                }
                // 投射到地面则移动
                if (hits[i].collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                {
                    hit = hits[i];
                    state = HitState.MOVE;
                }
            }

            /*
             * 这里这样处理是为了
             * 在先投射到移动层再投射到攻击层的时候
             * 不处理移动，只处理攻击
             */
            if (state == HitState.MOVE)
            {
                // 显示点击特效
                GameObject go = PoolManager.Instance.GetObject("ClickMove");
                go.transform.position = hit.point + Vector3.up * 2;

                Ctrl.Move(hit.point);
            }
            else if (state == HitState.ATTACK)
            {
                Ctrl.Target = hit.collider.gameObject.GetComponent<AIBaseCtrl>();
                Ctrl.ChangeState(AIStateEnum.ATTACK);
                // SkillId为普攻 技能id 为hero.typeId * skillid + n;
            }
        }

        #endregion
    }

    /// <summary>
    /// 点击状态
    /// </summary>
    enum HitState
    {
        INVALID,
        ATTACK,
        MOVE,
    }
}
