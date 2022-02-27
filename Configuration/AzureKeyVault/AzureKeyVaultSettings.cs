using System.Security.Cryptography.X509Certificates;

namespace Rstolsmark.WakeOnLanServer.Configuration;

public class AzureKeyVaultSettings
{
    public AzureKeyVaultCredentials Credentials { get; set; }
    public ClientCertificateSettings ClientCertificateSettings { get; set; }
    public string KeyVaultName { get; set; }
    
}

public class ClientCertificateSettings
{
    public StoreLocation StoreLocation { get; set; }
    public string CertificateThumbprint { get; set; }
    public bool ValidOnly { get; set; }
    public Guid TenantId { get; set; }
    public Guid ClientId { get; set; }
}

public enum AzureKeyVaultCredentials
{
    Default,
    ClientCertificate
}