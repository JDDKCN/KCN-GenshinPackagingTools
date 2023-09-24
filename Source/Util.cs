using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sunny.UI;
using System.Windows.Forms;
using System.IO;

namespace KCNPackagingTools
{
    public class Util
    {
        public static string GetGamePath()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "原神-CN (YuanShen.exe)|YuanShen.exe|原神-OS|Genshinimpact.exe"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;
                    string folderPath = Path.GetDirectoryName(selectedFilePath);
                    return folderPath;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                UIForm uIForm = new UIForm();
                uIForm.ShowErrorDialog(ex.Message);
                return null;
            }
        }

        public static string GetSavePath()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Zip files (*.zip)|*.zip"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return saveFileDialog.FileName;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                UIForm uIForm = new UIForm();
                uIForm.ShowErrorDialog(ex.Message);
                return null;
            }
        }

        public static bool IsPath(string pathA, string pathB)
        {
            try
            {
                string fullPathA = Path.GetFullPath(pathA);
                string fullPathB = Path.GetFullPath(pathB);

                bool pathsEqual = string.Equals(fullPathA, fullPathB, StringComparison.OrdinalIgnoreCase);

                bool pathAExists = Directory.Exists(fullPathA);
                bool pathBExists = Directory.Exists(fullPathB);

                return !pathsEqual && pathAExists && pathBExists;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
