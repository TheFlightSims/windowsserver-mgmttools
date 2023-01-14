Namespace Models
    Public Class ExtendedServiceModel
        Property Service As ServiceModel
        Property DependsOn As List(Of ServiceModel)
        Property DependOnThisService As List(Of ServiceModel)
    End Class
End Namespace