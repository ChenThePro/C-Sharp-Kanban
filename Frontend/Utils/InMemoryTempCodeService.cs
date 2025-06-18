namespace Frontend.Utils
{
    public class InMemoryTempCodeService
    {
        private readonly Dictionary<string, string> _store = new();
        public void Save(string email, string code) => _store[email] = code;
        public bool TryGetCode(string email, out string code) => 
            _store.TryGetValue(email, out code!);
        public void Remove(string email) => _store.Remove(email);
    }
}