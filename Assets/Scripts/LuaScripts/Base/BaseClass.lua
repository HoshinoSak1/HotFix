local BaseClass = {}

function  BaseClass.Create(super)
    local class = {}
    class.super = super
    class.base = super
    setmetatable(class,{__index = super})

    class.New = function ( ... )
        local instance = {} 
        setmetatable(instance,{__index = class})

        local Create
        Create = function (c,...)
            if c.super then
                Create(c.super,...)
            end
            if c.__init then
                c.__init(instance,...)
            end
        end
        Create(instance,...)

        function instance:Delete(...)
            local delete
            delete = function (c,...)
                if c.__delete then
                    c:__delete(...)
                end
                if c.super then
                    delete(c.super,...)
                end
            end
            delete(instance,...)
        end
        return instance
    end

    return class
end

return BaseClass