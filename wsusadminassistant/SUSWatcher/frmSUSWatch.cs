using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.UpdateServices.Administration;

using WSUSAdminAssistant;

namespace SUSWatcher
{
    public partial class frmSUSWatch : Form
    {
        private clsConfig cfg = new clsConfig();
        private clsWSUS wsus;

        private ComputerCollection ctc = new ComputerCollection();

        private class ComputerDetail
        {
            public ComputerDetail(IComputerTarget computer)
            {
                Computer = computer;
            }

            public IComputerTarget Computer;

            public Guid SusGuid
            {
                get { return Guid.Parse(this.Computer.Id); }
            }

            public string SusID
            {
                get { return this.Computer.Id; }
            }

            public string Name
            {
                get { return this.Computer.FullDomainName; }
            }
        }

        private class ComputerCollection : System.Collections.CollectionBase
        {
            public ComputerDetail this[int index]
            {
                get { return (ComputerDetail)this.List[index]; }
                set { this.List[index] = value; }
            }

            public ComputerDetail this[IComputerTarget computer]
            {
                get
                {
                    // Loop through all computers, looking for one that matches
                    foreach (ComputerDetail t in this.List)
                    {
                        // They match if both the fulldomain name *and* SUS ID match
                        if (computer.FullDomainName == t.Name && computer.Id == t.SusID)
                            // Got one - return it
                            return t;
                    }

                    // Didn't find one - return a null
                    return null;
                }
            }

            public int Add(ComputerDetail add)
            {
                return this.List.Add(add);
            }

            public int Add(IComputerTarget add)
            {
                return this.List.Add(new ComputerDetail(add));
            }

            public void Remove(int index)
            {
                // Check index is in the correct range
                if (index < 0 || index >= this.List.Count)
                    // Out of bounds - throw an exception
                    throw new IndexOutOfRangeException();

                this.List.RemoveAt(index);
            }

            public int SusIDCount(string susid)
            {
                int count = 0;

                // Loop through all computers, looking for the provided SUS ID
                foreach (ComputerDetail d in this.List)
                    if (d.SusID == susid)
                        // Found one - increment the count by one
                        count++;

                return count;
            }
        }

        public frmSUSWatch()
        {
            InitializeComponent();

            wsus = cfg.wsus;

            // Populate duplicated SUS ID grid from XML
            foreach (string susid in cfg.DefaultSusIDCollection)
            {
                DataGridViewRow r = grdSUSID.Rows[grdSUSID.Rows.Add()];

                r.Cells[susID.Index].Value = susid;
                r.Cells[susSource.Index].Value = "Previously Saved";
            }

            // Refresh display
            this.Refresh();

            // Get list of current computers
            ComputerTargetScope s = new ComputerTargetScope();
            s.IncludeDownstreamComputerTargets = true;

            ComputerTargetCollection cc = wsus.server.GetComputerTargets(s);

            // Add all existing computers to our array
            int count = 0;

            foreach (IComputerTarget c in cc)
            {
                ctc.Add(c);
                count++;
            }

            Log("Added " + count.ToString() + " computers already in database");
        }

        private void Log(string entry)
        {
            lstLog.SelectedIndex = lstLog.Items.Add(DateTime.Now.ToString("ddMMM HH:mm:ss ") + entry);
        }

        private void tim_Tick(object sender, EventArgs e)
        {
            // If it's not time, decrement the progress bar and exit
            if (prg.Value > prg.Minimum)
            {
                prg.Value--;
                return;
            }

            // Time to update SUS IDs
            prg.Value = prg.Maximum;

            // Refresh display
            this.Refresh();

            // Get list of current computers
            ComputerTargetScope s = new ComputerTargetScope();
            s.IncludeDownstreamComputerTargets = true;

            ComputerTargetCollection cc = wsus.server.GetComputerTargets(s);

            // Loop through all computers to see if we already know about them
            foreach (IComputerTarget t in cc)
            {
                // Try to find a known computer
                ComputerDetail d = ctc[t];

                if (d == null)
                {
                    // We got a new one!
                    ctc.Add(t);

                    int count = ctc.SusIDCount(t.Id);
                    Log("New computer found - " + t.FullDomainName + " (ID " + t.Id + ")  " + count.ToString() + " of this ID found.");

                    // Do we have a SUS ID we've seen associated with another PC?
                    if (count > 1)
                    {
                        // Yes!  Add it to the data grid, or update the existing entry
                        DataGridViewRow r = null;

                        foreach (DataGridViewRow gr in grdSUSID.Rows)
                            if (gr.Cells[susID.Index].Value.ToString() == t.Id)
                            {
                                // Found an existing entry - note it and break
                                r = gr;
                                break;
                            }

                        // Did we find a row, or do we need to add one?
                        if (r == null)
                            // We need to add one
                            grdSUSID.Rows.Insert(0, 1);
                            r = grdSUSID.Rows[0];

                        r.Cells[susID.Index].Value = t.Id;
                        r.Cells[susCount.Index].Value = count.ToString();
                        r.Cells[susSource.Index].Value = "Detected";
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Build list of duplicate SUS IDs and save them
            List<string> susid = new List<string>();

            foreach (DataGridViewRow r in grdSUSID.Rows)
                susid.Add(r.Cells[susID.Index].Value.ToString());

            cfg.DefaultSusIDCollection = susid.ToArray();
        }
    }
}
