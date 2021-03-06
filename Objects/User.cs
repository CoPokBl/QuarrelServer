using System;
using System.Security.Cryptography;
using System.Text;

namespace PisscordServer.Objects {
    public class User {
        public string Username;
        public string Password;
        public long CreationDate;
        public string Uuid;

        public User() { }

        public User(ExternalUser externalUser) {
            Username = externalUser.Username;
            StringBuilder builder = new StringBuilder();
            foreach (byte t in SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(externalUser.Password))) {
                builder.Append(t.ToString("x2"));
            }
            Password = builder.ToString();
            CreationDate = DateTime.Now.ToBinary();
            Uuid = Guid.NewGuid().ToString();
        }

        public ExternalUser ToExternal() {
            return new ExternalUser {
                Username = Username,
                Password = Password
            };
        }

    }
    
    public class ExternalUser {
        public string Username;
        public string Password;
    }
    
}