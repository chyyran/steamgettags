using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Web;
using System.Web.Http.Filters;
using SteamAppApi.Models;
namespace SteamAppApi.Controllers
{
    public class AllowCrossSiteJsonAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Response != null)
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            base.OnActionExecuted(actionExecutedContext);
        }
    }

    [AllowCrossSiteJson]
    public class SteamAppController : ApiController
    {
        public SteamApp GetApp(int id)
        {
            using (var client = new WebClient())
            {
                string html = client.DownloadString(@"http://store.steampowered.com/app/" + id + "/");
                var steam_doc = new HtmlDocument();
                steam_doc.LoadHtml(html);
                HtmlNode tags = steam_doc.DocumentNode.SelectSingleNode(@"//*[@id='game_highlights']/div[2]/div/div[5]/div[2]");
                var metadata = steam_doc.DocumentNode.SelectSingleNode(@"//*[@id='main_content']/div[4]/div[3]/div[2]/div").InnerHtml.Trim().Replace("\t", "");
                var matches = Regex.Matches(metadata, @"(?<=>).+(?=</)");
                var title = Regex.Match(metadata, @"(?<=<b>Title:</b>\s) .+(?=<br>)", RegexOptions.IgnorePatternWhitespace).Value;

                return new SteamApp()
                {
                    Title = title,
                    Publisher = matches[5].Value,
                    Developer = matches[3].Value,
                    SinglePlayer = metadata.Contains("Single-player"),
                    MultiPlayer = metadata.Contains("Multi-player"),
                    Controller = metadata.Contains("controller support"),
                    TradingCards = metadata.Contains("Steam Trading Cards"),
                    Tags = (from node in tags.ChildNodes where !(node.InnerText.Contains("+") || String.IsNullOrWhiteSpace(node.InnerText)) select HttpUtility.HtmlDecode(node.InnerText.Trim())).ToList(),
                    AppId = id,
                    ReleaseDate = Regex.Match(metadata, @"(?<=<b>Release Date:</b>\s).+(?=<br>)").Value

                };
            }
        }
    }
}
