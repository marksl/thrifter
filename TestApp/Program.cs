using System;
using System.Windows.Forms;
using TestApp.UI;

namespace TestApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ThrifterForm());
        }
    }
}
