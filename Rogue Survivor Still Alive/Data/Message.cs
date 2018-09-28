// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Data.Message
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;
using System.Drawing;

namespace djack.RogueSurvivor.Data
{
  internal class Message
  {
    private string m_Text;
    private Color m_Color;
    private readonly int m_Turn;

    public string Text
    {
      get
      {
        return this.m_Text;
      }
      set
      {
        this.m_Text = value;
      }
    }

    public Color Color
    {
      get
      {
        return this.m_Color;
      }
      set
      {
        this.m_Color = value;
      }
    }

    public int Turn
    {
      get
      {
        return this.m_Turn;
      }
    }

    public Message(string text, int turn, Color color)
    {
      if (text == null)
        throw new ArgumentNullException(nameof (text));
      this.m_Text = text;
      this.m_Color = color;
      this.m_Turn = turn;
    }

    public Message(string text, int turn)
      : this(text, turn, Color.White)
    {
    }
  }
}
