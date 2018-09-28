﻿// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.TextFile
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;
using System.Collections.Generic;
using System.IO;

namespace djack.RogueSurvivor.Engine
{
  internal class TextFile
  {
    private List<string> m_RawLines;
    private List<string> m_FormatedLines;

    public IEnumerable<string> RawLines
    {
      get
      {
        return (IEnumerable<string>) this.m_RawLines;
      }
    }

    public List<string> FormatedLines
    {
      get
      {
        return this.m_FormatedLines;
      }
    }

    public TextFile()
    {
      this.m_RawLines = new List<string>();
    }

    public bool Load(string fileName)
    {
      try
      {
        Logger.WriteLine(Logger.Stage.RUN_MAIN, string.Format("Loading text file {0}...", (object) fileName));
        StreamReader streamReader = File.OpenText(fileName);
        this.m_RawLines = new List<string>();
        while (!streamReader.EndOfStream)
          this.m_RawLines.Add(streamReader.ReadLine());
        streamReader.Close();
        Logger.WriteLine(Logger.Stage.RUN_MAIN, string.Format("done!", (object) fileName));
        return true;
      }
      catch (Exception ex)
      {
        Logger.WriteLine(Logger.Stage.RUN_MAIN, string.Format("Loading exception: {0}", (object) ex.ToString()));
        return false;
      }
    }

    public bool Save(string fileName)
    {
      try
      {
        Logger.WriteLine(Logger.Stage.RUN_MAIN, string.Format("Saving text file {0}...", (object) fileName));
        File.WriteAllLines(fileName, this.m_RawLines.ToArray());
        Logger.WriteLine(Logger.Stage.RUN_MAIN, string.Format("done!", (object) fileName));
        return true;
      }
      catch (Exception ex)
      {
        Logger.WriteLine(Logger.Stage.RUN_MAIN, string.Format("Saving exception: {0}", (object) ex.ToString()));
        return false;
      }
    }

    public void Append(string line)
    {
      this.m_RawLines.Add(line);
    }

    public void FormatLines(int charsPerLine)
    {
      if (this.m_RawLines == null || this.m_RawLines.Count == 0)
        return;
      this.m_FormatedLines = new List<string>(this.m_RawLines.Count);
      for (int index = 0; index < this.m_RawLines.Count; ++index)
      {
        string str1;
        string str2;
        for (str1 = this.m_RawLines[index]; str1.Length > charsPerLine; str1 = str2)
        {
          string str3 = str1.Substring(0, charsPerLine);
          str2 = str1.Remove(0, charsPerLine);
          this.m_FormatedLines.Add(str3);
        }
        this.m_FormatedLines.Add(str1);
      }
    }
  }
}
