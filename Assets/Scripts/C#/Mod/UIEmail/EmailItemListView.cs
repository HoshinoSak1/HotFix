//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EmailItemListView 
//{
//    public UIEmailItem nowSelected;

//    public int selectedId = -1;

//    public Action<UIEmailItem> SelectedHandler;

//    //当前加载后显示的UIEmailItem的EmailId顺序
//    public List<int> items;

//    public EmailItemListView(Action<UIEmailItem> action) 
//    { 
//        nowSelected = null;
//        selectedId = -1;
//        SelectedHandler += action;
//        items = new List<int>();
//    }
    
//    public void Reset()
//    {
//        nowSelected = null;
//        selectedId = -1;
//        items = new List<int> ();
//    }

//    public void InitItem(UIEmailItem item,EmailInfo emailInfo)
//    {
//        item.Init(emailInfo);
//        item.SetOwner(this); 

//        CheckSelectedItem(item);

//        if (selectedId!=-1 && item.emailId != selectedId)
//        {
//            item.SetSelected(false);
//        }
//        else
//        {
//            item.SetSelected(true);
//        }
//    }

//    public void ResetNowSelectedItem(int emailId)
//    {
//        if (nowSelected != null && selectedId == emailId)
//        {
//            nowSelected = null;
//            selectedId = -1;
//        }
//    }

//    public void SetSelectedItem(UIEmailItem item)
//    {
//        EmailInfo emailInfo = EmailManager.Instance.EmailInfoDir[item.emailId];

//        if (nowSelected != null)
//        {
//            nowSelected.SetSelected(false);
//        }


        
//        nowSelected = item;
//        selectedId = item.emailId;

//        if (EmailManager.Instance.ChangeEmailStatus(emailInfo, EmailEvent.EmailCheck))
//        {
//            item.Init(emailInfo);
//        }
//        item.SetSelected(true);

//        //更新对应选中的Email信息主体
//        SelectedHandler?.Invoke(item);

//    }

//    public void CheckSelectedItem(UIEmailItem item)
//    {
//        if (nowSelected == null&&selectedId == -1)
//        {
//            SetSelectedItem(item);
//        }
//    }

//    public void SetOrderList(int id)
//    {
//        items.Add(id);
//    }

//    public int GetPosFromID(int id)
//    {
//        for (int i = 0; i < items.Count; i++)
//        {
//            if (items[i]  == id) return i;
//        }
//        return -1;
//    }

//    public void RemoveIDfromList(int id)
//    {
//        items.Remove(id);
//    }

//    internal void EmailItemSet(UIEmailItem uIEmailItem, int emailPos)
//    {
//        int emailId = items[emailPos];

//        EmailInfo emailInfo = EmailManager.Instance.EmailInfoDir[emailId];

//        InitItem(uIEmailItem, emailInfo);
        
//    }
//}
