// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.CSVParser
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System.Collections.Generic;
using System.Text;

namespace djack.RogueSurvivor.Engine
{
  public class CSVParser
  {
    private char m_Delimiter;

    public char Delimiter
    {
      get
      {
        return this.m_Delimiter;
      }
      set
      {
        this.m_Delimiter = value;
      }
    }

    public CSVParser()
    {
      this.m_Delimiter = ',';
    }

    public string[] Parse(string line)
    {
      if (line == null)
        return new string[0];
      line = line.TrimEnd();
      List<string> stringList = new List<string>((IEnumerable<string>) line.Split(this.m_Delimiter));
      int index1 = 0;
      do
      {
        string str1 = stringList[index1];
        if (str1[0] == '"' && str1[str1.Length - 1] != '"')
        {
          string str2 = str1;
          int index2 = index1 + 1;
          while (index2 < stringList.Count)
          {
            string str3 = stringList[index2];
            str2 = str2 + "," + str3;
            stringList.RemoveAt(index2);
            if (str3[str3.Length - 1] == '"')
              break;
          }
          stringList[index1] = str2;
        }
        else
          ++index1;
      }
      while (index1 < stringList.Count - 1);
      return stringList.ToArray();
    }

    public List<string[]> Parse(string[] lines)
    {
      List<string[]> strArrayList = new List<string[]>(1);
      if (lines == null)
        return strArrayList;
      foreach (string line in lines)
        strArrayList.Add(this.Parse(line));
      return strArrayList;
    }

    public CSVTable ParseToTable(string[] lines, int nbFields)
    {
      CSVTable csvTable = new CSVTable(nbFields);
      foreach (string[] strArray in this.Parse(lines))
      {
        CSVLine line = new CSVLine(strArray.Length);
        for (int index = 0; index < line.FieldsCount; ++index)
          line[index] = new CSVField(strArray[index]);
        csvTable.AddLine(line);
      }
      return csvTable;
    }

    public string Format(string[] fields)
    {
      if (fields == null)
        return string.Format("{0}", (object) this.m_Delimiter);
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string field in fields)
      {
        stringBuilder.Append(field);
        stringBuilder.Append(this.m_Delimiter);
      }
      return stringBuilder.ToString();
    }
  }
}
