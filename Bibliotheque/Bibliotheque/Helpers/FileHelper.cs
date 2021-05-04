using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliotheque.Helpers
{
    public static class FileHelper
    {
        public static byte[] ConvertToBytes(IFormFile file)
        {
            byte[] fileContent = null;
            using (BinaryReader br = new(file.OpenReadStream()))
            {
                fileContent = br.ReadBytes((int)file.OpenReadStream().Length);
                return fileContent;
            }
        }
    }
}
