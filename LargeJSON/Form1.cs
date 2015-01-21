using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace LargeJSON
{
    public partial class Form1 : Form
    {

        List<long> nodeLocation;
        Stream s;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if(ofdLoadFile.ShowDialog() == DialogResult.OK)
            {
               s = ofdLoadFile.OpenFile();
            Thread t1 = new Thread(ReadFile);
            t1.Start();
            //ReadFile(nodeLocation);

            
            }
        }

        delegate void DLEProgress(int i);

        void Progress(int i)
        {
            progressBar1.Value = i;
        }

        delegate void DLEUpdate(List<long> l);

        void UpdateNode(List<long> l)
        {
            tvwJSON.Nodes.Clear();
            nodeLocation = l;
            TreeNode Document = new TreeNode("Document");
            foreach (long location in nodeLocation)
            {
                TreeNode node = new TreeNode(location.ToString());
                Document.Nodes.Add(node);
            }


            tvwJSON.Nodes.Add(Document);
            tvwJSON.ExpandAll();
        }


        public void  ReadFile()
        {
            List<long> nodeLocation = new List<long>();
            using (BinaryReader br = new BinaryReader(s))
            {
                //List<char> buffer = new List<char>();

                Stack<char> buffer = new Stack<char>();
                bool EOF = false;
                double counter = (double)br.BaseStream.Length / 1000;
                int x = (int)Math.Round(counter);
                while (br.BaseStream.Position != br.BaseStream.Length)
                {
                    if ((br.BaseStream.Position % x) == 0)
                    {
                        Double p = (double)br.BaseStream.Position / br.BaseStream.Length * 100;
                        int progress = (int)p;
                        Invoke(new DLEProgress(Progress), progress);
                    }
                   
                    char c = br.ReadChar();
                    bool skip = true;
                    if (c == '[' || c == ']' || c == '{' || c == '}')
                    {
                        skip = false;
                    }
                    if (skip)
                    {
                        continue;
                    }

                    if(c == '[')
                    {
                        buffer.Push(c);
                        continue;
                    }

                    if(c == '{')
                    {
                        if(buffer.Count == 1)
                            nodeLocation.Add(br.BaseStream.Position);
                        buffer.Push(c);
                        continue;
                    }
                    if(c == '}')
                    {
                        if (buffer.Peek() == '{')
                            buffer.Pop();
                        else
                            Console.WriteLine("error");
                        continue;
                    }
                    if(c == ']')
                    {
                        if (buffer.Peek() == '[')
                            buffer.Pop();
                        else
                            Console.WriteLine("error");
                        continue;
                    }
                }
                Invoke(new DLEUpdate(UpdateNode),nodeLocation);

            }
        }
    }
}
