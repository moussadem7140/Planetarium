using System.Reflection.PortableExecutable;
using System.Text;
using Planetarium.Classes;
namespace Planetarium.Models
{
    //TODO : Compléter la classe Constellation avec ces attributs, propriétés, constructeur et méthodes
	/// <summary>
	/// Classe representant l'arbre constitué d'étoile comme noeud
	/// </summary>
    public class Constellation
    {
		private Etoile _racine;

		public Etoile Racine
		{
			get { return _racine; }
			set { _racine = value; }
		}
		private string _code;

		public string Code
		{
			get { return _code; }
			set { _code = value; }
		}
		private string _nomFrancais;

		public string NomFrancais
		{
			get { return _nomFrancais; }
			set { _nomFrancais = value; }
		}
		private string _nomScientifique;

		public string NomScientifique
		{
			get { return _nomScientifique; }
			set { _nomScientifique = value; }
		}
		private string _description;

		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

        public Constellation(string code, string nomFrancais, string nomScientifique, string description)
        {
            Code = code;
            NomFrancais = nomFrancais;
            NomScientifique = nomScientifique;
            Description = description;
        }
        /// <summary>
        /// : Méthode d'insertion d'une étoile, basée sur la valeur de la magnitude (valeur du nœud).
        /// </summary>
        /// <param name="etoile">Etoile a ajouter</param>
        /// <exception cref="StarAlreadyExistsException">Lancer si etoile existe déja</exception>
        public void AjouterEtoile(Etoile etoile)
		{
			try
			{
                if (RechercherEtoile(Racine, etoile.Code) != null)
				{
					Journalisation.Tracer("L'étoile {etoile} existe déje dans cette constellation", Journalisation.Categorie.Erreur);
                    throw new StarAlreadyExistsException(etoile);
                }
                if (Racine == null)
                    Racine = etoile;
				else
				{
                    Racine = AjouterRecursif(Racine, etoile);
					Journalisation.Tracer($"Ajout de l'étoile : {etoile.Nom_commun}", Journalisation.Categorie.information);
                }
            }
			catch(Exception ex) 
			{
				throw ex ;
			}

        }
		/// <summary>
		/// Utilisée par AjouterEtoile, et effectue l'ajout recursif
		/// </summary>
		/// <param name="racine">Racine de l'arbre de la constellation</param>
		/// <param name="etoile">Etoile a ajouter</param>
		/// <returns>retourne l'arbre après l'ajout</returns>
		private Etoile AjouterRecursif(Etoile racine, Etoile etoile)
		{
			if(racine ==null)
				return etoile;
            if (etoile.Valeur < racine.Valeur)
                racine.Gauche = AjouterRecursif(racine.Gauche, etoile);
            if (etoile.Valeur > racine.Valeur)
                racine.Droite = AjouterRecursif(racine.Droite, etoile);
			return racine;
        }
        /// <summary>
        ///  Méthode supprimant toutes les étoiles de la constellation
        /// </summary>
        /// <param name="etoile">Etoile a supprimer</param>
        public void SupprimerEtoiles(Etoile etoile)
		{
			if(etoile.Gauche !=null)	
				SupprimerEtoiles(etoile.Gauche);
			if (etoile.Droite != null)
				SupprimerEtoiles(etoile.Droite);
			etoile= null;
		}
        /// <summary>
        /// Cette méthode permet de rechercher et de retourner 
		/// l'instance d'une étoile à partir du code de l'étoile passé en paramètre
        /// </summary>
        /// <param name="racine">Racine de l'arbre de la constellation</param>
        /// <param name="code">code a chercher</param>
        /// <returns>retourne l'etoile trouvée et null si elle nexiste pas)</returns>
        public Etoile RechercherEtoile(Etoile racine, string code)
		{
			if (racine == null)
				return null;
			if (racine.Code == code)
				return racine;
			Etoile trouvee=	RechercherEtoile(racine.Gauche, code);
			if(trouvee != null) return trouvee;
			return RechercherEtoile(racine.Droite, code);
		}
		/// <summary>
		/// Compte le nombre d'étoile d'un arbre
		/// </summary>
		/// <param name="etoile">Racine de l'arbre</param>
		/// <returns>nombre d'étoile</returns>
        public int	CompterEtoiles(Etoile etoile)
		{
			if(etoile==null)
				return 0;
			return 1+ CompterEtoiles(etoile.Gauche) + CompterEtoiles(etoile.Droite);
            
        }
		/// <summary>
		/// Affiche le visuel de la constellation
		/// </summary>
		/// <param name="etoile">Racine de la constellation</param>
		/// <returns>l'arbre déssiné</returns>
		public string AfficherVisuelConstellation(Etoile etoile)
		{
			Journalisation.Tracer("Affichage du visuel", Journalisation.Categorie.information);
			return arborescence(Racine, "", true);

        }
		/// <summary>
		/// Informations sur la constellation
		/// </summary>
		/// <returns>la chaine complète des informations</returns>
        public override string ToString()
        {
			Journalisation.Tracer("Affichage des informations de la constelation");
            return "Code: " + Code + "\n"+
                            "nom_scientifique: " + NomScientifique + "\n" +
                             "nom_francais: " + NomFrancais + "\n" +
                             "description: " + Description + "\n\n" +
                              "Etoile maître: " + Racine.Code + "\n" +
                            "Nombre d'étoile: " + CompterEtoiles(Racine) + "\n" +
                             "Profondeur: " + ObtenirProfondeur(Racine) + "\n" +
                             "Largeur Max: " + ObtenirLargeurMax() + "\n" +
                              "Etoile la plus brillante: " + ObtenirEtoilePlusBrillante(Racine) + "\n" +
                             "Etoile la plus loin: " + ObtenirEtoilePlusLoin(Racine) + "\n" +
                             "Somme des index de couleur: " + ObtenirSommeIndexCouleur(Racine) + "\n";	
        }
        /// <summary>
        /// Utilisée par AfficherVisuelConstellation, construis la chaîne d'un arbre
        /// </summary>
        /// <param name="etoile">Racine de l'arbre</param>
        /// <param name="prefixe">caractère en arrière</param>
        /// <param name="estDernier">booléen qui verifire si dernière étape</param>
        /// <param name="sens">Gauche / droite</param>
        /// <returns></returns>
        private string arborescence(Etoile etoile, string prefixe, bool estDernier, string sens = "")
		{
			if (etoile == null)
				return "";
			string couverture = String.IsNullOrEmpty(sens) ? "" : sens + ": ";
			string resultat = prefixe + (estDernier ? "└──" : "├──") + couverture + etoile.Code + "\n";
			bool testGauche = etoile.Gauche != null;
			bool testDroite = etoile.Droite != null;
			if(testGauche || testDroite)
			{
				string prefixe1 = prefixe + (estDernier ? "    " : "|   ");
				if (testGauche)
					resultat += arborescence(etoile.Gauche, prefixe1, !testDroite, "G");
				if(testDroite)
                    resultat += arborescence(etoile.Droite, prefixe1, true, "D");
            }

			return resultat;
        }
        public double  ObtenirProfondeur(Etoile racine)
		{
			if(racine == null)
				return 0;
			double profondeurGauche = ObtenirProfondeur(racine.Gauche);
			double profondeurDroite = ObtenirProfondeur(racine.Droite);
			return 1+  Math.Max(profondeurDroite, profondeurGauche);
			
		}
		/// <summary>
		/// Obtient la largeur maximal
		/// </summary>
		/// <returns>double largeur maximal</returns>
		public double ObtenirLargeurMax()
		{
			if(Racine == null)
				return 0;
			Queue<Etoile> queue = new Queue<Etoile>();
			queue.Enqueue(Racine);
			int largeur = 0;
			while(queue.Count > 0)
			{
				largeur= Math.Max(largeur, queue.Count);
				for(int i = 0; i < queue.Count; i++)
				{
					Etoile etoile= queue.Dequeue();
					if(etoile.Gauche !=null)
						queue.Enqueue(etoile.Gauche);
					if(etoile.Droite != null)
						queue.Enqueue(etoile.Droite);
				}
			}
			return largeur;
		}
		/// <summary>
		/// Obtiens l'étoile la plus brillante dans un arbre d'étoile
		/// </summary>
		/// <param name="racine">Racine de l'arbre</param>
		/// <returns>étoile avec la plus faible magnitude</returns>
		public Etoile ObtenirEtoilePlusBrillante(Etoile racine)
		{
			if (racine == null)
				return null;
			Etoile brillanteGauche = ObtenirEtoilePlusBrillante(racine.Gauche);
			Etoile brillanteDroite = ObtenirEtoilePlusBrillante(racine.Droite);

			Etoile brillante = racine;
			if(brillanteGauche != null && brillanteGauche.Valeur < brillante.Valeur)
				brillante = brillanteGauche;
			if (brillanteDroite != null && brillanteDroite.Valeur < brillante.Valeur)
				brillante = brillanteDroite;
			return brillante;
		}
		/// <summary>
		/// Obtiens l'étoile la plus loin de la terre
		/// </summary>
		/// <param name="racine">Racine de l'arbre</param>
		/// <returns>étoile avec la plus grande distance</returns>
        public Etoile ObtenirEtoilePlusLoin(Etoile racine)
		{
			if (racine == null)
				return null;
			Etoile loinGauche = ObtenirEtoilePlusLoin(racine.Gauche);
			Etoile loinDroite = ObtenirEtoilePlusLoin(racine.Droite);
			Etoile loin = racine;
			if (loinGauche != null && loinGauche.Distance > loin.Distance)
				loin = loinGauche;
			if(loinDroite != null && loinDroite.Distance > loin.Distance)
				loin = loinDroite;
			return loin;
		}
		/// <summary>
		/// Somme de toutes les index de couleur
		/// </summary>
		/// <param name="racine">Racine de l'arbre</param>
		/// <returns>la somme </returns>
		public double ObtenirSommeIndexCouleur(Etoile racine)
		{
			if (racine == null)
				return 0;
			if(racine.Gauche == null && racine.Droite == null)
				return racine.Index_couleur;
			return ObtenirSommeIndexCouleur(racine.Gauche) + ObtenirSommeIndexCouleur(racine.Droite);
		}
    }
	/// <summary>
	/// Exception personnalisée pour éviter les doublons dans l'arbre
	/// </summary>
	public class StarAlreadyExistsException: Exception
	{
		public StarAlreadyExistsException(Etoile etoile): base($"L'étoile {etoile} existe déja dans la constellation")
		{
		}
	}
}
