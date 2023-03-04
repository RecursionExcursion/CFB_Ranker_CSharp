using System.Net.Http.Headers;

namespace CFB_Ranker.API
{
    public class APICaller
    {
        /*
        private static APICaller? instance;
        public static APICaller Instance
        {
            get
            {
                return instance ??= new();
            }
        }

        
        private string bearer = "gLQdG5n7YtiTjzu/bxxxd+rdzzrhWftHTtIH7PAGVWlAQMOAA7h2ria3ai2Dl9zc";
        private string url = "https://api.collegefootballdata.com/teams/fbs?year=2022";
        */

        private APICaller()
        {
        }

        public static string CallAPI(string url, string bearer)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);

            //Add an accept header for json format
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //Add bearer
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);

            HttpResponseMessage responseMessage = client.GetAsync(url).Result;

            //If status code 200 -> return string of Json
            if (responseMessage.IsSuccessStatusCode)
            {
                var data = responseMessage.Content.ReadAsStringAsync().Result;
                return data;
            } else
            {
                Console.WriteLine($"Http response code: {(int) responseMessage.StatusCode} ({responseMessage.ReasonPhrase})");
                throw new Exception();
            }
        }
    }
}
