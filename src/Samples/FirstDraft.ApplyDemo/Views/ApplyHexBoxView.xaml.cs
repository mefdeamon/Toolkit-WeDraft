using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FirstDraft.ApplyDemo.Views
{
    /// <summary>
    /// ApplyHexBoxView.xaml 的交互逻辑
    /// </summary>
    public partial class ApplyHexBoxView : UserControl
    {
        public ApplyHexBoxView()
        {
            InitializeComponent();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {

            FileDialog fileDialog = new OpenFileDialog()
            {


            };

            fileDialog.Filter = "所有文件(*.*)|*.*";
            if (fileDialog.ShowDialog() == true)
            {
                List<uint> result = new List<uint>();
                using (FileStream fs = new FileStream(fileDialog.FileName, FileMode.Open))
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    // 每次读取4字节，直到文件末尾
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        try
                        {
                            uint value = reader.ReadUInt32();
                            result.Add(value);
                        }
                        catch (EndOfStreamException)
                        {
                            break; // 处理文件长度不是4的倍数的情况
                        }
                    }
                }

                fileHexBox.SetArray(result.ToArray());
            }

        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            fileHexBox.Text = "";
        }
    }
}