local EmailItemListView = BaseClass.Create()

function EmailItemListView:__init()
    self.nowSelected = nil
    self.selectedId = -1
    self.items = {}
end

function EmailItemListView:Reset()
    self.nowSelected = nil
    self.selectedId = -1
    self.items = {}
end

-- 初始化 EmailItem
function EmailItemListView:InitItem(item, emailInfo)
    item:Init(emailInfo)
    item:SetOwner(self)

    self:CheckSelectedItem(item)

    if self.selectedId ~= -1 and item.emailId ~= self.selectedId then
        item:SetSelected(false)
    else
        item:SetSelected(true)
    end
end

-- 设置选中的邮件项
function EmailItemListView:SetSelectedItem(item)
    local emailInfo = EmailManager.EmailInfoDir[item.emailId]

    if self.nowSelected ~= nil then
        self.nowSelected:SetSelected(false)
    end

    self.nowSelected = item
    self.selectedId = item.emailId

    if EmailManager:ChangeEmailStatus(emailInfo, EmailEvent.EmailCheck) then
        item:Init(emailInfo)
    end

    item:SetSelected(true)

    -- 更新选中的邮件信息主体
    Event:Invoke("UIIEmailInfoRefresh")
end

-- 检查选中的邮件项
function EmailItemListView:CheckSelectedItem(item)
    if self.nowSelected == nil and self.selectedId == -1 then
        self:SetSelectedItem(item)
    end
end

-- 设置邮件列表顺序
function EmailItemListView:SetOrderList(id)
    table.insert(self.items, id)
end

-- 根据 ID 获取在列表中的位置
function EmailItemListView:GetPosFromID(id)
    for i = 1, #self.items do
        if self.items[i] == id then
            return i
        end
    end
    return -1
end

-- 从列表中移除指定 ID
function EmailItemListView:RemoveIDfromList(id)
    for i = #self.items, 1, -1 do
        if self.items[i] == id then
            table.remove(self.items, i)
            break
        end
    end
end

-- 设置 EmailItem
function EmailItemListView:EmailItemSet(uIEmailItem, emailPos)
    local emailId = self.items[emailPos + 1]
    local emailInfo = EmailManager.EmailInfoDir[emailId]
    self:InitItem(uIEmailItem, emailInfo)
end


return EmailItemListView