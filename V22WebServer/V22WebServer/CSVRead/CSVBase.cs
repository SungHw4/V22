using System;
using System.Collections.Generic;
using CsvHelper;


namespace V22WebServer.CSVRead;

public class CSVBase
{
    public delegate bool Func();

    public bool Execute(CsvReader csv, Func func)
    {
        Int32 count = 0;

        while (csv.Read())
        {
            
            if (count == 0)
                csv.GetRecord<dynamic>();
           
            if (!func()) return false;
          
            ++count;
        }

        return true;
    }
    
    public virtual bool Load(CsvReader obj)
    {
        return false;
    }
}