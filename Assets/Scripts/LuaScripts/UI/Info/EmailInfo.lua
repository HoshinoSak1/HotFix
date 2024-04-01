EmailStatus = {
    Finish = 0,
    haveCheck = 1,
    haveItem = 2
}

local EmailInfo = BaseClass.Create()

function EmailInfo:__init(email)
    self:init(  email.emailId,email.conditionId,email.emailTitle,email.emailSender,
                email.emailBody,email.emailSendTime,email.lostTimer,email.itemIdLis,email.emailStatus)
end


function EmailInfo:init( emailId, conditionId, emailTitle, emailSender, emailBody, emailSendTime, lostTimer, itemIdLis, status )
    self.emailId = emailId
    self.conditionId = conditionId
    self.emailTitle = emailTitle
    self.emailSender = emailSender
    self.emailBody = emailBody
    self.emailSendTime = self:TimeTranslate(emailSendTime)
    self.lostTimer = lostTimer
    self.emailStatus = status

    self.itemIdLis = {}
    -- for itemId in itemIdLis:gmatch("[^|]+") do
    --     table.insert(itemIdLis, itemId)
    -- end
    for i = 0, itemIdLis.Count - 1 do
        table.insert(self.itemIdLis, itemIdLis[i])
    end

    self.hasItem = #self.itemIdLis ~= 0 

end

function EmailInfo:TimeTranslate(emailSendTime)

    local timeTable = {}
    
    timeTable["year"] = emailSendTime.Year;
    timeTable["month"] = emailSendTime.Month;
    timeTable["day"] = emailSendTime.Day;
    timeTable["hour"] = emailSendTime.Hour;
    timeTable["min"] = emailSendTime.Minute;
    timeTable["sec"] = emailSendTime.Second;

    local translatedTime = os.time(timeTable)

    timeTable = nil

    return translatedTime
end

function EmailInfo:ChangeStatus()
    self.emailStatus = EmailStatus.Finish
    return self:isOverTime()
end

function EmailInfo:isOverTime()
    local currentTime = os.time()
    local timeSpan = currentTime - self.emailSendTime
    local lastTime = self.lostTimer - timeSpan

    if lastTime <= 0 then
        print(self.emailId.." ["..self.emailTitle.."] : ".."邮件已过期")
        return false
    end

    return true
end

return EmailInfo
