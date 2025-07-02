using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OBS
{
    internal static class Program
    {

        public static GirisForm MainGirisForm { get; private set; }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainGirisForm = new GirisForm();
            Application.Run(MainGirisForm);
        }
    }
}
