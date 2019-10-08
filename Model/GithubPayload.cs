using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web_hook.Model;

namespace web_hook.Models
{


    public class PullRequest
    {
        public string Url { get; set; }
        public string Html_url { get; set; }
        public string Title { get; set; }
        public User User { get; set; }
        public string Body { get; set; }
        public string Review_comments_url { get; set; }
        public string Review_comment_url { get; set; }
        public string Comments_url { get; set; }
        public int Comments { get; set; }
        public int Review_comments { get; set; }

    }

    public class GitHubPayLoad
    {
        public string Action { get; set; }
        public PullRequest Pull_request { get; set; }
    }