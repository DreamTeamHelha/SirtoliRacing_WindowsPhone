using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ProjetSirtoliRacingWindowsPhone.Resources;
using System.Json;


namespace ProjetSirtoliRacingWindowsPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructeur
        public MainPage()
        {
            InitializeComponent();

            //Ajoute une colonne à la grille
            ContentPanel.ColumnDefinitions.Add(new ColumnDefinition());

            //Crée un client web pour récupérer la liste des circuits
            WebClient client = new WebClient();         
            //Défini la méthode à appelé lorsque le chargement est terminé
            client.DownloadStringCompleted += client_DownloadStringCompleted;
            //Lance le téléchargement de la liste des circuits 
            // ATTENTION : Il se fait de manière asynchrone et n'est donc pas bloquant pour l'application !
            client.DownloadStringAsync(new Uri("http://193.190.66.14:6080/SirtoliRacing/tracks"));

        }

        private void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                //Transforme le résultat de la requête au web service en tableau json
                JsonArray json = (JsonArray)JsonArray.Parse(e.Result);

                //Parcours du tableau
                foreach (JsonValue value in json)
                {
                    //Ajout d'une ligne dans la grille
                    ContentPanel.RowDefinitions.Add(new RowDefinition());

                    //Transforme le nom du circuit recu pour qu'il soit correctement affichable
                    String valueString = value.ToString();
                    valueString = valueString.Trim(new Char[] { '"' });

                    //Crée le bouton lié au circuit
                    Button button = new Button();
                    button.Name = valueString;
                    valueString = valueString.Replace("_", " ");
                    button.Content = valueString;
                    button.Tap += button_Tap;

                    //Ajoute le bouton à la grille
                    Grid.SetColumn(button, 0);
                    Grid.SetRow(button, ContentPanel.RowDefinitions.Count - 1);
                    ContentPanel.Children.Add(button);
                }
            }
        }

        private void button_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            PhoneApplicationService.Current.State["name"] = ((Button)sender).Name;
            NavigationService.Navigate(new Uri("/TrackScore.xaml", UriKind.Relative));
        }


    }
}
       
