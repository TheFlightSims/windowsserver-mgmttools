<%@ Page Async="true" Title="CM App Creator" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CM_App_Creator.Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
	<ContentTemplate>
		<div class="update-panel">
			<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="0">
				<ProgressTemplate>
                    <div class="fa-4x update-progress-gif">
                        <i class="fas fa-cog fa-spin"></i>
                    </div>
                    <div class="update-progress"></div>
				</ProgressTemplate>
			</asp:UpdateProgress>

        <div id="alertError" class="alert alert-danger" runat="server" visible="false">
            <strong>Error!</strong> <span id="spanError" runat="server"></span>
        </div>

        <div id="contentdiv" runat="server">
        <div class="alert alert-info p-1"><strong>Current path:</strong>
            <label id="lblCurrentDir" runat="server" class="panel-body"></label>
        </div>

            <div class="form-group row form-group-sm">
                <div class="col-sm-4">
                    <label for="ex1">Folders</label>
                    <asp:ListBox id="ListDirs" CssClass="form-control" AutoPostBack="true" Font-Size="Small" Height="140px" Width="380px" runat="server" OnSelectedIndexChanged="ListDirs_SelectedIndexChanged" ></asp:ListBox>
                    <asp:LinkButton runat="server" ID="FolderParent" ToolTip="Previous folder" CssClass="btn btn-danger btn-sm" Text='<i class="far fa-folder-open"></i> Go back' OnClick="FolderParent_Click" />
                    <asp:LinkButton runat="server" ID="SetContentFolder" CssClass="btn btn-primary btn-sm" Text='<i class="fas fa-wrench"></i> Set Content Path' OnClick="SetContentFolder_Click" />
                </div>
                <div class="col-sm-4">
                    <label for="ex2">Files</label>
                    <asp:ListBox id="ListFiles" CssClass="form-control" AutoPostBack="true" Font-Size="Small" Height="140px" runat="server" OnSelectedIndexChanged="ListFiles_SelectedIndexChanged"></asp:ListBox>
                    <asp:Button ID="ShowInfo" CssClass="btn btn-light btn-sm" runat="server" Text="Show FileInfo" OnClick="ShowInfo_Click" />
                        <div class="form-check form-check-inline bg-light smalltext">
                            <asp:CheckBox ID="chkboxIcon2Exe" Checked="true" runat="server" CssClass="form-check-input" ClientIDMode="Static" />
                            <label class="form-check-label" id="chkboxIcon2Exelbl" for="chkboxIcon2Exe" data-toggle="tooltip" data-placement="bottom" data-delay="500" title="Automatically try to extract app-icon when selecting an .exe file (will overwrite existing app-icon)">EXE-ExtractIcon</label>
                        </div>
                </div>
                <div class="col-sm-2" id="imgdiv" data-toggle="tooltip" data-placement="left" data-delay="500" title="Icon will be automatically imported and shown here. If there is an .ico or .png file in the path it will be imported (you can also select a .EXE file and let this app try and extract an Icon)">
                <label for="ex3" id="lblappikon" runat="server">AppIcon</label>
                    <div class="panel panel-default">
                        <asp:HiddenField ID="imghidden" runat="server"/>
                        <img runat="server" class="img-thumbnail" id="imgCtrl" />
                    </div>
                </div>

                <div id="fileinfo" runat="server" visible="false" class="alert alert-info alert-dismissible col-sm-2 p-2 fade show" role="alert">
                    <span class="smalltext"><b>Info</b></span><br />
                    <div id="lblFileInfo" runat="server" class="panel-body smalltext"></div>
                    <button type="button" class="close btn btn-sm p-0" data-dismiss="alert" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
            </div>

            <div id="msiinfo" runat="server" class="col-sm-2" visible="false">
                <button type="button" class="btn btn-primary btn-sm" data-toggle="collapse" data-target="#MainContent_info" id="btntable" runat="server" visible="true"></button>
                <div id="info" runat="server" class="collapse">
                <asp:GridView ID="GridView1" runat="server" CssClass="table table-sm smalltext table-hover table-striped" AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField HeaderText="Property">
                        <ItemTemplate>
                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Key") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Value">
                        <ItemTemplate>
                            <asp:Label ID="lblCountry" runat="server" Text='<%# Eval("Value") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                </asp:GridView>
                </div>
            </div>

            <hr />

        <div class="form-row">
            <div class="form-group col-auto" id="appnamediv">
                <label for="appname">AppName:&nbsp
                    <asp:LinkButton runat="server" ID="SearchCMApp" ToolTip="Search for Applications in CM based on AppName field (wildcard search is automatically done)" CssClass="btn btn-info btn-sm p-0" OnClick="SearchCMApp_Click"><i>Search CM </i><i class="fas fa-search"></i></asp:LinkButton>
                    &nbsp;<asp:LinkButton runat="server" ID="SearchOnline" ToolTip="Internet search based on AppName and Manufacturer fields" CssClass="btn btn-dark btn-sm p-0" OnClick="SearchOnline_Click"><i>Search Online </i><i class="fas fa-search"></i></asp:LinkButton>
                </label>
                <input id="appnamehidden" runat="server" hidden/>
                <input class="form-control form-control-sm is-invalid" id="appname" data-toggle="tooltip" data-placement="bottom" data-delay="500" placeholder="App Name" title="AppName has to end with number (versionnr). Approved characters before number is space, period(.) or hyphen(-)" type="text" runat="server">
            </div>
            <div class="form-group col-auto" id="appversiondiv">
                <label for="appversion">Version:</label>
                <input class="form-control form-control-sm is-invalid" id="appversion" type="text" runat="server" placeholder="App Version">
            </div>
            <div class="form-group col-3" id="appmanufacturerdiv">
                <label for="manufacturer">Manufacturer:</label>
                <input class="form-control form-control-sm is-invalid" id="manufacturer" type="text" runat="server" placeholder="Manufacturer">
            </div>
            <div class="form-group col-3">
                <select class="form-control form-control-sm bg-light text-black" id="dropdownDetectionMethods" runat="server" data-toggle="tooltip" data-placement="top" data-delay="500" title="If you choose MSI detection and select an MSI file, the productcode will be automatically imported here. You can also skip this step, the app will generate a mockup-productcode and you can adjust detection after import to CM">
                    <option value="msi">MSI product code (detection)</option>
                    <option value="powershell">Powershell (detection)</option>
                </select>
                <textarea class="form-control form-control-sm smalltext" id="detectioncode" runat="server" rows="1"></textarea>
            </div>
        </div>

        <!-- Modal -->
        <div id="myModal" class="modal" tabindex="-1" role="dialog">
            <div class="modal-dialog smalltext" id="modaldialog" runat="server" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title" id="modaltitle" runat="server"></h4>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <table class="table table-sm" id="tblmodalbodytxt" enableviewstate="true" runat="server">
                            <thead>
                                <tr>
                                <th runat="server" class="row" id="modaltblheader">Info</th>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary btn-sm" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="input-group input-group-sm mb-3">
          <div class="input-group-prepend">
            <span class="input-group-text">Content path:</span>
          </div>
            <input class="form-control input-sm appcontent" id="appcontentpath" type="text" runat="server" aria-describedby="contenthelpBlock" data-toggle="tooltip" data-placement="right" data-delay="500" title="Browse to a folder (that contains either an MSI file, or the PSADT toolkit and click the 'Set Content Path' button to populate this field automatically. The app will also calculate folder size * 2 and set as disk requirement.">
            <span><small id="contenthelpBlock" class="form-text text-muted" runat="server"></small></span>
        </div>

        <div class="input-group input-group-sm mb-3">
          <div class="input-group-prepend">
            <span class="input-group-text">Install cmd:</span>
          </div>
            <input class="form-control input-sm" id="appinstallcmd" type="text" runat="server">
        </div>

        <div class="input-group input-group-sm mb-3">
          <div class="input-group-prepend">
            <span class="input-group-text">Uninstall cmd:</span>
          </div>
            <input class="form-control input-sm" id="appuninstallcmd" type="text" runat="server">
        </div>

        <div class="input-group input-group-sm mb-3">
          <div class="input-group-prepend">
            <span class="input-group-text">Repair cmd:</span>
          </div>
            <input class="form-control input-sm" id="apprepaircmd" type="text" runat="server">
        </div>

        <%-- Inputs for Application contact info (Application Owner, Application Support Contact and Max Execution Time and Estimated Install time. Can be toggled as visible or not visible via setting in web.config --%>
        <div class="container-fluid bg-light" runat="server" id="divContactAndExectutionTimeInputs" visible="false">
            <div class="form-inline">
                <label for="appowner" class="m-2"><span class="badge badge-secondary">App Owner:</span></label>
                <input class="form-control form-control-sm m-2" id="appowner" type="text" runat="server" placeholder="App Owner">
                <label for="appcontact" class="m-2"><span class="badge badge-secondary">Support Contact:</span></label>
                <input class="form-control form-control-sm m-2" id="appcontact" type="text" runat="server" placeholder="Support Contact">
                <label for="maxexecutiontime" class="m-2"><span class="badge badge-secondary">Max Execution Time:</span></label>
                <input class="form-control form-control-sm m-2 integerinput" id="maxexecutiontime" type="text" runat="server">
                <label for="estimatedexecutiontime" class="m-2"><span class="badge badge-secondary">Est. Execution Time:</span></label>
                <input class="form-control form-control-sm m-2 integerinput" id="estimatedexecutiontime" type="text" runat="server">
                <span><small id="helpblockestimatedexectime" class="form-text text-muted" runat="server"></small></span>
            </div>
        </div>

        <div class="form-group row">
            <label for="appdescription" class="col-sm-2 col-form-label">Administrator comments:</label>
            <div class="col-sm-10">
                <textarea class="form-control input-sm" rows="2" id="appdescription" runat="server"></textarea>
                <label class="checkbox-inline"><input type="checkbox" id="chkbox_appcategories" runat="server" value="">Administrative Categories</label>
                <div id="app_categories" runat="server">
                    <asp:Listbox ID="dropdownappcategories" class="form-control input-sm" runat="server" ToolTip="Hold CTRL key to select multiple app categories" SelectionMode="Multiple"></asp:Listbox>

                </div>
            </div>
        </div>

        <div class="form-group row">
            <label for="softwarecenterdescription" class="col-sm-2 col-form-label">Description for end users:</label>
            <div class="col-sm-10">
                <textarea class="form-control input-sm" rows="2" id="softwarecenterdescription" runat="server"></textarea>
                <label class="checkbox-inline"><input type="checkbox" id="chkbox_usercategories" runat="server" value="">User Categories</label>
                <div id="usercategories" runat="server">
                    <asp:Listbox ID="dropdownusercategories" class="form-control input-sm" runat="server" ToolTip="Hold CTRL key to select multiple user categories" SelectionMode="Multiple"></asp:Listbox>

                </div>
            </div>
        </div>

        <div class="card">
            <div class="card-body p-0">
                <div class="form-group row">
                    <label for="inputPassword" class="col-sm-2 col-form-label">Settings:</label>
                    <div class="col-sm-10">
                        <div class="form-check form-check-inline">
                            <asp:CheckBox ID="chkboxDistribute" runat="server" CssClass="form-check-input" ClientIDMode="Static" />
                            <label for="chkboxDistribute" class="form-check-label">Distribute App</label>
                        </div>
                        <div class="form-check form-check-inline">
                            <asp:CheckBox ID="chkboxInteractive" runat="server" CssClass="form-check-input" ClientIDMode="Static" />
                            <label class="form-check-label" title="Sets logon requirement to 'Only when a user is logged on' and ticks 'Allow users to view and interact...'" for="chkboxInteractive">Interactive?</label>
                        </div>
                        <div class="form-check form-check-inline">
                            <asp:CheckBox ID="chkboxDeploy" Enabled="false" runat="server" CssClass="form-check-input" ClientIDMode="Static" />
                            <label class="form-check-label" id="chkboxDeploylbl" for="chkboxDeploy" title="You need to tick Distribute App in order to deploy">Deploy App?</label>
                        </div>
                        <div class="form-check form-check-inline">
                            <asp:CheckBox ID="chkboxDeployRepair" Enabled="false" runat="server" CssClass="form-check-input" ClientIDMode="Static" />
                            <label class="form-check-label" id="chkboxDeployRepairlbl" for="chkboxDeployRepair" title="You need to deploy the app to enable this (=Allow End users to attempt to repair app)">Allow Repair</label>
                        </div>
                        <div class="form-check form-check-inline">
                            <asp:CheckBox ID="chkboxDeployApproval" Enabled="false" runat="server" CssClass="form-check-input" ClientIDMode="Static" />
                            <label class="form-check-label" id="chkboxDeployApprovallbl" for="chkboxDeployApproval" title="You need to deploy the app to enable this (=An admin must approve a request..)">Approval Required</label>
                        </div>
                      <div id="mydiv">
                          <select class="form-control form-control-sm" id="dropdowncollections" runat="server">
                            <option>-Choose Collection-</option>
                        </select>

                      </div>
                    </div>
                </div>
            </div>
        </div>

        <hr />

        <div class="row">
            <div class="col-lg-4">
                <label for="ex4">Create Application:</label>
                    <asp:Button CssClass="btn btn-danger btn-sm" ID="CreateCMApp" runat="server" CausesValidation="false" Text="Create Application!" ToolTip="Fill in required fields to activate" OnClick="CreateCMApp_Click" />
            </div>

        </div>

		</div>
        </div>
	</ContentTemplate>
</asp:UpdatePanel>
        
<script type="text/javascript">
    $(document).ready(function () {
        BindControls();
        verifyappname();
    });

    function pageLoad() { // this gets fired when the UpdatePanel.Update() completes
        verifyappname();
    }

    function BindControls() {
        // Write your codes inside this function.
        $('#chkboxDistribute').on('click', function () {
            if ( $(this).is(':checked') ) {
                $("#chkboxDeploy").prop('disabled', false);
            } 
            else {
                $("#chkboxDeploy").prop("checked", false).prop('disabled', true);
                $('#mydiv').hide();
            }
        });

        $('#chkboxDeploy').on('click', function () {
            if ($(this).is(':checked')) {
                $("#chkboxDeployRepair").prop('disabled', false);
                $("#chkboxDeployApproval").prop('disabled', false);
            }
            else {
                $("#chkboxDeployRepair").prop("checked", false).prop('disabled', true);
                $("#chkboxDeployApproval").prop("checked", false).prop('disabled', true);
            }
        });

        $('#chkboxDeploy').on('click', function(){
            if ( $(this).is(':checked') ) {
                $('#mydiv').show();
            } 
            else {
                $('#mydiv').hide();
            }
        });

        $('#MainContent_chkbox_appcategories').on('click', function () {
            if ($(this).is(':checked')) {
                $('#MainContent_app_categories').show();
            }
            else {
                $('#MainContent_app_categories').hide();
            }
        });

        if ($('#MainContent_chkbox_appcategories').is(':checked')) {
            $('#MainContent_app_categories').show();
        };

        $('#MainContent_chkbox_usercategories').on('click', function(){
            if ( $(this).is(':checked') ) {
                $('#MainContent_usercategories').show();
            } 
            else {
                $('#MainContent_usercategories').hide();
            }
        });

        if ($('#MainContent_chkbox_usercategories').is(':checked')) {
            $('#MainContent_usercategories').show();
        };

        $("#MainContent_appname, #MainContent_appversion, #MainContent_manufacturer").bind('keyup mouseup cut paste', function () {
            setTimeout(function () {
                verifyappname();
            }, 100);
        });

        $("#MainContent_dropdownDetectionMethods").change(function () {
            var Value = this.value;
            if (Value == "powershell") {
                $('#MainContent_detectioncode').val("# Sample detection\n$filepath = \"$env:ProgramFiles\\Mozilla Firefox\\firefox.exe\"\nif (Test-Path $filepath) \n{\n    $appVersion = (Get-Command $filepath).FileVersionInfo.ProductVersion\n    if ($appVersion -ge '84.0.2') \n    {\n        Write-Host 'Installed'\n    }\n}");
                $("#MainContent_detectioncode").height($("#MainContent_detectioncode")[0].scrollHeight);
            }
            else if (Value == "msi") {
                $('#MainContent_detectioncode').val("");
                $("#MainContent_detectioncode").height(20);
            } 
        });
    }

    var req = Sys.WebForms.PageRequestManager.getInstance();
        req.add_endRequest(function () {
            BindControls();
    });

    function openModal() {
        $('#myModal').modal('show');
    }

    function verifyappname() {
        $('[data-toggle="tooltip"]').tooltip();

        var regexinteger = /^\d+$/; // check for only integer

        var MAXEXECTIME = $("#MainContent_maxexecutiontime").val();
        var APPNAME = $("#MainContent_appname").val();
        var APPVERSION = $("#MainContent_appversion").val();
        var MANUFACTURER = $("#MainContent_manufacturer").val();

        if (/ (\d)+$|\-(\d)+$|\.(\d)+$/i.test(APPNAME)) {
            $('#MainContent_appname').removeClass("is-invalid");
            $('#MainContent_appname').addClass("is-valid");
        }
        else {
            $('#MainContent_appname').removeClass("is-valid");
            $('#MainContent_appname').addClass("is-invalid");
        }

        if (Boolean(APPVERSION)) {
            $('#MainContent_appversion').removeClass("is-invalid");
            $('#MainContent_appversion').addClass("is-valid");
        }
        else {
            $('#MainContent_appversion').removeClass("is-valid");
            $('#MainContent_appversion').addClass("is-invalid");
        }

        if (Boolean(MANUFACTURER)) {
            $('#MainContent_manufacturer').removeClass("is-invalid");
            $('#MainContent_manufacturer').addClass("is-valid");
        }
        else {
            $('#MainContent_manufacturer').removeClass("is-valid");
            $('#MainContent_manufacturer').addClass("is-invalid");
        }

        if (Boolean(APPNAME) && Boolean(APPVERSION) && Boolean(MANUFACTURER) && (/ (\d)+$|\-(\d)+$|\.(\d)+$/i.test(APPNAME))) {
            $('#MainContent_CreateCMApp').prop('disabled', false);
        }
        else {
            $('#MainContent_CreateCMApp').prop('disabled', true);
        }

    }
</script>
</asp:Content>
