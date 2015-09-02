using System;
using System.Linq;
using System.Net;
using RestSharp;
using RestSharp.Authenticators;

namespace Stripe
{
	public class StripeAuthenticator : IAuthenticator
	{
		private readonly string _apiKey;

		public StripeAuthenticator(string apiKey)
		{
			_apiKey = apiKey;
		}

		public void Authenticate(IRestClient client, IRestRequest request)
		{
			request.Credentials = new NetworkCredential(_apiKey, "");
		}
	}
}
