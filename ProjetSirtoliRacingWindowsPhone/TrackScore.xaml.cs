using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Json;

namespace ProjetSirtoliRacingWindowsPhone
{
    public partial class TrackScore : PhoneApplicationPage
    {
        public TrackScore()
        {
            InitializeComponent();
        }

        private void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if(e.Error == null)
            {
                JsonArray array = (JsonArray)JsonArray.Parse(e.Result);
                JsonValue [] arraySorted = array.OrderBy(val => val["Time"].ToString()).ToArray();
                foreach(JsonValue score in arraySorted)
                {
          
                    ContentPanel.RowDefinitions.Add(new RowDefinition());
                    TextBlock name = new TextBlock();
                    name.Text = score["Name"];
                    Grid.SetColumn(name,0);
                    Grid.SetRow(name,ContentPanel.RowDefinitions.Count-1);
                    ContentPanel.Children.Add(name);
                        
                    TextBlock time = new TextBlock();
                    time.Text = showableTime(score["Time"]);
                    Grid.SetColumn(time,1);
                    Grid.SetRow(time,ContentPanel.RowDefinitions.Count-1);
                    ContentPanel.Children.Add(time);

                    if(ContentPanel.RowDefinitions.Count == 10)
                        break;
                }                  
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            String name = (string)PhoneApplicationService.Current.State["name"]; ;
            TrackName.Text = name;

            //Ajout des colonnes à la grille
            ContentPanel.ColumnDefinitions.Add(new ColumnDefinition());
            ContentPanel.ColumnDefinitions.Add(new ColumnDefinition());

            //Appel au web service pour récupérer la liste des scores
            WebClient client = new WebClient();
            client.DownloadStringCompleted += client_DownloadStringCompleted;
            client.DownloadStringAsync(new Uri("http://193.190.66.14:6080/SirtoliRacing/track/" + name));
            
            base.OnNavigatedTo(e);

        }

        private String showableTime(int ms)
        {
            ms/=10;
            int csec = ms%100;
            ms/=100;
            int sec = ms%60;
            ms/=60;
            String time = String.Format("{0:00} : {1:00} : {2:00}", ms, sec, csec);
            return time;

        }
    }
}