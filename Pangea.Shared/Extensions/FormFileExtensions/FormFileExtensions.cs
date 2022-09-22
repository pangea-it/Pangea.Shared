using Microsoft.AspNetCore.Http;

namespace Pangea.Shared.Extensions.FormFileExtensions
{
    public static class FormFileExtensions
    {

        public static async Task<byte[]> GetBytes(this IFormFile formFile)
        {
            await using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
