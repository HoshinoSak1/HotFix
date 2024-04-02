local test = {
    
}
local function test1 ()
    print("test")
end

function test:test( ... )
    print("test2")
end
function test:test1( ... )
    
end
return test