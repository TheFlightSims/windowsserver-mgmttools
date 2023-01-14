Namespace Models
    Public Class ServiceModel
        Property ServiceName As String
        Property DisplayName As String

        ReadOnly Property DisplayNameDepend As String
            Get
                Return If(IsSystem, "* " & DisplayName, DisplayName)
            End Get
        End Property

        Property Description As String

        ''' <summary>
        ''' Added as param function in order to fix the error while path contains space
        ''' </summary>
        Private _PathName As String
        Public Property PathName() As String
            Get
                Return _PathName
            End Get
            Set(ByVal value As String)
                _PathName = value
                If Not String.IsNullOrEmpty(_PathName) AndAlso _PathName.Contains(" ") AndAlso (Not _PathName.StartsWith("""")) Then
                    _PathName = String.Format("""{0}""", _PathName)
                End If
            End Set
        End Property

        Property AcceptPause As Boolean
        Property AcceptStop As Boolean
        Property DesktopInteract As Boolean
        Property ErrorControl As String
        Property IsSystem As Boolean
        Property ProcessId As UInt32
        ''' <summary>
        ''' Type of the process, OwnProcess ...
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property ServiceType As String
        Property Started As Boolean
        ''' <summary>
        ''' Values: OK ..
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property Status As String
        ''' <summary>
        ''' Log On As
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property StartName As String
        ''' <summary>
        ''' Values: Running, Stopped ...
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property State As String
        ''' <summary>
        ''' Values: Running, Stopped ...
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Return empty string is status is Stopped for list view</remarks>
        ReadOnly Property StateView As String
            Get
                Return If(State = "Stopped", "", State)
            End Get
        End Property
        Property SystemName As String
        ''' <summary>
        ''' Values: Manual, Auto, Disabled
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property StartMode As String

        Public ReadOnly Property StartModeView() As String
            Get
                Return If(StartMode = "Auto", "Automatic", StartMode)
            End Get
        End Property

        Public ReadOnly Property StartNameView() As String
            Get
                If String.IsNullOrEmpty(StartName) Then Return ""

                If StartName.ToUpperInvariant.Contains("NT AUTHORITY".ToUpperInvariant) Then
                    Return StartName.Substring(13)
                End If
                Return StartName
            End Get
        End Property

        Public ReadOnly Property DescriptionView As String
            Get
                If String.IsNullOrEmpty(Description) Then Return ""
                If Description.Length < 50 Then Return Description
                Return Description.Substring(0, 50) & " ..."
            End Get
        End Property
    End Class
End Namespace