// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.UI.IGameCanvas
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System.Drawing;

namespace djack.RogueSurvivor.UI
{
  public interface IGameCanvas
  {
    bool ShowFPS { get; set; }

    bool NeedRedraw { get; set; }

    Point MouseLocation { get; set; }

    float ScaleX { get; }

    float ScaleY { get; }

    void BindForm(RogueForm form);

    void FillGameForm();

    void Clear(Color clearColor);

    void AddImage(Image img, int x, int y);

    void AddImage(Image img, int x, int y, Color tint);

    void AddImageTransform(Image img, int x, int y, float rotation, float scale);

    void AddTransparentImage(float alpha, Image img, int x, int y);

    void AddPoint(Color color, int x, int y);

    void AddLine(Color color, int xFrom, int yFrom, int xTo, int yTo);

    void AddRect(Color color, Rectangle rect);

    void AddFilledRect(Color color, Rectangle rect);

    void AddString(Font font, Color color, string text, int gx, int gy);

    void ClearMinimap(Color color);

    void SetMinimapColor(int x, int y, Color color);

    void DrawMinimap(int gx, int gy);

    string SaveScreenShot(string filePath);

    string ScreenshotExtension();

    void DisposeUnmanagedResources();
  }
}
