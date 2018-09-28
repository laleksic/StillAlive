// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.CSVTable
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;
using System.Collections.Generic;

namespace djack.RogueSurvivor.Engine
{
  public class CSVTable
  {
    private int m_nbFields;
    private List<CSVLine> m_Lines;

    public CSVField this[int field, int line]
    {
      get
      {
        return this.m_Lines[line][field];
      }
    }

    public IEnumerable<CSVLine> Lines
    {
      get
      {
        return (IEnumerable<CSVLine>) this.m_Lines;
      }
    }

    public int CountLines
    {
      get
      {
        return this.m_Lines.Count;
      }
    }

    public CSVTable(int nbFields)
    {
      this.m_nbFields = nbFields;
      this.m_Lines = new List<CSVLine>();
    }

    public void AddLine(CSVLine line)
    {
      if (line.FieldsCount != this.m_nbFields)
        throw new ArgumentException(string.Format("line fields count {0} does not match with table fields count {1}", (object) line.FieldsCount, (object) this.m_nbFields));
      this.m_Lines.Add(line);
    }
  }
}
