public static class TempData
{
    private static readonly Dictionary<string, string> _store = new();

    public static void Save(string email, string code)
    {
        _store[email] = code;
    }

    public static bool TryGetCode(string email, out string code)
    {
        return _store.TryGetValue(email, out code);
    }

    public static void Remove(string email)
    {
        _store.Remove(email);
    }
}
