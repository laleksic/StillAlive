namespace djack.RogueSurvivor
{
    /// <summary>
    /// *** constains custom code ***
    /// </summary>
    partial class RogueForm
    {
        
        //@@MP - see https://stackoverflow.com/questions/16260654/code-analysis-finds-ca2213-error-in-designer-code

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            // dispose canvas resources.
            if (disposing && m_GameCanvas != null)
            {
                m_GameCanvas.DisposeUnmanagedResources();
                if (m_BoldFont != null)
                {
                    Logger.WriteLine(Logger.Stage.CLEAN_GFX, "disposing bold font...");
                    m_BoldFont.Dispose();
                    m_BoldFont = null;
                }

                if (m_NormalFont != null)
                {
                    Logger.WriteLine(Logger.Stage.CLEAN_GFX, "disposing normal font...");
                    m_NormalFont.Dispose();
                    m_NormalFont = null;
                }
            }

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Windows Form Designer generated code

        /// <summary>
        /// *** contains custom ugly code! ***
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        private void InitializeComponent()
        {
            Logger.WriteLine(Logger.Stage.INIT_MAIN, "creating GameCanvas...");
            switch (SetupConfig.Video)
            {
                default:
                    Logger.WriteLine(Logger.Stage.INIT_MAIN, "GDIPlusGameCanvas implementation...");
                    this.m_GameCanvas = new djack.RogueSurvivor.UI.GDIPlusGameCanvas();
                    break;
            }
            Logger.WriteLine(Logger.Stage.INIT_MAIN, "SuspendLayout...");
            this.SuspendLayout();
            // 
            // m_GameCanvas
            // 
            Logger.WriteLine(Logger.Stage.INIT_MAIN, "setup GameCanvas...");
            m_GameCanvas.NeedRedraw = true;
            // FIXME ugly hax :) use some proper design pattern instead...
            System.Windows.Forms.UserControl canvasAsUserControl = (m_GameCanvas as System.Windows.Forms.UserControl);
            canvasAsUserControl.Location = new System.Drawing.Point(279, 83);
            canvasAsUserControl.Name = "canvasCtrl";
            canvasAsUserControl.Size = new System.Drawing.Size(150, 150);
            canvasAsUserControl.TabIndex = 0;
            // 
            // RogueForm
            // 
            Logger.WriteLine(Logger.Stage.INIT_MAIN, "setup RogueForm");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(canvasAsUserControl);
            this.Icon = new System.Drawing.Icon("IconPNG.ico");
            this.Name = "RogueForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rogue Survivor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            Logger.WriteLine(Logger.Stage.INIT_MAIN, "ResumeLayout");
            this.ResumeLayout(false);

            Logger.WriteLine(Logger.Stage.INIT_MAIN, "InitializeComponent() done.");
        }

        #endregion

        private djack.RogueSurvivor.UI.IGameCanvas m_GameCanvas;




    }
}

