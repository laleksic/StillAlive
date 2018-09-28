// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.CSVField
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

namespace djack.RogueSurvivor.Engine
{
  public class CSVField
  {
    private string m_RawString;

    public CSVField(string rawString)
    {
      this.m_RawString = rawString;
    }

    public int ParseInt()
    {
      return int.Parse(this.m_RawString);
    }

    public float ParseFloat()
    {
      return float.Parse(this.m_RawString);
    }

    public string ParseText()
    {
      return this.m_RawString.Trim('"');
    }

    public bool ParseBool()
    {
      return this.ParseInt() > 0;
    }
  }
}
