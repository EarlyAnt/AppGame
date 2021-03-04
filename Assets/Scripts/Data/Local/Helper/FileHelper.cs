using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace AppGame.Data.Local
{
    public class FileHelper : IFileHelper
    {
        private static readonly object obj = new object();

        public void WriteFile(string path, string name, string info)
        {
            lock (obj)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                StreamWriter sw;
                FileInfo file = new FileInfo(path + Path.DirectorySeparatorChar + name);
                if (!file.Exists)
                {
                    sw = file.CreateText();
                }
                else
                {
                    sw = file.AppendText();
                }
                sw.WriteLine(info);
                sw.Close();
                sw.Dispose();
            }
        }

        public string ReadFile(string path, string name)
        {
            lock (obj)
            {
                StreamReader sr = null;
                try
                {
                    sr = File.OpenText(path + Path.DirectorySeparatorChar + name);
                }
                catch (Exception e)
                {
                    String str = e.ToString();
                    Debug.Log(str);
                    return "";
                }
                string line;
                StringBuilder stringBulder = new StringBuilder();
                while ((line = sr.ReadLine()) != null)
                {
                    stringBulder.Append(line);
                }
                sr.Close();
                sr.Dispose();
                return stringBulder.ToString();
            }

        }

        public bool DeleteFile(string path, string name)
        {
            lock (obj)
            {
                if (!Directory.Exists(path))
                {
                    return false;
                }

                bool deleteStatus = false;
                FileInfo file = new FileInfo(path + Path.DirectorySeparatorChar + name);
                if (file.Exists)
                {
                    file.Delete();
                    deleteStatus = true;
                }

                return deleteStatus;
            }
        }

        public bool DeleteAllFile(string path)
        {
            lock (obj)
            {
                if (!Directory.Exists(path))
                {
                    return false;
                }

                Directory.Delete(path + Path.DirectorySeparatorChar, true);

                return true;
            }
        }
    }
}
