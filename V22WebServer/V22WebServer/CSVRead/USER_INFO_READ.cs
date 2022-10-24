using System;
using CsvHelper;
using WebDotNetCore.Table;
using V22WebServer.BasicInterFace;
using V22WebServer.Service.Database;

namespace V22WebServer.CSVRead;

public class USER_INFO_READ : CSVBase
{
    public Int64 user_id;
    public string Nickname;
    public int Token;
    public int PW;
    public DateTime Last_Conn;


    public USER_INFO_READ getUserInfo()
    {
        USER_INFO_READ info = (USER_INFO_READ) this.MemberwiseClone();
        info.Last_Conn = DateTime.Now;
        return this;


    }

    public bool Load(CsvReader obj)
    {
        return Execute(obj, () =>
        {

            Int32 idx = 0;
            var tuple = new USER_INFO_READ();
            tuple.user_id = obj.GetField<Int64>(idx++);
            tuple.Nickname = obj.GetField<string>(idx++);
            tuple.Token = obj.GetField<int>(idx++);
            tuple.PW = obj.GetField<int>(idx++);
            tuple.Last_Conn = obj.GetField<DateTime>(idx++);



            TableMap<Int64, USER_INFO_READ>.GetInstance.InsertRow(tuple.user_id, tuple);
            return true;
        });
    }


    public static TableMap<Int64, USER_INFO_READ> Instance
    {
        get { return TableMap<Int64, USER_INFO_READ>.GetInstance; }
    }
}
//     public async Task<bool> UpdateDB()
//     {
//         const string query = "update V22UserData set" +
//                              "nickname = @nickname, ";
//
//         int result = await getUserInfo();
//     }
// }