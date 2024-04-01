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
    /// �����ʼ���������
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
            //����Ҫ�Ը�ʽ���в���
            emailTimeTxt.text = emailInfo.emailSendTime.ToString();
        }

        //SetEmailItems(emailInfo);
    }

    /// <summary>
    /// �����Ƿ��н�����������
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
    /// �����ʼ�����
    /// </summary>
    /// <param name="emailCount">��ǰ�ʼ�����</param>
    public void SetEmailCountTxt()
    {
        EmailCountTxt.text = string.Format("{0}/{1}", EmailManager.Instance.emailTotalCount.ToString(),EmailManager.Instance.emailMaxCount.ToString());
    }

    private void SetlItems2Content(EmailInfo emailInfo)
    {
        for (int i = 0;i<emailInfo.itemIdLis.Count;i++)
        {
            //��DataManager��ȡ�ö�Ӧ���ߣ�������prefab��init
            //GameObject item = DataManager.GetItemPrefab(emailInfo.itemIdLis[i])
            
            //����item�ĸ��ڵ㵽content
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

        //ʹ��emailView
        if(emailListView.selectedId == -1 )return;
        InitEmailInfo(EmailManager.Instance.EmailInfoDir[emailListView.selectedId]);
        Refresh(false);
    }

    /// <summary>
    /// ��ȡ�ʼ��ĵ���
    /// </summary>
    public void GetItem(int emailId = -1)
    {
        emailId = emailId == -1? emailListView.selectedId : emailId;
        EmailInfo info =  EmailManager.Instance.EmailInfoDir[emailId];
        bool haveGetItem =  EmailManager.Instance.GetItem(emailId);
        if (haveGetItem)
        {
            //�����ǰ��ʾ�ʼ���ѡ���ʼ�,���°�ť��ʾ
            if (emailListView.selectedId == emailId)
            {
                InitEmailInfo(info);
            }

        }
        else
        {
            Debug.LogError(info.emailId + " : ��ȡʧ��");
        }
    }
    
    /// <summary>
    /// ��ȡ�����ʼ��ĵ���
    /// </summary>
    public void GetAllItem()
    {
        if (EmailManager.Instance.EmailInfoDir.Count == 0 ||
           (EmailManager.Instance.NewEmailWithItem.Count == 0 ||
           EmailManager.Instance.NewEmailWithoutItem.Count == 0)
           )
        {
            UIMessage.Show("��ȡ�ʼ�", "������ʼ�",
                        MessageBoxType.Information, "ȷ��", "");
        }
        else
        {
            UIMessage messagebox = UIMessage.Show("��ȡ�ʼ�", "ȷ����ȡ�����ʼ�������",
                        MessageBoxType.Confirm, "ȷ��", "����");
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
    /// ɾ���ʼ�
    /// </summary>
    public void DeleteEmail(int emailId = -1)
    {

        UIMessage messagebox = UIMessage.Show("ɾ���ʼ�", "ȷ��ɾ����ǰ���ʼ���",
                        MessageBoxType.Confirm, "ȷ��", "����");
        messagebox.OnYes = () =>
        {
            emailId = emailId == -1 ? emailListView.selectedId : emailId;

            EmailManager.Instance.DeleteOneEmail(emailId);

        };

    }

    /// <summary>
    /// ɾ�������Ѷ��ʼ�
    /// </summary>
    public void DeleteAllEmail()
    {
        if(EmailManager.Instance.EmailInfoDir.Count == 0 ||
           (EmailManager.Instance.OldEmailWithItem.Count == 0&&
           EmailManager.Instance.OldEmailWithoutItem.Count == 0)
           )
        {
            UIMessage.Show("ɾ���ʼ�", "������ʼ�",
                        MessageBoxType.Information, "ȷ��", "");
        }
        else{
            UIMessage messagebox = UIMessage.Show("ɾ���ʼ�", "ȷ��ɾ�������Ѷ����ʼ���",
                        MessageBoxType.Confirm, "ȷ��", "����");
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
