using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using static UIMessage;

public class UIEmail : MonoBehaviour
{
    public TextMeshProUGUI EmailCountTxt;
    public int emailCount = 0;


    public Button backButton;
    public Button deleteAllReadedButton;
    public Button getAllItemButton;

    private EmailItemListView emailListView;

    public TextMeshProUGUI emailTitleTxt;
    public TextMeshProUGUI emailSenderTxt;
    public TextMeshProUGUI emailBodyTxt;

    public TextMeshProUGUI emailTimeTxt;

    public GameObject EmailItemArea;
    public Button deleteButton;
    public Button getItemButton;

    public CustomScroller emailList;
    public GameObject emailItemPrefab;

    public GameObject emailAreaNoEmail;
    public GameObject emailInfoNoEmail;

    public void Awake()
    {
        InitInfo();

        Init();

        emailList.Init(emailItemPrefab, emailListView);

        emailList.Refresh(true);
    }

    public void Refresh(bool isInit)
    {
        if(isInit)
        {
            Init();
        }

        emailList.Refresh(isInit);
    }

    public void CloseUIEmail()
    {
        this.gameObject.SetActive(false);
    }

    public void InitInfo()
    {
        emailListView = new EmailItemListView(SetSelectedItem);
    }

    public void Init()
    {

        EmailListInit();

        SetEmailCountTxt();

        if (EmailManager.Instance.EmailInfoDir.Count == 0)
        {
            emailAreaNoEmail.SetActive(true);
            emailInfoNoEmail.SetActive(true);
        }
        else
        {
            emailAreaNoEmail.SetActive(false);
            emailInfoNoEmail.SetActive(false);
        }
        
    }

    private void EmailListInit()
    {
        emailListView.Reset();

        for (int i = 0; i < EmailManager.Instance.NewEmailWithItem.Count; i++)
        {
            emailListView.SetOrderList(EmailManager.Instance.NewEmailWithItem[i]);
        }
        for (int i = 0; i < EmailManager.Instance.NewEmailWithoutItem.Count; i++)
        {
            emailListView.SetOrderList(EmailManager.Instance.NewEmailWithoutItem[i]);
        }
        for (int i = 0; i < EmailManager.Instance.OldEmailWithItem.Count; i++)
        {
            emailListView.SetOrderList(EmailManager.Instance.OldEmailWithItem[i]);
        }
        for (int i = 0; i < EmailManager.Instance.OldEmailWithoutItem.Count; i++)
        {
            emailListView.SetOrderList(EmailManager.Instance.OldEmailWithoutItem[i]);
        }
    }

    /// <summary>
    /// 设置邮件主体内容
    /// </summary>
    /// <param name="emailInfo"></param>
    public void InitEmailInfo(EmailInfo emailInfo)
    {
        if(emailInfo == null)return;
        if (emailTitleTxt != null)
        {
            emailTitleTxt.text = emailInfo.emailTitle;
        }
        if (emailSenderTxt != null)
        {
            emailSenderTxt.text = emailInfo.emailSender;
        }
        if (emailBodyTxt != null)
        {
            emailBodyTxt.text = emailInfo.emailBody;
            RectTransform rect = emailBodyTxt.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x,emailBodyTxt.preferredHeight);
        }
        if (emailTimeTxt != null)
        {
            //可能要对格式进行测试
            emailTimeTxt.text = emailInfo.emailSendTime.ToString();
        }

