using HelixToolkit.Wpf;
using Planetarium.Classes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Planetarium.Models;
using _14C_H25_TP3_Moussa_Dembélé.Classes;
namespace Planetarium
{
    public partial class MainWindow : Window
    {
        Model3DGroup _etoiles = new Model3DGroup();
        List<GeometryModel3D> _etoilesSelectionnees = new List<GeometryModel3D>();

        public MainWindow()
        {
            InitializeComponent();

            txtblockSeparateur.Text = new string(Enumerable.Repeat("+*o.\n\n\n\n\n\n\n\n\n\n", 1000).Select(s => s[new Random().Next(s.Length)]).ToArray());

            DataContext = _etoiles;
        }

        private void ChoixCouleur_PreviewMouseDown(object pSender, MouseButtonEventArgs pEvent)
        {
            Resources["Couleur"] = (pSender as Canvas).Background;

            foreach (GeometryModel3D geometry in _etoilesSelectionnees)
                geometry.Material = MaterialHelper.CreateMaterial(Resources["Couleur"] as Brush) as MaterialGroup;
        }

        private void Musique_PreviewMouseDown(object pSender, MouseButtonEventArgs pEvent)
        {
            if (lblMusique.Content.Equals("𝅘𝅥"))
            {
                mediaAmbiance.Play();
                lblMusique.Content = "𝄽";
            }
            else
            {
                mediaAmbiance.Stop();
                lblMusique.Content = "𝅘𝅥";
            }
        }

        private void Charger_PreviewMouseDown(object pSender, MouseButtonEventArgs pEvent)
        {
            Moteur.ChargerFichier();

            //
            // TODO: Boucler sur toutes les constellations se trouvant dans la structure de données de la classe Moteur.
            // Sur chaque itération, appeler les méthodes suivantes définies ci-bas (attention, vous ne pouvez pas les modifier):
            // - CreerEtoiles
            // - AjouterListeConstellations
            foreach(Constellation C in Moteur.Constellations.Values)
            {
                CreerEtoiles(C.Racine, C.Code);
                AjouterListeConstellations(C);
            }
    




            // Fin du TODO.

            if (Moteur.CompterConstellations() > 0)
                lblListeConstellations.Content = $"Liste des constellations ({Moteur.CompterConstellations()}) :";
            else
                lblListeConstellations.Content = "Liste des constellations :";
        }

        private void CreerEtoiles(Etoile pEtoile, string pCodeConstellation)
        {
            if (pEtoile != null)
            {
                CreerEtoiles(pEtoile.Gauche, pCodeConstellation);

                MeshBuilder meshBuilder = new MeshBuilder(false, false);
                meshBuilder.AddSphere(new Point3D(pEtoile.X, pEtoile.Y, pEtoile.Z), pEtoile.Rayon);

                GeometryModel3D geometry = new GeometryModel3D()
                {
                    Geometry = meshBuilder.ToMesh(true),
                    Material = MaterialHelper.CreateMaterial(Colors.White) as MaterialGroup
                };

                geometry.SetValue(UidProperty, pEtoile.Code);
                geometry.SetValue(TagProperty, pCodeConstellation);
                _etoiles.Children.Add(geometry);

                CreerEtoiles(pEtoile.Droite, pCodeConstellation);
            }
        }

        private void Decharger_PreviewMouseDown(object pSender, MouseButtonEventArgs pEvent)
        {
            Moteur.DechargerFichier();

            _etoiles.Children.Clear();

            if (Moteur.CompterConstellations() > 0)
                lblListeConstellations.Content = $"Liste des constellations ({Moteur.CompterConstellations()}) :";
            else
            {
                lblListeConstellations.Content = "Liste des constellations :";
                borderInfoEtoile.Visibility = Visibility.Hidden;
                txtboxListeConstellations.Text = "▼";
                txtblockInfoConstellation.Text = "";
                txtblockVisuelConstellation.Text = "";
                stackpanelListeConstellations.Children.Clear();
                ((stackpanelListeConstellations.Parent as ScrollViewer).Parent as Border).Visibility = Visibility.Hidden;
            }
        }

        private void ListeConstellations_PreviewMouseDown(object pSender, MouseButtonEventArgs pEvent)
        {
            Border listeConstellations = (stackpanelListeConstellations.Parent as ScrollViewer).Parent as Border;

            if (Moteur.CompterConstellations() == 0)
                listeConstellations.Visibility = Visibility.Hidden;
            else
                listeConstellations.Visibility = Visibility.Visible;
        }

