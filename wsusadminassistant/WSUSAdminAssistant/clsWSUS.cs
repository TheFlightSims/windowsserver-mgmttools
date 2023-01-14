using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.UpdateServices.Administration;
using Microsoft.UpdateServices.Administration.Internal;

namespace WSUSAdminAssistant
{
    class clsWSUS
    {
        private clsConfig cfg = new clsConfig();

        private SqlConnection sql = new SqlConnection();

        private SqlCommand cmdUnapproved = new SqlCommand(@"
            use SUSDB

            select c.targetid, fulldomainname, gi.targetgroupid, gi.name into #computers
            from tbcomputertarget c with (nolock)
            join tbtargetintargetgroup g with (nolock) on g.targetid = c.targetid
            join tbtargetgroup gi with (nolock) on gi.targetgroupid = g.targetgroupid

            create clustered index ix_targetid on #computers (targetid)

            /***********************************************************************************************************************************/

            select updateid, max(revisionnumber) rn into #updaterevs
            from PUBLIC_VIEWS.vUpdate
            group by updateid

            /***********************************************************************************************************************************/

            select uc.localupdateid, ud.updateid, defaulttitle, defaultdescription, knowledgebasearticle, fulldomainname as PC, ud.arrivaldate, c.name as pcgroup into #updates
            from tbupdatestatuspercomputer uc with (nolock)
            join #computers c on c.targetid = uc.targetid
            join tbupdate u with (nolock) on u.localupdateid = uc.localupdateid and ishidden = 0
            join PUBLIC_VIEWS.vUpdate ud on ud.updateid = u.updateid and isdeclined = 0
            join #updaterevs ur on ud.updateid = ur.updateid
            left outer join PUBLIC_VIEWS.vUpdateApproval ua on ua.updateid = u.updateid and ua.computertargetgroupid = c.targetgroupid
            where uc.summarizationstate = 2 and ua.updateapprovalid is null

            create clustered index ix_localupdateid on #updates (localupdateid)
            create index ix_updateid on #updates (updateid)
            create index ix_pcgroup on #updates (PC, pcgroup)

            /***********************************************************************************************************************************/

            select ua.updateid, ua.creationdate approvaldate, ctg.name groupname into #updateapprovals
            from PUBLIC_VIEWS.vUpdateApproval ua
            join PUBLIC_VIEWS.vComputerTargetGroup ctg on ua.computertargetgroupid = ctg.computertargetgroupid
            where ua.action = 'Install'

            create clustered index ix on #updateapprovals (updateid,groupname)
            /***********************************************************************************************************************************/

            select * into #vCategory
            from PUBLIC_VIEWS.vCategory

            create clustered index ix on #vcategory (categoryid, categorytype, defaulttitle)

            /***********************************************************************************************************************************/

            select u.updateid, count(xp.defaulttitle) xp, count(v.defaulttitle) v, count(w7.defaulttitle) w7, count(w8.defaulttitle) w8,
	            count(s3.defaulttitle) s3, count(s8.defaulttitle) s8, count(s82.defaulttitle) s82, count(s12.defaulttitle) s12,
	            count(o3.defaulttitle) o3, count(o7.defaulttitle) o7, count(o10.defaulttitle) o10, count(o13.defaulttitle) o13 into #updatecategory
            from #updates u
            join vwUpdateInCategory uic on u.updateid = uic.updateid
            left outer join #vCategory xp on xp.categorytype = 'Product' and xp.defaulttitle like 'Windows XP%' and uic.categoryupdateid = xp.categoryid
            left outer join #vCategory v on v.categorytype = 'Product' and v.defaulttitle like 'Windows Vista' and uic.categoryupdateid = v.categoryid
            left outer join #vCategory w7 on w7.categorytype = 'Product' and w7.defaulttitle like 'Windows 7' and uic.categoryupdateid = w7.categoryid
            left outer join #vCategory w8 on w8.categorytype = 'Product' and w8.defaulttitle like 'Windows 8' and uic.categoryupdateid = w8.categoryid
            left outer join #vCategory s3 on s3.categorytype = 'Product' and s3.defaulttitle like 'Windows Server 2003%' and uic.categoryupdateid = s3.categoryid
            left outer join #vCategory s8 on s8.categorytype = 'Product' and s8.defaulttitle like 'Windows Server 2008' and uic.categoryupdateid = s8.categoryid
            left outer join #vCategory s82 on s82.categorytype = 'Product' and s82.defaulttitle like 'Windows Server 2008 R2' and uic.categoryupdateid = s82.categoryid
            left outer join #vCategory s12 on s12.categorytype = 'Product' and s12.defaulttitle like 'Windows Server 2012' and uic.categoryupdateid = s12.categoryid
            left outer join #vCategory o3 on o3.categorytype = 'Product' and o3.defaulttitle like 'Office 2003' and uic.categoryupdateid = o3.categoryid
            left outer join #vCategory o7 on o7.categorytype = 'Product' and o7.defaulttitle like 'Office 2007' and uic.categoryupdateid = o7.categoryid
            left outer join #vCategory o10 on o10.categorytype = 'Product' and o10.defaulttitle like 'Office 2010' and uic.categoryupdateid = o10.categoryid
            left outer join #vCategory o13 on o13.categorytype = 'Product' and o13.defaulttitle like 'Office 2013' and uic.categoryupdateid = o13.categoryid
            group by u.updateid

            /***********************************************************************************************************************************/

            select localupdateid, count(*) as PCs into #test
            from #updates
            where pcgroup = 'Testing'
            group by localupdateid

            /***********************************************************************************************************************************/

            select localupdateid, count(*) as PCs into #t
            from #updates
            where pcgroup = 'Workstations T'
            group by localupdateid

            /***********************************************************************************************************************************/

            select u.localupdateid, count(*) as PCs into #a
            from #updates u
            where pcgroup = 'Workstations A'
            group by u.localupdateid

            /***********************************************************************************************************************************/

            select u.localupdateid, count(*) as PCs into #b
            from #updates u
            where pcgroup = 'Workstations B'
            group by u.localupdateid

            /***********************************************************************************************************************************/

            select localupdateid, count(*) as PCs into #st
            from #updates
            where pcgroup = 'Servers T'
            group by localupdateid

            /***********************************************************************************************************************************/

            select localupdateid, count(*) as PCs into #ct
            from #updates
            where pcgroup = 'Minfos Workstations T'
            group by localupdateid

            /***********************************************************************************************************************************/

            select u.localupdateid, count(*) as PCs into #ca
            from #updates u
            where pcgroup = 'Minfos Workstations A'
            group by u.localupdateid

            /***********************************************************************************************************************************/

            select u.localupdateid, count(*) as PCs into #cb
            from #updates u
            where pcgroup = 'Minfos Workstations B'
            group by u.localupdateid

            /***********************************************************************************************************************************/

            select localupdateid, count(*) as PCs into #cst
            from #updates
            where pcgroup = 'Minfos Servers T'
            group by localupdateid

            /***********************************************************************************************************************************/

            select u.localupdateid, u.updateid, u.defaulttitle, u.defaultdescription, knowledgebasearticle, arrivaldate, t.PCs as [T], tua.approvaldate [T Approved],
	            aua.approvaldate [A Approved], a.PCs as [A], b.PCs as [B], st.PCs as [Servers T], stua.approvaldate [Servers T Approved],
	            ct.PCs as [Chemist T], ctua.approvaldate [Chemist T Approved], ca.PCs as [Chemist A], caua.approvaldate [Chemist A Approved], cb.PCs as [Chemist B],
	            cst.PCs as [Chemist Servers T], cstua.approvaldate [Chemist Servers T Approved], test.PCs as [Testing],
	            uc.xp, uc.v, uc.w7, uc.w8, uc.s3, uc.s8, uc.s82, uc.s12, uc.o3, uc.o7, uc.o10, uc.o13
            from (
                    select distinct localupdateid, defaultdescription, updateid, defaulttitle, knowledgebasearticle, arrivaldate
                    from #updates
                ) u
            left outer join #t t on u.localupdateid = t.localupdateid
            left outer join #updateapprovals tua on tua.updateid = u.updateid and tua.groupname = 'Workstations T'
            left outer join #a a on u.localupdateid = a.localupdateid
            left outer join #updateapprovals aua on aua.updateid = u.updateid and aua.groupname = 'Workstations A'
            left outer join #b b on u.localupdateid = b.localupdateid
            left outer join #st st on u.localupdateid = st.localupdateid
            left outer join #updateapprovals stua on stua.updateid = u.updateid and stua.groupname = 'Servers T'
            left outer join #ct ct on u.localupdateid = ct.localupdateid
            left outer join #updateapprovals ctua on ctua.updateid = u.updateid and ctua.groupname = 'Minfos Workstations T'
            left outer join #ca ca on u.localupdateid = ca.localupdateid
            left outer join #updateapprovals caua on caua.updateid = u.updateid and caua.groupname = 'Minfos Workstations A'
            left outer join #cb cb on u.localupdateid = cb.localupdateid
            left outer join #cst cst on u.localupdateid = cst.localupdateid
            left outer join #test test on u.localupdateid = test.localupdateid
            left outer join #updateapprovals cstua on cstua.updateid = u.updateid and cstua.groupname = 'Minfos Servers T'
            left outer join #updatecategory uc on uc.updateid = u.updateid
            where t.PCs > 0 or a.PCs > 0 or b.PCs > 0 or ct.PCs > 0 or ca.PCs > 0 or cb.PCs > 0 or cst.PCs > 0 or test.PCs > 0

            /***********************************************************************************************************************************/

            drop table #computers, #updaterevs, #updates, #updateapprovals, #vCategory, #updatecategory, #test, #t, #a, #b, #st, #ct, #ca, #cb, #cst
        ");

        private SqlCommand cmdApprovedUpdates = new SqlCommand(@"
            use susdb
            select fulldomainname, ipaddress, count(*) approvedupdates, max(lastsynctime) lastsynctime
            from tbupdatestatuspercomputer uc with (nolock)
            join tbcomputertarget c on c.targetid = uc.targetid
            join tbtargetintargetgroup g with (nolock) on g.targetid = c.targetid
            join tbupdate u with (nolock) on u.localupdateid = uc.localupdateid and ishidden = 0
            join PUBLIC_VIEWS.vUpdateApproval ua on ua.updateid = u.updateid and ua.computertargetgroupid = g.targetgroupid
            	and datediff(d, ua.creationdate, getdate()) > 5
            where uc.summarizationstate = 2
            group by fulldomainname, ipaddress
        ");

        private SqlCommand cmdUpdateErrors = new SqlCommand(@"
            use susdb
            select fulldomainname, ipaddress, count(*) updateerrors, max(lastsynctime) lastsynctime
            from tbupdatestatuspercomputer uc with (nolock)
            join tbcomputertarget c on c.targetid = uc.targetid
            join tbtargetintargetgroup g with (nolock) on g.targetid = c.targetid
            join tbupdate u with (nolock) on u.localupdateid = uc.localupdateid and ishidden = 0
            join PUBLIC_VIEWS.vUpdateApproval ua on ua.updateid = u.updateid and ua.computertargetgroupid = g.targetgroupid
            	and datediff(d, ua.creationdate, getdate()) > 5
            where uc.summarizationstate = 5
            group by fulldomainname, ipaddress
        ");

        private SqlCommand cmdLastUpdate = new SqlCommand(@"
            use susdb
            
            select max(deploymenttime) lastchange from tbDeployment;
        ");

        private SqlCommand cmdLastSync = new SqlCommand(@"
            use susdb
            select max(lastsynctime) lastsynctime from tbcomputertarget;
        ");

        private SqlCommand cmdUnassignedComputers = new SqlCommand(@"
            use susdb;

            select c.name, c.ipaddress
            from public_views.vcomputertarget c
            join public_views.vcomputergroupmembership cg on cg.computertargetid = c.computertargetid
            join public_views.vcomputertargetgroup g on g.computertargetgroupid = cg.computertargetgroupid and g.name = 'Unassigned Computers'
            order by c.ipaddress
        ");

        private SqlCommand cmdComputerGroups = new SqlCommand(@"
            use susdb

            select c.name, c.ipaddress, cg.name groupname
            from PUBLIC_VIEWS.vComputerTarget c
            join PUBLIC_VIEWS.vComputerGroupMembership cm on cm.computertargetid = c.computertargetid and cm.isexplicitmember = 1
            join PUBLIC_VIEWS.vComputerTargetGroup cg on cm.computertargetgroupid = cg.computertargetgroupid and cg.name not in ('Unassigned Computers', 'All Computers')
            order by c.name
        ");

        private SqlCommand cmdSupercededUpdates = new SqlCommand(@"
                use susdb

                select distinct u.updateid, u.defaulttitle
                from vwminimalupdate mu
                join public_views.vupdate u on u.updateid = mu.updateid
                join PUBLIC_VIEWS.vUpdateInstallationInfoBasic ui on ui.state = 2 and ui.updateid = mu.updateid
                where issuperseded = 1 and isdeclined = 0
                order by u.defaulttitle
            ");

        // String properties to pass status messages back to form
        public string dbStatus { get; set; }
        public string wsusStatus { get; set;}

        // Various WSUS variables, some with lazy initialisation
        
        public IUpdateServer server;

        private ComputerTargetGroupCollection _computergroups = null;
        public ComputerTargetGroupCollection computergroups
        {
            get
            {
                if (_computergroups == null)
                    _computergroups = server.GetComputerTargetGroups();

                return _computergroups;
            }
        }

        public bool CheckDBConnection()
        {
            // Check connection state to ensure it's open.
            if (sql.State == ConnectionState.Closed || sql.State == ConnectionState.Broken)
            {
                // It's not open - let's try to open it
                try
                {
                    sql.Open();

                    // If we got here, the connection is OK.
                    dbStatus = "OK";
                    return true;
                }
                catch (Exception ex)
                {
                    // That didn't work - return false and display error
                    dbStatus = "Error: " + ex.Message;
                    return false;
                }
            }
            else if (sql.State == ConnectionState.Open)
            {
                // Connection is OK.
                dbStatus = "OK";
                return true;
            }
            else
            {
                // Connection is in an unknown state - display message and return false
                dbStatus = "SQL server connection in an unknown state - " + sql.State.ToString();

                return false;
            }
        }

        public clsWSUS()
        {
            // Initialise SQL query
            sql.ConnectionString = cfg.SQLConnectionString();

            if (CheckDBConnection())
            {
                // Set connection to SQL server for all queries
                cmdUnapproved.Connection = sql;
                cmdLastUpdate.Connection = sql;
                cmdApprovedUpdates.Connection = sql;
                cmdLastSync.Connection = sql;
                cmdUnassignedComputers.Connection = sql;
                cmdUpdateErrors.Connection = sql;
                cmdComputerGroups.Connection = sql;
            }

            // Connect to WSUS server
            try
            {
                server = AdminProxy.GetUpdateServer(cfg.WSUSServer, cfg.WSUSSecureConnection);
                wsusStatus = "OK";
            }
            catch (Exception ex)
            {
                wsusStatus = "Error: " + ex.Message;
            }
        }

        public DateTime GetLastUpdated(DateTime lastupdate)
        {
            // Retrieve and parse date last updated
            cmdLastUpdate.ExecuteNonQuery();

            SqlDataReader d = cmdLastUpdate.ExecuteReader();

            DateTime lu = lastupdate;

            while (d.Read())
            {
                lu = Convert.ToDateTime(d["lastchange"].ToString());
            }

            d.Close();

            return lu;
        }

        public DataTable GetApprovedUpdates()
        {
            // Run query and return results
            cmdApprovedUpdates.ExecuteNonQuery();

            SqlDataReader r = cmdApprovedUpdates.ExecuteReader();
            DataTable t = new DataTable();

            // Load the data table
            t.Load(r);
            r.Close();

            return t;
        }

        public DataTable GetUnapprovedUpdates()
        {
            // Run query and return results
            cmdUnapproved.ExecuteNonQuery();

            SqlDataReader r = cmdUnapproved.ExecuteReader();
            DataTable t = new DataTable();

            // Load the data table
            t.Load(r);
            r.Close();

            return t;
        }

        public DataTable GetUnassignedComputers()
        {
            // Run query and return results
            cmdUnassignedComputers.ExecuteNonQuery();

            SqlDataReader r = cmdUnassignedComputers.ExecuteReader();
            DataTable t = new DataTable();

            // Load the data table
            t.Load(r);
            r.Close();

            return t;
        }

        public DataTable GetComputerGroups()
        {
            // Run query and return results
            cmdComputerGroups.ExecuteNonQuery();

            SqlDataReader r = cmdComputerGroups.ExecuteReader();
            DataTable t = new DataTable();

            // Load the data table
            t.Load(r);
            r.Close();

            return t;
        }

        public DataTable GetUpdateErrors()
        {
            // Run query and return results
            cmdUpdateErrors.ExecuteNonQuery();

            SqlDataReader r = cmdUpdateErrors.ExecuteReader();
            DataTable t = new DataTable();

            // Load the data table
            t.Load(r);
            r.Close();

            return t;
        }

        public DataTable GetSupercededUpdates()
        {
            // Run query and return results
            cmdSupercededUpdates.Connection = sql;
            cmdSupercededUpdates.ExecuteNonQuery();

            SqlDataReader r = cmdSupercededUpdates.ExecuteReader();
            DataTable t = new DataTable();

            // Load the data table
            t.Load(r);
            r.Close();

            return t;
        }

    }
}
