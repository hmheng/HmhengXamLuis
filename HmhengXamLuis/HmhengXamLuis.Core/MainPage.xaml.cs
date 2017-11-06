using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;
using Newtonsoft.Json;
using HmhengXamLuis.Core.Models;

namespace HmhengXamLuis
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            btnConnect.Clicked += MakeRequest;
        }

        public async void MakeRequest(object sender, EventArgs e)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // This app ID is for a public sample app that recognizes requests to turn on and turn off lights
            var luisAppId = "<replace with your LUIS App ID here>";
            var subscriptionKey = "<replace with your LUIS subscription Key here>";

            // The request header contains your subscription key
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // The "q" parameter contains the utterance to send to LUIS
            queryString["q"] = txtMessage.Text;

            // These optional request parameters are set to their default values
            queryString["timezoneOffset"] = "0";
            queryString["verbose"] = "false";
            queryString["spellCheck"] = "false";
            queryString["staging"] = "false";

            var uri = "https://southeastasia.api.cognitive.microsoft.com/luis/v2.0/apps/" + luisAppId + "?" + queryString; //remember to change to the region that you set in LUIS.ai
            var response = await client.GetAsync(uri);

            var strResponseContent = await response.Content.ReadAsStringAsync();

            //// Display the JSON result from LUIS
            //Console.WriteLine(strResponseContent.ToString());

            try
            {
                lblIntent.Text = "";
                lblEntities.Text = "";
                LuisResponse luisResponse = JsonConvert.DeserializeObject<LuisResponse>(strResponseContent);
                if (luisResponse != null)
                {
                    if (luisResponse.topScoringIntent != null)
                    {
                        lblIntent.Text = luisResponse.topScoringIntent.intent;
                    }

                    if(luisResponse.entities.Count() > 0)
                    {
                        foreach (var entities in luisResponse.entities)
                        {
                            lblEntities.Text += entities.entity + "(" + entities.type + ")\n";
                        }
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }
}
