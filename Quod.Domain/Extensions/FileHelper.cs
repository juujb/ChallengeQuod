using Microsoft.AspNetCore.Http;

namespace Quod.Domain
{
    public static class FormFileExtension
    {

        public async static Task<byte[]> ConvertToByteArrayAsync(this IFormFile file)
        {
            try
            {
                using var ms = new MemoryStream();

                await file.CopyToAsync(ms);
                return ms.ToArray();
            }
            catch(Exception)
            {
                return [];
            }
        }
    }
}
