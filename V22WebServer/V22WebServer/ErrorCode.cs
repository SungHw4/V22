namespace V22WebServer;

public enum ErrorCode : int
{
    None = 0,
    
    Create_Account_Fail_Duplicate = 11,
    Create_Account_Fail_Exception = 12,
    
    Login_Fail_NotUser = 16,
    Login_Fail_PW = 17,
    Login_Fail_Update = 18,
    Login_Fail_Update_Inventory = 19,
    
    GET_Inventory_Create = 20,
    GET_Inventory_Fail = 21
}