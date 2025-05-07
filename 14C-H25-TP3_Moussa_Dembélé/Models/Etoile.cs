namespace Planetarium.Models
{
    ////TODO : Compléter la classe Etoile avec ces attributs, propriétés, constructeur et méthodes
    public class Etoile
    {
		private Etoile _gauche;

		public Etoile Gauche
		{
			get { return _gauche; }
			set { _gauche = value; }
		}
		private Etoile _droite;

		public Etoile Droite
		{
			get { return _droite; }
			set { _droite = value; }
		}
		private double _valeur;

		public double Valeur
		{
			get { return _valeur; }
			set { _valeur = value; }
		}

		private string nom_commun;

		public string Nom_commun
		{
			get { return nom_commun; }
			set {
				if (value == "Null")
				{
                    nom_commun = null;
					throw new NomCommunEstNullException();
                }
				else
					nom_commun= value;
			}
		}
		private string _code;

		public string Code
		{
			get { return _code; }
			set { _code = value; }
		}
		private double _distance;

		public double Distance
		{
			get { return _distance; }
			set { _distance = value; }
		}

		private double index_couleur;

		public double Index_couleur
		{
			get { return index_couleur; }
			set { index_couleur = value; }
		}
		private double _rayon;

		public double Rayon
		{
			get { return _rayon; }
			set { _rayon = value; }
		}
		private double _x;

		public double X
		{
			get { return _x; }
			set { _x = value; }
		}
		private double _y;

		public double Y
		{
			get { return _y; }
			set { _y = value; }
		}
		private double _z;

		public double Z
		{
			get { return _z; }
			set { _z = value; }
		}
		

        public Etoile(double valeur, string nom_commun, string code, double distance, double index_couleur, double rayon, double x, double y, double z) 
        {
            Nom_commun = nom_commun;
            Code = code;
            Distance = distance;
            Index_couleur = index_couleur;
            Rayon = rayon;
            X = x;
            Y = y;
            Z = z;
			Valeur= valeur;
        }
		public override string ToString()
		{
			return "Code: " + Code + "\n\r" +
				   "Nom_commun:" + Nom_commun + "\n\r" +
				   "Magnitude:" + Valeur + "\n\r" +
				   "Distance:" + Distance + "\n\r" +
				   "index_couleur:" + Index_couleur + "\n\r" +
				   "X:" + X + "\n\r" +
				   "Y:" + Y + "\n\r" +
				   "Z:" + Z + "\n\r";
        }
        // "distance": 231.8408,
        // "index_couleur": -0.03,
        // "rayon": 0.380371,
        // "x": -40.516266,
        // "y": 49.207344,
        // "z": -31.440558
        public class NomCommunEstNullException : Exception
        {
            public NomCommunEstNullException() : base($"Le nom commun de l'étoile a une valeur null")
            {
            }
        }

    }
}
