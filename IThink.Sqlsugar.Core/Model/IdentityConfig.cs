/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IThink.Sqlsugar.Core
{
    /// <summary>
    /// 数据初始化
    /// </summary>
    public static class IdentityConfig
    {
        /// <summary>
        /// 身份资源
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        /// <summary>
        /// api资源
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("api", "api")
                {
                    Scopes = new []{ "api" }
                },
                // 添加认证服务的扩展api
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
            };
        }

        /// <summary>
        /// 客户端资源
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    // api客户端
                    ClientId = "xxx_client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("xxx_secret".Sha256())
                    },
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowedScopes = { 
                        "api",
                        IdentityServerConstants.LocalApi.ScopeName},
                    AllowOfflineAccess = true
                }
            };
        }
    }
}