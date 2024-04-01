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
    //�ʼ�ID
    public int emailId;
    //��������
    public int conditionId;
    //�ʼ�����
    public string emailTitle;
    //�ʼ�����
    public string emailBody;
    //������
    public string emailSender;
    //����ʱ��
    public DateTime emailSendTime;
    //��Чʱ��
    public long lostTimer;
    //�ʼ���������ID�б�
    public List<int> itemIdLis = new List<int>();
    //�ʼ�״̬
    public EmailStatus emailStatus;
    //�Ƿ��е���
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
    /// ���EmailInfo�� Status
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
            Debug.LogError("�ʼ��ѹ���");
            return false;
        }
        return true;
    }
}
