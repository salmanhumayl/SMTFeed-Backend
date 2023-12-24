using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Service.Interface
{
    public interface IFileUtilityService
    {
        string GetFileNameWithoutExtension(string path);
        Task<string> WriteIFormFileToDisk(IFormFile file);

        Task<string> WriteIFormFileOnNetwork(IFormFile file, string fileName);

        
    }
}
