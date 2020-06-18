using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System;

namespace CompetitionGameWebSite.Pages
{
    public class IndexModel : PageModel
    {
        
        private readonly ILogger<IndexModel> _logger;
        protected string baseApiAddress = "https://localhost:44308/api";
        public string leagueStatsJson;
        public string teamsJson;
        public JObject leagueStatsDynamic;
        public List<JObject> jsonobjects;

        public bool showScorePanel;
        public List<JObject> apiResults;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            leagueStatsJson = new WebClient().DownloadString($"{baseApiAddress}/HistoryStats");
            teamsJson = new WebClient().DownloadString($"{baseApiAddress}/teams");
            leagueStatsDynamic = JObject.Parse(leagueStatsJson);
            var deserializedteamsJson = JsonConvert.DeserializeObject<List<string>>(teamsJson);
            jsonobjects = (from item in deserializedteamsJson
                                         select JObject.Parse(item)).ToList();
        }

        public void OnPostSimulate(string[] AreChecked, string[] jsonObj, IFormCollection form, int? page)
        {
            leagueStatsJson = new WebClient().DownloadString($"{baseApiAddress}/HistoryStats");
            leagueStatsDynamic = JObject.Parse(leagueStatsJson);

            var requestingObj = new JObject();
            var requestingObjTeams = new JArray();
            foreach (var checkedItem in AreChecked)
            {
                var teamnumber = Convert.ToInt32(checkedItem);
                JObject item = JObject.Parse(jsonObj[teamnumber]);
                requestingObjTeams.Add(item);
            }
            requestingObj.Add("teams", requestingObjTeams);
            requestingObj.Add("historyleaguestats", leagueStatsDynamic);
            string dataJson = requestingObj.ToString();

            var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{baseApiAddress}/Competition");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamwriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamwriter.Write(dataJson);
            }



            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string apiResult;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                apiResult = streamReader.ReadToEnd();
            }
            var deserialisedApiJson = JsonConvert.DeserializeObject<List<string>>(apiResult);


            showScorePanel = true;
            jsonobjects = (from item in jsonObj
                           select JObject.Parse(item)).ToList();
                

            apiResults = (from item in deserialisedApiJson
                          select JObject.Parse(item)).ToList();
        }
    }
}
