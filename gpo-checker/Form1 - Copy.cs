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
using System.DirectoryServices;
using Microsoft.GroupPolicy;
using System.Xml;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Win32;
using System.Collections.ObjectModel;

namespace WindowsFormsApp1
{

    public partial class Form1 : Form
    {
        private int state = 0;
        public int page = 0;
        private int maxPage = 0;
        private string path = "";
        public Gpo gpo;
        public GpoBackup gpb;
        private Dictionary<string, int> Policies = new Dictionary<string, int>();


        private const int WINDOWS_10 = 10;

        public List<Policy> Pols = new List<Policy>();

        private String[] PolicyNames = { "MinimumPasswordAge", "MaximumPasswordAge", "MinimumPasswordLength", "PasswordComplexity" };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            string s = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            path = s+ "\\SYSVOL\\domain\\Policies\\{31B2F340-016D-11D2-945F-00C04FB984F9}\\MACHINE\\Microsoft\\Windows NT\\SecEdit\\GptTmpl.inf";
            //path = s + "\\SYSVOL\\domain\\Policies\\{31B2F340-016D-11D2-945F-00C04FB984F9}\\MACHINE\\Microsoft\\WinNT\\SecEdit\\GptTmpl.inf";
            if (File.Exists(path)){
                InfoBox.Text = "Group policy files found, click Go to begin.";
            }
            else
            {
                InfoBox.Text = "Group policy files not found.\nTry navigating to your SYSVOL folder, which should be located in your windows folder in your root drive. If you cannot find this file, make sure this server is correctly set up as a domain controller.";
                FolderSearch.Visible = true;
                NextButton.Visible = false;
            }

            /*GPDomain domain = new GPDomain();
            GPSearchCriteria searchCriteria = new GPSearchCriteria();
            foreach (Gpo gpo in domain.SearchGpos(searchCriteria))
            {

            }*/

