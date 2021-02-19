using System;

namespace AppGame.Data.Local
{
    public interface ILocalPathManager
    {
        string GetStreamingFilePath(string filename);

        string GetPersistentFilePath(string filename);
    }
}