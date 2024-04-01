using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EmailStatus
{
    Finish = 0,
    haveCheck = 1,
    haveItem = 2,
}

public class EmailInfo
{
    //邮件ID
    public int emailId;
    //发放条件
    public int conditionId;
    //邮件标题
    public string emailTitle;
    //邮件正文
    public string emailBody;
    //发件人
    public string emailSender;
    //发件时间
    public DateTime emailSendTime;
    //有效时间
    public long lostTimer;
    //邮件奖励道具ID列表
    public List<int> itemIdLis = new List<int>();
    //邮件状态
    public EmailStatus emailStatus;
    //是否有道具
    public bool hasItem = false;

    public EmailInfo() { }

    public EmailInfo(int emailId, 
                     int conditionId, 
                     string emailTitle, 
                     string emailSender, 
                     string emailBody, 
                     DateTime emailSendTime, 
                     string lostTimer, 
                     string itemIdLis, EmailStatus status)
    {
        this.emailId = emailId;
        this.conditionId = conditionId;
        this.emailTitle = emailTitle;
        this.emailSender = emailSender;
        this.emailBody = emailBody;
        this.emailSendTime = emailSendTime;
        this.lostTimer = long.Parse(lostTimer);

        string[] itemIds = itemIdLis.Split(new char[] { '|' });
        for (int i = 0; i < itemIds.Length; i++)
        {
            if (itemIds[i]=="")break;
            this.itemIdLis.Add(int.Parse(itemIds[i]));
        }
        emailStatus = status;
        if(itemIdLis.Length > 0)
        {
            hasItem = true; 
        }
    }

    /// <summary>
    /// 变更EmailInfo的 Status
    /// </summary>
    public bool ChangeStatus()
    {
        this.emailStatus = EmailStatus.Finish;
        return isOverTime();
    }

    public bool isOverTime()
    {
        DateTime clientTime = DateTime.Now;
        TimeSpan ts = clientTime - emailSendTime;
        long lastTime = (long)(lostTimer - ts.TotalSeconds);
        if (lastTime <= 0)
        {
            Debug.LogError("邮件已过期");
            return false;
        }
        return true;
    }
}
