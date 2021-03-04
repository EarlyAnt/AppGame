using System;

namespace AppGame.Data.Local
{
    public interface IFileHelper
    {
        void WriteFile(string path, string name, string info);

        string ReadFile(string path, string name);

        bool DeleteFile(string path, string name);

        bool DeleteAllFile(string path);
    }
}