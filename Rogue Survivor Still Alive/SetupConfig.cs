// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.SetupConfig
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;
using System.IO;
using System.Windows.Forms;

namespace djack.RogueSurvivor
{
  public static class SetupConfig
  {
    public static string GAME_VERSION = "Still Alive " + Application.ProductVersion;

    public static SetupConfig.eVideo Video { get; set; }

    public static SetupConfig.eSound Sound { get; set; }

    public static string DirPath
    {
      get
      {
        return Environment.CurrentDirectory + "\\Config\\";
      }
    }

    private static string FilePath
    {
      get
      {
        return SetupConfig.DirPath + "\\setup.dat";
      }
    }

    public static void Save()
    {
      using (StreamWriter text = File.CreateText(SetupConfig.FilePath))
      {
        text.WriteLine(SetupConfig.toString(SetupConfig.Video));
        text.WriteLine(SetupConfig.toString(SetupConfig.Sound));
      }
    }

    public static void Load()
    {
      if (File.Exists(SetupConfig.FilePath))
      {
        using (StreamReader streamReader = File.OpenText(SetupConfig.FilePath))
        {
          SetupConfig.Video = SetupConfig.toVideo(streamReader.ReadLine());
          SetupConfig.Sound = SetupConfig.toSound(streamReader.ReadLine());
        }
      }
      else
      {
        if (!Directory.Exists(SetupConfig.DirPath))
          Directory.CreateDirectory(SetupConfig.DirPath);
        SetupConfig.Video = SetupConfig.eVideo.VIDEO_MANAGED_DIRECTX;
        SetupConfig.Sound = SetupConfig.eSound.SOUND_MANAGED_DIRECTX;
        SetupConfig.Save();
      }
    }

    public static string toString(SetupConfig.eVideo v)
    {
      return v.ToString();
    }

    public static string toString(SetupConfig.eSound s)
    {
      return s.ToString();
    }

    public static SetupConfig.eVideo toVideo(string s)
    {
      if (s == SetupConfig.eVideo.VIDEO_MANAGED_DIRECTX.ToString())
        return SetupConfig.eVideo.VIDEO_MANAGED_DIRECTX;
      return s == SetupConfig.eVideo.VIDEO_GDI_PLUS.ToString() ? SetupConfig.eVideo.VIDEO_GDI_PLUS : SetupConfig.eVideo.VIDEO_INVALID;
    }

    public static SetupConfig.eSound toSound(string s)
    {
      if (s == SetupConfig.eSound.SOUND_MANAGED_DIRECTX.ToString())
        return SetupConfig.eSound.SOUND_MANAGED_DIRECTX;
      if (s == SetupConfig.eSound.SOUND_SFML.ToString())
        return SetupConfig.eSound.SOUND_SFML;
      return s == SetupConfig.eSound.SOUND_NOSOUND.ToString() ? SetupConfig.eSound.SOUND_NOSOUND : SetupConfig.eSound.SOUND_INVALID;
    }

    public enum eVideo
    {
      VIDEO_INVALID,
      VIDEO_MANAGED_DIRECTX,
      VIDEO_GDI_PLUS,
      _COUNT,
    }

    public enum eSound
    {
      SOUND_INVALID,
      SOUND_MANAGED_DIRECTX,
      SOUND_SFML,
      SOUND_NOSOUND,
      _COUNT,
    }
  }
}
