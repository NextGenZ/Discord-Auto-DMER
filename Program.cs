using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using SuperEngine.xNet;

namespace Discord_Auto_PMER
{
    class Program
    {
        static List<Users> list = new List<Users>();
        static string message;
        static string token;
        static void Main(string[] args)
        {
            Console.Title = "Discord AUTO-DM'ER | By xPolish | For Coders.GG";
            Console.WriteLine("Made for https://Coders.GG by xPolish");
            Console.WriteLine("What message u want to spam with");
            message = Console.ReadLine();
            Console.WriteLine("Whats ur token");
            token = Console.ReadLine();
            Scrape();
            HttpRequest req = new HttpRequest { IgnoreProtocolErrors = true};

            foreach (var i in list)
            {
                req.AddHeader("Authorization", token);
                string id = new Regex("id\": \"([^\"]*)\", \"recipients\"").Match(req.Post("https://discordapp.com/api/v6/users/425211029614624768/channels", "{\"recipients\":[\"" + i.id + "\"]}", "application/json").ToString()).Groups[1].Value;
                req.AddHeader("Authorization", token);
                req.Post("https://discordapp.com/api/v6/channels/" + id + "/messages", "{\"content\":\"" + message + "\",\"nonce\":\"" + id +"\",\"tts\":false}", "application/json").ToString(out var request);
                if (request.StatusCode == HttpStatusCode.OK)
                    Console.WriteLine($"[Sent Message] {i.name}#{i.discrim} | {message}");
                if (request.StatusCode == HttpStatusCode.Forbidden)
                    Console.WriteLine($"[Cannot send message!] {i.name}#{i.discrim}");
                Thread.Sleep(5000);
            }
            Console.Read();
        }
        static void Scrape()
        {
            {
            ////SCRAPE Guilds
            HttpRequest req = new HttpRequest();
            req.AddHeader("Authorization", token);
            string x = req.Get("https://discordapp.com/api/v6/users/@me/guilds?limit=100").ToString();
            Regex expression = new Regex("id\": \"([^\"]*)");
            var results = expression.Matches(x);
            foreach (Match match in results)
            {
                req.AddHeader("Authorization", token);
                string xx = req.Get("https://discordapp.com/api/v6/guilds/" + match.Groups[1] + "/members?limit=1000").ToString();
                Regex expressions = new Regex("\"username\": \"([^\"]*)\", \"discriminator\": \"([^\"]*)\", \"id\": \"([^\"]*)\"");
                var resultss = expressions.Matches(xx);
                foreach (Match matchs in resultss)
                {
                    list.Add(new Users
                    {
                        id = matchs.Groups[3].Value,
                        discrim = matchs.Groups[2].Value,
                        name = matchs.Groups[1].Value
                    }); 
                }
            }
            }
            {
            HttpRequest req = new HttpRequest();
            ////SCRAPE Friends
            req.AddHeader("Authorization", token);
            string xxx = req.Get("https://discordapp.com/api/v6/users/@me/relationships").ToString();
            Regex expressionss = new Regex("\"username\": \"([^\"]*)\", \"discriminator\": \"([^\"]*)\", \"id\": \"([^\"]*)\"");
            var resultsss = expressionss.Matches(xxx);
            foreach (Match matchss in resultsss)
            {
                list.Add(new Users
                {
                    id = matchss.Groups[3].Value,
                    discrim = matchss.Groups[2].Value,
                    name = matchss.Groups[1].Value
                });
            }
        }
            Console.Title = $"Discord AUTO-DM'ER | By xPolish | For Coders.GG | Scraped Users: {list.Count}";
        }
    }
    public class Users
    {
        public string id { get; set; }
        public string name { get; set; }
        public string discrim { get; set; }
    }
}
