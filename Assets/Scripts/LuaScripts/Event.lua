local Event = {
    eventList = {}
}

function Event:AddEvent(eventName,callback)
    if self.eventList[eventName] then
        table.insert(self.eventList[eventName],callback)
    else
        local callbackTable
        table.insert(callbackTable,callback)
        self.eventList[eventName] = callbackTable
    end
end

function Event:RemoveEvent(eventName,callback)
    local callbackTable = self.eventList[eventName]
    for pos, callbackItem in ipairs(callbackTable) do
        if callbackItem == callback then
            table.remove(callbackTable,pos)
        end
    end
end

function Event:Invoke(eventName,...)
    local callbackTable = self.eventList[eventName]
    for __, callback in ipairs(callbackTable) do
        callback(eventName,...)
    end
end

return Event