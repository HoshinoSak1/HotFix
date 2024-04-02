local UIEmailInfo = BaseClass.Create(UIBase)

function UIEmailInfo:__init(prefab)
    self.emailId = -1
    self.go = prefab
end

function UIEmailInfo:SetComponent()

    self.uiEmailItem = self.go:GetComponent("UIEmailItem")

    self.uiEmailItem.onPointerDown = function() 
                                        self:OnPointDown() 
                                    end                         
end

-- 初始化
function UIEmailInfo:Init(emailInfo)
    self:NormalInfoSet(emailInfo)
    self:RedPointSet(emailInfo)
    self:SetCompleteItem(emailInfo)
end

-- 设置所有者
function UIEmailInfo:SetOwner(owner)
    self.owner = owner
end

-- 设置常态显示要素
function UIEmailInfo:NormalInfoSet(emailInfo)
    self.emailId = emailInfo.emailId
    if self.uiEmailItem.emailTitle then
        self.uiEmailItem.emailTitle.text = self:SetEmailTitle(emailInfo.emailTitle)
    end
    if self.uiEmailItem.emailSender then
        self.uiEmailItem.emailSender.text = emailInfo.emailSender
    end
    if self.uiEmailItem.emailTimer then
        self.uiEmailItem.emailTimer.text = self:SetTime(emailInfo.lostTimer, emailInfo.emailSendTime)
    end

    self:SetItemIcon(emailInfo)
    self:RedPointSet(emailInfo)
    self:SetCompleteItem(emailInfo)

    self:SetSelected(false)
end

-- 设置图标
function UIEmailInfo:SetItemIcon(emailInfo)
    if #emailInfo.itemIdLis > 0 then
        self.uiEmailItem.itemIcon.sprite = self.uiEmailItem.itemSprite
        -- 通过其他系统更新应该显示的道具图标
        -- self.itemIcon.sprite = ItemManager.GetItemIconFromId(emailInfo.itemIdLis[1])
    else
        self.uiEmailItem.itemIcon.sprite = self.uiEmailItem.emailSprite
    end
end

-- 设置红点
function UIEmailInfo:RedPointSet(emailInfo)
    self.uiEmailItem.redPoint.gameObject:SetActive(emailInfo.emailStatus ~= EmailStatus.Finish)
end

-- 设置透明化
function UIEmailInfo:SetCompleteItem(emailInfo)
    self.uiEmailItem.CompletedCanvas.alpha = emailInfo.emailStatus == EmailStatus.Finish and 0.5 or 1
end

-- 截断 EmailTitle
function UIEmailInfo:SetEmailTitle(emailTitle)
    return string.len(emailTitle) >= 12 and string.sub(emailTitle, 1, 10) .. "..." or emailTitle
end

-- 设置邮件剩余时间
function UIEmailInfo:SetTime( lostTimer, time)
    local clientTime = os.time()
    local ts = clientTime - time
    local lastTime = lostTimer - ts

    -- 过期
    if lastTime < 0 then
        return "已过期"
    end

    -- 小于1分钟
    if lastTime < 60 then
        return "小于1分钟"
    end

    -- 分钟
    if lastTime < 3600 then
        return string.format("%d 分", math.floor(lastTime / 60))
    end

    -- 小时
    if lastTime < 86400 then
        return string.format("%d 小时", math.floor(lastTime / 3600))
    end

    -- 天
    if lastTime < 2592000 then -- 大约30天
        return string.format("%d 天", math.floor(lastTime / 86400))
    end

    return tostring(lastTime)
end

-- 设置选中背景
function UIEmailInfo:SetSelected(selectedTrigger)
    self.uiEmailItem.Selected.gameObject:SetActive(selectedTrigger)
end

function UIEmailInfo:OnPointDown()
    self.owner:SetSelectedItem(self)
end

function UIEmailInfo:__delete()
    --self.uiEmailItem.onPointerDown = nil
end

return UIEmailInfo