            var os = Environment.OSVersion;
            if (os.Version.Major < 6 || os.Version.Major == 6 && os.Version.Minor == 0)
            {
                MessageBox.Show("Your version of windows is unsupported by miscrosoft and completely vulnerable. Any attempt to secure this device with the current operating system is meaningless as the operating system will remain vulnerable. You should update this to the latest version of Windows Server as soon as possible.");
                this.Close();
            }else if(os.Version.Major == 6)
            {
                MessageBox.Show("You do not appear to have the latest version of windows. You are recommended to upgrade to the latest version, as newer versions generally have fewer vulnerabilities and better overall security features. Certain policies that are recommended will be unavailable to you.");
            }


        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (state == 0)
            {
                /*StreamReader sr = new StreamReader(path);
                string line = sr.ReadLine();
                string[] parsed = line.Split(new String[] { " = " }, StringSplitOptions.None);
                while (line != null)
                {
                    parsed = line.Split(new String[] { " = " },StringSplitOptions.None);
                    if (parsed.Length > 1) { 
                        Policies.Add(parsed[0], Int32.Parse(parsed[parsed.Length - 1]));
                    }

                    /*parsed = line.Split(new Char[] { ' ', '=', ' ' });
                    if (parsed.Length > 1)
                    {
                        if (Policies.ContainsKey(parsed[0]))
                        {
                            advice.Add(Policies[parsed[0]].DynamicInvoke(Int32.Parse(parsed[parsed.Length-1])) + "\n");
                        }
                    }*/
            //        line = sr.ReadLine();
            //    }
                //label1.Text = advice;


                var guid = new Guid("31B2F340-016D-11D2-945F-00C04FB984F9");
                var domain = new GPDomain(System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName);
                gpo = domain.GetGpo(guid);
                var gpoReport = gpo.GenerateReport(ReportType.Xml);

                //gpb = gpo.Backup(Path.GetTempPath(), null);
                //path = Path.GetTempPath() + "{" + gpb.Id + "}\\DomainSysvol\\GPO\\Machine\\microsoft\\windows nt\\SecEdit\\GptTmpl.inf";
                //StreamReader sr = new StreamReader(path);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(gpoReport);

                List<XmlNode> extensionData = new List<XmlNode>();
                XmlNode extIter = GetChildByName(GetChildByName(doc.LastChild, "Computer"), "ExtensionData");
                while (extIter != null)
                {
                    if (extIter.Name == "ExtensionData")
                    {
                        extensionData.Add(extIter);
                    }
                    extIter = extIter.NextSibling;
                }

                extIter = GetChildByName(GetChildByName(doc.LastChild, "User"), "ExtensionData");
                while (extIter != null)
                {
                    if (extIter.Name == "ExtensionData")
                    {
                        extensionData.Add(extIter);
                    }
                    extIter = extIter.NextSibling;
                }

                /*XmlNode extensiondata1 = GetChildByName(GetChildByName(doc.LastChild, "Computer"), "ExtensionData");
                MessageBox.Show(extensiondata1.ToString());
                XmlNode extensiondata5 = extensiondata1.NextSibling.NextSibling.NextSibling.NextSibling;
                MessageBox.Show(extensiondata5.ToString());
                XmlNode extensiondata6 = GetChildByName(GetChildByName(doc.LastChild, "User"), "ExtensionData");
                MessageBox.Show(extensiondata6.ToString());
                MessageBox.Show(extensions.ToString());*/
                var i = 2;
                foreach (XmlNode data in extensionData)
                {
                    XmlNode extensions = data.FirstChild;
                    if (data == extensionData[0])
                    {
                        foreach (XmlNode pol in extensions)
                        {
                            if (pol.Name == "q1:Account")
                            {
                                var name = GetChildByName(pol, "q1:Name");
                                var num = GetChildByName(pol, "q1:SettingNumber");
                                var tf = GetChildByName(pol, "q1:SettingBoolean");
                                if (name != null && num != null)
                                {
                                    Policies.Add(pol.ChildNodes[0].InnerText, Int32.Parse(pol.ChildNodes[1].InnerText));
                                }
                                if (name != null && tf != null)
                                {
                                    if (tf.InnerText == "true")
                                    {
                                        Policies.Add(pol.ChildNodes[0].InnerText, 1);
                                    }
                                    else
                                    {
                                        Policies.Add(pol.ChildNodes[0].InnerText, 0);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {

                        foreach (XmlNode pol in extensions)
                        {
                            if (pol.Name == "q"+i.ToString()+":Policy")
                            {
                                var name = GetChildByName(pol, "q" + i.ToString()+":Name");
                                var state = GetChildByName(pol, "q" + i.ToString()+":State");
                                if (name != null && state != null)
                                {
                                    if (state.InnerText == "Enabled")
                                    {
                                        Policies.Add(pol.ChildNodes[0].InnerText, 1);
                                    }
                                    else if (state.InnerText == "Disabled")
                                    {
                                        Policies.Add(pol.ChildNodes[0].InnerText, 0);
                                    }
                                    else
                                    {
                                        Policies.Add(pol.ChildNodes[0].InnerText, 2);
                                    }
                                }
                            }
                        }
                        i++;
                    }
                }
                


                XmlDocument policies = new XmlDocument();
                //XmlDocument updater = new XmlDocument();
                Assembly assembly = Assembly.GetExecutingAssembly();
                var a = assembly.GetManifestResourceNames();
                Stream stream = assembly.GetManifestResourceStream("WindowsFormsApp1.PoliciesXML.xml");
                //Stream stream1 = assembly.GetManifestResourceStream("WindowsFormsApp1.UpdaterXML.xml");
                policies.Load(stream);
                //updater.Load(stream1);

                XmlNode p = policies.ChildNodes[1].ChildNodes[0];
                //XmlNode u = updater.ChildNodes[1].ChildNodes[0];

                while (p!=null)
                {
                    if (p.NodeType.ToString() == "Element") { 
                        Pols.Add(new Policy(p));
                    }
                    p = p.NextSibling;
                    //u = u.NextSibling;
                }


                maxPage = Pols.Count;
                state = 1;
                GuidanceButton.Visible = true;
               // UpdateButton.Visible = true;
                MarkAsDone.Visible = true;
                NextButton.Text = "Next >>";
            }
            if (state == 1)
            {
                if (page == maxPage)
                {
                    InfoBox.Text = "";
                    state = 2;
                    PrevButton.Visible = false;
                    GuidanceButton.Visible = false;
                    //UpdateButton.Visible = false;
                }
                else
                {
                    var name = Pols[page].getName();
                    if (Policies.ContainsKey(name))
                    {
                        InfoBox.Text = Pols[page].check(Policies[name]).Trim();
                    }
                    else
                    {
                        InfoBox.Text = Pols[page].useDefault().Trim();
                    }
                    if (Pols[page].guidance() == "")
                    {
                        GuidanceButton.Visible = false;
                    }
                    else
                    {
                        GuidanceButton.Visible = true;
                    }
                    
                    page += 1;
                    if (page == maxPage)
                    {
                        NextButton.Text = "Finish";
                    }
                    if (page == 2)
                    {
                        PrevButton.Visible = true;
                    }
                }
            }
            else
            {
                this.Close();
            }

        }

        private void PrevButton_Click(object sender, EventArgs e)
        {
            if (state == 1)
            {
                page -= 2;

                var name = Pols[page].getName();
                if (Policies.ContainsKey(name))
                {
                    InfoBox.Text = Pols[page].check(Policies[name]).Trim();
                }
                else
                {
                    InfoBox.Text = Pols[page].useDefault().Trim();
                }
                if (Pols[page].guidance() == "")
                {
                    GuidanceButton.Visible = false;
                }
                else
                {
                    GuidanceButton.Visible = true;
                }

                page += 1;
                if (page == 1)
                {
                    PrevButton.Visible = false;
                }
                else
                {
                    NextButton.Text = "Next >>";
                }
            }
        }

        private void MarkAsDone_CheckedChanged(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("This will remove this page. Only do this if you are satisfied with your choices for this policy. Continue?", "Confirm done", MessageBoxButtons.OKCancel);
            if (confirmResult == DialogResult.OK)
            {
                if (page == maxPage)
                {
                    page -= 2;
                    var name = Pols[page].getName();
                    if (Policies.ContainsKey(name))
                    {
                        InfoBox.Text = Pols[page].check(Policies[name]).Trim();
                    }
                    else
                    {
                        InfoBox.Text = Pols[page].useDefault().Trim();
                    }
                    if (Pols[page].guidance() == "")
                    {
                        GuidanceButton.Visible = false;
                    }
                    else
                    {
                        GuidanceButton.Visible = true;
                    }
                    page += 1;
                    Pols.RemoveAt(page);
                    maxPage -= 1;
                }
                else
                {
                    var name = Pols[page].getName();
                    if (Policies.ContainsKey(name))
                    {
                        InfoBox.Text = Pols[page].check(Policies[name]).Trim();
                    }
                    else
                    {
                        InfoBox.Text = Pols[page].useDefault().Trim();
                    }
                    if (Pols[page].guidance() == "")
                    {
                        GuidanceButton.Visible = false;
                    }
                    else
                    {
                        GuidanceButton.Visible = true;
                    }
                    Pols.RemoveAt(page - 1);
                    maxPage -= 1;
                }
                if (page == maxPage)
                {
                    NextButton.Text = "Finish";
                }
            }
            MarkAsDone.CheckedChanged -= MarkAsDone_CheckedChanged;
            MarkAsDone.Checked = false;
            MarkAsDone.CheckedChanged += MarkAsDone_CheckedChanged;
        }

        private void FolderSearch_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                FolderSearch.Text = folderBrowserDialog1.SelectedPath;
                if (FolderSearch.Text.EndsWith("SYSVOL"))
                {
                    path = FolderSearch.Text + "\\domain\\Policies\\{31B2F340-016D-11D2-945F-00C04FB984F9}\\MACHINE\\Microsoft\\Windows NT\\SecEdit\\GptTmpl.inf";
                    FolderSearch.Visible = false;
                    NextButton.Visible = true;
                    if (File.Exists(path))
                    {
                        InfoBox.Text = "Group policy files found, click Go to begin.";
                    }
                }
                else
                {
                    MessageBox.Show("This is not the SYSVOL folder.");
                }
            }
            
        }

        public class Policy
        {
            private int Default;
            private string[] Advice;
            private int[] LThresholds;
            private int[] UThresholds;
            private string Guidance;
            public string name;

            /*public string description;
            public string header;
            public string unit;
            public string recVal;
            private string[] AdviceColour;
            private string[] AdviceUpdate;
            private int[] LAdvThrs;
            private int[] UAdvThrs;

            public int minimumSetting;
            public int maximumSetting;*/


            public Policy(XmlNode polNode)
            {
                name = polNode.Attributes["name"].InnerText;
                Default = Int32.Parse(polNode.Attributes["default"].InnerText);
                Guidance = polNode.FirstChild.InnerText;
                var num = 0;

                var size = polNode.ChildNodes[1].ChildNodes.Count;
                Advice = new string[size];
                LThresholds = new int[size];
                UThresholds = new int[size];

                foreach (XmlNode node in polNode.ChildNodes[1].ChildNodes)
                {
                    var thre = node.Attributes["threshold"].InnerText.Split('-');
                    LThresholds[num] = Int32.Parse(thre[0]);
                    UThresholds[num] = Int32.Parse(thre[1]);
                    Advice[num] = node.InnerText;
                    num++;
                }

               /* recVal = updNode.Attributes["recommended"].InnerText;
                description = updNode.FirstChild.InnerText;
                header = updNode.ChildNodes[1].InnerText;
                unit = updNode.ChildNodes[2].InnerText;

                size = updNode.ChildNodes[3].ChildNodes.Count;
                AdviceColour = new string[size];
                AdviceUpdate = new string[size];
                LAdvThrs = new int[size];
                UAdvThrs = new int[size];
                num = 0;

                foreach (XmlNode node in updNode.ChildNodes[3].ChildNodes)
                {
                    var thre = node.Attributes["threshold"].InnerText.Split('-');
                    LAdvThrs[num] = Int32.Parse(thre[0]);
                    UAdvThrs[num] = Int32.Parse(thre[1]);
                    AdviceColour[num] = node.Attributes["colour"].InnerText;
                    AdviceUpdate[num] = node.InnerText;

                    if (LAdvThrs[num] < minimumSetting) minimumSetting = LAdvThrs[num];
                    if (UAdvThrs[num] > maximumSetting) maximumSetting = UAdvThrs[num];

                    num++;
                }*/
            }

            public string check(int num)
            {

                for (var i = 0; i < Advice.Length; i++){
                    if (num <= UThresholds[i] && num >= LThresholds[i])
                    {
                        return Advice[i].Replace("%NUM%",num.ToString());
                    }
                }
                return "";
            }

            /*public Label updateAdvice(Label label, int num)
            {

                for (var i = 0; i < AdviceUpdate.Length; i++)
                {
                    if (num <= UAdvThrs[i] && num >= LAdvThrs[i])
                    {
                        label.Text = AdviceUpdate[i].Trim();
                        label.ForeColor = (AdviceColour[i] == "red") ? Color.Red : (AdviceColour[i] == "orange") ? Color.DarkOrange : Color.Green;
                        return label;
                    }
                }
                return label;
            }*/

            public string guidance()
            {
                return Guidance;
            }

            public string useDefault()
            {
                return check(Default);
            }

            public string getName()
            {
                return name;
            }
        }

        private void GuidanceButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Pols[page - 1].guidance().Trim());
        }

        public XmlNode GetChildByName(XmlNode node,string name)
        {
            XmlNode child = node.FirstChild;
            while (child != null&&child.Name != name)
            {
                child = child.NextSibling;
            }
            return child;
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            Update u = new Update();
            u.form = this;
            u.editPath = path;
            u.Show();
        }
    }
}
