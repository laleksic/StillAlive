// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.CSVLine
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

namespace djack.RogueSurvivor.Engine
{
  public class CSVLine
  {
    private CSVField[] m_Fields;

    public CSVField this[int field]
    {
      get
      {
        return this.m_Fields[field];
      }
      set
      {
        this.m_Fields[field] = value;
      }
    }

    public int FieldsCount
    {
      get
      {
        return this.m_Fields.Length;
      }
    }

    public CSVLine(int nbFields)
    {
      this.m_Fields = new CSVField[nbFields];
    }
  }
}
