using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XLua;

public class UIMessage : MonoBehaviour
{
    public Text title;
    public Text message;
    public Button OkButton;
    public Text oKButtonTitle;
    public Button CancelButton;
    public Text cancelButtonTitle;

    public UnityAction OnYes;
    public UnityAction OnNo;

    [LuaCallCSharp]
    public enum MessageBoxType
    {
       
        Information = 1,
 
        Confirm = 2,

    }

    public static UIMessage Show(string message, string title = "", MessageBoxType type = MessageBoxType.Information, string btnOK = "", string btnCancel = "")
    {
        GameObject go = (GameObject)GameObject.Instantiate(Resources.Load<Object>("UIPrefabs/UIMessage/UIMessage"));
        UIMessage msgbox = go.GetComponent<UIMessage>();
        msgbox.Init(title, message, type, btnOK, btnCancel);
        return msgbox;
    }
    public static UIMessage Show(string message, string title = "", int status = 1, string btnOK = "", string btnCancel = "")
    {
        GameObject go = (GameObject)GameObject.Instantiate(Resources.Load<Object>("UIPrefabs/UIMessage/UIMessage"));
        UIMessage msgbox = go.GetComponent<UIMessage>();
        MessageBoxType type = status == 1 ? MessageBoxType.Information : MessageBoxType.Confirm;
        msgbox.Init(title, message, type, btnOK, btnCancel);
        return msgbox;
    }

    public void Init(string title, string message, MessageBoxType type = MessageBoxType.Information, string btnOK = "", string btnCancel = "")
    {
        if (!string.IsNullOrEmpty(title)) this.title.text = message;

        this.message.text = title;

        if (!string.IsNullOrEmpty(btnOK)) this.oKButtonTitle.text = btnOK;
        if (!string.IsNullOrEmpty(btnCancel)) this.cancelButtonTitle.text = btnCancel;

        this.OkButton.onClick.AddListener(OnClickYes);
        this.CancelButton.onClick.AddListener(OnClickNo);

        this.CancelButton.gameObject.SetActive(type == MessageBoxType.Confirm);

    }

    void OnClickYes()
    {

        Destroy(this.gameObject);
        if (this.OnYes != null)
            this.OnYes();
    }

    void OnClickNo()
    {

        Destroy(this.gameObject);
        if (this.OnNo != null)
            this.OnNo();
    }
}
