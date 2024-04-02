local UIEmail = BaseClass.Create(UIBase)

function UIEmail:__init()
    self.emailCount = 0
    -- Event:AddEvent("UIRefresh",function(needReset)self:UIRefresh(needReset)end)
    -- Event:AddEvent("UIIEmailInfoRefresh",function()self:SetSelectedItem()end)
    self:AddEvent()
    self.uiEmailgo = {}
end

function UIEmail:SetComponent()

    self.uiEmail = self:GetUIComponent("UIEmail","UIEmail")

    self.DltAllReadBtn = self:GetUIComponent("DltAllReadBtn","Button")
    self.GetAllItemBtn = self:GetUIComponent("GetAllItemBtn","Button")
    self.DeleteBtn = self:GetUIComponent("DeleteBtn","Button")
    self.GetItemBtn = self:GetUIComponent("GetItemBtn","Button")
    self.BackButton = self:GetUIComponent("BackButton","Button")

    self:AddButtonClickListener("DltAllReadBtn",function()self:DeleteAllEmail()end)
    self:AddButtonClickListener("GetAllItemBtn",function()self:GetAllItem()end)
    self:AddButtonClickListener("DeleteBtn",function()self:DeleteEmail(-1)end)
    self:AddButtonClickListener("GetItemBtn",function()self:GetItem(-1)end)
    self:AddButtonClickListener("BackButton",function()self:CloseUIEmail()end)
end

function UIEmail:Init()
    self:InitInfo()

    self:ViewReset()

    self:AddDelegate()

    self.uiEmail.emailList:Init(self.uiEmail.emailItemPrefab)
end

function UIEmail:UPdateChildItem(uiEmailPrefab,emailPos)
    if emailPos < 0 or emailPos >= EmailManager.emailTotalCount then
        uiEmailPrefab.gameObject:SetActive(false)
    else
        local rectTransform = uiEmailPrefab:GetComponent(typeof(UnityEngine.RectTransform))
        local childRect = rectTransform.rect
        local pivot = rectTransform.pivot

        local ytopPos = emailPos * self.uiEmail.emailList.elementHeight

        local yPos = ytopPos + (1 - pivot.y) * childRect.height
        rectTransform.anchoredPosition = UnityEngine.Vector2(0, -yPos)

        local uiEmailItem = UIEmailInfo.New(uiEmailPrefab)
        uiEmailItem:SetComponent()
        table.insert(self.uiEmailgo,uiEmailItem)
        --local uiEmailItem = uiEmailPrefab:GetComponent("UIEmailItem")

        self.emailListView:EmailItemSet(uiEmailItem, emailPos)

        uiEmailPrefab.gameObject:SetActive(true)
    end
end

function UIEmail:InitInfo()
    self.emailListView = EmailItemListView.New()
end

function UIEmail:ViewReset()
        self.uiEmail.emailList.ElementCount = EmailManager.emailTotalCount
        self:EmailListInit()
        self:SetEmailCountTxt()
    
        if EmailManager:GetDirLength() == 0 then
            self.uiEmail.emailAreaNoEmail:SetActive(true)
            self.uiEmail.emailInfoNoEmail:SetActive(true)
        else
            self.uiEmail.emailAreaNoEmail:SetActive(false)
            self.uiEmail.emailInfoNoEmail:SetActive(false)
        end
end

function UIEmail:EmailListInit()
    self.emailListView:Reset()

    -- 添加新邮件（带物品）的邮件ID到列表
    for i = 1, #EmailManager.NewEmailWithItem do
        self.emailListView:SetOrderList(EmailManager.NewEmailWithItem[i])
    end

    -- 添加新邮件（不带物品）的邮件ID到列表
    for i = 1, #EmailManager.NewEmailWithoutItem do
        self.emailListView:SetOrderList(EmailManager.NewEmailWithoutItem[i])
    end

    -- 添加旧邮件（带物品）的邮件ID到列表
    for i = 1, #EmailManager.OldEmailWithItem do
        self.emailListView:SetOrderList(EmailManager.OldEmailWithItem[i])
    end

    -- 添加旧邮件（不带物品）的邮件ID到列表
    for i = 1, #EmailManager.OldEmailWithoutItem do
        self.emailListView:SetOrderList(EmailManager.OldEmailWithoutItem[i])
    end
end

function UIEmail:InitEmailInfo(emailInfo)
    if emailInfo == nil then
        return
    end

    if self.uiEmail.emailTitleTxt ~= nil then
        self.uiEmail.emailTitleTxt.text = emailInfo.emailTitle
    end

    if self.uiEmail.emailSenderTxt ~= nil then
        self.uiEmail.emailSenderTxt.text = emailInfo.emailSender
    end

    if self.uiEmail.emailBodyTxt ~= nil then
        self.uiEmail.emailBodyTxt.text = emailInfo.emailBody
        local rect = self.uiEmail.emailBodyTxt:GetComponent("RectTransform")
        rect.sizeDelta = UnityEngine.Vector2(rect.sizeDelta.x, self.uiEmail.emailBodyTxt.preferredHeight)
    end

    if emailTimeTxt ~= nil then
        -- 这里可能需要对日期格式进行适当的处理
        self.uiEmail.emailTimeTxt.text = tostring(emailInfo.emailSendTime)
    end

    self:SetEmailItems(emailInfo)
end

