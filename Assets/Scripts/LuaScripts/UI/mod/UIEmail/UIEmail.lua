local UIEmail = BaseClass.Create(UIBase)

function UIEmail:SetComponent()

    self.uiEmail = self:GetUIComponent("UIEmail","UIEmail")

    self.DltAllReadBtn = self:GetUIComponent("DltAllReadBtn","Button")
    self.GetAllItemBtn = self:GetUIComponent("GetAllItemBtn","Button")
    self.DeleteBtn = self:GetUIComponent("DeleteBtn","Button")
    self.GetItemBtn = self:GetUIComponent("GetItemBtn","Button")
    self.BackButton = self:GetUIComponent("BackButton","Button")

    self:AddButtonClickListener("DltAllReadBtn",self.DeleteAllEmail)
    self:AddButtonClickListener("GetAllItemBtn",self.GetAllItem)
    self:AddButtonClickListener("DeleteBtn",self.DeleteEmail)
    self:AddButtonClickListener("GetItemBtn",self.GetItem)
    self:AddButtonClickListener("BackButton",self.CloseUIEmail)

end

function UIEmail:Init()
    
end

function UIEmail:ViewReset()
    
end

function UIEmail:UIRefresh(needReset)
    if needReset then
       self:ViewReset() 
    end
    self.uiEmail.emailList.Refresh(needReset)
end

function UIEmail:DeleteAllEmail()
    
end

function UIEmail:GetAllItem()
    
end


function UIEmail:DeleteEmail()
    
end

function UIEmail:GetItem()
    
end

function UIEmail:CloseUIEmail()
    self:Delete()
end

return UIEmail