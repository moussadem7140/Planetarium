using System.Reflection.PortableExecutable;
using System.Text;

namespace Planetarium.Models
{
    //TODO : Compléter la classe Constellation avec ces attributs, propriétés, constructeur et méthodes
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
		public void AjouterEtoile(Etoile etoile)
		{
			if (RechercherEtoile(Racine, etoile.Code) != null)
				throw new StarAlreadyExistsException(etoile);
            if (Racine==null)
				Racine= etoile;
			else
				Racine = AjouterRecursif(Racine, etoile);

        }
		public Etoile AjouterRecursif(Etoile racine, Etoile etoile)
		{
			if(racine ==null)
				return etoile;
            if (etoile.Valeur < racine.Valeur)
                racine.Gauche = AjouterRecursif(racine.Gauche, etoile);
            if (etoile.Valeur > racine.Valeur)
                racine.Droite = AjouterRecursif(racine.Droite, etoile);
			return racine;
        }
		public void SupprimerEtoiles(Etoile etoile)
		{
			if(etoile.Gauche !=null)	
				SupprimerEtoiles(etoile.Gauche);
			if (etoile.Droite != null)
				SupprimerEtoiles(etoile.Droite);
			etoile= null;
		}
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
        public int	CompterEtoiles(Etoile etoile)
		{
			if(etoile==null)
				return 0;
			return 1+ CompterEtoiles(etoile.Gauche) + CompterEtoiles(etoile.Droite);
            //if (etoile.Gauche != null)
            // return	CompterEtoiles(etoile.Gauche);
            //if (etoile.Droite != null)
            //	CompterEtoiles(etoile.Droite);
        }
		public string AfficherVisuelConstellation(Etoile etoile)
		{
			return arborescence(Racine, "", true);

        }
        public override string ToString()
        {
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
        public string arborescence(Etoile etoile, string prefixe, bool estDernier, string sens = "")
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
		public double ObtenirSommeIndexCouleur(Etoile racine)
		{
			if (racine == null)
				return 0;
			if(racine.Gauche == null && racine.Droite == null)
				return racine.Index_couleur;
			return ObtenirSommeIndexCouleur(racine.Gauche) + ObtenirSommeIndexCouleur(racine.Droite);
		}
    }
	public class StarAlreadyExistsException: Exception
	{
		public StarAlreadyExistsException(Etoile etoile): base($"L'étoile {etoile} existe déja dans la constellation")
		{
		}
	}
}
