﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BoardGame.Infrastractures
{
    public class JWTHelper(IOptionsMonitor<JWTSettingsOptions> settings)
    {
        private readonly JWTSettingsOptions _settings = settings.CurrentValue;

        public string GenerateToken(ObjectId Id, string account, string role, int expireHours = 1)
        {
            var token = BuildToken(Id, account, role);

            return token;
        }

        private string BuildToken(ObjectId Id, string account, string role)
        {
            List<Claim> claims =
            [
                new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                new Claim(ClaimTypes.Name, account),
                new Claim(ClaimTypes.Role, role),
            ];

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_settings.IssuerSigningKey));
            var cred = new SigningCredentials(key, _settings.Algorithm);
            var token = new JwtSecurityToken
            (
                issuer: _settings.ValidIssuer,
                audience: _settings.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddSeconds(double.Parse(_settings.ExpiredTime.ToString()!)),
                signingCredentials: cred
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }

    /// <summary>
    /// Jwt settings
    /// </summary>
    public sealed class JWTSettingsOptions
    {
        /// <summary>
        /// Whether to validate the issuer signing key.
        /// </summary>
        public bool? ValidateIssuerSigningKey { get; set; }

        /// <summary>
        /// The issuer signing key.
        /// </summary>
        public string IssuerSigningKey { get; set; } = string.Empty;

        /// <summary>
        /// Whether to validate the issuer.
        /// </summary>
        public bool? ValidateIssuer { get; set; }

        /// <summary>
        /// The valid issuer.
        /// </summary>
        public string ValidIssuer { get; set; } = string.Empty;

        /// <summary>
        /// Whether to validate the audience.
        /// </summary>
        public bool? ValidateAudience { get; set; }

        /// <summary>
        /// The valid audience.
        /// </summary>
        public string ValidAudience { get; set; } = string.Empty;

        /// <summary>
        ///  Whether to validate the lifetime.
        /// </summary>
        public bool? ValidateLifetime { get; set; }

        /// <summary>
        /// The clock skew value (in seconds) to tolerate time discrepancies between servers.
        /// </summary>
        public long? ClockSkew { get; set; }

        /// <summary>
        /// The expiration time (in minutes).
        /// </summary>
        public long? ExpiredTime { get; set; }

        /// <summary>
        /// The encryption algorithm.
        /// </summary>
        public string Algorithm { get; set; } = string.Empty;

        /// <summary>
        /// Whether to validate the expiration time. Set to false for no expiration. The default value is true.
        /// </summary>
        public bool ExpirationEnabled { get; set; } = true;
    }
}
