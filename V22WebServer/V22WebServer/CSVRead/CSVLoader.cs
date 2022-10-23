using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CsvHelper;


namespace V22WebServer.CSVRead;

public class CSVLoader
{
    private Dictionary<string, CSVBase> RedisDic;

    public void CsvTableLoader()
    {
        RedisDic = new Dictionary<string, CSVBase>();
        Regist("", new USER_INFO_READ());
        
    }
    public bool Regist(string filepath, CSVBase tbl)
    {
        if (filepath == null || tbl == null)
        {
            // 에러
            return false;
        }

        if (RedisDic.ContainsKey(filepath))
        {
            // 에러
            return false;
        }

        RedisDic[filepath] = tbl;

        return true;
    }
    
    public bool Load()
    {
        string currentPos = "Empty";
        try
        {
            foreach (var pair in RedisDic)
            {
                TextReader readFile = new StreamReader(pair.Key);
                var csv = new CsvReader(readFile,System.Globalization.CultureInfo.CurrentCulture, false);
                if (csv == null)
                {
                    //throw new CustomException("failed to make CsvReader.");
                }
                currentPos = pair.Key;
                pair.Value.Load(csv);
            }

            return true;
        }

        catch (Exception e)
        {
            Console.WriteLine(currentPos);
            Console.WriteLine(e);
        }

        return false;
    }
    
}