using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using dotnet_learning.Data;
using dotnet_learning.Entities;
using dotnet_learning.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static dotnet_learning.installers.JwtInstaller;

namespace dotnet_learning.Services
{
    public class AccountService : IAccountService
    {
        public DatabaseContext DatabaseContext { get; }
        public JwtSettings JwtSetting { get; }

        public AccountService(DatabaseContext databaseContext, JwtSettings jwtSetting)
        {
            DatabaseContext = databaseContext;
            JwtSetting = jwtSetting;
        }

        public string GenerateToken(Account account)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub , account.Username),
                new Claim("role", account.Role.Name),
                new Claim("additional","todo")
            };
            
            return buildToken(claims);
        }

        public Account GetInfo(string accessToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Account?> Login(string username, string password)
        {
            var account = await DatabaseContext.Accounts.Include(a => a.Role).SingleOrDefaultAsync(a => a.Username == username);
            if (account != null && VerifyPassword(account.Password, password))
            {
                return account;
            }
            return null;
        }

        public async Task Register(Account account)
        {
            var existingAccount = await DatabaseContext.Accounts.SingleOrDefaultAsync(a => a.Username == account.Username);
            if (existingAccount != null)
            {
                throw new Exception("Existing Account");
            }
            account.Password = CreatePasswordHash(account.Password);
            DatabaseContext.Accounts.Add(account);
            await DatabaseContext.SaveChangesAsync();
        }

        private String CreatePasswordHash(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 258 / 8
            ));
            return $"{Convert.ToBase64String(salt)}.{hashed}";
        }

        private bool VerifyPassword(string hashedPassword, string password)
        {
            var parts = hashedPassword.Split('.',2);
            if (parts.Length != 2)
            {
                return false;
            }
            var salt = Convert.FromBase64String(parts[0]);
            var passwordHashed = parts[1];
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 258 / 8
            ));

            return passwordHashed == hashed;
        }

        private string buildToken(Claim[] claims)
        {
            var expires = DateTime.Now.AddDays(Convert.ToDouble(JwtSetting.Expire));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSetting.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: JwtSetting.Issuer,
                audience: JwtSetting.Audience,
                claims: claims,
                expires : expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}