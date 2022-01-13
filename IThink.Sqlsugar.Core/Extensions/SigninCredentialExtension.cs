/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace IThink.Sqlsugar.Core
{
    /// <summary>
    /// Impl of adding a signin key for identity server 4,
    /// </summary>
    public static class SigninCredentialExtension
    {
        public static IIdentityServerBuilder AddSigninCredentialFromConfig(
            this IIdentityServerBuilder builder, IConfigurationSection options, 
            IWebHostEnvironment hostingEnvironment)
        {
            string keyType = options.GetValue<string>("KeyType");

            switch (keyType)
            {
                case "KeyFile":
                    AddCertificateFromFile(builder, options, hostingEnvironment);
                    break;

                case "KeyStore":
                    AddCertificateFromStore(builder, options);
                    break;
            }

            return builder;
        }

        private static void AddCertificateFromStore(IIdentityServerBuilder builder,
            IConfigurationSection options)
        {
            var keyIssuer = options.GetValue<string>("KeyStoreIssuer");

            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);

            var certificates = store.Certificates.Find(X509FindType.FindByIssuerName, keyIssuer, true);

            if (certificates.Count > 0)
                builder.AddSigningCredential(certificates[0]);
        }

        private static void AddCertificateFromFile(IIdentityServerBuilder builder,
            IConfigurationSection options,
            IWebHostEnvironment hostingEnvironment)
        {
            var basePath = File.Exists(hostingEnvironment.WebRootPath)
                ? Path.GetDirectoryName(hostingEnvironment.WebRootPath)
                : hostingEnvironment.ContentRootPath;

            var keyFilePath = Path.Combine(basePath, "Data", "DPAAuth.pfx");
            var keyFilePassword = options.GetValue<string>("KeyFilePassword");

            if (File.Exists(keyFilePath))
            {
                var cer = new X509Certificate2(keyFilePath, keyFilePassword);
                builder.AddSigningCredential(cer);
            }
        }
    }
}
