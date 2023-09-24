using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sunny.UI;

namespace KCNPackagingTools
{
    public partial class Form1 : Sunny.UI.UIForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = Ver.APPName + Ver.Version;
            label5.Text = Ver.copyright;
        }

        private void button0_Click(object sender, EventArgs e)
        {
            string path = Util.GetGamePath();

            if (path == null)
            {
                return;
            }

            Structure.OldFolderPath = path;
            textBox3.Text = path;
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            string path = Util.GetGamePath();

            if (path == null)
            {
                return;
            }

            Structure.NewFolderPath = path;
            uiTextBox1.Text = path;
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            string path = Util.GetSavePath();

            if (path == null)
            {
                return;
            }

            Structure.OutputPath = path;
            uiTextBox2.Text = path;
        }

        private async void uiButton7_Click(object sender, EventArgs e)
        {
            if(!Util.IsPath(Structure.OldFolderPath, Structure.NewFolderPath))
            {
                ShowTipCode.ShowTip("填写路径有误，请检查！",textBox3);
                return;
            }

            try
            {
                uiButton7.Enabled = false;
                uiButton7.Text = "请等待...";

                Structure.DifferentFiles = await GameVersionPackager.CompareFoldersAsync(Structure.OldFolderPath, Structure.NewFolderPath);

                if(Structure.DifferentFiles == null)
                {
                    uiRichTextBox1.Text = "对比的两方文件相同或出现错误！";
                }
                else
                {
                    uiRichTextBox1.Text = string.Join(", ", Structure.DifferentFiles);
                }

                uiButton7.Enabled = true;
                uiButton7.Text = "比对数据";
            }
            catch (Exception ex)
            {
                UIForm uIForm = new UIForm();
                uIForm.ShowErrorDialog(ex.Message);
                uiButton7.Enabled = true;
                uiButton7.Text = "比对数据";
            }

        }

        private async void uiButton3_Click(object sender, EventArgs e)
        {
            if (!Util.IsPath(Structure.OldFolderPath, Structure.NewFolderPath))
            {
                ShowTipCode.ShowTip("填写路径有误，请检查！", textBox3);
                return;
            }

            try
            {
                uiButton3.Enabled = false;
                uiButton3.Text = "请等待...";

                if (Structure.DifferentFiles == null)
                {
                    Structure.DifferentFiles = await GameVersionPackager.CompareFoldersAsync(Structure.OldFolderPath, Structure.NewFolderPath);
                }

                await GameVersionPackager.CreateVersionSplitPackageAsync(Structure.OutputPath, Structure.NewFolderPath, Structure.DifferentFiles);

                uiButton3.Enabled = true;
                uiButton3.Text = "制作拆分包";
                ShowSuccessDialog("KCN-Server","制作成功！");
            }
            catch (Exception ex)
            {
                UIForm uIForm = new UIForm();
                uIForm.ShowErrorDialog(ex.Message);
                uiButton3.Enabled = true;
                uiButton3.Text = "制作拆分包";
            }
        }

        private void uiLinkLabel1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Ver.biliURL);
        }

        private void uiLinkLabel2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Ver.githubURL);
        }
    }
}
