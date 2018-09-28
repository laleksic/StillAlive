// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.RogueForm
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using djack.RogueSurvivor.Engine;
using djack.RogueSurvivor.Gameplay;
using djack.RogueSurvivor.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace djack.RogueSurvivor
{
  public class RogueForm : Form, IRogueUI
  {
    private RogueGame m_Game;
    private Font m_NormalFont;
    private Font m_BoldFont;
    private const int CP_NOCLOSE_BUTTON = 512;
    private bool m_HasKey;
    private KeyEventArgs m_InKey;
    private bool m_HasMouseButtons;
    private MouseButtons m_MouseButtons;
    private IContainer components;
    private IGameCanvas m_GameCanvas;

    internal RogueGame Game
    {
      get
      {
        return this.m_Game;
      }
    }

    public RogueForm()
    {
      Logger.WriteLine(Logger.Stage.INIT_MAIN, "creating main form...");
      Logger.WriteLine(Logger.Stage.INIT_MAIN, "Form::InitializeComponent...");
      this.InitializeComponent();
      this.Text = "Rogue Survivor - " + SetupConfig.GAME_VERSION;
      if (SetupConfig.Video == SetupConfig.eVideo.VIDEO_GDI_PLUS)
        this.Text += " (GDI+)";
      switch (SetupConfig.Sound)
      {
        case SetupConfig.eSound.SOUND_SFML:
          this.Text += " (sndSFML)";
          break;
        case SetupConfig.eSound.SOUND_NOSOUND:
          this.Text += " (nosound)";
          break;
      }
      Logger.WriteLine(Logger.Stage.INIT_MAIN, "Form::SetClientSizeCore...");
      this.SetClientSizeCore(1024, 768);
      Logger.WriteLine(Logger.Stage.INIT_MAIN, "Form::SetStyle...");
      this.SetStyle(ControlStyles.Opaque | ControlStyles.AllPaintingInWmPaint, true);
      Logger.WriteLine(Logger.Stage.INIT_MAIN, "create font 1...");
      this.m_NormalFont = new Font("Lucida Console", 8.25f, FontStyle.Regular);
      Logger.WriteLine(Logger.Stage.INIT_MAIN, "create font 2...");
      this.m_BoldFont = new Font("Lucida Console", 8.25f, FontStyle.Bold);
      Logger.WriteLine(Logger.Stage.INIT_MAIN, "create RogueGame...");
      this.m_Game = new RogueGame((IRogueUI) this);
      Logger.WriteLine(Logger.Stage.INIT_MAIN, "bind form...");
      this.m_GameCanvas.BindForm(this);
      Logger.WriteLine(Logger.Stage.INIT_MAIN, "creating main form done.");
    }

    private void LoadResources()
    {
      Logger.WriteLine(Logger.Stage.INIT_GFX, "loading images...");
      GameImages.LoadResources((IRogueUI) this);
      Logger.WriteLine(Logger.Stage.INIT_GFX, "loading images done");
    }

    protected override CreateParams CreateParams
    {
      get
      {
        CreateParams createParams = base.CreateParams;
        createParams.ClassStyle |= 512;
        return createParams;
      }
    }

    protected override void OnShown(EventArgs e)
    {
      base.OnShown(e);
      this.LoadResources();
      this.m_Game.Run();
    }

    protected override void OnSizeChanged(EventArgs e)
    {
      base.OnSizeChanged(e);
      this.m_GameCanvas.FillGameForm();
      this.Invalidate(true);
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      if (!this.m_Game.IsGameRunning)
        return;
      e.Cancel = true;
      int num = (int) MessageBox.Show("The game is still running. Please quit inside the game.");
    }

    public KeyEventArgs UI_WaitKey()
    {
      this.m_HasKey = false;
      while (true)
      {
        Application.DoEvents();
        if (!this.m_HasKey)
          Thread.Sleep(1);
        else
          break;
      }
      return this.m_InKey;
    }

    public KeyEventArgs UI_PeekKey()
    {
      Thread.Sleep(1);
      Application.DoEvents();
      if (!this.m_HasKey)
        return (KeyEventArgs) null;
      this.m_HasKey = false;
      return this.m_InKey;
    }

    public void UI_PostKey(KeyEventArgs e)
    {
      switch (e.KeyCode)
      {
        case Keys.ShiftKey:
        case Keys.ControlKey:
          break;
        case Keys.LShiftKey:
        case Keys.RShiftKey:
        case Keys.LControlKey:
        case Keys.RControlKey:
          break;
        case Keys.Shift:
          break;
        case Keys.Control:
          break;
        case Keys.Alt:
          break;
        default:
          this.m_HasKey = true;
          this.m_InKey = e;
          e.Handled = true;
          break;
      }
    }

    public Point UI_GetMousePosition()
    {
      Thread.Sleep(1);
      Application.DoEvents();
      return this.m_GameCanvas.MouseLocation;
    }

    public void UI_PostMouseButtons(MouseButtons buttons)
    {
      this.m_HasMouseButtons = true;
      this.m_MouseButtons = buttons;
    }

    public MouseButtons? UI_PeekMouseButtons()
    {
      if (!this.m_HasMouseButtons)
        return new MouseButtons?();
      this.m_HasMouseButtons = false;
      return new MouseButtons?(this.m_MouseButtons);
    }

    public void UI_SetCursor(Cursor cursor)
    {
      if (cursor == this.Cursor)
        return;
      this.Cursor = cursor;
      Application.DoEvents();
    }

    public void UI_Wait(int msecs)
    {
      this.UI_Repaint();
      Thread.Sleep(msecs);
    }

    public void UI_Repaint()
    {
      this.Refresh();
      Application.DoEvents();
    }

    public void UI_Clear(Color clearColor)
    {
      this.m_GameCanvas.Clear(clearColor);
    }

    public void UI_DrawImage(string imageID, int gx, int gy)
    {
      this.m_GameCanvas.AddImage(GameImages.Get(imageID), gx, gy);
    }

    public void UI_DrawImage(string imageID, int gx, int gy, Color tint)
    {
      this.m_GameCanvas.AddImage(GameImages.Get(imageID), gx, gy, tint);
    }

    public void UI_DrawImageTransform(string imageID, int gx, int gy, float rotation, float scale)
    {
      this.m_GameCanvas.AddImageTransform(GameImages.Get(imageID), gx, gy, rotation, scale);
    }

    public void UI_DrawGrayLevelImage(string imageID, int gx, int gy)
    {
      this.m_GameCanvas.AddImage(GameImages.GetGrayLevel(imageID), gx, gy);
    }

    public void UI_DrawTransparentImage(float alpha, string imageID, int gx, int gy)
    {
      this.m_GameCanvas.AddTransparentImage(alpha, GameImages.Get(imageID), gx, gy);
    }

    public void UI_DrawPoint(Color color, int gx, int gy)
    {
      this.m_GameCanvas.AddPoint(color, gx, gy);
    }

    public void UI_DrawLine(Color color, int gxFrom, int gyFrom, int gxTo, int gyTo)
    {
      this.m_GameCanvas.AddLine(color, gxFrom, gyFrom, gxTo, gyTo);
    }

    public void UI_DrawString(Color color, string text, int gx, int gy, Color? shadowColor)
    {
      if (shadowColor.HasValue)
        this.m_GameCanvas.AddString(this.m_NormalFont, shadowColor.Value, text, gx + 1, gy + 1);
      this.m_GameCanvas.AddString(this.m_NormalFont, color, text, gx, gy);
    }

    public void UI_DrawStringBold(Color color, string text, int gx, int gy, Color? shadowColor)
    {
      if (shadowColor.HasValue)
        this.m_GameCanvas.AddString(this.m_BoldFont, shadowColor.Value, text, gx + 1, gy + 1);
      this.m_GameCanvas.AddString(this.m_BoldFont, color, text, gx, gy);
    }

    public void UI_DrawRect(Color color, Rectangle rect)
    {
      if (rect.Width <= 0 || rect.Height <= 0)
        throw new ArgumentOutOfRangeException("rectangle Width/Height <= 0");
      this.m_GameCanvas.AddRect(color, rect);
    }

    public void UI_FillRect(Color color, Rectangle rect)
    {
      if (rect.Width <= 0 || rect.Height <= 0)
        throw new ArgumentOutOfRangeException("rectangle Width/Height <= 0");
      this.m_GameCanvas.AddFilledRect(color, rect);
    }

    public void UI_DrawPopup(string[] lines, Color textColor, Color boxBorderColor, Color boxFillColor, int gx, int gy)
    {
      int num1 = 0;
      int num2 = 0;
      Size[] sizeArray = new Size[lines.Length];
      for (int index = 0; index < lines.Length; ++index)
      {
        sizeArray[index] = TextRenderer.MeasureText(lines[index], this.m_BoldFont);
        if (sizeArray[index].Width > num1)
          num1 = sizeArray[index].Width;
        num2 += sizeArray[index].Height;
      }
      Point location = new Point(gx, gy);
      Size size = new Size(num1 + 4, num2 + 4);
      Rectangle rect = new Rectangle(location, size);
      try
      {
        this.m_GameCanvas.AddFilledRect(boxFillColor, rect);
        this.m_GameCanvas.AddRect(boxBorderColor, rect);
      }
      catch (Exception ex)
      {
        Logger.WriteLine(Logger.Stage.RUN_GFX, "failed to draw popup box:: " + ex.ToString());
      }
      int gx1 = location.X + 2;
      int gy1 = location.Y + 2;
      for (int index = 0; index < lines.Length; ++index)
      {
        this.m_GameCanvas.AddString(this.m_BoldFont, textColor, lines[index], gx1, gy1);
        gy1 += sizeArray[index].Height;
      }
    }

    public void UI_ClearMinimap(Color color)
    {
      this.m_GameCanvas.ClearMinimap(color);
    }

    public void UI_SetMinimapColor(int x, int y, Color color)
    {
      this.m_GameCanvas.SetMinimapColor(x, y, color);
    }

    public void UI_DrawMinimap(int gx, int gy)
    {
      this.m_GameCanvas.DrawMinimap(gx, gy);
    }

    public float UI_GetCanvasScaleX()
    {
      return this.m_GameCanvas.ScaleX;
    }

    public float UI_GetCanvasScaleY()
    {
      return this.m_GameCanvas.ScaleY;
    }

    public string UI_SaveScreenshot(string filePath)
    {
      return this.m_GameCanvas.SaveScreenShot(filePath);
    }

    public string UI_ScreenshotExtension()
    {
      return this.m_GameCanvas.ScreenshotExtension();
    }

    public void UI_DoQuit()
    {
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.m_GameCanvas != null)
        this.m_GameCanvas.DisposeUnmanagedResources();
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      Logger.WriteLine(Logger.Stage.INIT_MAIN, "creating GameCanvas...");
      if (SetupConfig.Video == SetupConfig.eVideo.VIDEO_MANAGED_DIRECTX)
      {
        Logger.WriteLine(Logger.Stage.INIT_MAIN, "DXGameCanvas implementation...");
        this.m_GameCanvas = (IGameCanvas) new DXGameCanvas();
      }
      else
      {
        Logger.WriteLine(Logger.Stage.INIT_MAIN, "GDIPlusGameCanvas implementation...");
        this.m_GameCanvas = (IGameCanvas) new GDIPlusGameCanvas();
      }
      Logger.WriteLine(Logger.Stage.INIT_MAIN, "SuspendLayout...");
      this.SuspendLayout();
      Logger.WriteLine(Logger.Stage.INIT_MAIN, "setup GameCanvas...");
      this.m_GameCanvas.NeedRedraw = true;
      UserControl gameCanvas = this.m_GameCanvas as UserControl;
      gameCanvas.Location = new Point(279, 83);
      gameCanvas.Name = "canvasCtrl";
      gameCanvas.Size = new Size(150, 150);
      gameCanvas.TabIndex = 0;
      Logger.WriteLine(Logger.Stage.INIT_MAIN, "setup RogueForm");
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(800, 600);
      this.Controls.Add((Control) gameCanvas);
      this.Icon = new Icon("IconPNG.ico");
      this.Name = nameof (RogueForm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Rogue Survivor";
      this.WindowState = FormWindowState.Maximized;
      Logger.WriteLine(Logger.Stage.INIT_MAIN, "ResumeLayout");
      this.ResumeLayout(false);
      Logger.WriteLine(Logger.Stage.INIT_MAIN, "InitializeComponent() done.");
    }
  }
}
