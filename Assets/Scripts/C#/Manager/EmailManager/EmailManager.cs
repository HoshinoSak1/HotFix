using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public enum EmailEvent{
    EmailCheck,
    EmailGetItem
}


public class EmailManager : MonoBehaviour
{
    private static EmailManager instance;

    public static EmailManager Instance
    {
        get { 
            if (instance != null)
            {
                return instance;
            }
            instance = FindObjectOfType<EmailManager>();
            if (instance == null)
            {
                var go = new GameObject("EmailManager");
                instance = go.AddComponent<EmailManager>();
            } 
        instance.Init();
        return instance;
        }
    }

    public Dictionary<int,EmailInfo> EmailInfoDir = new Dictionary<int,EmailInfo>();

    public Button UIEmailBtn;   
    public GameObject UIEmail;
    public Transform UIRoots;
    

    public UIEmail uiEmail;

    public List<int> NewEmailWithoutItem = new List<int>();
    public List<int> NewEmailWithItem = new List<int>();
    public List<int> OldEmailWithoutItem = new List<int>();
    public List<int> OldEmailWithItem = new List<int>();

    //所有涉及邮件数量的值从这里取
    public int emailTotalCount = 0;

    public int emailMaxCount = 10;

    public void Init()
    {
        for(int i = 0;i<EmailTester.Instance.emailInfos.Count;i++)
        {

            EmailInfo item = EmailTester.Instance.emailInfos[i];
            SetNewEmail(item);
        }
        DontDestroyOnLoad(gameObject);
        EmailTester.Instance.EmailManagerLister += GetNewEmail;
    }

    public void GetNewEmail(EmailInfo item)
    {
        SetNewEmail(item);

        uiEmail.Refresh(true);
    }

    public void SetNewEmail(EmailInfo item)
    {

        if (emailTotalCount >= emailMaxCount)
        {
            OverCountLimit();
        }

        EmailInfoDir[item.emailId] = item;

        if (item.emailStatus == EmailStatus.Finish && item.hasItem == false)
        {
            ListAddEmail(OldEmailWithoutItem, item);

        }
        if (item.emailStatus == EmailStatus.Finish && item.hasItem == true)
        {
            ListAddEmail(OldEmailWithItem, item);

        }
        if (item.emailStatus == EmailStatus.haveCheck)
        {
            ListAddEmail(NewEmailWithoutItem, item);

        }
        if (item.emailStatus == EmailStatus.haveItem)
        {
            ListAddEmail(NewEmailWithItem, item);
        }
        emailTotalCount++;
    }
    
    private void ListAddEmail(List<int> list, EmailInfo newEmail)
    {
        int insertpos = list.Count;

        for (; insertpos > 0; insertpos--)
        {
            EmailInfo oldEmail = EmailInfoDir[list[insertpos-1]];
            if (!CompareSendTime(newEmail, oldEmail))
            {
                list.Insert(insertpos, newEmail.emailId);
                return;
            }
        }
        list.Insert(insertpos, newEmail.emailId);
    }

    private bool CompareSendTime(EmailInfo newEmail,EmailInfo oldEmail)
    {
        if (newEmail.emailSendTime >= oldEmail.emailSendTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    internal bool GetItem(int emailId)
    {
        EmailInfo emailInfo = EmailInfoDir[emailId];
        bool res = false;
        //等待道具相关接口提供获取成功
        if (true)
        {
            //成功后变更属性
            res =  ChangeEmailStatus(emailInfo, EmailEvent.EmailGetItem);

        }
        uiEmail.Refresh(false);
        return res;
    }

    public void DeleteEmail(int emailId)
    {
        DeleteEmailOnTester(emailId);

        EmailInfoDir.Remove(emailId);

        NewEmailWithItem.Remove(emailId);

        NewEmailWithoutItem.Remove(emailId);

        OldEmailWithoutItem.Remove(emailId);

        OldEmailWithItem.Remove(emailId);

        emailTotalCount--;

    }

    public void DeleteOneEmail(int emailId)
    {
        DeleteEmail(emailId);

        uiEmail.Refresh(true);
    }

    public void DeleteAllEmail(List<int> emailIdList)
    {
        for(int i = 0; i < emailIdList.Count; i++)
        {
            DeleteEmail(emailIdList[i]);
        }

        uiEmail.Refresh(true);
    }

    public bool ChangeEmailStatus(EmailInfo emailInfo, EmailEvent _event)
    {
        if (emailInfo != null)
        {
        
            if (_event == EmailEvent.EmailCheck)
            {

                if (emailInfo.emailStatus == EmailStatus.haveCheck)
                {
                    NewEmailWithoutItem.Remove(emailInfo.emailId);

                    ListAddEmail(OldEmailWithoutItem, emailInfo);

                    return ChangeEmailInfoStatus(emailInfo);
                }
                if(emailInfo.emailStatus == EmailStatus.Finish)
                {
                    return ChangeEmailInfoStatus(emailInfo);
                }


            }
            if (emailInfo.emailStatus == EmailStatus.haveItem && _event == EmailEvent.EmailGetItem)
            {
                NewEmailWithItem.Remove(emailInfo.emailId);

                ListAddEmail(OldEmailWithItem, emailInfo);

                return ChangeEmailInfoStatus(emailInfo);
            }
        }
        return false;
    }

    private bool ChangeEmailInfoStatus(EmailInfo emailInfo)
    {
        bool changeRes = emailInfo.ChangeStatus();
        if (!changeRes)
        {
            uiEmail.DeleteEmail(emailInfo.emailId);
        }
        return changeRes;
    }

    public void OverCountLimit()
    {
        if (DeleteOvetLimitEmail(OldEmailWithoutItem))
        {
            return;
        }
        if (DeleteOvetLimitEmail(OldEmailWithItem))
        {
            return;
        }
        if (DeleteOvetLimitEmail(NewEmailWithoutItem))
        {
            return;
        }
        if (DeleteOvetLimitEmail(NewEmailWithItem))
        {
            return;
        }
    }

    private bool DeleteOvetLimitEmail(List<int> list)
    {
        int deletePoint = -1;
        deletePoint = GetLessTimeEmailId(list);
        if (deletePoint > 0)
        {
            EmailManager.Instance.DeleteOneEmail(deletePoint);
            return true;
        }
        return false;
    }

    int GetLessTimeEmailId(List<int> list)
    {
        int lessTime = int.MaxValue;
        DateTime clientTime = DateTime.Now;
        TimeSpan ts;
        int res = -1;
        for (int i = 0; i < list.Count; i++)
        {
            EmailInfo info = EmailInfoDir[list[i]];
            ts = clientTime - info.emailSendTime;
            long lastTime = (long)(info.lostTimer - ts.TotalSeconds);
            if (lastTime < lessTime)
            {
                res = list[i];
                lessTime = (int)lastTime;
            }
        }
        return res;
    }

    private void DeleteEmailOnTester(int emailId)
    {
        EmailTester.Instance.emailInfos.Remove(EmailInfoDir[emailId]);
    }


    //public void OpenUIEmail()
    //{
    //    if(uiEmail == null)
    //    {
    //        uiEmail = Instantiate(UIEmail, UIRoots).GetComponent<UIEmail>();
    //    }
    //    else
    //    {
    //        uiEmail.gameObject.SetActive(true);
    //    }

    //    uiEmail.Refresh(true);
    //}


}