function UIEmail:SetEmailItems(emailInfo)
    if emailInfo.hasItem then
        self.uiEmail.EmailItemArea:SetActive(true)
        if emailInfo.emailStatus == EmailStatus.Finish then
            self.DeleteBtn.gameObject:SetActive(true)
            self.GetItemBtn.gameObject:SetActive(false)
        else
            self.DeleteBtn.gameObject:SetActive(false)
            self.GetItemBtn.gameObject:SetActive(true)
        end
        --SetlItems2Content(emailInfo)
    else
        self.uiEmail.EmailItemArea:SetActive(false)
    end
end

-- 设置选中项方法
function UIEmail:SetSelectedItem()
    -- 使用 emailView
    if self.emailListView.selectedId == -1 then
        return
    end
    self:InitEmailInfo(EmailManager.EmailInfoDir[self.emailListView.selectedId])
    self:UIRefresh(false)
end

function UIEmail:SetEmailCountTxt()
    self.uiEmail.EmailCountTxt.text = string.format("%d/%d", EmailManager.emailTotalCount, EmailManager.emailMaxCount)
end

function UIEmail:UIRefresh(needReset)
    if needReset then
        self:ViewReset() 
    end
    self.uiEmail.emailList:Refresh(needReset)
end



function UIEmail:GetItem(emailId)
    print("GetItem")
    emailId = emailId == -1 and self.emailListView.selectedId or emailId
    local info = EmailManager.EmailInfoDir[emailId]
    local haveGetItem = EmailManager:GetItem(emailId)

    if haveGetItem then
        -- 如果当前显示邮件是选中邮件，则更新按钮显示
        if self.emailListView.selectedId == emailId then
            self:InitEmailInfo(info)
        end
    else
        print(info.emailId .. " : 领取失败")
    end
end

function UIEmail:GetAllItem()
    print("GetAllItem")

    if EmailManager.EmailInfoDir.Count == 0 or
        (#EmailManager.NewEmailWithItem == 0 and
        #EmailManager.NewEmailWithoutItem == 0)
    then
        UIMessage.Show("领取邮件", "无相关邮件", MessageBoxType.Information, "确定", "")
    else
        local messagebox = UIMessage.Show("领取邮件", "确认领取所有邮件奖励？", 2, "确定", "返回")
        messagebox.OnYes = function()
            for i = #EmailManager.NewEmailWithItem, 1, -1 do
                local emailId = EmailManager.NewEmailWithItem[i]
                self:GetItem(emailId)
            end

            self:UIRefresh(true)
        end
    end

end

function UIEmail:DeleteEmail(emailId)
    print("DeleteEmail")

    emailId = emailId == -1 and self.emailListView.selectedId or emailId

    EmailManager:DeleteOneEmail(emailId)
end


function UIEmail:DeleteAllEmail()
    print("DeleteAllEmail")
    if EmailManager.EmailInfoDir.Count == 0 or
        (#EmailManager.OldEmailWithItem == 0 and
        #EmailManager.OldEmailWithoutItem == 0)
    then
        UIMessage.Show("删除邮件", "无相关邮件", MessageBoxType.Information, "确定", "")
    else
        local messagebox = UIMessage.Show("删除邮件", "确认删除所有已读的邮件？",2, "确定", "返回")
        messagebox.OnYes = function()
            local EmailIdWaitForDelete = {}
            for i = #EmailManager.OldEmailWithItem, 1, -1 do
                local emailId = EmailManager.OldEmailWithItem[i]
                table.insert(EmailIdWaitForDelete, emailId)
            end
            for i = #EmailManager.OldEmailWithoutItem, 1, -1 do
                local emailId = EmailManager.OldEmailWithoutItem[i]
                table.insert(EmailIdWaitForDelete, emailId)
            end
            EmailManager:DeleteAllEmail(EmailIdWaitForDelete)
        end
    end
end


function UIEmail:CloseUIEmail()
    print("CloseUIEmail")
    self:RemoveEvent()

    --self.uiEmail.emailList:UPdateChildItem("-",function(uiEmailPrefab,emailPos) self:UPdateChildItem(uiEmailPrefab,emailPos) end)
    self:RemoveDelegate()

    for i = 1, #self.uiEmailgo do
        self.uiEmailgo[i]:Close()
    end

    self:Close()
end

function UIEmail:AddEvent()

    self.UIRefreshEvent = function(needReset)
        self:UIRefresh(needReset)
    end
    Event:AddEvent("UIRefresh",self.UIRefreshEvent)

    self.UIIEmailInfoRefreshEvent = function()
        self:SetSelectedItem()
    end
    Event:AddEvent("UIIEmailInfoRefresh",self.UIIEmailInfoRefreshEvent)
end

function UIEmail:RemoveEvent()

    Event:RemoveEvent("UIRefresh",self.UIRefreshEvent)
    Event:RemoveEvent("UIIEmailInfoRefresh",self.UIIEmailInfoRefreshEvent)

end

function UIEmail:AddDelegate()
    self.UPdateChildItemDelegate = function(uiEmailPrefab,emailPos)
        self:UPdateChildItem(uiEmailPrefab,emailPos)
    end
    self.uiEmail.emailList:UPdateChildItem("+",self.UPdateChildItemDelegate)
end

function UIEmail:RemoveDelegate()
    self.uiEmail.emailList:UPdateChildItem("-",self.UPdateChildItemDelegate)
end


return UIEmail