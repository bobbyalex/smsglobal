using System.Security.Cryptography;

namespace SmsGlobal
{
	public class AuthenticationHelper
	{
		private const char NewLineChar = '\n';

		/// <summary>
		/// Creates the mac hash as defined in https://www.smsglobal.com/rest-api/
		/// </summary>
		public static string CreateMacHash(
			long unixTime, 
			string nonce,
			string apiSecret,
			string httpRequestMethod,
			string httpRequestUri,
			string httpHost,
			string httpPort)
		{
			var stringToHash = unixTime.ToString() + NewLineChar +
				nonce + NewLineChar +
				httpRequestMethod + NewLineChar +
				httpRequestUri + NewLineChar +
				httpHost + NewLineChar +
				httpPort + NewLineChar +
				NewLineChar;

			var secretBytes = System.Text.Encoding.UTF8.GetBytes(apiSecret);
			var sha256 = new HMACSHA256(secretBytes);
			var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(stringToHash));
			return System.Convert.ToBase64String(hashedBytes);
			
		}
	}
}
