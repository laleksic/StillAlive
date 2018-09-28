// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.ISoundManager
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;

namespace djack.RogueSurvivor.Engine
{
  internal interface ISoundManager : IDisposable
  {
    bool IsMusicEnabled { get; set; }

    int Volume { get; set; }

    bool Load(string musicname, string filename);

    void Unload(string musicname);

    void Play(string musicname);

    void PlayIfNotAlreadyPlaying(string musicname);

    void PlayLooping(string musicname);

    void ResumeLooping(string musicname);

    void Stop(string musicname);

    void StopAll();

    bool IsPlaying(string musicname);

    bool IsPaused(string musicname);

    bool HasEnded(string musicname);
  }
}
