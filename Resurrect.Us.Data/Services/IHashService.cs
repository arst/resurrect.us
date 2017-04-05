namespace Resurrect.Us.Data.Services
{
    public interface IHashService
    {
        string GetHash(long input);
        long GetRecordId(string input);
    }
}