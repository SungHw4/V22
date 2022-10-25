namespace V22WebServer;

public enum ErrorCode : int
{
    None = 0,
    
    Create_Account_Fail_Duplicate = 11,
    Create_Account_Fail_Exception = 12,
    
    Login_Fail_NotUser = 16,
    Login_Fail_PW = 17,
    Login_Fail_Update = 18,
    Get_PlayerInfo_Fail = 19
}