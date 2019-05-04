using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sports.API.Helper.JWT
{
	public class JWT
	{
		public JWT(IConfiguration configuration)
		{
			Configuration = configuration;
			Key = Encoding.ASCII.GetBytes(Configuration.GetSection("JWTSettings")["Secret"]);
			ExpiresInMinutes =int.Parse( Configuration.GetSection("JWTSettings")["ExpiresInMinutes"]);
			
		}

		private IConfiguration Configuration { get; }

		private byte[] Key { get; }
		private DateTime Expires { get; }
        private int ExpiresInMinutes { get; }
        /// <summary>
        /// overload method load configuration from app settings
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userRole"></param>
        /// <returns>JWT token</returns>
        public string GetTokenFor(string userId, string userRole)
		{

			return GetTokenFor(userId, userRole, ExpiresInMinutes, Key);
		}
		/// <summary>
		/// genrate JWT Token signed by app secret and contains user claims 
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="userRole"></param>
		/// <param name="expires"></param>
		/// <param name="key"></param>
		/// <returns>JWT token</returns>
		private string GetTokenFor(string userId,string userRole,int expiresInMinutes, byte [] key)
		{
            var expires = DateTime.UtcNow.AddMinutes(expiresInMinutes);
            var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, userId),
					new Claim(ClaimTypes.Role,userRole)
					
				}),
				Expires = expires,
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			var output = tokenHandler.WriteToken(token);
			return output;
		}
	}
}
