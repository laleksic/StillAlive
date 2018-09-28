// Decompiled with JetBrains decompiler
// Type: djack.RogueSurvivor.Bugreport
// Assembly: Rogue Survivor Still Alive, Version=1.1.8.0, Culture=neutral, PublicKeyToken=null
// MVID: 88F4F53B-0FB3-47F1-8E67-3B4712FB1F1B
// Assembly location: C:\Users\Mark\Documents\Visual Studio 2017\Projects\Rogue Survivor Still Alive\New folder\Rogue Survivor Still Alive.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace djack.RogueSurvivor
{
  public class Bugreport : Form
  {
    private string NL = Environment.NewLine;
    private Exception m_Exception;
    private IContainer components;
    private Button m_OkButton;
    private TextBox m_HeaderTextBox;
    private TextBox m_LogTextBox;

    public Bugreport(Exception e)
    {
      this.InitializeComponent();
      this.m_Exception = e;
    }

    private void m_OkButton_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void Bugreport_Load(object sender, EventArgs e)
    {
      this.m_HeaderTextBox.Text = "Rogue Survivor encoutered a fatal error." + this.NL + "Please report all the text in the textbox below to the author (copypaste it, remember to scroll all the way down from start to end)." + this.NL + "Press OK to exit.";
      this.m_LogTextBox.Clear();
      this.m_LogTextBox.AppendText("Start of report." + this.NL);
      this.m_LogTextBox.AppendText("-----------------------------------------------" + this.NL);
      this.m_LogTextBox.AppendText("EXCEPTION" + this.NL);
      this.m_LogTextBox.AppendText(this.m_Exception.ToString() + this.NL);
      this.m_LogTextBox.AppendText("-----------------------------------------------" + this.NL);
      this.m_LogTextBox.AppendText("LOG" + this.NL);
      foreach (string line in Logger.Lines)
        this.m_LogTextBox.AppendText(line + this.NL);
      this.m_LogTextBox.AppendText("-----------------------------------------------" + this.NL);
      this.m_LogTextBox.AppendText("End of report." + this.NL);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.m_OkButton = new Button();
      this.m_HeaderTextBox = new TextBox();
      this.m_LogTextBox = new TextBox();
      this.SuspendLayout();
      this.m_OkButton.Dock = DockStyle.Bottom;
      this.m_OkButton.Location = new Point(0, 343);
      this.m_OkButton.Name = "m_OkButton";
      this.m_OkButton.Size = new Size(592, 23);
      this.m_OkButton.TabIndex = 0;
      this.m_OkButton.Text = "OK";
      this.m_OkButton.UseVisualStyleBackColor = true;
      this.m_OkButton.Click += new EventHandler(this.m_OkButton_Click);
      this.m_HeaderTextBox.AcceptsReturn = true;
      this.m_HeaderTextBox.AcceptsTab = true;
      this.m_HeaderTextBox.Dock = DockStyle.Top;
      this.m_HeaderTextBox.Location = new Point(0, 0);
      this.m_HeaderTextBox.Multiline = true;
      this.m_HeaderTextBox.Name = "m_HeaderTextBox";
      this.m_HeaderTextBox.ReadOnly = true;
      this.m_HeaderTextBox.ScrollBars = ScrollBars.Vertical;
      this.m_HeaderTextBox.Size = new Size(592, 97);
      this.m_HeaderTextBox.TabIndex = 1;
      this.m_LogTextBox.AcceptsReturn = true;
      this.m_LogTextBox.AcceptsTab = true;
      this.m_LogTextBox.Location = new Point(12, 103);
      this.m_LogTextBox.Multiline = true;
      this.m_LogTextBox.Name = "m_LogTextBox";
      this.m_LogTextBox.ReadOnly = true;
      this.m_LogTextBox.ScrollBars = ScrollBars.Vertical;
      this.m_LogTextBox.Size = new Size(568, 234);
      this.m_LogTextBox.TabIndex = 2;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(592, 366);
      this.Controls.Add((Control) this.m_LogTextBox);
      this.Controls.Add((Control) this.m_HeaderTextBox);
      this.Controls.Add((Control) this.m_OkButton);
      this.Name = nameof (Bugreport);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Rogue Survivor Error Report";
      this.TopMost = true;
      this.Load += new EventHandler(this.Bugreport_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
