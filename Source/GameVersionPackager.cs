using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace KCNPackagingTools
{
    public class GameVersionPackager
    {
        public static async Task<List<string>> CompareFoldersAsync(string oldFolderPath, string newFolderPath)
        {
            List<string> differentFiles = new List<string>();

            var newFiles = Directory.GetFiles(newFolderPath, "*", SearchOption.AllDirectories);

            foreach (var newFile in newFiles)
            {
                string relativePath = newFile.Substring(newFolderPath.Length + 1);

                string oldFilePath = Path.Combine(oldFolderPath, relativePath);

                if (!File.Exists(oldFilePath) || !(await IsFileContentEqualAsync(newFile, oldFilePath)))
                {
                    differentFiles.Add(relativePath);
                }
            }

            return differentFiles;
        }

        public static async Task<bool> IsFileContentEqualAsync(string file1, string file2)
        {
            using (var hash1 = MD5.Create())
            using (var hash2 = MD5.Create())
            using (var stream1 = File.OpenRead(file1))
            using (var stream2 = File.OpenRead(file2))
            {
                var hashBytes1 = await hash1.ComputeHashAsync(stream1);
                var hashBytes2 = await hash2.ComputeHashAsync(stream2);

                return StructuralComparisons.StructuralEqualityComparer.Equals(hashBytes1, hashBytes2);
            }
        }

        public static async Task CreateVersionSplitPackageAsync(string outputPath, string newFolderPath, List<string> differentFiles)
        {
            using (ZipArchive archive = ZipFile.Open(outputPath, ZipArchiveMode.Create))
            {
                foreach (var filePath in differentFiles)
                {
                    string fullPath = Path.Combine(newFolderPath, filePath);
                    string entryName = filePath.Replace("\\", "/");

                    // 获取相对文件夹路径
                    string relativeFolderPath = Path.GetDirectoryName(entryName);
                    if (!string.IsNullOrEmpty(relativeFolderPath))
                    {
                        string[] folders = relativeFolderPath.Split('/');
                        string currentFolder = "";
                        foreach (string folder in folders)
                        {
                            currentFolder = Path.Combine(currentFolder, folder);
                            archive.CreateEntry(currentFolder + "/");
                        }
                    }

                    ZipArchiveEntry entry = archive.CreateEntry(entryName);
                    using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                    using (Stream es = entry.Open())
                    {
                        await fs.CopyToAsync(es);
                    }
                }
            }
        }
    }

    public static class MD5Extensions
    {
        public static async Task<byte[]> ComputeHashAsync(this MD5 md5, Stream stream)
        {
            return await Task.Run(() => md5.ComputeHash(stream));
        }
    }
}
