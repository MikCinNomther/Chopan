using Chopan.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Chopan.Pages
{
    static class ApplicationValues
    {

        public static WindowState WindowState = WindowState.Normal;

        public static MainWindow MainCF = null;
        public static Page MainContent
        {
            get
            {
                return (Page)MainCF.PageSetter.Content;
            } set
            {
                MainCF.PageSetter.Content = value;
                GC.Collect();
            }
        }

        public static ChopanClient ChopanClient = null;
    }
}
