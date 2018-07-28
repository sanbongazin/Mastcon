
using System;
using System.Linq;
using Mastodot;
using Mastodot.Entities;
using Mastodot.Enums;
using Mastodot.Utils;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Mastcon
{
    class Program
	{
		private void OAuth(){
			var registeredApp = ApplicaionManager.RegistApp("qiitadon.com","MastCondot", Scope.Read | Scope.Write | Scope.Follow).Result;
			//var registeredApp = ApplicaionManager.RegistApp("pawoo.net", "MastCondot", Scope.Read | Scope.Write | Scope.Follow).Result;
			var url = ApplicaionManager.GetOAuthUrl(registeredApp);
			Console.WriteLine(url);
			Console.Write("please copy and peaste AccessToken:");
			var code = Console.ReadLine();
			var tokens = ApplicaionManager.GetAccessTokenByCode(registeredApp, code).Result;

			Console.WriteLine(tokens.AccessToken);
			url = "https://qiitadon.com/web/getting-started";
			//url = "https://pawoo.net/web/timelines/public";
			Console.WriteLine("OAuthSuccess!");

			var p = new Program();
			for (; ; )
			{
				Console.WriteLine("May I help you?(readmode: r, tootmode: t, exit: other):");
				var answer = Console.ReadLine();
				if (answer == "r")
				{
					p.Read(url, tokens.AccessToken);
				}
				else if (answer == "t")
				{
					Console.WriteLine("Please Write to send Toot:");
					var content = Console.ReadLine();
					p.Toot(url,tokens.AccessToken,content);
				}
				else
				{
					break;
				}
			}
          
		}

		private async Task Read(string url, string AccessToken){
			var client = new MastodonClient(url, AccessToken);

			var data = client.GetObservablePublicTimeline().OfType<Status>();
			var publicStream = client.GetObservablePublicTimeline()
			                         .OfType<Status>()
			                         .Subscribe(x => Console.WriteLine($@"{x.Account.UserName} 
			                                                       Tooted: {x.Content}"));
			
			//var maxid = 10045138;
			//var statuses = await client.GetRecentPublicTimeline(maxId: maxid);
            //// do show some statuses
            //statuses = await client.GetRecentPublicTimeline(sinceId: statuses.Links.Prev.Value);
            //// do show next statuses

		}

		private void Toot(string url, string AccessToken, string content){
			var client = new MastodonClient(url, AccessToken);

            
		}
		static void Main(string[] args)
		{
			//var p = new Program();
			new Program().OAuth();
        }
    }
}
