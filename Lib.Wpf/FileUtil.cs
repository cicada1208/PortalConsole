using Microsoft.Win32;
using System;
using System.IO;
using System.Text;

namespace Lib.Wpf
{
    public class FileUtil
    {
        /// <summary>
        /// 讀取文字檔
        /// </summary>
        public string ReadFile(string path)
        {
            string result = string.Empty;
            using (var sr = new StreamReader(path, Encoding.Default))
                result = sr.ReadToEnd();
            return result;
        }

        /// <summary>
        /// 寫入文字檔
        /// </summary>
        public bool WriteFile(string content, string path, bool append = true)
        {
            bool succ = true;

            try
            {
                using (var sw = new StreamWriter(path, append, Encoding.Default))
                    sw.WriteLine(content);
            }
            catch { succ = false; }

            return succ;
        }

        /// <summary>
        /// 寫入文字檔
        /// </summary>
        public void WriteFileDialog(string content, string fileName = "", string title = Params.MsgParam.TitleExportFile, string filter = "txt")
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = $"Files(*.{filter})|*.{filter}";
            saveFileDialog.AddExtension = true;
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.FileName = fileName;
            saveFileDialog.Title = title;
            if (saveFileDialog.ShowDialog() != true) return;

            using (var sw = new StreamWriter(saveFileDialog.FileName, true, Encoding.Default))
                sw.WriteLine(content);
        }

        /// <summary>
        /// 複製檔案
        /// </summary>
        /// <param name="sourcePath">來源資料或資料夾路徑</param>
        /// <param name="targetPath">目的資料或資料夾路徑</param>
        public bool CopyFilesRecursively(string sourcePath, string targetPath)
        {
            bool succ;

            try
            {
                // Create all of the directories
                if (Directory.Exists(sourcePath))
                {
                    if (!Directory.Exists(targetPath)) Directory.CreateDirectory(targetPath);
                    foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                        Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
                }

                // Copy all the files & Replaces any files with the same name
                if (Directory.Exists(sourcePath))
                {
                    foreach (string path in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
                        File.Copy(path, path.Replace(sourcePath, targetPath), true);
                }
                else
                    File.Copy(sourcePath, targetPath, true);

                succ = true;
            }
            catch (Exception)
            {
                succ = false;
            }

            return succ;
        }

    }
}