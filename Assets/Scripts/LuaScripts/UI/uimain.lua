local UIMain = BaseClass.Create(UIBase)

function UIMain:SetComponent()
    self:GetUIComponent("EmailButton","Button")

    self:AddButtonClickListener("EmailButton",UIMain.OpenUIMail)
end

function UIMain:OpenUIMail()

    local uiEmail = UIEmail.New()
    uiEmail:OpenUI("UIEmail")
    
end

return UIMain