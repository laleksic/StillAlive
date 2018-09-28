// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Engine.IRogueUI
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System.Drawing;
using System.Windows.Forms;

namespace djack.RogueSurvivor.Engine
{
  internal interface IRogueUI
  {
    KeyEventArgs UI_WaitKey();

    KeyEventArgs UI_PeekKey();

    void UI_PostKey(KeyEventArgs e);

    Point UI_GetMousePosition();

    MouseButtons? UI_PeekMouseButtons();

    void UI_PostMouseButtons(MouseButtons buttons);

    void UI_SetCursor(Cursor cursor);

    void UI_Wait(int msecs);

    void UI_Repaint();

    void UI_Clear(Color clearColor);

    void UI_DrawImage(string imageID, int gx, int gy);

    void UI_DrawImage(string imageID, int gx, int gy, Color tint);

    void UI_DrawImageTransform(string imageID, int gx, int gy, float rotation, float scale);

    void UI_DrawGrayLevelImage(string imageID, int gx, int gy);

    void UI_DrawTransparentImage(float alpha, string imageID, int gx, int gy);

    void UI_DrawPoint(Color color, int gx, int gy);

    void UI_DrawLine(Color color, int gxFrom, int gyFrom, int gxTo, int gyTo);

    void UI_DrawRect(Color color, Rectangle rect);

    void UI_FillRect(Color color, Rectangle rect);

    void UI_DrawString(Color color, string text, int gx, int gy, Color? shadowColor = null);

    void UI_DrawStringBold(Color color, string text, int gx, int gy, Color? shadowColor = null);

    void UI_DrawPopup(string[] lines, Color textColor, Color boxBorderColor, Color boxFillColor, int gx, int gy);

    void UI_ClearMinimap(Color color);

    void UI_SetMinimapColor(int x, int y, Color color);

    void UI_DrawMinimap(int gx, int gy);

    float UI_GetCanvasScaleX();

    float UI_GetCanvasScaleY();

    string UI_SaveScreenshot(string filePath);

    string UI_ScreenshotExtension();

    void UI_DoQuit();
  }
}
