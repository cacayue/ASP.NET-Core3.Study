using System;
using System.Threading.Tasks;

namespace FileSystem
{
    public interface IFileManager
    {
        void ShowStructure(Action<int, string> render);

        Task<String> ReadAllTextAsync(string path);
    }
}