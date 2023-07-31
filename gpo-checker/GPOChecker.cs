using Microsoft.GroupPolicy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace GPOChecker
{

    public partial class GPOChecker : Form
    {
        private int state = 0;
        private int page = 0;
        private int section = 0;
        private int maxPage = 0;
        private string path = "";
        private Gpo gpo;
        private GpoBackup gpb;
        private Dictionary<string, int> Policies = new Dictionary<string, int>();


        private List<PolicySection> pols = new List<PolicySection>();

        public int State { get => state; set => state = value; }
        public int Page { get => page; set => page = value; }
        public int Section { get => section; set => section = value; }
        public int MaxPage { get => maxPage; set => maxPage = value; }
        public string Path { get => path; set => path = value; }
        public Gpo Gpo { get => gpo; set => gpo = value; }
        public GpoBackup Gpb { get => gpb; set => gpb = value; }
        public Dictionary<string, int> Policies1 { get => Policies; set => Policies = value; }
        public List<PolicySection> Pols { get => pols; set => pols = value; }

        public GPOChecker()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (State == 0)
            {
                try 
                {
                    var domain = new GPDomain(System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName);
                    var guid = new Guid("31B2F340-016D-11D2-945F-00C04FB984F9");
                    Gpo = domain.GetGpo(guid);
                    var gpoReport = Gpo.GenerateReport(ReportType.Xml);
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
                                        Policies1.Add(pol.ChildNodes[0].InnerText, Int32.Parse(pol.ChildNodes[1].InnerText));
                                    }
                                    if (name != null && tf != null)
                                    {
                                        if (tf.InnerText == "true")
                                        {
                                            Policies1.Add(pol.ChildNodes[0].InnerText, 1);
                                        }
                                        else
                                        {
                                            Policies1.Add(pol.ChildNodes[0].InnerText, 0);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (GetChildByName(data, "Name") != null && GetChildByName(data, "Name").InnerText == "Windows Firewall")
                            {
                                //Special case as the firewall is displayed differently

                                foreach (XmlNode pol in extensions)
                                {
                                    if (pol.Name == "q" + i + ":DomainProfile")
                                    {
                                        var lpm = GetChildByName(pol, "q" + i + ":AllowLocalPolicyMerge");
                                        var lpmState = (lpm == null) ? 2 : (lpm.InnerText == "true") ? 1 : 0;
                                        Policies1.Add("Domain Profile - Apply local firewall rules", lpmState);
                                        var efw = GetChildByName(pol, "q" + i + ":EnableFirewall");
                                        var efwState = (efw == null) ? 2 : (efw.InnerText == "true") ? 1 : 0;
                                        Policies1.Add("Domain Profile", efwState);
                                    }
                                    else if (pol.Name == "q" + i + ":PrivateProfile")
                                    {
                                        var lpm = GetChildByName(pol, "q" + i + ":AllowLocalPolicyMerge");
                                        var lpmState = (lpm == null) ? 2 : (lpm.InnerText == "true") ? 1 : 0;
                                        Policies1.Add("Private Profile - Apply local firewall rules", lpmState);
                                        var efw = GetChildByName(pol, "q" + i + ":EnableFirewall");
                                        var efwState = (efw == null) ? 2 : (efw.InnerText == "true") ? 1 : 0;
                                        Policies1.Add("Private Profile", efwState);
                                    }
                                    else if (pol.Name == "q5:PublicProfile")
                                    {
                                        var lpm = GetChildByName(pol, "q" + i + ":AllowLocalPolicyMerge");
                                        var lpmState = (lpm == null) ? 2 : (lpm.InnerText == "true") ? 1 : 0;
                                        Policies1.Add("Public Profile - Apply local firewall rules", lpmState);
                                        var efw = GetChildByName(pol, "q" + i + ":EnableFirewall");
                                        var efwState = (efw == null) ? 2 : (efw.InnerText == "true") ? 1 : 0;
                                        Policies1.Add("Public Profile", efwState);

                                    }
                                }
                            }
                            else
                            {
                                foreach (XmlNode pol in extensions)
                                {
                                    if (pol.Name == "q" + i.ToString() + ":Policy")
                                    {
                                        var name = GetChildByName(pol, "q" + i.ToString() + ":Name");
                                        var state = GetChildByName(pol, "q" + i.ToString() + ":State");
                                    }
                                }
                            }
                            i++;
                        }
                    }

                    XmlDocument policies = new XmlDocument();
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    _ = assembly.GetManifestResourceNames();
                    Stream stream = assembly.GetManifestResourceStream("GPOChecker.PoliciesXML.xml");
                    policies.Load(stream);

                    XmlNode p = policies.ChildNodes[1].ChildNodes[0];

                    while (p != null)
                    {
                        if (p.NodeType.ToString() == "Element")
                        {
                            Pols.Add(new PolicySection(p));
                        }
                        p = p.NextSibling;
                    }

                    State = 1;
                    GuidanceButton.Visible = true;
                    MarkAsDone.Visible = true;
                    NextButton.Text = "Next >>";
                    this.Text = Pols[Section].Name;

                }
                catch (Exception)
                {
                    MessageBox.Show($"Your computer is not joined into the domain, nor the domain cannot be contacted. \nThe program will exit.", $"Error", MessageBoxButtons.OK);
                }

            }

            if (State == 1)
            {
                if (!Pols[Section].isPolicyAt(Page - 1) && Page != 0 && Section == Pols.Count - 1)
                {
                    InfoBox.Text = "";
                    State = 2;
                    PrevButton.Visible = false;
                    GuidanceButton.Visible = false;
                    MarkAsDone.Visible = false;
                    NameLabel.Text = "";
                    NextButton.Text = "Finish";
                }
                else
                {
                    if (!Pols[Section].isPolicyAt(Page - 1) && Page != 0)
                    {
                        Section += 1;
                        this.Text = Pols[Section].Name;
                        Page = 0;
                    }
                    if (Page == 0)
                    {
                        NameLabel.Text = "";
                        InfoBox.Text = Pols[Section].headerText(Policies1);
                        if (Pols[Section].isSafe())
                        {
                            SkipButton.Visible = true;
                        }
                        else
                        {

                            SkipButton.Visible = false;
                        }
                        GuidanceButton.Visible = false;
                        Page += 1;
                        MarkAsDone.Visible = false;
                    }
                    else
                    {
                        SkipButton.Visible = false;
                        var currentPolicy = Pols[Section].policyAt(Page - 1);
                        var name = currentPolicy.getName();
                        NameLabel.Text = name;
                        InfoBox.Height = 329 - NameLabel.Height;
                        InfoBox.Top = 18 + NameLabel.Height;
                        if (Policies1.ContainsKey(name))
                        {
                            InfoBox.Text = currentPolicy.check(Policies1[name]).Trim();
                            NameLabel.ForeColor = (currentPolicy.isRecommended(Policies1[name])) ? currentPolicy.recColour() : currentPolicy.nonRecColour();
                        }
                        else
                        {
                            InfoBox.Text = currentPolicy.useDefault().Trim();
                            NameLabel.ForeColor = (currentPolicy.isRecommended(-1)) ? currentPolicy.recColour() : currentPolicy.nonRecColour();
                        }
                        if (currentPolicy.guidance() == "")
                        {
                            GuidanceButton.Visible = false;
                        }
                        else
                        {
                            GuidanceButton.Visible = true;
                        }

                        Page += 1;
                        PrevButton.Visible = true;
                        MarkAsDone.Visible = true;
                        //Very last page show finish instead of next
                        if (!Pols[Section].isPolicyAt(Page - 1) && Section == Pols.Count - 1)
                        {
                            NextButton.Text = "Finish";
                            MarkAsDone.Visible = false;
                        }
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
            if (State == 1)
            {
                if (Page == 1)
                {
                    Section -= 1;
                    this.Text = Pols[Section].Name;
                    Page = Pols[Section].Size;
                    NameLabel.Text = "";
                }
                else
                {
                    Page -= 2;
                }
                if (Page == 0)
                {
                    InfoBox.Text = Pols[Section].headerText(Policies1);
                    if (Pols[Section].isSafe())
                    {
                        SkipButton.Visible = true;
                    }
                    else
                    {

                        SkipButton.Visible = false;
                    }
                    GuidanceButton.Visible = false;
                    Page += 1;
                    if (Section == 0) PrevButton.Visible = false;
                    MarkAsDone.Visible = false;
                    NameLabel.Text = "";
                }
                else
                {
                    //Go back a page
                    SkipButton.Visible = false;
                    var currentPolicy = Pols[Section].policyAt(Page - 1);
                    var name = currentPolicy.getName();
                    NameLabel.Text = name;
                    InfoBox.Height = 329 - NameLabel.Height;
                    InfoBox.Top = 18 + NameLabel.Height;
                    if (Policies1.ContainsKey(name))
                    {
                        InfoBox.Text = currentPolicy.check(Policies1[name]).Trim();
                        NameLabel.ForeColor = (currentPolicy.isRecommended(Policies1[name])) ? currentPolicy.recColour() : currentPolicy.nonRecColour();
                    }
                    else
                    {
                        InfoBox.Text = currentPolicy.useDefault().Trim();
                        NameLabel.ForeColor = (currentPolicy.isRecommended(-1)) ? currentPolicy.recColour() : currentPolicy.nonRecColour();
                    }
                    if (currentPolicy.guidance() == "")
                    {
                        GuidanceButton.Visible = false;
                    }
                    else
                    {
                        GuidanceButton.Visible = true;
                    }

                    Page += 1;
                    NextButton.Text = "Next >>";

                    MarkAsDone.Visible = true;
                }
            }
        }

        private void MarkAsDone_CheckedChanged(object sender, EventArgs e)
        {
            //Remove the current page and move the the next
            var confirmResult = MessageBox.Show("This will remove this page. Only do this if you are satisfied with your choices for this policy. Continue?", "Confirm done", MessageBoxButtons.OKCancel);
            if (confirmResult == DialogResult.OK)
            {

                var name = Pols[Section].policyAt(Page - 1).getName();
                NameLabel.Text = name;
                if (Policies1.ContainsKey(name))
                {
                    InfoBox.Text = Pols[Section].policyAt(Page - 1).check(Policies1[name]).Trim();
                }
                else
                {
                    InfoBox.Text = Pols[Section].policyAt(Page - 1).useDefault().Trim();
                }
                if (Pols[Section].policyAt(Page - 1).guidance() == "")
                {
                    GuidanceButton.Visible = false;
                }
                else
                {
                    GuidanceButton.Visible = true;
                }
                Pols[Section].removePolicyAt(Page - 2);
                if (!Pols[Section].isPolicyAt(Page - 1) && Section == Pols.Count - 1)
                {
                    NextButton.Text = "Finish";
                    MarkAsDone.Visible = false;
                    NameLabel.Text = "";
                }
            }
            MarkAsDone.CheckedChanged -= MarkAsDone_CheckedChanged;
            MarkAsDone.Checked = false;
            MarkAsDone.CheckedChanged += MarkAsDone_CheckedChanged;
        }

        //The object for sections
        public class PolicySection
        {
            private string name;
            private string baseText;
            private string vulnText;
            private string invulnText;
            private int size;
            private List<Vulnerabilites> vulnerabilites = new List<Vulnerabilites>();
            private List<Policy> sectionPolicies = new List<Policy>();
            private bool safe;

            public string Name { get => name; set => name = value; }
            public string BaseText { get => baseText; set => baseText = value; }
            public string VulnText { get => vulnText; set => vulnText = value; }
            public string InvulnText { get => invulnText; set => invulnText = value; }
            public int Size { get => size; set => size = value; }
            public List<Vulnerabilites> Vulnerabilites { get => vulnerabilites; set => vulnerabilites = value; }
            public List<Policy> SectionPolicies { get => sectionPolicies; set => sectionPolicies = value; }
            public bool Safe { get => safe; set => safe = value; }

            public PolicySection(XmlNode node)
            {
                Size = Int32.Parse(node.Attributes["number"].Value);
                var iternode = node.FirstChild;
                Name = iternode.InnerText;
                iternode = iternode.NextSibling;
                BaseText = iternode.InnerText;
                iternode = iternode.NextSibling;
                VulnText = iternode.InnerText;
                iternode = iternode.NextSibling;
                InvulnText = iternode.InnerText;
                iternode = iternode.NextSibling;
                foreach (XmlNode v in iternode.ChildNodes)
                {
                    Vulnerabilites.Add(new Vulnerabilites(v));
                }
                iternode = iternode.NextSibling;
                foreach (XmlNode p in iternode.ChildNodes)
                {
                    if (p.Name == "Policy")
                    {
                        SectionPolicies.Add(new Policy(p));
                    }
                }
            }

            public string headerText(Dictionary<string, int> Policies)
            {
                List<int> vulns = new List<int>();
                int i = 0;
                foreach (Policy p in SectionPolicies)
                {
                    if (Policies.ContainsKey(p.Name))
                    {
                        if (!p.isRecommended(Policies[p.Name])) vulns.Add(i);
                    }
                    else
                    {
                        if (!p.isRecommended(-1)) vulns.Add(i);
                    }
                    i++;
                }
                String header = BaseText.Trim() + Environment.NewLine;
                Safe = vulns.Count == 0;
                if (Safe) return header + InvulnText.Trim();
                else
                {
                    String vulnStr = "";
                    foreach (Vulnerabilites v in Vulnerabilites) vulnStr += v.vulnerabilites(vulns).Trim();
                    header += VulnText.Trim() + Environment.NewLine + Environment.NewLine;
                    if (vulnStr.Length > 0) header = header.Replace("%EXAMPLES?%", " Some examples are given below.");
                    else header = header.Replace("%EXAMPLES?%", "");
                    header += vulnStr;

                }
                return header;
            }

            public Policy policyAt(int index)
            {
                return SectionPolicies[index];
            }

            public bool isPolicyAt(int index)
            {
                return index < Size && index >= 0;
            }

            public void removePolicyAt(int index)
            {
                if (index < Size && index >= 0)
                {
                    SectionPolicies.RemoveAt(index);
                    Size -= 1;
                }
            }

            public bool isSafe()
            {
                return Safe;
            }
        }

        //The object for vulnerabilities
        public class Vulnerabilites
        {
            private string vulnString;
            private int position;

            public Vulnerabilites(XmlNode node)
            {
                VulnString = node.InnerText;
                Position = Int32.Parse(node.Attributes["position"].Value);
            }

            public string VulnString { get => vulnString; set => vulnString = value; }
            public int Position { get => position; set => position = value; }

            public string vulnerabilites(List<int> pols)
            {
                return (pols.Contains(Position)) ? "\n" + VulnString : "";
            }
        }

        //The object for individual policies
        public class Policy
        {
            private int Default;
            private string[] Advice;
            private int[] LThresholds;
            private int[] UThresholds;
            private string Guidance;
            private string name;
            private int Recommended;
            private string nRColour;
            private string rColour;

            public int Default1 { get => Default; set => Default = value; }
            public string[] Advice1 { get => Advice; set => Advice = value; }
            public int[] LThresholds1 { get => LThresholds; set => LThresholds = value; }
            public int[] UThresholds1 { get => UThresholds; set => UThresholds = value; }
            public string Guidance1 { get => Guidance; set => Guidance = value; }
            public string Name { get => name; set => name = value; }
            public int Recommended1 { get => Recommended; set => Recommended = value; }
            public string NRColour { get => nRColour; set => nRColour = value; }
            public string RColour { get => rColour; set => rColour = value; }

            public Policy(XmlNode polNode)
            {
                Name = polNode.Attributes["name"].InnerText;
                Default1 = Int32.Parse(polNode.Attributes["default"].InnerText);
                Recommended1 = Int32.Parse(polNode.Attributes["recommended"].InnerText);
                RColour = (polNode.Attributes["recommendedColour"] == null) ? null : polNode.Attributes["recommendedColour"].InnerText;
                NRColour = (polNode.Attributes["nonRecommendedColour"] == null) ? null : polNode.Attributes["nonRecommendedColour"].InnerText;
                Guidance1 = polNode.FirstChild.InnerText;
                var num = 0;

                var size = polNode.ChildNodes[1].ChildNodes.Count;
                Advice1 = new string[size];
                LThresholds1 = new int[size];
                UThresholds1 = new int[size];

                foreach (XmlNode node in polNode.ChildNodes[1].ChildNodes)
                {
                    var thre = node.Attributes["threshold"].InnerText.Split('-');
                    LThresholds1[num] = Int32.Parse(thre[0]);
                    UThresholds1[num] = Int32.Parse(thre[1]);
                    Advice1[num] = node.InnerText;
                    num++;
                }
            }

            public string check(int num)
            {

                for (var i = 0; i < Advice1.Length; i++)
                {
                    if (num <= UThresholds1[i] && num >= LThresholds1[i])
                    {
                        return Advice1[i].Replace("%NUM%", num.ToString());
                    }
                }
                return "";
            }

            public string guidance()
            {
                return Guidance1;
            }

            public string useDefault()
            {
                return check(Default1);
            }

            public string getName()
            {
                return Name;
            }

            public bool isRecommended(int val) //-1 is default
            {
                if (val == -1) val = Default1;
                return val >= LThresholds1[Recommended1] && val <= UThresholds1[Recommended1];
            }

            public Color nonRecColour()
            {
                if (NRColour == null)
                {
                    return Color.Red;
                }
                else
                {
                    return Color.FromName(NRColour);
                }
            }

            public Color recColour()
            {
                if (RColour == null)
                {
                    return Color.Green;
                }
                else
                {
                    return Color.FromName(RColour);
                }
            }
        }

        //Shows guidance
        private void GuidanceButton_Click(object sender, EventArgs e)
        {
            _ = MessageBox.Show(Pols[Section].policyAt(Page - 2).guidance().Trim());
        }

        //Helper function to find named child in xml
        public XmlNode GetChildByName(XmlNode node, string name)
        {
            XmlNode child = node.FirstChild;
            while (child != null && child.Name != name)
            {
                child = child.NextSibling;
            }
            return child;
        }

        //Skip the current section
        private void SkipButton_Click(object sender, EventArgs e)
        {
            Page = Pols[Section].Size + 1;
            NextButton_Click(sender, e);
        }
    }
}
