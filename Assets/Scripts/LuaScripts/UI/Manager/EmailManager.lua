EmailEvent = {
    EmailCheck = 1,
    EmailGetItem = 2
}

local EmailManager = {
    EmailInfoDir = {},
    NewEmailWithoutItem = {},
    NewEmailWithItem = {},
    OldEmailWithoutItem = {},
    OldEmailWithItem = {},

    emailTotalCount = 0,
    emailMaxCount = 10,
}

---@func 初始化Manager
function EmailManager:Init()
    local totalEmailCount = EmailTester.Instance.emailInfos.Count

    for i = 0, totalEmailCount - 1 do
        local emailInfo = EmailInfo.New(EmailTester.Instance.emailInfos[i])
        self:SetNewEmail(emailInfo)
    end

    self:AddDelegate()
end

function EmailManager:Close()
    self:RemoveDelegate()
end

function EmailManager:AddDelegate()
    self.getNewItemDelegate = function(email)
        self:GetNewEmail(email)
    end
    EmailTester.Instance:getNewItem('+',self.getNewItemDelegate)
end

function EmailManager:RemoveDelegate()
    EmailTester.Instance:getNewItem('-',self.getNewItemDelegate)
end

function EmailManager:GetDirLength()
    local count = 0

    for k,v in pairs(self.EmailInfoDir) do
        count = count + 1
    end
    return count
end

---@func 插入一封新邮件
function EmailManager:SetNewEmail(emailInfo)
    --计算插入前是否超过总上限
    if self.emailTotalCount >= self.emailMaxCount then
        self:OverCountLimit()
    end

    --key       emailID 
    --value     emailInfo
    self.EmailInfoDir[emailInfo.emailId] = emailInfo

    --判定新邮件应该插入哪一组List
    if emailInfo.emailStatus == EmailStatus.Finish and emailInfo.hasItem == false then
        self:ListAddEmail(self.OldEmailWithoutItem,emailInfo)
    end
    if emailInfo.emailStatus == EmailStatus.Finish and emailInfo.hasItem == true then
        self:ListAddEmail(self.OldEmailWithItem,emailInfo)
    end
    if emailInfo.emailStatus == EmailStatus.haveCheck then
        self:ListAddEmail(self.NewEmailWithoutItem,emailInfo)
    end
    if emailInfo.emailStatus == EmailStatus.haveItem then
        self:ListAddEmail(self.NewEmailWithItem,emailInfo)
    end

    --插入完成 总计增加
    self.emailTotalCount = self.emailTotalCount + 1
end

---@func 超过上限处理
function EmailManager:OverCountLimit()
    if self:DeleteOvetLimitEmail(self.OldEmailWithoutItem) then
        return
    elseif self:DeleteOvetLimitEmail(self.OldEmailWithItem) then
        return
    elseif self:DeleteOvetLimitEmail(self.NewEmailWithoutItem)  then
        return
    elseif self:DeleteOvetLimitEmail(self.NewEmailWithItem)  then
        return
    end
end

---@func 获得指定List中优先级最低的邮件ID
function EmailManager:GetLessTimeEmailId(list)
    local lessTime = math.maxinteger
    local res = -1
    local ts
    for i = 1, #list do
        local emailInfo = self.EmailInfoDir[list[i]]
        ts = os.time() - emailInfo.emailSendTime
        local lastTime = emailInfo.lostTimer - ts
        if lessTime > lastTime then
            res = list[i]
            lessTime = lastTime
        end
    end
    return res
end

---@func 删除指定id的邮件数据
function EmailManager:DeleteEmail(emailId)
    self:DeleteEmailOnTester(emailId)
    
    self.emailTotalCount = self.emailTotalCount - 1

    DirRemovekey(self.EmailInfoDir,emailId)

    ListRemoveValue(self.OldEmailWithoutItem,emailId)
    ListRemoveValue(self.OldEmailWithItem,emailId)
    ListRemoveValue(self.NewEmailWithoutItem,emailId)
    ListRemoveValue(self.NewEmailWithItem,emailId)
