using System;
using System.Windows.Forms;

namespace Martini
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());
            //Application.Run(new ApplicationForm());
            Application.Run(new MainForm());
        }
    }
}