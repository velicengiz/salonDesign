using System;
using System.Windows.Forms;
using SalonDesign.Forms;

namespace SalonDesign
{
    static class Program
    {
        /// <summary>
        /// Uygulamanın ana giriş noktası.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SalonDesignForm());
        }
    }
}
