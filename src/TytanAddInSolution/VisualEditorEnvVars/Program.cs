﻿using System;
using System.Windows.Forms;

namespace VisualEditor.Environment
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm form = new MainForm();
            Application.Run(form);

            form.SaveHistory();
        }
    }
}