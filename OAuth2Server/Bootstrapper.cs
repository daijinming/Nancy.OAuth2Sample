using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.TinyIoc;
using Nancy.Bootstrapper;
using Nancy.Owin;
using Nancy.OAuth2;
using Nancy.Authentication.Forms;
using Nancy.Cryptography;

namespace OAuth2Server
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            Nancy.Session.CookieBasedSessions.Enable(pipelines);

            base.ApplicationStartup(container, pipelines);
        }
        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            // Default configuration for OAuth.Enable()
            
            var cryptographyConfiguration = new CryptographyConfiguration(
                new RijndaelEncryptionProvider(new PassphraseKeyGenerator("SuperSecretPass", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 })),
                new DefaultHmacProvider(new PassphraseKeyGenerator("UberSuperSecure", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 })));
            

            var formsAuthConfiguration =
               new FormsAuthenticationConfiguration()
               {
                   RedirectUrl = "/Account/Login",
                   UserMapper = container.Resolve<IUserMapper>(),
                   CryptographyConfiguration = cryptographyConfiguration
               };

            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);

            OAuth.Enable(config =>
            {   
                config.Base = "/oauth";
                config.AuthorizationRequestRoute = "/authorize";
                config.TokenRoute = "/token";
            });

            base.RequestStartup(container, pipelines, context);
        }
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {   
            StaticConfiguration.DisableErrorTraces = false;

            base.ConfigureApplicationContainer(container);
        }
    }

}