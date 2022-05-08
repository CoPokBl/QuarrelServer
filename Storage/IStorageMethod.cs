using PisscordServer.Objects;

namespace PisscordServer.Storage {
    
    public interface IStorageMethod {
        
        // Control
        void Initialize();
        void Shutdown();
        
        // Users
        void AddUser(User user);
        void RemoveUser(User user);
        User GetUser(string id);
        User GetFromNameUser(string name);
        
    }
    
}