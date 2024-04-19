using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Demo.PL.Helper
{
    public class DocumentSettings
    {
            // IFormFile => file from html form
        public static string UploadFile(IFormFile file , string folderName)
        {
            // 1. Get Located Folder Path [Directory.GetCurrentDirectory() => project folder directory]
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\files", folderName);

            // 2. Get File Name and make it UNIQUE
            string fileName = $"{file.FileName}";
            // /*{Guid.NewGuid()}*/
            // 3. Get FilePath => FolderPath/FileName
            string filePath = Path.Combine(folderPath,fileName);

            // 4. Save file as Streams [Data Per Time] => load by time
            using var fileStream = new FileStream(filePath, FileMode.Create);
            /*
             * FileMode.Create Vs FileMode.CreateNew
             * FileMode.Create => if file already exists it will be overritten
             * FileMode.CreateNew => if file already exists an IOException will be thrown
             */
            // 5. Copy content of uploaded file to target fileStream
            file.CopyTo(fileStream);
            return fileName;
        }

        public static void DeleteFile(string fileName , string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory (),"wwwroot\\files",folderName , fileName);
            if (File.Exists(filePath)) 
                File.Delete(filePath);
        }

    }
}
