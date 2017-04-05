namespace Resurrect.Us.Data.Services
{
    public interface IHashStrategy
    {
        string EncodeHash(long input);
        long DecodeHash(string input);
    }
}