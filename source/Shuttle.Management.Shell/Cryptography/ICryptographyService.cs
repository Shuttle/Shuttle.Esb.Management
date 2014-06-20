namespace Shuttle.Management.Shell
{
    public interface ICryptographyService
    {
	    string TripleDESEncrypt(string plain, string key);
        string TripleDESDecrypt(string encrypted, string key);
    }
}