        private void AjouterListeConstellations(Constellation pConstellation)
        {
            TextBlock constellation = new TextBlock()
            {
                Text = pConstellation.NomScientifique,
                Uid = pConstellation.Code,
                Margin = new Thickness(5, 5, 5, 5),
                Cursor = Cursors.Hand
            };

            constellation.MouseLeftButtonUp += (pSender, pEvent) =>
            {
                Constellation constellationCourante = Moteur.RechercherConstellation((pSender as TextBlock).Uid);

                SelectionnerEtoilesConstellations(constellationCourante.Code);

                txtboxListeConstellations.Text = $"▼ {constellationCourante.NomScientifique}";
                txtblockInfoConstellation.Text = constellationCourante.ToString();
                txtblockVisuelConstellation.Text = constellationCourante.AfficherVisuelConstellation(constellationCourante.Racine);

                viewportGalaxie.CameraController.CameraTarget = new Point3D(constellationCourante.Racine.X, constellationCourante.Racine.Y, constellationCourante.Racine.Z);
                ((stackpanelListeConstellations.Parent as ScrollViewer).Parent as Border).Visibility = Visibility.Hidden;
            };

            constellation.MouseEnter += (pSender, pEvent) =>
            {
                TextBlock choix = pSender as TextBlock;

                choix.Foreground = Brushes.Black;
                choix.Background = Resources["Couleur"] as Brush;
            };

            constellation.MouseLeave += (pSender, pEvent) =>
            {
                TextBlock choix = pSender as TextBlock;

                choix.Foreground = Resources["Couleur"] as Brush;
                choix.Background = Brushes.Black;
            };

            stackpanelListeConstellations.Children.Add(constellation);
        }

        private void Galaxie_PreviewMouseMove(object pSender, MouseEventArgs pEvent)
        {
            Galaxie_PreviewMouseMoveMouseDoubleClick(pEvent, false);
        }

        private void Galaxie_MouseDoubleClick(object pSender, MouseEventArgs pEvent)
        {
            Galaxie_PreviewMouseMoveMouseDoubleClick(pEvent, true);
        }

        private void Galaxie_PreviewMouseMoveMouseDoubleClick (MouseEventArgs pEvent, bool pMouseDoubleClick)
        {
            RayMeshGeometry3DHitTestResult mesh_result = VisualTreeHelper.HitTest(viewportGalaxie, pEvent.GetPosition(viewportGalaxie)) as RayMeshGeometry3DHitTestResult;

            if (mesh_result != null)
            {
                GeometryModel3D SelectedModel = mesh_result.ModelHit as GeometryModel3D;

                if (SelectedModel.GetValue(TagProperty) != null)
                {
                    Constellation constellationCourante = Moteur.RechercherConstellation(SelectedModel.GetValue(TagProperty).ToString());
                    Etoile etoileCourante = constellationCourante.RechercherEtoile(constellationCourante.Racine, SelectedModel.GetValue(UidProperty).ToString());

                    lblInfoEtoile.Content = $"{etoileCourante}\n\nConstellation : {constellationCourante.NomScientifique}";
                    borderInfoEtoile.Visibility = Visibility.Visible;

                    if (pMouseDoubleClick)
                        viewportGalaxie.CameraController.CameraTarget = new Point3D(etoileCourante.X, etoileCourante.Y, etoileCourante.Z);
                }
            }
            else
                borderInfoEtoile.Visibility = Visibility.Hidden;
        }

        private void SelectionnerEtoilesConstellations(string pCodeConstellation)
        {
            if (_etoilesSelectionnees.Count > 0)
            {
                foreach (GeometryModel3D g in _etoilesSelectionnees)
                    g.Material = MaterialHelper.CreateMaterial(Colors.White) as MaterialGroup;

                _etoilesSelectionnees.Clear();
            }

            foreach (GeometryModel3D geometry in _etoiles.Children)
            {
                if (geometry.GetValue(TagProperty).ToString().Equals(pCodeConstellation))
                {
                    _etoilesSelectionnees.Add(geometry);
                    geometry.Material = MaterialHelper.CreateMaterial(Resources["Couleur"] as Brush) as MaterialGroup;
                }
            }
        }
       
    }
}
