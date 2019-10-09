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
        
        [HttpPost]
        public async Task PostAsync(GitHubPayLoad gitHubPayLoad)
        {
        
            var pullCommentUrl = gitHubPayLoad.Pull_request.Review_comments_url;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.github.com");
            var token = "5f623172ae647c25f325d2a83bb97392543704ec";
            client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("AppName", "1.0"));
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", token);

            var response = await client.GetAsync(new Uri(pullCommentUrl).LocalPath);
            string body= "";
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var comments = JsonConvert.DeserializeObject<IEnumerable<CommentPayload>>(responseString).ToList<CommentPayload>();
                comments.ForEach(comment => body += comment.User.Login + "\n" + comment.Body + "\n\n");
                body += $"Total Comments :{comments.Count}";

            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                body = $"Error In Fetching Data fro api call {response.ReasonPhrase}";



            }
            System.IO.File.WriteAllText(@"C:\Users\vjola\source\ui\web-hook\web-hook\Output.txt", body);
            Console.WriteLine("jihf");

        }

    }
}