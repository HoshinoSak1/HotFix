using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua;

[RequireComponent(typeof(RectTransform))]
public class UIEmailItem : MonoBehaviour,IPointerDownHandler
{
    public CanvasGroup CompletedCanvas;

   // private EmailItemListView owner;

    public Image itemIcon;
    public Image redPoint;
    public Image Selected;

    public TextMeshProUGUI emailTitle;
    public TextMeshProUGUI emailSender;
    public TextMeshProUGUI emailTimer;


    public Sprite itemSprite;
    public Sprite emailSprite;

    public int emailId;

    [CSharpCallLua]
    public delegate void OnPointerDownDelegate();

    public Action onPointerDown;

    /// <summary>
    /// 初始化UIEmailItem
    /// </summary>
    /// <param name="emailInfo"></param>
    //public void Init(EmailInfo emailInfo)
    //{

    //    NormalInfoSet(emailInfo);

    //    RedPointSet(emailInfo);

    //    SetCompleteItem(emailInfo);

    //}

    //public void SetOwner(EmailItemListView owner)
    //{
    //    this.owner = owner; 
    //}

    ///// <summary>
    ///// 设置常态显示要素（icon，title，sender，time）
    ///// </summary>
    ///// <param name="emailInfo"></param>
    //private void NormalInfoSet(EmailInfo emailInfo)
    //{
    //    emailId = emailInfo.emailId;
    //    if (emailTitle != null)
    //    {
    //        emailTitle.text = SetEmailTitle(emailInfo.emailTitle);
    //    }
    //    if (emailSender != null)
    //    {
    //        emailSender.text = emailInfo.emailSender;
    //    }
    //    if (emailTimer != null)
    //    {
    //        emailTimer.text = SetTime(emailInfo.lostTimer, emailInfo.emailSendTime);
    //    }

    //    SetItemIcon(emailInfo);
    //    RedPointSet(emailInfo);
    //    SetCompleteItem(emailInfo);

    //    SetSelected(false);
    //}

    //private void SetItemIcon(EmailInfo emailInfo)
    //{
    //    if (emailInfo.itemIdLis.Count > 0)
    //    {
    //        itemIcon.sprite = itemSprite;
    //        //通过另外的系统更新应该显示的道具icon
    //        //itemIcon.sprite = ItemManager.GetItemIconFromId(emailInfo.itemIdLis[0]);
    //    }
    //    else
    //    {           
    //        itemIcon.sprite = emailSprite;
    //    }
    //}

    ///// <summary>
    ///// 根据邮件状态设置红点
    ///// </summary>
    ///// <param name="emailInfo"></param>
    //private void RedPointSet(EmailInfo emailInfo)
    //{
    //    if(emailInfo.emailStatus != EmailStatus.Finish)
    //    {
    //        redPoint.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        redPoint.gameObject.SetActive(false);
    //    }
    //}


    ///// <summary>
    ///// 根据邮件状态设置透明化
    ///// </summary>
    ///// <param name="emailStatus"></param>
    //private void SetCompleteItem(EmailInfo emailInfo)
    //{
    //    if (emailInfo.emailStatus == EmailStatus.Finish)
    //    {
    //        CompletedCanvas.alpha = 0.5f;
    //    }
    //    else
    //    {
    //        CompletedCanvas.alpha = 1;
    //    }
    //}

    ///// <summary>
    ///// 12位截断EmailTitle
    ///// </summary>
    ///// <param name="emailTitle"></param>
    ///// <returns></returns>
    //private String SetEmailTitle(string emailTitle)
    //{

    //    return emailTitle.Length >= 12 ? emailTitle.Substring(0, 10) + "..." : emailTitle; ;
    //}



    ///// <summary>
    ///// 设置邮件剩余时间
    ///// </summary>
    ///// <param name="lostTimer"></param>
    ///// <param name="time"></param>
    ///// <returns></returns>
    //private string SetTime(long lostTimer,DateTime time)
    //{
    //    DateTime clientTime = DateTime.Now;
    //    TimeSpan ts = clientTime - time;
    //    long lastTime = (long)(lostTimer - ts.TotalSeconds);

    //    //过期
    //    if (lastTime < 0)
    //        return string.Format("已过期", lastTime);
    //    //秒 nowtime 分
    //    if (lastTime < 60 && lastTime >=0)
    //        return string.Format("小于1分钟", lastTime);
    //    //分 nowtime 小时
    //    if (lastTime >= 60 && lastTime < 3600)
    //        return string.Format("{0} 分", lastTime/60);
    //    //小时 nowtime 天
    //    if (lastTime >= 3600 && lastTime < 3600*24)
    //        return string.Format("{0} 小时", lastTime / 3600);
    //    //天 nowtime 月
    //    if (lastTime >= 3600 * 24 && lastTime < 3600*24*30)
    //        return string.Format("{0} 天", lastTime / 3600 * 24);
    //    return lastTime.ToString();
    //}

    ///// <summary>
    ///// 设置选中背景
    ///// </summary>
    ///// <param name="selectedTrigger"></param>
    //public void SetSelected(bool selectedTrigger)
    //{
    //    if (selectedTrigger)
    //    {
    //        Selected.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        Selected.gameObject.SetActive(false);
    //    }
    //}

    public void OnPointerDown(PointerEventData eventData)
    {
        onPointerDown();
        //owner.SetSelectedItem(this);
    }
}
