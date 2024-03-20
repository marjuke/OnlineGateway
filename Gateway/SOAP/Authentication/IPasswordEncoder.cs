namespace SOAP.Authentication;
public interface IPasswordEncoder
{
    public string encode(string rawPassword);
    public (bool matches, bool needsUpgrade) matches(string rawPassword, string encodedPassword);
}