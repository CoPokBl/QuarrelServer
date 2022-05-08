using System.Collections.Generic;
using PisscordServer.Objects;

namespace PisscordServer.Storage {
    
    public class RamStorage : IStorageMethod {
        private readonly List<User> _users = new List<User>();
        
        // Control
        public void Initialize() { }
        public void Shutdown() { }
        
        // Users
        public void AddUser(User user) => _users.Add(user);
        public void RemoveUser(User user) => _users.Remove(user);
        public User GetUser(string id) => _users.Find(x => x.Uuid == id);
        public User GetFromNameUser(string name) => _users.Find(x => x.Username == name);
        
    }
    
}