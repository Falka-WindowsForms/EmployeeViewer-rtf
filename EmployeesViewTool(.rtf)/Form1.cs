using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace EmployeesViewTool_.rtf_
{
    public partial class Form1 : Form
    {
        List<string> _XMLPath;
        List<XDocument> _docs;
        string _nodePath;
        string node;
        int icon;
        public Form1()
        {
            InitializeComponent();
            _docs = new List<XDocument>();
            _XMLPath = new List<string>();
            LoadDocuments();
            LoadContent();
        }
        private void LoadDocuments()
        {
          
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo("..\\..\\Departments");
                foreach (DirectoryInfo info in directoryInfo.GetDirectories())
                {
                    int next = _XMLPath.Count;
                    _XMLPath.Add($"{info.FullName}\\Content.xml");
                    _docs.Add(XDocument.Load(_XMLPath[next]));
                    

                }
            }
            catch(Exception err)
            {
                MessageBox.Show($"Error:\n {err}","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

        }
        private void LoadContent()
        {
            foreach (var doc in _docs)
            {
                var root = doc.Element("root");
                TreeNode rootNode = new TreeNode(root.Attribute("name").Value, 0, 1);
                var employees = root.Elements("employee");
                foreach(var employee in employees)
                {
                    TreeNode employeeNode = new TreeNode(employee.Attribute("FIO").Value, 4, 5);
                    employeeNode.ImageIndex = 2;
                    employeeNode.SelectedImageIndex = 3;
                    rootNode.Nodes.Add(employeeNode);
                }
                treeView1.Nodes.Add(rootNode);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            icon = treeView1.SelectedNode.ImageIndex;
            if (icon<1)
                return;
            node = treeView1.SelectedNode.Text;
            _nodePath = @"..\..\Departments\" + treeView1.SelectedNode.Parent?.Text+"\\" + node + ".rtf";
            if (!File.Exists(_nodePath))
            {
                MessageBox.Show("Path not exist.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                richTextBox1.LoadFile(_nodePath);
                icon = treeView1.SelectedNode.SelectedImageIndex;
            }
        }
    }
}
