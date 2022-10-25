using System;
using CsvHelper;
using WebDotNetCore.Table;
using V22WebServer.BasicInterFace;
using V22WebServer.Service.Database;

namespace V22WebServer.CSVRead;

public class USER_INFO_READ : CSVBase
{
    public string ID;
    public string Nickname;
    public string AuthToken;
    public string Salt;
    public string PW;
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
            tuple.ID = obj.GetField<string>(idx++);
            tuple.Nickname = obj.GetField<string>(idx++);
            tuple.AuthToken = obj.GetField<string>(idx++);
            tuple.Salt = obj.GetField<string>(idx++);
            tuple.PW = obj.GetField<string>(idx++);
            tuple.Last_Conn = obj.GetField<DateTime>(idx++);



            TableMap<string, USER_INFO_READ>.GetInstance.InsertRow(tuple.ID, tuple);
            return true;
        });
    }


    public static TableMap<string, USER_INFO_READ> Instance
    {
        get { return TableMap<string, USER_INFO_READ>.GetInstance; }
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