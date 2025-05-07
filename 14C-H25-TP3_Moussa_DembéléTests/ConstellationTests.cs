using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planetarium.Models;
namespace _14C_H25_TP3_Moussa_DembéléTests
{
    public class ConstellationTests
    {
        double valeurSuperieur = 5;
        double valeurInferieur = 4;
        public Constellation creerConstellation()
        {
            return new Constellation("moussa", "moussa", "moussa ", "moussa");
        }
        public Etoile creerEtoile(string code, double magnitude = 1)
        {
            return new Etoile(magnitude, "dembele", code, 12, 12, 5, 8, 9, 2);
        }
        [Fact]
        public void AjouterEtoile_PourLePremierAppel_devrait_Correspondre_ALaRacine()
        {
            //***ARRANGE ***
            Constellation constellation = creerConstellation();
            Etoile etoile = creerEtoile("0");
            //*** Act ***
            constellation.AjouterEtoile(etoile);
            //*** Assert ***
            Assert.Equal(etoile, constellation.Racine);

        }
        [Fact]
        public void AjouterEtoile_Devrait_Placer_Etoile_A_Gauche_si_Magnitude_Inférieur_ACelle_DeLaRacine()
        {
            //***ARRANGE ***
            Constellation constellation = creerConstellation();
            Etoile racine = creerEtoile("1", valeurSuperieur);
            constellation.AjouterEtoile(racine);
            Etoile etoileInferieur= creerEtoile("2", valeurInferieur);
            //*** Act ***
            constellation.AjouterEtoile(etoileInferieur);
            //*** Assert ***
            Assert.Equal(etoileInferieur, constellation.Racine.Gauche);
        }
        [Fact]
        public void AjouterEtoile_Devrait_Placer_Etoile_A_Droite_si_Magnitude_Supérieur_ACelle_DeLaRacine()
        {
            //***ARRANGE ***
            Constellation constellation = creerConstellation();
            Etoile racine = creerEtoile("3", valeurInferieur);
            constellation.AjouterEtoile(racine);
            Etoile etoileSuperieur = creerEtoile("4", valeurSuperieur);
            //*** Act ***
            constellation.AjouterEtoile(etoileSuperieur);
            //*** Assert ***
            Assert.Equal(etoileSuperieur, constellation.Racine.Droite);
        }
        [Fact]
        public void AjouterEtoile_Devrait__Lancer_Un_StarAlreadyExistsException_Si_Etoile_EXISTE_Deja ()
        {
            //*** ARRANGE ***
            Constellation constellation = creerConstellation();
            Etoile etoile = creerEtoile("5");
            constellation.AjouterEtoile(etoile);
            //***ACT ET ASSERT ***
            Assert.Throws<StarAlreadyExistsException>(() => constellation.AjouterEtoile(etoile));
        }
       
    }

}