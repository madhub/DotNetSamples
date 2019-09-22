
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;

namespace JwtCreate
{
    class Program
    {
        public static RSACryptoServiceProvider PrivateKeyFromPemFile(String filePath)
        {
            using (TextReader privateKeyTextReader = new StringReader(File.ReadAllText(filePath)))
            {
                AsymmetricCipherKeyPair readKeyPair = (AsymmetricCipherKeyPair)new PemReader(privateKeyTextReader).ReadObject();
                RsaPrivateCrtKeyParameters privateKeyParams = ((RsaPrivateCrtKeyParameters)readKeyPair.Private);
                RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
                
                var rasParams = DotNetUtilities.ToRSAParameters(privateKeyParams);
                cryptoServiceProvider.ImportParameters(rasParams);
                return cryptoServiceProvider;
            }
        }
        static void Main(string[] args)
        {
            String privateKeyPath = @"E:\dev\java\intellijws\e2e-microservice\demo\demo\target\test-classes\privateKey.pem";
            using (var rsa = PrivateKeyFromPemFile(privateKeyPath))
            {
                RsaSecurityKey rsaSecurityKey = new RsaSecurityKey(rsa);
                ClaimsIdentity subject = new ClaimsIdentity();
                subject.AddClaim(new Claim("a", "b"));
                SigningCredentials rsaSigningCredentials =
                    new SigningCredentials(
                        rsaSecurityKey,
                        SecurityAlgorithms.RsaSha256,
                        SecurityAlgorithms.Sha256Digest);

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                //var token = tokenHandler.CreateEncodedJwt(issuer: "abc",
                //    audience: "default",
                //    subject: subject,
                //    notBefore: DateTime.UnixEpoch,
                //    expires: DateTime.UnixEpoch + TimeSpan.FromHours(1),
                //    issuedAt: DateTime.UnixEpoch,
                //    signingCredentials: rsaSigningCredentials);
                //Console.WriteLine(token);

                JwtSecurityToken jwt = tokenHandler.CreateJwtSecurityToken(
                    issuer: "abc",
                    audience: "default",
                    subject: subject,
                    notBefore: DateTime.UnixEpoch,
                    expires: DateTime.UnixEpoch + TimeSpan.FromHours(1),
                    signingCredentials: rsaSigningCredentials);
                Console.WriteLine(tokenHandler.WriteToken(jwt));
                //Console.WriteLine(jwt.RawData);
            }
            
            Console.ReadLine();
        }
    }
}
