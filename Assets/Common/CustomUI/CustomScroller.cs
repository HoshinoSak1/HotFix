using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
using XLua;

public class CustomScroller : ScrollRect
{
    public int elementPos = 0;

    public float elementHeight;
    public float elementSpace = 2;


    public int maxViewElementCount = 0;

    private GameObject emailItemPrefab;
    public int ElementCount;

    private GameObject[] childItems;
    private int childBufferStart = 0;
    private int dataIDStart = 0;


    private bool moveTrigger = true;

    [CSharpCallLua]
    public delegate void UPdateChildItemDelegate(GameObject uiEmailPrefab, int emailPos);
    public event UPdateChildItemDelegate UPdateChildItem;
    public void Init(GameObject emailItemPrefab)
    {
        this.onValueChanged.AddListener(OnScrollChanged);

        this.emailItemPrefab = emailItemPrefab;

        elementHeight = emailItemPrefab.GetComponent<RectTransform>().rect.height + elementSpace;

        elementPos = 0;

        maxViewElementCount = Mathf.Min(ElementCount, (int)(this.GetComponent<RectTransform>().rect.height / elementHeight + 2));

        childItems =  new GameObject[maxViewElementCount];

        for (int i = 0; i < maxViewElementCount; i++)
        {
            GameObject itemItem = Instantiate(emailItemPrefab, this.content);
            itemItem.SetActive(false);
            childItems[i] = itemItem;
        }

        Refresh(true);
    }

    public void Refresh(bool isInit)
    {
        this.content.sizeDelta = new Vector2(0, ElementCount * (elementHeight));
        
        ResetContent(isInit);
    }

    private void OnScrollChanged(Vector2 arg0)
    {
        if (moveTrigger)
        {
            ResetContent(false);
        }
    }

    private void ResetContent(bool clearContents)
    {
        if(clearContents)
        {
            this.StopMovement();
            this.verticalNormalizedPosition = 1;
        }

        float ymin = this.content.localPosition.y;
        int firsetVisibleIndex = (int)(ymin / elementHeight);

        int newRowStart = firsetVisibleIndex;

        int diff = newRowStart - dataIDStart;
        if(Mathf.Abs(diff) >= childItems.Length||diff == 0)
        {
            dataIDStart = newRowStart;
            childBufferStart = 0;
            int rowIdx = newRowStart;
            foreach(var item in childItems)
            {
                UPdateChildItem(item, rowIdx++);
            }
        }else if (diff != 0)
        {
            int newBufferStart = (childBufferStart + diff)% childItems.Length;
            if (diff < 0)
            {
                for(int i = 1;i<= -diff; ++i)
                {
                    int warpIndex = WrapChildIndex(childBufferStart - i);
                    int rowIdx = dataIDStart - i;
                    UPdateChildItem(childItems[warpIndex], rowIdx);
                }
            }
            else
            {
                int prevLastBufIdx = childBufferStart + childItems.Length - 1;
                int prevLastRowIdx = dataIDStart + childItems.Length - 1;
                for (int i = 1; i <= diff; ++i)
                {
                    int wrapIndex = WrapChildIndex(prevLastBufIdx + i);
                    int rowIdx = prevLastRowIdx + i;
                    UPdateChildItem(childItems[wrapIndex], rowIdx);
                }
            }
            childBufferStart = newBufferStart;
            dataIDStart = newRowStart;
        }

    }

    private int WrapChildIndex(int idx)
    {
        while (idx < 0)
            idx += childItems.Length;

        return idx % childItems.Length;
    }

    //private void UPdateChildItem(GameObject uiEmailPrefab,int emailPos)
    //{
    //    if (emailPos < 0 || emailPos >= ElementCount)
    //    {
    //        uiEmailPrefab.gameObject.SetActive(false);
    //    }
    //    else
    //    {
    //        RectTransform rectTransform = uiEmailPrefab.GetComponent<RectTransform>();
    //        Rect childRect = rectTransform.rect;
    //        Vector2 pivot = rectTransform.pivot;

    //        float ytopPos = emailPos * elementHeight;

    //        float yPos = ytopPos + (1f - pivot.y) * childRect.height;
    //        rectTransform.anchoredPosition = new Vector2(0, -yPos);

    //        UIEmailItem uiEmailItem = uiEmailPrefab.GetComponent<UIEmailItem>();
            
    //        emailListView.EmailItemSet(uiEmailItem, emailPos);
           
    //        uiEmailPrefab.gameObject.SetActive(true);
    //    }
        
    //}


}