        //SetEmailItems(emailInfo);
    }

    /// <summary>
    /// 根据是否有奖励进行设置
    /// </summary>
    /// <param name="emailInfo"></param>
    //public void SetEmailItems(EmailInfo emailInfo)
    //{
    //    if(emailInfo.hasItem)
    //    {
    //        EmailItemArea.SetActive(true);
    //        if(emailInfo.emailStatus == EmailStatus.Finish)
    //        {
    //            deleteButton.gameObject.SetActive(true);
    //            getItemButton.gameObject.SetActive(false);
    //        }
    //        else
    //        {
    //            deleteButton.gameObject.SetActive(false);
    //            getItemButton.gameObject.SetActive(true);
    //        }
    //        SetlItems2Content(emailInfo);
    //    }
    //    else
    //    {
    //        EmailItemArea.SetActive(false);
    //    }
    //}

    /// <summary>
    /// 设置邮件数量
    /// </summary>
    /// <param name="emailCount">当前邮件数量</param>
    public void SetEmailCountTxt()
    {
        EmailCountTxt.text = string.Format("{0}/{1}", EmailManager.Instance.emailTotalCount.ToString(),EmailManager.Instance.emailMaxCount.ToString());
    }

    private void SetlItems2Content(EmailInfo emailInfo)
    {
        for (int i = 0;i<emailInfo.itemIdLis.Count;i++)
        {
            //从DataManager中取得对应道具，并生成prefab的init
            //GameObject item = DataManager.GetItemPrefab(emailInfo.itemIdLis[i])
            
            //设置item的父节点到content
            //
            //GameObject item = new GameObject();
            //item.transform.parent = itemsRect.content;
        }
    }

    public void SetSelectedItem(UIEmailItem nowSelectedItem)
    {
        //UIEmailItem lastSelectedItem = emailList.SetSelectedItem(nowSelectedItem);
        //InitEmailInfo(EmailManager.Instance.EmailInfoDir[lastSelectedItem.emailId]);
        //EmailListInit();

        //使用emailView
        if(emailListView.selectedId == -1 )return;
        InitEmailInfo(EmailManager.Instance.EmailInfoDir[emailListView.selectedId]);
        Refresh(false);
    }

    /// <summary>
    /// 获取邮件的道具
    /// </summary>
    public void GetItem(int emailId = -1)
    {
        emailId = emailId == -1? emailListView.selectedId : emailId;
        EmailInfo info =  EmailManager.Instance.EmailInfoDir[emailId];
        bool haveGetItem =  EmailManager.Instance.GetItem(emailId);
        if (haveGetItem)
        {
            //如果当前显示邮件是选中邮件,更新按钮显示
            if (emailListView.selectedId == emailId)
            {
                InitEmailInfo(info);
            }

        }
        else
        {
            Debug.LogError(info.emailId + " : 领取失败");
        }
    }
    
    /// <summary>
    /// 获取所有邮件的道具
    /// </summary>
    public void GetAllItem()
    {
        if (EmailManager.Instance.EmailInfoDir.Count == 0 ||
           (EmailManager.Instance.NewEmailWithItem.Count == 0 ||
           EmailManager.Instance.NewEmailWithoutItem.Count == 0)
           )
        {
            UIMessage.Show("领取邮件", "无相关邮件",
                        MessageBoxType.Information, "确定", "");
        }
        else
        {
            UIMessage messagebox = UIMessage.Show("领取邮件", "确认领取所有邮件奖励？",
                        MessageBoxType.Confirm, "确定", "返回");
            messagebox.OnYes = () =>
            {

                for (int i = EmailManager.Instance.NewEmailWithItem.Count - 1; i >= 0; i--)
                {
                    int emailId = EmailManager.Instance.NewEmailWithItem[i];
                    GetItem(emailId);
                }

                Refresh(true);

            };


        }

    }

    /// <summary>
    /// 删除邮件
    /// </summary>
    public void DeleteEmail(int emailId = -1)
    {

        UIMessage messagebox = UIMessage.Show("删除邮件", "确认删除当前的邮件？",
                        MessageBoxType.Confirm, "确定", "返回");
        messagebox.OnYes = () =>
        {
            emailId = emailId == -1 ? emailListView.selectedId : emailId;

            EmailManager.Instance.DeleteOneEmail(emailId);

        };

    }

    /// <summary>
    /// 删除所有已读邮件
    /// </summary>
    public void DeleteAllEmail()
    {
        if(EmailManager.Instance.EmailInfoDir.Count == 0 ||
           (EmailManager.Instance.OldEmailWithItem.Count == 0&&
           EmailManager.Instance.OldEmailWithoutItem.Count == 0)
           )
        {
            UIMessage.Show("删除邮件", "无相关邮件",
                        MessageBoxType.Information, "确定", "");
        }
        else{
            UIMessage messagebox = UIMessage.Show("删除邮件", "确认删除所有已读的邮件？",
                        MessageBoxType.Confirm, "确定", "返回");
            messagebox.OnYes = () =>
            {
                List<int> EmailIdWaitForDelete = new List<int>();
                for (int i = EmailManager.Instance.OldEmailWithItem.Count - 1; i >= 0; i--)
                {
                    int emailId = EmailManager.Instance.OldEmailWithItem[i];
                    EmailIdWaitForDelete.Add(emailId);
                }
                for (int i = EmailManager.Instance.OldEmailWithoutItem.Count - 1; i >= 0; i--)
                {
                    int emailId = EmailManager.Instance.OldEmailWithoutItem[i];
                    EmailIdWaitForDelete.Add(emailId);

                }
                EmailManager.Instance.DeleteAllEmail(EmailIdWaitForDelete);
            };
        }
       

    }
}
