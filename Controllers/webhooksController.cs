using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using web_hook.Model;
using web_hook.Models;

namespace web_hook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class webhooksController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpPost]
        public  void Post(GitHubPayLoad githubPayload)
        {
            string pullCommentUrl = githubPayload.Pull_request.Review_comments_url;
            ProcessWebHookPayloadAsync(pullCommentUrl).Wait();




        }
        private async Task ProcessWebHookPayloadAsync(string pullCommentUrl)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.github.com");
            var token = "3039b3e31e5e125eab40ab902e21f6d69ea8f918";
            client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("AppName", "1.0"));
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", token);

            var response = await client.GetAsync(new Uri(pullCommentUrl).LocalPath);
            string data = "";
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var comments = JsonConvert.DeserializeObject<IEnumerable<CommentPayload>>(responseString).ToList<CommentPayload>();
                comments.ForEach(c => data += c.User.Login + "\n" + c.Body + "\n");
                data += $"Total Comments: {comments.Count}\n\n";
            }
            else
            {
                data = $"Error In Fetching Data fro api call {response.ReasonPhrase}";
                data += $"Total Comments: 0\n\n";
            }

            System.IO.File.WriteAllText(@"C:\Users\vjola\source\ui\web-hook\web-hook\LogFile.txt", data);
     
        }

    }
}