end

---@func 删除指定id的邮件并刷新页面
function EmailManager:DeleteOneEmail(emailId)
    self:DeleteEmail(emailId)
    --通知uimail刷新
    Event:Invoke("UIRefresh",true)

end

---@func 删除指定list内的所有邮件
function EmailManager:DeleteAllEmail(emailIdList)
    for i = 1, #emailIdList do
        self:DeleteEmail(emailIdList[i])
    end
    --通知uimail刷新
    Event:Invoke("UIRefresh",true)
end

---@func 按优先级删除指定列表中的邮件
function EmailManager:DeleteOvetLimitEmail(list)
    local deleteEmailId = -1
    deleteEmailId = self:GetLessTimeEmailId(list)
    if deleteEmailId > 0 then
        self:DeleteOneEmail(deleteEmailId)
        return true
    end
    return false
end

---@func 插入一封新邮件到指定列表A
function EmailManager:ListAddEmail(list,newEmailInfo)
    local len = #list
    local pos = len
    for i = len, 1, -1 do
        local oldEmailInfo = self.EmailInfoDir[list[i]]
        pos = i
        if self:CompareSendTime(newEmailInfo,oldEmailInfo) == false then
            table.insert(list,pos,newEmailInfo.emailId)
            return
        end
    end
    table.insert(list,1,newEmailInfo.emailId)
end

---@func 比较两封邮件的发送时间
function EmailManager:CompareSendTime(newEmailInfo,oldEmailInfo)
    return newEmailInfo.emailSendTime >= oldEmailInfo.emailSendTime
end

---@func 接受C#侧Tester发来的新数据
function EmailManager:GetNewEmail(email)
    local emailInfo = EmailInfo.New(email)
    self:SetNewEmail(emailInfo)

    --使用事件系统通知UIEmail重绘
    Event:Invoke("UIRefresh",true)
end

---@func 通知C#侧服务端删除过期数据
function EmailManager:DeleteEmailOnTester(emailId)
    EmailTester.Instance.emailInfos:Remove(self.EmailInfoDir[emailId])
end

---@func 获取邮件道具
function EmailManager:GetItem(emailId)
    local emailInfo = self.EmailInfoDir[emailId];
    local res = false;
    --等待道具相关接口提供获取成功
    if true then
        res =  self:ChangeEmailStatus(emailInfo, EmailEvent.EmailGetItem);
    end
        
    --通知ui刷新
    Event:Invoke("UIRefresh",false)

    return res;
end

---@func 根据输入事件改编邮件状态
function EmailManager:ChangeEmailStatus(emailInfo,event)
    if emailInfo ~= nil then
        if event == EmailEvent.EmailCheck then
            if emailInfo.emailStatus == EmailStatus.haveCheck then
                ListRemoveValue(self.NewEmailWithoutItem,emailInfo.emailId)
                self:ListAddEmail(self.OldEmailWithoutItem, emailInfo)
                return self:ChangeEmailInfoStatus(emailInfo)
            end
            if emailInfo.emailStatus == EmailStatus.Finish then
                return self:ChangeEmailInfoStatus(emailInfo)
            end
        end
        
        if emailInfo.emailStatus == EmailStatus.haveItem and event == EmailEvent.EmailGetItem then
            ListRemoveValue(self.NewEmailWithItem,emailInfo.emailId)
            self:ListAddEmail(self.OldEmailWithItem, emailInfo)
            return self:ChangeEmailInfoStatus(emailInfo)
        end
    end
    return false
end

---@func 判断指定邮件是否满足状态改变
function EmailManager:ChangeEmailInfoStatus(emailInfo)
    local changeRes = emailInfo:ChangeStatus()
    if not changeRes then
        self:DeleteOneEmail(emailInfo.emailId)
    end
    return changeRes
end


return EmailManager