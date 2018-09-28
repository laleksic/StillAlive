// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.NullSoundManager
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;

namespace djack.RogueSurvivor.Engine
{
  internal class NullSoundManager : ISoundManager, IDisposable
  {
    public bool IsMusicEnabled { get; set; }

    public int Volume { get; set; }

    public bool Load(string musicname, string filename)
    {
      return true;
    }

    public void Unload(string musicname)
    {
    }

    public void Play(string musicname)
    {
    }

    public void PlayIfNotAlreadyPlaying(string musicname)
    {
    }

    public void PlayLooping(string musicname)
    {
    }

    public void ResumeLooping(string musicname)
    {
    }

    public void Stop(string musicname)
    {
    }

    public void StopAll()
    {
    }

    public bool IsPlaying(string musicname)
    {
      return false;
    }

    public bool IsPaused(string musicname)
    {
      return false;
    }

    public bool HasEnded(string musicname)
    {
      return true;
    }

    public void Dispose()
    {
    }
  }
}
