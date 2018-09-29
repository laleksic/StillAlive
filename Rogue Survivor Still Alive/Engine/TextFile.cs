﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace djack.RogueSurvivor.Engine
{
    class TextFile
    {
        #region Fields
        List<string> m_RawLines;
        List<string> m_FormatedLines;
        #endregion

        #region Properties
        public IEnumerable<string> RawLines
        {
            get { return m_RawLines; }
        }

        public List<string> FormatedLines
        {
            get { return m_FormatedLines; }
        }
        #endregion

        #region Init
        public TextFile()
        {
            m_RawLines = new List<string>();
        }
        #endregion

        #region Loading & Saving
        public bool Load(string fileName)
        {
            StreamReader inStream = null; //@@MP - try/finally ensures that the stream is always closed (Release 5-7)
            try
            {
                Logger.WriteLine(Logger.Stage.RUN_MAIN, String.Format("Loading text file {0}...", fileName));

                inStream = File.OpenText(fileName);
                m_RawLines = new List<string>();
                while (!inStream.EndOfStream)
                {
                    string line = inStream.ReadLine();
                    m_RawLines.Add(line);
                }

                Logger.WriteLine(Logger.Stage.RUN_MAIN, String.Format("done!")); //@@MP - removed unused argument (Release 5-5)
                return true;
            }
            catch (FileNotFoundException e)
            {
                Logger.WriteLine(Logger.Stage.RUN_MAIN, String.Format("Text file not found: {0}", e.ToString()));
                return false;
            }
            catch (Exception e)
            {
                Logger.WriteLine(Logger.Stage.RUN_MAIN, String.Format("Loading exception: {0}", e.ToString()));
                throw;
            }
            finally
            {
                if (inStream != null)
                    inStream.Close();
            }
        }

        public bool Save(string fileName)
        {
            try
            {
                Logger.WriteLine(Logger.Stage.RUN_MAIN, String.Format("Saving text file {0}...", fileName));
                File.WriteAllLines(fileName, m_RawLines.ToArray());
                Logger.WriteLine(Logger.Stage.RUN_MAIN, String.Format("done!")); //@@MP - removed unused argument (Release 5-5)
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine(Logger.Stage.RUN_MAIN, String.Format("Saving exception: {0}", e.ToString()));
                throw; //return false;
            }
        }
        #endregion

        #region Raw Editing
        public void Append(string line)
        {
            m_RawLines.Add(line);
        }
        #endregion

        #region Parsing and Formatting
        public void FormatLines(int charsPerLine)
        {
            if (m_RawLines == null || m_RawLines.Count == 0)
                return;

            m_FormatedLines = new List<string>(m_RawLines.Count);
            for (int iRawLine = 0; iRawLine < m_RawLines.Count; iRawLine++)
            {
                string rawLine = m_RawLines[iRawLine];
                while (rawLine.Length > charsPerLine)
                {
                    string head = rawLine.Substring(0, charsPerLine);
                    string rest = rawLine.Remove(0, charsPerLine);
                    m_FormatedLines.Add(head);
                    rawLine = rest;
                }
                m_FormatedLines.Add(rawLine);               
            }
        }
        #endregion
    }
}
