using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace AwsZapasAdrianJacek.Helper;

public static class HelperSecretManager
{
    public static async Task<string> GetSecretAsync()
    {
        // OJO: En tu código pone "datasecrets". 
        // Asegúrate de que ese es el nombre exacto que le has puesto en la consola de AWS.
        string secretName = "datasecrets"; 
        string region = "us-east-2";

        IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

        GetSecretValueRequest request = new GetSecretValueRequest
        {
            SecretId = secretName,
            VersionStage = "AWSCURRENT",
        };

        GetSecretValueResponse response;

        try
        {
            response = await client.GetSecretValueAsync(request);
        }
        catch (Exception e)
        {
            throw e;
        }

        // Aquí es donde "tu código va", devolviendo el string para usarlo después
        return response.SecretString; 
    }
}