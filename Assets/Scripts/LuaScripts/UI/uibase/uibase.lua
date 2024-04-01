local UIBase = BaseClass.Create()

UIBase.componentMap = {}
UIBase.listener = {}

function UIBase:OpenUI(uiName)
    self:Show(uiName)
    self:SetComponent()
end

function UIBase:SetComponent()
    
end

function UIBase:Show(uiName)
    self.go = UnityEngine.GameObject.Find(uiName)
    if self.go == nil then
        print("gameobject 未加载")

        local prefab = UnityEngine.Resources.Load(string.format("UIPrefabs/%s/%s",uiName,uiName))

        if prefab ~= nil then
            self.go = UnityEngine.GameObject.Instantiate(prefab,UIRoot)
            self.go.name = uiName
        else
            print("Prefab not found: PrefabName")
        end
    end
    self.transform = self.go:GetComponent("Transform")
    self.go:SetActive(true)
end

function UIBase:__delete()
    self:Close()
end

function UIBase:Close()
    self:removeButtonClickListener()
    UnityEngine.Object.Destroy(self.go)
    self.go = nil
end

function UIBase:GetUIComponent(ComponentName,type)
    local Component = nil
    --如果有缓存 取缓存
    if self.componentMap and self.componentMap[ComponentName] then
        Component = self.componentMap[ComponentName]
    --没有缓存 从go取
    else
        local ts = UnityEngine.GameObject.Find(ComponentName)
        Component = ts:GetComponent(type)
        if Component == nil then
            print("Component not found" .. ComponentName)
        else
            self.componentMap[ComponentName] = Component
        end
    end
    return Component
end

function UIBase:AddButtonClickListener(buttonName,callback)
    local button = self:GetUIComponent(buttonName)
    if button then
        button.onClick:AddListener(callback)

        table.insert(self.listener,button)
    else
        print("Button not found: " .. buttonName)
    end
end

function UIBase:removeButtonClickListener()
    for _, button in pairs(self.listener) do
        button.onClick:RemoveAllListeners()
    end
end

return UIBase