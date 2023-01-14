Imports System.Globalization
Imports System.Management
Imports System.Runtime.InteropServices
Imports System.Security.Principal
Imports WinMan.Services.Models

Public Class Util
    Public Shared Async Function GetServicesAsync() As Task(Of List(Of ServiceModel))
        Dim rtnVal = Await Task.Run(Function()
                                        Return GetServices(".")
                                    End Function)
        Return rtnVal
    End Function

    Public Shared Async Function GetServiceAsync(serviceName As String) As Task(Of ServiceModel)
        Dim rtnVal = Await Task.Run(Function()
                                        Return GetService(".", serviceName)
                                    End Function)
        Return rtnVal
    End Function

    Private Shared Function GetServices(ByVal machine As String) As List(Of ServiceModel)
        _lastError = ""
        Dim context As WindowsImpersonationContext = Nothing
        Try
            Dim rtnVal As New List(Of ServiceModel)
            Dim opt As ObjectGetOptions
            opt = New ObjectGetOptions(Nothing, TimeSpan.MaxValue, True)

            Using manClass As New ManagementClass(String.Format(CultureInfo.InvariantCulture, "\\{0}\root\cimv2", machine), "Win32_Service", opt)
                Try
                    manClass.Scope.Options.EnablePrivileges = True
                    manClass.Scope.Options.Impersonation = ImpersonationLevel.Impersonate

                    For Each manObj As ManagementObject In manClass.GetInstances
                        Dim service As New ServiceModel
                        service.ServiceName = ToObjString(manObj.GetPropertyValue("Name"))
                        service.AcceptPause = ToObjBool(manObj.GetPropertyValue("AcceptPause"))
                        service.AcceptStop = ToObjBool(manObj.GetPropertyValue("AcceptStop"))
                        service.Description = ToObjString(manObj.GetPropertyValue("Description"))
                        service.DesktopInteract = ToObjBool(manObj.GetPropertyValue("DesktopInteract"))
                        service.DisplayName = ToObjString(manObj.GetPropertyValue("DisplayName"))
                        service.ErrorControl = ToObjString(manObj.GetPropertyValue("ErrorControl"))
                        service.PathName = ToObjString(manObj.GetPropertyValue("PathName"))
                        service.ProcessId = ToObjInt(manObj.GetPropertyValue("ProcessId"))
                        service.ServiceType = ToObjString(manObj.GetPropertyValue("ServiceType"))
                        service.Started = ToObjBool(manObj.GetPropertyValue("Started"))
                        service.StartMode = ToObjString(manObj.GetPropertyValue("StartMode"))
                        service.StartName = ToObjString(manObj.GetPropertyValue("StartName"))
                        service.State = ToObjString(manObj.GetPropertyValue("State"))
                        service.Status = ToObjString(manObj.GetPropertyValue("Status"))
                        service.SystemName = ToObjString(manObj.GetPropertyValue("SystemName"))
                        service.IsSystem = False
                        rtnVal.Add(service)
                    Next
                Catch ex As COMException
                    _lastError = ex.Message
                Catch ex As Exception
                    _lastError = ex.ToString
                End Try
            End Using

            Using manClass As New ManagementClass(String.Format(CultureInfo.InvariantCulture, "\\{0}\root\cimv2", machine), "Win32_SystemDriver", opt)
                Try
                    manClass.Scope.Options.EnablePrivileges = True
                    manClass.Scope.Options.Impersonation = ImpersonationLevel.Impersonate

                    For Each manObj As ManagementObject In manClass.GetInstances
                        Dim service As New ServiceModel
                        service.IsSystem = True
                        service.ServiceName = ToObjString(manObj.GetPropertyValue("Name"))
                        service.AcceptPause = ToObjBool(manObj.GetPropertyValue("AcceptPause"))
                        service.AcceptStop = ToObjBool(manObj.GetPropertyValue("AcceptStop"))
                        service.Description = ToObjString(manObj.GetPropertyValue("Description"))
                        service.DesktopInteract = ToObjBool(manObj.GetPropertyValue("DesktopInteract"))
                        service.DisplayName = ToObjString(manObj.GetPropertyValue("DisplayName"))
                        service.ErrorControl = ToObjString(manObj.GetPropertyValue("ErrorControl"))
                        service.PathName = ToObjString(manObj.GetPropertyValue("PathName"))
                        service.ServiceType = ToObjString(manObj.GetPropertyValue("ServiceType"))
                        service.Started = ToObjBool(manObj.GetPropertyValue("Started"))
                        service.StartMode = ToObjString(manObj.GetPropertyValue("StartMode"))
                        service.StartName = ToObjString(manObj.GetPropertyValue("StartName"))
                        service.State = ToObjString(manObj.GetPropertyValue("State"))
                        service.Status = ToObjString(manObj.GetPropertyValue("Status"))
                        service.SystemName = ToObjString(manObj.GetPropertyValue("SystemName"))
                        rtnVal.Add(service)
                    Next
                Catch ex As COMException
                    _lastError = ex.Message
                Catch ex As Exception
                    _lastError = ex.ToString
                End Try
            End Using
            Return rtnVal
        Catch ex As Exception
            _lastError = ex.ToString
            Return Nothing
        End Try
    End Function

    Private Shared Function GetService(ByVal machine As String, serviceName As String) As ServiceModel
        _lastError = ""
        Dim context As WindowsImpersonationContext = Nothing
        Try
            Dim rtnVal As New List(Of ServiceModel)
            Dim opt As ObjectGetOptions
            opt = New ObjectGetOptions(Nothing, TimeSpan.MaxValue, True)

            Dim query = $"SELECT * FROM Win32_Service WHERE Name='{serviceName}'"

            Using searcher As New ManagementObjectSearcher(query)
                Dim result = searcher.Get()
                If result IsNot Nothing AndAlso result.Count = 1 Then
                    Dim manObj As ManagementObject = result(0)
                    Dim service As New ServiceModel
                    service.ServiceName = ToObjString(manObj.GetPropertyValue("Name"))
                    service.AcceptPause = ToObjBool(manObj.GetPropertyValue("AcceptPause"))
                    service.AcceptStop = ToObjBool(manObj.GetPropertyValue("AcceptStop"))
                    service.Description = ToObjString(manObj.GetPropertyValue("Description"))
                    service.DesktopInteract = ToObjBool(manObj.GetPropertyValue("DesktopInteract"))
                    service.DisplayName = ToObjString(manObj.GetPropertyValue("DisplayName"))
                    service.ErrorControl = ToObjString(manObj.GetPropertyValue("ErrorControl"))
                    service.PathName = ToObjString(manObj.GetPropertyValue("PathName"))
                    service.ProcessId = ToObjInt(manObj.GetPropertyValue("ProcessId"))
                    service.ServiceType = ToObjString(manObj.GetPropertyValue("ServiceType"))
                    service.Started = ToObjBool(manObj.GetPropertyValue("Started"))
                    service.StartMode = ToObjString(manObj.GetPropertyValue("StartMode"))
                    service.StartName = ToObjString(manObj.GetPropertyValue("StartName"))
                    service.State = ToObjString(manObj.GetPropertyValue("State"))
                    service.Status = ToObjString(manObj.GetPropertyValue("Status"))
                    service.SystemName = ToObjString(manObj.GetPropertyValue("SystemName"))
                    service.IsSystem = False
                    Return service
                End If
            End Using

            Dim querySystem = $"SELECT * FROM Win32_SystemDriver WHERE Name='{serviceName}'"

            Using searcher As New ManagementObjectSearcher(querySystem)
                Dim result = searcher.Get()
                If result IsNot Nothing AndAlso result.Count = 1 Then
                    Dim manObj As ManagementObject = result(0)
                    Dim service As New ServiceModel
                    service.IsSystem = True
                    service.ServiceName = ToObjString(manObj.GetPropertyValue("Name"))
                    service.AcceptPause = ToObjBool(manObj.GetPropertyValue("AcceptPause"))
                    service.AcceptStop = ToObjBool(manObj.GetPropertyValue("AcceptStop"))
                    service.Description = ToObjString(manObj.GetPropertyValue("Description"))
                    service.DesktopInteract = ToObjBool(manObj.GetPropertyValue("DesktopInteract"))
                    service.DisplayName = ToObjString(manObj.GetPropertyValue("DisplayName"))
                    service.ErrorControl = ToObjString(manObj.GetPropertyValue("ErrorControl"))
                    service.PathName = ToObjString(manObj.GetPropertyValue("PathName"))
                    service.ServiceType = ToObjString(manObj.GetPropertyValue("ServiceType"))
                    service.Started = ToObjBool(manObj.GetPropertyValue("Started"))
                    service.StartMode = ToObjString(manObj.GetPropertyValue("StartMode"))
                    service.StartName = ToObjString(manObj.GetPropertyValue("StartName"))
                    service.State = ToObjString(manObj.GetPropertyValue("State"))
                    service.Status = ToObjString(manObj.GetPropertyValue("Status"))
                    service.SystemName = ToObjString(manObj.GetPropertyValue("SystemName"))
                    Return service
                End If
            End Using
            Return Nothing
        Catch ex As Exception
            _lastError = ex.ToString
            Return Nothing
        End Try
    End Function
    Public Shared Function ControlService(serviceName As String, action As ControlType) As WmiReturn
        _lastError = ""
        Try
            Dim objPath = String.Format(CultureInfo.InvariantCulture, "\\{0}\root\cimv2:Win32_Service.Name='{1}'", ".", serviceName)
            Using objService As New ManagementObject(objPath)
                Dim outParams = objService.InvokeMethod(action.ToString, Nothing)
                Return CInt(outParams)
            End Using
        Catch ex As Exception
            _lastError = ex.ToString
            Return WmiReturn.HandledError
        End Try
    End Function

    Public Shared Async Function GetExtendedServiceAsync(serviceName) As Task(Of ExtendedServiceModel)
        Dim service = Await GetServiceAsync(serviceName)
        Dim exService As New ExtendedServiceModel
        exService.Service = service

        Dim allServices = Await GetServicesAsync()

        Dim dependsOn = Await Task.Run(Function()
                                           Return GetDependsOn(serviceName, allServices)
                                       End Function)
        Dim dependOnThisService = Await Task.Run(Function()
                                                     Return GetDependOnThisService(serviceName, allServices)
                                                 End Function)
        exService.DependsOn = dependsOn
        exService.DependOnThisService = dependOnThisService
        Return exService
    End Function

    Private Shared Function GetDependsOn(serviceName As String, services As List(Of ServiceModel)) As List(Of ServiceModel)
        Try
            Dim queryPath = String.Format("SELECT * FROM Win32_DependentService WHERE Dependent='Win32_Service.Name=\'{0}\''", serviceName)
            Dim query As New ObjectQuery(queryPath)
            Dim manScope As New ManagementScope(String.Format(CultureInfo.InvariantCulture, "\\{0}\root\cimv2", "."))
            manScope.Options.EnablePrivileges = True

            manScope.Connect()
            Dim rtnVal As New List(Of ServiceModel)
            Using mos As ManagementObjectSearcher = New ManagementObjectSearcher(manScope, query)
                Dim queryCollection As ManagementObjectCollection = mos.Get
                For Each mo As ManagementObject In queryCollection
                    Dim depServiceName = GetServiceName(mo.Item("Antecedent").ToString)
                    Dim dependsOnService = services.Where(Function(p) p.ServiceName.ToUpperInvariant = depServiceName.ToUpperInvariant).FirstOrDefault
                    If dependsOnService IsNot Nothing Then
                        rtnVal.Add(dependsOnService)
                    End If
                Next
            End Using
            Return rtnVal
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Shared Function GetDependOnThisService(serviceName As String, services As List(Of ServiceModel)) As List(Of ServiceModel)
        Try
            Dim queryPath = String.Format("SELECT * FROM Win32_DependentService WHERE Antecedent='Win32_Service.Name=\'{0}\''", serviceName)
            Dim query As New ObjectQuery(queryPath)
            Dim manScope As New ManagementScope(String.Format(CultureInfo.InvariantCulture, "\\{0}\root\cimv2", "."))
            manScope.Options.EnablePrivileges = True

            manScope.Connect()
            Dim rtnVal As New List(Of ServiceModel)
            Using mos As ManagementObjectSearcher = New ManagementObjectSearcher(manScope, query)
                Dim queryCollection As ManagementObjectCollection = mos.Get
                For Each mo As ManagementObject In queryCollection
                    Dim depServiceName = GetServiceName(mo.Item("Dependent").ToString)
                    Dim dependsOnService = services.Where(Function(p) p.ServiceName.ToUpperInvariant = depServiceName.ToUpperInvariant).FirstOrDefault
                    If dependsOnService IsNot Nothing Then
                        rtnVal.Add(dependsOnService)
                    End If
                Next
            End Using
            Return rtnVal
        Catch ex As Exception
            Return Nothing
        End Try
    End Function


    Shared _lastError As String
    Shared ReadOnly Property LastError As String
        Get
            Return _lastError
        End Get
    End Property

    Private Shared Function ToObjString(obj As Object, Optional [default] As String = "") As String
        Return If(obj Is Nothing, [default], obj.ToString)
    End Function

    Private Shared Function ToObjBool(obj As Object, Optional [default] As Boolean = False) As Boolean
        Return If(obj Is Nothing, [default], CBool(obj.ToString))
    End Function

    Private Shared Function ToObjInt(obj As Object, Optional [default] As UInt32 = 0) As UInt32
        Return If(obj Is Nothing, [default], Convert.ToUInt32(obj.ToString, CultureInfo.InvariantCulture))
    End Function

    ''' <summary>
    ''' Retrieve Service name from WMI Path
    ''' Example: \\Computer\root\cimv2:Win32_Service.Name="RpcSs"
    ''' </summary>
    ''' <param name="wmiPath">WMI Path</param>
    ''' <returns>String, service name</returns>
    ''' <remarks>I think there is another way to get the name, need to consider change this approach</remarks>
    Private Shared Function GetServiceName(wmiPath As String) As String
        Try
            wmiPath = wmiPath.ToUpperInvariant
            Dim pos = wmiPath.IndexOf(".Name=".ToUpperInvariant, StringComparison.Ordinal)
            Dim name = wmiPath.Substring(pos + ".Name=".Length)
            name = name.Replace("""", "")
            name = name.Replace("'", "")
            Return name
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function
End Class
