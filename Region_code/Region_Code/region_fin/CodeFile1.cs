using System.IO;
using System;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

class RegionCode
{
    public class RegionCodeData
    {
      //  "과거법정동코드": null,
      //"리명": null,
      //"법정동코드": 4806005500,
      //"삭제일자": "1988-04-23",
      //"생성일자": "1988-04-23",
      //"순위": 0,
      //"시군구명": "울산군",
      //"시도명": "경상남도",
      //"읍면동명": "서생면"

        public string Pastcode { get; set; }
        public string LeeName { get; set; }
        public long CurrentCode { get; set; }
        public string DeleteDate { get; set; }
        public string CreateDate { get; set; }
        public string Rank { get; set; }
        public string SiGunGuName { get; set; }
        public string SiDoName { get; set; }
        public string OepMyeonDongName { get; set; }

    }
        static void Main(string[] args)
        {
        for (int i = 1; i <48; i++) 
        {
            string api_key = "4n4Miwzm5p37SLb9Jk9bJa%2FMhFYSTl8mkQIensYxsOuwWyjpePzkk6oyRp3pOsd8GVnzwwQelKHMwSc0bPVfSA%3D%3D";
            string openApi_Url = $@"https://api.odcloud.kr/api/15063424/v1/uddi:6d7fd177-cc7d-426d-ba80-9b137edf6066?page={i}&perPage=1000&serviceKey={api_key}";
            string result = string.Empty;

            WebRequest req = null;
            WebResponse res = null;
            StreamReader reader = null;

            try
            {
                req = WebRequest.Create(openApi_Url);
                res = req.GetResponse();
                reader = new StreamReader(res.GetResponseStream());
                result = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                reader.Close();
                res.Close();
            }
            var jsonResult = JObject.Parse(result);
            // Console.WriteLine(jsonResult.ToString());

            var data = jsonResult["data"];
            var json_Array = data as JArray;

            foreach (var sensor in json_Array)
            {
                try
                {

                    using (MySqlConnection conn = new MySqlConnection("Server=210.119.12.66;" +
                                                                                  "Port=3306;" +
                                                                                  "Database=miniproject01;" +
                                                                                  "Uid=root;" +
                                                                                  "Pwd=12345;"))
                    {
                        if (conn.State == System.Data.ConnectionState.Closed) conn.Open();

                        var query = @"INSERT INTO region_code
                                                                    (Pastcode,
                                                                    LeeName,
                                                                    CurrentCode,
                                                                    DeleteDate,
                                                                    CreateDate,
                                                                    Sunwi,
                                                                    SiGunGuName,
                                                                    SiDoName,
                                                                    OepMyeonDongName)
                                                                    VALUES
                                                                    (@Pastcode,
                                                                    @LeeName,
                                                                    @CurrentCode,
                                                                    @DeleteDate,
                                                                    @CreateDate,
                                                                    @Sunwi,
                                                                    @SiGunGuName,
                                                                    @SiDoName,
                                                                    @OepMyeonDongName)";

                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        MySqlDataReader myReader;
                        cmd.Parameters.AddWithValue("@Pastcode", Convert.ToString(sensor["과거법정동코드"]));
                        cmd.Parameters.AddWithValue("@LeeName", Convert.ToString(sensor["리명"]));
                        cmd.Parameters.AddWithValue("@CurrentCode", Convert.ToString(sensor["법정동코드"]));
                        cmd.Parameters.AddWithValue("@DeleteDate", Convert.ToString(sensor["삭제일자"]));
                        cmd.Parameters.AddWithValue("@CreateDate", Convert.ToString(sensor["생성일자"]));
                        cmd.Parameters.AddWithValue("@Sunwi", Convert.ToString(sensor["순위"]));
                        cmd.Parameters.AddWithValue("@SiGunGuName", Convert.ToString(sensor["시군구명"]));
                        cmd.Parameters.AddWithValue("@SiDoName", Convert.ToString(sensor["시도명"]));
                        cmd.Parameters.AddWithValue("@OepMyeonDongName", Convert.ToString(sensor["읍면동명"]));

                        cmd.ExecuteNonQuery();
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }
        Console.WriteLine("저장 끝");
         }
    }



