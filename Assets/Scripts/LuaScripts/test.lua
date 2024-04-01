local test = {
    
}
local function test1 ()
    print("test")
end

function test:test( ... )
    EmailTester.Instance:getNewItem('+',test1)
end
function test:test1( ... )
    EmailTester.Instance:getNewItem('-',test1)
end
return test