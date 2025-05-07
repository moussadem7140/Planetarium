using Microsoft.Win32;

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
}
