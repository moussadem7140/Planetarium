using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _14C_H25_TP3_Moussa_Dembélé;
using Microsoft.Win32;
using Planetarium.Models;
using System.Text.Json;
using System.Globalization;
using System.Windows.Forms;
using Planetarium.Classes;
namespace _14C_H25_TP3_Moussa_Dembélé.Classes
{
    public static class Moteur
    {
        public static Dictionary<string, Constellation>  Constellations = new Dictionary<string, Constellation>();


        /// <summary>
        /// Permet d'aller chercher les information a partir d'une carte donnée
        /// </summary>
        public static void ChargerFichier()
        {
            string fichier = Utils.OuvrirFichierJSON();
            if (fichier != null)
            {
                string sCarte= File.ReadAllText(fichier);
                JsonDocument doc = JsonDocument.Parse(sCarte);
                JsonElement root = doc.RootElement;
                foreach(JsonElement constellation in   root.EnumerateArray())
                {
                    Constellation c = new Constellation(constellation.GetProperty("code").GetString(),
                        constellation.GetProperty("nom_francais").GetString(), 
                        constellation.GetProperty("nom_scientifique").GetString(),
                        constellation.GetProperty("description").GetString());
                    JsonElement etoiles = constellation.GetProperty("etoiles");
                    foreach (JsonElement etoile in etoiles.EnumerateArray())
                    {
                        c.AjouterEtoile(new Etoile(etoile.GetProperty("magnitude").GetDouble(), 
                            etoile.GetProperty("nom_commun").GetString(), etoile.GetProperty("code").GetString(),
                            etoile.GetProperty("distance").GetDouble(), etoile.GetProperty("index_couleur").GetDouble(),
                            etoile.GetProperty("rayon").GetDouble(), etoile.GetProperty("x").GetDouble(), etoile.GetProperty("y").GetDouble(),
                            etoile.GetProperty("z").GetDouble()));
                    }
                    Constellations.Add(c.Code, c);
                }
                Journalisation.Tracer( $"Chargement de la carte du ciel: {fichier}", Journalisation.Categorie.information);
            }
        }
        /// <summary>
        /// Permet de decharger une carte dans le visuel
        /// </summary>
        public static void DechargerFichier()
        {
            foreach(Constellation c in Constellations.Values)
            {
                c.SupprimerEtoiles(c.Racine);
            }
            Constellations.Clear();
            Journalisation.Tracer("Dechargement de la constellation", Journalisation.Categorie.information);
        }
        /// <summary>
        /// Cette méthode permet de compter le nombre de constellations de votre structure de données.
        /// </summary>
        /// <returns>Le nombre d'étoiles</returns>
        public static int CompterConstellations()
        {
            return Constellations.Count;
        }
        /// <summary>
        /// cette méthode retourne la constellation trouvée à partir de notre structure de données
        /// </summary>
        /// <param name="code">Code de la constellation a troubver</param>
        /// <returns></returns>
        public static Constellation RechercherConstellation(string code)
        {
            return Constellations[code];
        }
    }
}