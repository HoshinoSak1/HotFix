local Event = {
    eventList = { eventName = { callback1 ,{ name =  c2} , c3 } }
}

function Event:AddEvent(eventName,callback)
    if self.eventList[eventName] then
        table.insert(self.eventList[eventName],callback)
    else
        local callbackTable = {}
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
    
    if not callbackTable then
        return
    end

    for __, callback in ipairs(callbackTable) do
        callback(...)
    end
end

return Event