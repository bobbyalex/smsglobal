using System;
using System.Net.Http;
using System.Text;

namespace SmsGlobal
{
	class Program
	{
		static void Main(string[] args)
		{
			var uri = "api.smsglobal.com";
			var client = new HttpClient {BaseAddress = new Uri("http://" + uri)};
			client.DefaultRequestHeaders.Add("Accept", " application/json");

			var unixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			//A random string. Should be unique to each request. A guid is an easy way to do this
			var nonce = Guid.NewGuid().ToString();
			var apiKey = "--your-api-key--";
			var apiSecret = "--your-api-secret-issued-with-your-api-key--";
			var httpPort = "80";	//Change this to 443 if you are sending over https
			var endPoint = "/v2/sms";
			var hash = AuthenticationHelper.CreateMacHash(unixTime, nonce, apiSecret, "POST", endPoint, uri, httpPort);
			
			var authorizationHeaderValue = $"MAC id=\"{apiKey}\", ts=\"{unixTime}\", nonce=\"{nonce}\", mac=\"{hash}\"";
			client.DefaultRequestHeaders.Add("Authorization", authorizationHeaderValue);

			//For the purposes of this sample, we will send a text SMS to the number below.
			//This is just one of the many endpoint available. For more details, check: https://www.smsglobal.com/rest-api/
			var parameters = "{\"destination\": \"61404040404\",\"message\":\"test sms\"}";
			var content = new StringContent(parameters, Encoding.UTF8, "application/json");
			//Remember, if the HTTP request is a GET then it must be passed to the httpRequest parameter of AuthenticationHelper.CreateMacHash()
			var responseMessage = client.PostAsync(endPoint, content).Result;

		}
	}
}
