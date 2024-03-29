﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSprite : MonoBehaviour
{
    private Transform player;
    private SpriteRenderer thisSprite;
    private SpriteRenderer playSprite;
    private Color color;
    [Header("时间控制参数")]
    public float activeTime;  //显示时间
    public float activeStart;   //开始显示的时间点
    [Header("不透明度控制")]
    private float alpha;
    public float alphaSet;  //初始值
    public float alphaMultiplier;

    // Update is called once per frame
    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        thisSprite = GetComponent<SpriteRenderer>();
        playSprite = player.GetComponent<SpriteRenderer>();
        alpha = alphaSet;
        thisSprite.sprite = playSprite.sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;
        transform.localScale = player.localScale;
        activeStart = Time.time;
    }
    void Update()
    {
        alpha *= alphaMultiplier;
        color = new Color(0.5f, 0.5f, 1, alpha);
        thisSprite.color = color;
        if (Time.time >= activeStart + activeTime)
        {
            //返回对象池
            ShadowPool.instance.ReturnPool(this.gameObject);
        }
    }
}
