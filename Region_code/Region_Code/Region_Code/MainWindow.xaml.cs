using MahApps.Metro.Controls;
using Newtonsoft.Json.Linq;
using Region_Code.Logics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace Region_Code
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

        // 메인창 키자마자 api 조회 
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string api_key = "4n4Miwzm5p37SLb9Jk9bJa%2FMhFYSTl8mkQIensYxsOuwWyjpePzkk6oyRp3pOsd8GVnzwwQelKHMwSc0bPVfSA%3D%3D";
            
            string openApi_Url = $@"https://api.odcloud.kr/api/15063424/v1/uddi:6d7fd177-cc7d-426d-ba80-9b137edf6066?page=1&perPage=1000&serviceKey={api_key}";
            string result = string.Empty;

            WebRequest req= null;
            WebResponse res = null;
            StreamReader reader = null;

            try
            {
                req = WebRequest.Create(openApi_Url);
                res = await req.GetResponseAsync();
                reader = new StreamReader(res.GetResponseStream());
                result = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                await Commons.ShowMessageAsync("오류", $"OpenAPI 조회 오류 : {ex.Message}");
            }
            finally
            {
                reader.Close();
                res.Close();
            }
            var jsonResult = JObject.Parse(result);
            var status = Convert.ToInt32(jsonResult["status"]);
        }
    }
}
