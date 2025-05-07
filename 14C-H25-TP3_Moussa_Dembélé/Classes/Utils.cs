using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;

namespace Planetarium.Classes
{
    public static class Utils
    {
        public static string OuvrirFichierJSON()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Fichiers JSON (*.json)|*.json";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            else
            {
                return null;
            }
        }

    }
    public static class Journalisation
    {
        public enum Categorie
        {
            Erreur,
            information,
            etape,
        }
        static Journalisation()
        {
            Stream leFichier = File.Create("FichierTrace.txt");
            TextWriterTraceListener leListener = new TextWriterTraceListener(leFichier);
            Trace.Listeners.Add(leListener);
            Trace.AutoFlush = true;
        }
        public static void Tracer(string? message)
        {
            Trace.WriteLine($"{DateTime.Now.ToString()}: {message}");
        }
        public static void Tracer(string? message, Categorie? categorie)
        {
            Trace.WriteLine($"{DateTime.Now.ToString()}: {message}", categorie.ToString());
        }
        public static void Tracer(string? message, TextBox traceTextBox)
        {
            Tracer(message);
            traceTextBox.Text += message;
            traceTextBox.ScrollToEnd();
        }
    }
}
