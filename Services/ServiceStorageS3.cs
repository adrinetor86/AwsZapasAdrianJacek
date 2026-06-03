using Amazon.S3;
using Amazon.S3.Model;

namespace AwsZapasAdrianJacek.Services;

public class ServiceStorageS3
{
    private string BucketName;
    private IAmazonS3 clientS3;

    public ServiceStorageS3(IConfiguration configuration, IAmazonS3 clientS3)
    {
        this.BucketName = configuration.GetValue<string>("AWS:BucketName");
        this.clientS3 = clientS3;
    }

    public async Task<int> UploadFileAsync(string fileName, Stream stream)
    {
        PutObjectRequest request = new PutObjectRequest
        {
            Key = fileName,
            BucketName = BucketName,
            InputStream = stream
        };

        PutObjectResponse response = await clientS3.PutObjectAsync(request);

        int code = (int)response.HttpStatusCode;
        return code;
    }

    public async Task<List<string>> GetFilesAsync()
    {
        ListVersionsResponse response = await this.clientS3.ListVersionsAsync(this.BucketName);

        if (response == null || response.Versions == null || response.Versions.Count == 0)
        {
            return new List<string>();
        }

        List<string> ficheros = response.Versions.Select(x => x.Key).ToList();

        return ficheros;
    }

    public async Task DeleteFileAsync(string fileName)
    {
        DeleteObjectResponse response = await this.clientS3.DeleteObjectAsync(this.BucketName, fileName);
    }
}