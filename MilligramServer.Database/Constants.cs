namespace MilligramServer.Database;

public static class Constants
{
    public static readonly int MaxUserLoginLength = 50;
    public static readonly int MaxUserNameLength = 50;
    public static readonly int MinUserPasswordLength = 8;
    public static readonly int MaxRoleNameLength = 50; 
    public static readonly int MaxChatNameLength = 50; 
    public static readonly int MaxContactNameLength = 50; 
    public static readonly string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MilligramDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"; 
}