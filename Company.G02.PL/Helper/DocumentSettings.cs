namespace Company.G02.PL.Helper
{
    public static class DocumentSettings
    {
        // Upload

        public static string Upload(IFormFile file, string folderName)
        {
            // 1. GetAsync Location Of Folder

            // string folderPath = $"C:\\Users\\Mcpex\\source\\repos\\Company.G02 Solution\\Company.G02.PL\\wwwroot\\files\\{folderName}";

            string folderPath =  Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\files\\{folderName}");

            // 2. GetAsync File Name And Make It Unique

            string fileName = $"{Guid.NewGuid()}{file.FileName}";

            // 3. GetAsync File Path: FolderPath + FileName

            string filePath = Path.Combine(folderPath, fileName);

            // 4. Create Object Of Type File Stream (Data per Second)

            // unmanaged resource requires opening then closing the connection
            using var FileStream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(FileStream);

            return fileName;
        }

        // Delete

        public static void Delete(string fileName, string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\files\\{folderName}", fileName);

            if(File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
