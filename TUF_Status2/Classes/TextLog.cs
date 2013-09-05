using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TUFStatus
{
    // Simple text log file
    public class TextLog
    {
        private Boolean _open;
        private StreamWriter _logfile;

        public TextLog()
        {
            string strFolder;

            try
            {
                strFolder = Application.StartupPath + "\\log";

                if (!Directory.Exists(strFolder))
                {
                    Directory.CreateDirectory(strFolder);
                }

                _logfile = File.AppendText(strFolder + "\\log " + System.DateTime.Now.ToString("yyyy-MM-dd") + ".txt");

                if (!(_logfile==null))
                {
                    _open = true;
                    _logfile.AutoFlush = true;
                    _logfile.WriteLine("--------------------------------------------------------");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error creating the text log file:" + ex.Message);
                _open = false;
            }
        }

        public void Close()
        {
            if (_open)
            {
            _logfile.WriteLine("--------------------------------------------------------");
            _logfile.Close();
            }
        }

        public void WriteLog(string strMessage)
        {
            if (_open)
            {
                _logfile.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + strMessage);
            }
        }

        public void WriteErrorLog(string strGearCode, string strInfo, string strMessage)
        {
            if (_open)
            {
                _logfile.WriteLine(System.DateTime.Now.ToString("ERROR: yyyy-MM-dd HH:mm:ss") + " - Gear code=" + strGearCode);
            _logfile.WriteLine("    : " + strInfo);
            _logfile.WriteLine("    : " + strMessage);
            }
        }
    }
}
