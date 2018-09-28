﻿// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Logger
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace djack.RogueSurvivor
{
  internal static class Logger
  {
    private static List<string> s_Lines = new List<string>();
    private static object s_Mutex = new object();

    public static IEnumerable<string> Lines
    {
      get
      {
        return (IEnumerable<string>) Logger.s_Lines;
      }
    }

    public static void Clear()
    {
      Monitor.Enter(Logger.s_Mutex);
      Logger.s_Lines.Clear();
      Monitor.Exit(Logger.s_Mutex);
    }

    public static void CreateFile()
    {
      Monitor.Enter(Logger.s_Mutex);
      if (File.Exists(Logger.LogFilePath()))
        File.Delete(Logger.LogFilePath());
      Directory.CreateDirectory(SetupConfig.DirPath);
      using (StreamWriter text = File.CreateText(Logger.LogFilePath()))
        text.Close();
      Monitor.Exit(Logger.s_Mutex);
    }

    public static void WriteLine(Logger.Stage stage, string text)
    {
      Monitor.Enter(Logger.s_Mutex);
      string str = string.Format("{0} {1} : {2}", (object) Logger.s_Lines.Count, (object) Logger.StageToString(stage), (object) text);
      Logger.s_Lines.Add(str);
      Console.Out.WriteLine(str);
      using (StreamWriter streamWriter = File.AppendText(Logger.LogFilePath()))
      {
        streamWriter.WriteLine(str);
        streamWriter.Flush();
        streamWriter.Close();
      }
      Monitor.Exit(Logger.s_Mutex);
    }

    private static string LogFilePath()
    {
      return SetupConfig.DirPath + "\\log.txt";
    }

    private static string StageToString(Logger.Stage s)
    {
      switch (s)
      {
        case Logger.Stage.INIT_MAIN:
          return "init main";
        case Logger.Stage.RUN_MAIN:
          return "run main";
        case Logger.Stage.CLEAN_MAIN:
          return "clean main";
        case Logger.Stage.INIT_GFX:
          return "init gfx";
        case Logger.Stage.RUN_GFX:
          return "run gfx";
        case Logger.Stage.CLEAN_GFX:
          return "clean gfx";
        case Logger.Stage.INIT_SOUND:
          return "init sound";
        case Logger.Stage.RUN_SOUND:
          return "run sound";
        case Logger.Stage.CLEAN_SOUND:
          return "clean sound";
        default:
          return "misc";
      }
    }

    public enum Stage
    {
      INIT_MAIN,
      RUN_MAIN,
      CLEAN_MAIN,
      INIT_GFX,
      RUN_GFX,
      CLEAN_GFX,
      INIT_SOUND,
      RUN_SOUND,
      CLEAN_SOUND,
    }
  }
}
