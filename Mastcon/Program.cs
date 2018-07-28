
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
			var url1 = "https://qiitadon.com";
			url = "https://streaming.qiitadon.com:4000";
			//url = "https://pawoo.net/web/timelines/home";
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
					Console.Write("Please Write to send Toot:");
					var content = Console.ReadLine();
					p.Toot(url1,tokens.AccessToken,content);
				}
				else
				{
					break;
				}
			}
          
		}

		private void Read(string url, string AccessToken){
			var client = new MastodonClient(url, AccessToken);

			//var data = client.GetObservablePublicTimeline().OfType<Status>();
			var statusDs = client.GetObservablePublicTimeline()
			                     .OfType<Status>()
                                 .Subscribe(x => Console.WriteLine($@"Tooted: {x.Content}"));
			Console.WriteLine("Press Key then Finish");
            Console.ReadKey();
            statusDs.Dispose();
			Console.WriteLine("Finish");         
		}

		private void Toot(string url, string AccessToken, string content){
			var client = new MastodonClient(url, AccessToken);
			client.PostNewStatus(status: content);
            
		}
		static void Main(string[] args)
		{
			//var p = new Program();
			new Program().OAuth();
        }
    }
}
