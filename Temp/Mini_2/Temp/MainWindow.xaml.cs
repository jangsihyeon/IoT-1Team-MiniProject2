using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Temp.Logics;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;

namespace Temp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        public  string SkyCondition {get;set;}
        public string T1H { get;set;}

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Commons.myConnString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var query = @"SELECT 
	                                                T1H,
                                                    SkyCondition 
                                                 FROM ultrasrtfcst 
                                                  JOIN SKY
                                                  ON ultrasrtfcst.sky = sky.code
                                                 WHERE ultrasrtfcst.Idx=5";

                    var cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read()) 
                    {
                        T1H = reader.GetString("T1H");
                        SkyCondition = reader.GetString("SkyCondition");
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                await Commons.ShowMessageAsync("오류", $"DB조회 오류 {ex.Message}");
            }

            LblSkyCon.Content = SkyCondition;
            LblTemp.Content = T1H;
            
            switch (SkyCondition)
            {
                case "맑음":
                    iconsky.Kind = MahApps.Metro.IconPacks.PackIconFontistoKind.DaySunny;
                    break;
                case "구름많음":
                    iconsky.Kind = MahApps.Metro.IconPacks.PackIconFontistoKind.DayCloudy;
                    break;
                case "흐림":
                    iconsky.Kind = MahApps.Metro.IconPacks.PackIconFontistoKind.Cloudy;
                    break;
            }
        }
    }
 }
