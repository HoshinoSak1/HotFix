function LuaReload(path,reloadType)

    print(path .. " : Reloading!!!")

    if reloadType == CS.ReloadType.Create then
        Create(path)
    elseif reloadType == CS.ReloadType.Change then
        Change(path)
    elseif reloadType == CS.ReloadType.Delete then   
        Delete(path)
    end
end

function Create(path)
    local ok,err = pcall(require,path)
    if not ok then
        print("新模块加载失败 : ERROR MESSAGE : "..err)
        return
    end
    print("新模块加载成功")
    local newModel = package.loaded[path]
    newModel:Init()
    return newModel
end

function Change(path)
    print("新模块变更")
    local oldModel = Delete(path)
    local newModel = Create(path)

    if not oldModel or not newModel then
        print("新模块变更失败")
        return
    end

    TableUpdate(newModel,oldModel)

    package.loaded[path] = oldModel
end

function Delete(path)
    local oldModel 
    if package.loaded[path] then
        oldModel = package.loaded[path]
        oldModel:Close()
        package.loaded[path] = nil
    else
        print("不存在此模块！")
        return nil
    end
    print("模块卸载成功")
    return oldModel
end

function TableUpdate(new_table,old_table)
    if new_table == old_table then
        print("实际功能模块无更新，返回")
        return
    end
    assert("table" == type(new_table))
    assert("table" == type(old_table))
    --对比新旧表
    for key,newValue in pairs(new_table) do
        local oldValue = old_table[key]
        --类型无更新，查看是否值更新
        if type(oldValue) == type(newValue) then
            
            --类型为Func，需要处理函数内的upvalue
            if type(newValue) == "function" then
                FuncUpdate(newValue,oldValue)
                old_table[key] = newValue
            --类型为table，需要递归处理
            elseif type(newValue) == "table"  then
                TableUpdate(newValue,oldValue)
            end
        else
            --类型更新，将新的值赋值给旧表
            old_table[key] = newValue
        end
    end

    --更新元表的数据
    local old_meta = getmetatable(old_table)
    local new_meta = getmetatable(new_table)

    if type(old_meta) == "table" and type(new_meta) == "table" then
        TableUpdate(new_meta, old_meta)
    end
end

function FuncUpdate(new_Func,old_Func)
    local old_upvalue_map = {}
    
    --构造原函数的upvalue表
    for i = 1,math.huge do
        local name,value = debug.getupvalue(old_Func,i)
        if not name then
            break
        end
        old_upvalue_map[name] = value
    end

    --更新到新函数的upvalue表
    for i = 1,math.huge do
        local name,value = debug.getupvalue(new_Func,i)
        if not name then
            break
        end

        local oldValue = old_upvalue_map[name]
        if oldValue then
            debug.setupvalue(new_Func,i,oldValue)
        end
    end
end