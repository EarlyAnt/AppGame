using System;

namespace AppGame.Data.Local
{
    public interface ILocalPathHelper
    {
        string GetStreamingFilePath(string filename);

        string GetPersistentFilePath(string filename);
    }
}