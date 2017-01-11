using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.Security;
using Nancy.Authentication.Forms;

namespace OAuth2Server
{
    public class UserService : IUserService, IUserMapper
    {   
        private List<Tuple<string, string, Guid>> users = new List<Tuple<string, string, Guid>>();

        public UserService()
        {
            users.Add(new Tuple<string, string, Guid>("admin", "password", new Guid("55E1E49E-B7E8-4EEA-8459-7A906AC4D4C0")));
            users.Add(new Tuple<string, string, Guid>("user", "password", new Guid("56E1E49E-B7E8-4EEA-8459-7A906AC4D4C0")));
        }
        
        public OAuth2UserIdentity FindUserByUsername(string username)
        {   

            return new OAuth2UserIdentity { UserName = "admin " };
        }
        
        public  Guid? ValidateUser(string username, string password)
        {
            var userRecord = users.Where(u => u.Item1 == username && u.Item2 == password).FirstOrDefault();

            if (userRecord == null)
            {   
                return null;
            }

            return userRecord.Item3;
        }

        public OAuth2UserIdentity GetUserFromToken(string token)
        {
            return new OAuth2UserIdentity { UserName = "admin " };
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {   
            var userRecord = users.Where(u => u.Item3 == identifier).FirstOrDefault();

            return userRecord == null
                       ? null
                       : new OAuth2UserIdentity { UserName = userRecord.Item1 };
        }

    }
    
 
}