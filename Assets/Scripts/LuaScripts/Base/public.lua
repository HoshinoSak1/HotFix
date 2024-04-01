UnityEngine = CS.UnityEngine
System = CS.System
DateTime = System.DateTime
EmailTester = CS.EmailTester
UIRoot = UnityEngine.GameObject.Find("UI").transform

---@移除指定list的指定value
function ListRemoveValue(list,value)
    for key, v in pairs(list) do
        if v == value then
            table.remove(list,key)
            return
        end
    end
end
---@移除指定dir的指定dir
function DirRemovekey(dir,key)
    rawset(dir,key,nil)
end