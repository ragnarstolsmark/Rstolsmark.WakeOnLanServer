using System.Security.Cryptography.X509Certificates;
using Azure.Core;
using Azure.Identity;

namespace Rstolsmark.WakeOnLanServer.Configuration;

public static class AzureKeyVaultConfiguration
{
    public static void ConfigureAzureKeyVault(this WebApplicationBuilder builder)
    {
        var azureKeyVaultSection = builder.Configuration.GetSection("AzureKeyVault");
        if (!azureKeyVaultSection.Exists())
        {
            return;
        }
        var settings = azureKeyVaultSection.Get<AzureKeyVaultSettings>();
        TokenCredential credential;
        switch (settings.Credentials)
        {
            case AzureKeyVaultCredentials.ClientCertificate:
                credential = GetClientCertificateCredentialFromStore(settings.ClientCertificateSettings);
                break;
            default:
                credential = new DefaultAzureCredential();
                break;
        }
        builder.Configuration.AddAzureKeyVault(new Uri($"https://{settings.KeyVaultName}.vault.azure.net"), credential);
    }

    private static TokenCredential GetClientCertificateCredentialFromStore(ClientCertificateSettings settings)
    {
        using var certificateStore = new X509Store(settings.StoreLocation);
        certificateStore.Open(OpenFlags.ReadOnly);
        var certificate = certificateStore.Certificates.Find
            (
                X509FindType.FindByThumbprint,
                settings.CertificateThumbprint,
                validOnly: settings.ValidOnly
            )
            .Single();
        return new ClientCertificateCredential(settings.TenantId.ToString(), settings.ClientId.ToString(), certificate);
    }
}