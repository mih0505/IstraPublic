using Istra.Entities;
using System;
using System.IO;
using System.Windows.Forms;

namespace Istra
{
    static class CurrentSession
    {
        public static Worker CurrentUser { get; set; }
        public static Role CurrentRole { get; set; }
        public static Housing CurrentHousing { get; set; }
        public static DateTime TimeRun { get; set; }
        public static int GroupId { get; set; }
        public static int StudentId { get; set; }

        public static DateTime dateOrder { get; set; }//дата печати квитанции
        public static string namePaymaster { get; set; }//кассир
        public static bool enableDate { get; set; }


        public static string version = "v3.4";



        public static void ReportError(string methodName, string error)
        {
            try
            {
                using (var sw = new StreamWriter(@"errors.txt", true))
                {                    
                    string currentError = string.Format("{0} : {1} \r\n  Details : {2}", DateTime.Now.ToString(), error, methodName);
                    sw.WriteLine(currentError);                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка записи в файл", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
