using System.IO;

namespace AppGame.Data.Local
{
    public class FileManagerHelper
    {
        public static bool CheckFileExists(string filePath, string fileName){

            if(Directory.Exists(filePath)){
                return false;
            }

            FileInfo file = new FileInfo(filePath + Path.DirectorySeparatorChar + fileName + ".bak");

            return file.Exists;
        }

        public static void FileRename(string filePath, string fileName, string fileNewName){
            File.Move(filePath + Path.DirectorySeparatorChar + fileName, filePath + Path.DirectorySeparatorChar + fileNewName);
        }
    }
}