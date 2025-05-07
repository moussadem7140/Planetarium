using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planetarium.Models;
using static Planetarium.Models.Etoile;
namespace _14C_H25_TP3_Moussa_DembéléTests
{
    public class EtoileTests
    {
        public Etoile creerEtoile(double magnitude = 1)
        {
            return new Etoile(magnitude, "dembele", "dembele", 12, 12, 5, 8, 9, 2);
        }
        [Fact]
        public void NomCommun_Devrait_Lancer_Un_NomCommunEstNullException_Si_NomCommun_EST_NULL()
        {
            //*** ARRANGE ***
            Etoile etoile = creerEtoile();
            //***ACT ET ASSERT ***
            Assert.Throws<NomCommunEstNullException>(() => etoile.Nom_commun="Null");
        }   
    }
}
