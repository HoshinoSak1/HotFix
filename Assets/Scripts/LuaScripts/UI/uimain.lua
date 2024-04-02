local UIMain = BaseClass.Create(UIBase)

function UIMain:SetComponent()

    self:GetUIComponent("EmailButton","Button")

    self:AddButtonClickListener("EmailButton",UIMain.OpenUIMail)
end

function UIMain.OpenUIMail()
    print("ClickOpenUIMail")
    local uiEmail = UIEmail.New()
    uiEmail:OpenUI("UIEmail")
    print("OpenUIMail")
    uiEmail = nil
end



return UIMain