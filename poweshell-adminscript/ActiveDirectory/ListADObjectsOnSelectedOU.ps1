#requires -version 2
<#
    .SYNOPSIS
    List all AD Objects on a selected OU, and export results into a CSV

    .DESCRIPTION
    List all AD objects on a graphically selected OU, and export results into a CSV (CommonName and Description)

    .INPUTS
    User Interface

    .OUTPUTS
    Create transcript log file similar to $ScriptDir\[SCRIPTNAME]_[YYYY_MM_DD]_[HHhMMmSSs].log
    Create a list of AD Objects, similar to $ScriptDir\[SCRIPTNAME]_FoundADObjects_[YYYY_MM_DD]_[HHhMMmSSs].csv
   
   
    .NOTES
    Version:        1.1
    Author:         ALBERT Jean-Marc
    Creation Date:  15/07/2015
    Purpose/Change: 1.0 - 2015.07.15 - ALBERT Jean-Marc - Initial script development
                  1.1 - 2015.07.16 - ALBERT Jean-Marc - Optimize code (minification)
                  1.2 - 2015.07.17 - ALBERT Jean-Marc - Add sAMAccountName and homedirectory to "Get-ADObject parameters"
                  
                                                  
    .SOURCES
  
  
    .EXAMPLE
    <None>
#>
 
#---------------------------------------------------------[Initialisations]--------------------------------------------------------
 
#Set Error Action to Silently Continue
Add-Type -AssemblyName System.Windows.Forms
$ErrorActionPreference = "SilentlyContinue"

# Powershell ActiveDirectory importation
Import-Module ActiveDirectory

#----------------------------------------------------------[Declarations]----------------------------------------------------------

#Script Version
$sScriptVersion = "1.2"

#Write script directory path on "ScriptDir" variable
$ScriptDir = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent

#Log file creation, similar to $ScriptDir\[SCRIPTNAME]_[YYYY_MM_DD].log
$SystemTime = Get-Date -uformat %Hh%Mm%Ss
$SystemDate = Get-Date -uformat %Y_%m_%d
$ScriptLogFile = "$ScriptDir\$([System.IO.Path]::GetFileNameWithoutExtension($MyInvocation.MyCommand.Definition))" + "_" + $SystemDate + "_" + $SystemTime + ".log"

#Define CSV file export of AD Group objects
$CSVExportADObjects = "$ScriptDir\$([System.IO.Path]::GetFileNameWithoutExtension($MyInvocation.MyCommand.Definition))" + "_" + "FoundADObjects" + "_" + $SystemDate + "_" + $SystemTime + ".csv"


#-----------------------------------------------------------[Functions]------------------------------------------------------------
function OnApplicationLoad {
  return $true #return true for success or false for failure
                           }

function OnApplicationExit {
  $script:ExitCode = 0 #Set the exit code for the Packager
                           }

function Stop-TranscriptOnLog {   
  Stop-Transcript
    # On met dans le transcript les retours à la ligne nécessaire à Notepad
    [string]::Join("`r`n",(Get-Content $ScriptLogFile)) | Out-File $ScriptLogFile
                              }

function Call-AD_OU_select_pff {
  #----------------------------------------------
  #region Import the Assemblies
  #----------------------------------------------
  Add-Type -AssemblyName System.Windows.Forms
  [void][reflection.assembly]::Load("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
  [void][reflection.assembly]::Load("System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
  [void][reflection.assembly]::Load("System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")
  [void][reflection.assembly]::Load("System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
  [void][reflection.assembly]::Load("System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
  [void][reflection.assembly]::Load("System.Xml, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
  [void][reflection.assembly]::Load("System.DirectoryServices, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")
  [void][reflection.assembly]::Load("System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
  [void][reflection.assembly]::Load("System.ServiceProcess, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")
  #endregion Import Assemblies

  #----------------------------------------------
  #region Generated Form Objects
  #----------------------------------------------
  [System.Windows.Forms.Application]::EnableVisualStyles()
  $formChooseOU = New-Object 'System.Windows.Forms.Form'
  $treeview1 = New-Object 'System.Windows.Forms.TreeView'
  $buttonOK = New-Object 'System.Windows.Forms.Button'
  $imagelist1 = New-Object 'System.Windows.Forms.ImageList'
  $ErrorProviderOU = New-Object 'System.Windows.Forms.ErrorProvider'
  $InitialFormWindowState = New-Object 'System.Windows.Forms.FormWindowState'
  #endregion Generated Form Objects

  #----------------------------------------------
  # User Generated Script
  #----------------------------------------------
		
  function Add-Node { 
          param ( 
              $selectedNode, 
              $dname,
              $name,
              $ou
          )
          Add-Type -AssemblyName System.Windows.Forms
    $newNode = new-object System.Windows.Forms.TreeNode  
        $newNode.Name = $dname 
        $newNode.Text = $name 
          IF($ou -eq $false) {
              $newnode.ImageIndex = 1
        $newNode.SelectedImageIndex = 1
          }
          $selectedNode.Nodes.Add($newNode) | Out-Null 
          If($dname -eq $strDomainDN){
        $newNode.ImageIndex = 2
        $newNode.SelectedImageIndex = 2
          }
          return $newNode 
  } 
	
  function Get-NextLevel {
      param (
          $selectedNode,
          $dn,
          $name,
          $OU
     )
	   
      $OUs = Get-ADObject -Filter 'ObjectClass -eq "organizationalUnit" -or ObjectClass -eq "container"' -SearchScope OneLevel -SearchBase $dn
	    
      If ($OUs -eq $null) {
          $node = Add-Node $selectedNode $dn $name $OU
      } Else {
          $node = Add-Node $selectedNode $dn $name $OU
	        
          $OUs | ForEach-Object {
              If ($_.ObjectClass -eq "organizationalUnit") {
                  $OU = $true
              }
              else {
                  $OU = $false
              }
              Get-NextLevel $node $_.distinguishedName $_.name $OU
          }
      }
  }
	
  function Build-TreeView { 
      if ($treeNodes)  
      {  
            $treeview1.Nodes.remove($treeNodes) 
          $formChooseOU.Refresh() 
      } 
	    
      $treeNodes = $treeview1.Nodes[0]
	    	    
    #Generate Module nodes 
    Get-NextLevel $treeNodes $strDomainDN $strDNSDomain
		
    $treeNodes.Expand() 
      $treeNodes.FirstNode.Expand()
  } 
	
  $treeView1.add_AfterSelect({ 
        $script:SelectedOU = $this.SelectedNode
    })
	
  $FormEvent_Load={
    $objIPProperties = [System.Net.NetworkInformation.IPGlobalProperties]::GetIPGlobalProperties()
      $strDNSDomain = $objIPProperties.DomainName.toLower()
      $strDomainDN = $strDNSDomain.toString().split('.'); foreach ($strVal in $strDomainDN) {$strTemp += "dc=$strVal,"}; $strDomainDN = $strTemp.TrimEnd(",").toLower()
    Build-TreeView
  }
	
  $buttonOK_Click={
    $script:objSelectedOU = Get-ADObject -Identity $SelectedOU.name
		
  }
	
  # --End User Generated Script--
  #----------------------------------------------
  #region Generated Events
  #----------------------------------------------
	
  $Form_StateCorrection_Load=
  {
    #Correct the initial state of the form to prevent the .Net maximized form issue
    $formChooseOU.WindowState = $InitialFormWindowState
  }
	
  $Form_Cleanup_FormClosed=
  {
    #Remove all event handlers from the controls
    try
    {
      $treeview1.remove_AfterLabelEdit($CreateOU)
      $buttonOK.remove_Click($buttonOK_Click)
      $formChooseOU.remove_Load($FormEvent_Load)
      $formChooseOU.remove_Load($Form_StateCorrection_Load)
      $formChooseOU.remove_FormClosed($Form_Cleanup_FormClosed)
    }
    catch [Exception]
    { }
  }
  #endregion Generated Events

  #----------------------------------------------
  #region Generated Form Code
  #----------------------------------------------
  #
  # formChooseOU
  #
  $formChooseOU.Controls.Add($treeview1)
  $formChooseOU.Controls.Add($buttonOK)
  $formChooseOU.AcceptButton = $buttonOK
  $formChooseOU.ClientSize = '342, 502'
  $formChooseOU.FormBorderStyle = 'FixedDialog'
  #region Binary Data
  $formChooseOU.Icon = [System.Convert]::FromBase64String('
      AAABAAkAICAQAAEABADoAgAAlgAAABgYEAABAAQA6AEAAH4DAAAQEBAAAQAEACgBAABmBQAAICAA
      AAEACACoCAAAjgYAABgYAAABAAgAyAYAADYPAAAQEAAAAQAIAGgFAAD+FQAAICAAAAEAIACoEAAA
      ZhsAABgYAAABACAAiAkAAA4sAAAQEAAAAQAgAGgEAACWNQAAKAAAACAAAABAAAAAAQAEAAAAAAAA
      AgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAIAAAIAAAACAgACAAAAAgACAAICAAACAgIAAwMDAAAAA
      /wAA/wAAAP//AP8AAAD/AP8A//8AAP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAiIAAAAAAAAAAAAAAAACIiIiHMAAAAAAAAAAAAAiI//iIiHcAAAAAAAAAAIiP//+IiIiH
      dwAAAAAAiI//////iIiIiHhwAAAAAIj///////iIiIiHeHAAAACIj////4iIiIiIiIiHgAAAiIj/
      iIiIiIj/iIiHeIcAAIiIiIiIiIiIiP+IiIeIcACIiHd3d3d3eIeI+IiHeIhwczMRMzd3czeIeI/4
      iIiIcAczN3iIiIczeId4j4iHiHAACIiP///4i7OIh4iPiHiAAAAIiI//iIu7M4h3iP+HcAAAAAiI
      iIi7u7u3iHiIiHAAAAAACIeP///4i7iHeIgAAAAAAAAAiI+IiLu7t3AAAAAAAAAAAACDMzMzMzM3
      AAAAAAAAAAAAAAAAAAAACIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////////////////////////////////////
      ///////8f///wB///gAP//AAA/8AAAH/AAAAfwAAAB8AAAAPAAAABwAAAAEAAAABgAAAAeAAAAH4
      AAAB/gAAAf+AAAP/8AAf//wAD////+f/////////////////////KAAAABgAAAAwAAAAAQAEAAAA
      AAAgAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAIAAAIAAAACAgACAAAAAgACAAICAAACAgIAAwMDA
      AAAA/wAA/wAAAP//AP8AAAD/AP8A//8AAP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAIiIAAAAAAAA
      AAAIiIiIcAAAAAAAAIiI/4iIhwAAAAAIiP//+IiIh3gAAACI/////4iHiHeAAACIj//4iIiIeIiI
      AACIiIiIiIiPiIh4iACIiId4iIiI+IiHiHCHczMzd3eIiPiHeIeDAzd4h3M3iI+Id4gAiIj//4iz
      eIiPh4gAAIiIiIu7M3iIiHcAAACIiIiIizeIiIcAAAAAiI//iLt3gAAAAAAAAIhzMzMzMAAAAAAA
      AAAAAAAAdwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD///8A////AP///wD///8A////AP///wD/
      w/8A/gH/APAA/wCAAD8AAAAfAAAADwAAAAMAAAABAAAAAAAAAAAAwAAAAPAAAAD8AAAA/wAHAP/A
      BwD///MA////AP///wAoAAAAEAAAACAAAAABAAQAAAAAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAgAAAgAAAAICAAIAAAACAAIAAgIAAAICAgADAwMAAAAD/AAD/AAAA//8A/wAAAP8A/wD//wAA
      ////AAAzMzMzMzMAAHd3d3MzMwAAd3d3czMzAACPiIiIiIMAAI+IiIiIgwAAj4iIiIiDAACPiIiI
      iIMAAI+IiIiIgwAAj4iIh3eDAACPj///94MAAI+P///3gwAAj4////eDAACPiIiId4MAAHd3d3Mz
      MwAAd3d3d3d3AAAAAAAAAAAAwAMAAMADAADAAwAAwAMAAMADAADAAwAAwAMAAMADAADAAwAAwAMA
      AMADAADAAwAAwAMAAMADAADAAwAA//8AACgAAAAgAAAAQAAAAAEACAAAAAAAAAQAAAAAAAAAAAAA
      AAEAAAABAAAAAAAAEj1BABlFSgAZSlIAIUpNAC5dXgAwV1kAMWRtAD9jaQA8aXoAVWttAFhpaQBK
      bnQATH5/AF16ewAAaZMACmuTAAl0mgA/eIIAHXqgACp+owBNd4EAS32LAGF9gAAShJgALouZADyG
      mQAKh6kAE4ujACWNqAA/jaMAMourAC6SrgAumbIAOJayAEmDjABEgpMAS4eQAFCHkABSiZIAVZOd
      AFmUngBhh4kAaIeMAGmMjQBnlZsAaZmbAHWUkwB7lJMAcJyfAHWdnwBQkKQAX5ujAEKfugBqnaEA
      UKK7AF2puwBpoaUAYqSqAGmlqQBvqq4AfaOsAHWprQB4q68Aaqm4AGqtvQB0rrIAea6xAHKxtgB5
      s7cAdbS4AHy1uQBzu74Adbm+AH64vAAXo8kAAKbUABSy1QAdtdkAHrvbACGjyQA4uM4ALL3eABi7
      4ABdssEATLbQAGOqwABmqsAAZq/AAGGtxQBqrcIAbrDBAHW8wQB5u8EAf73CAHy0yAB4v8oAZLfQ
      ADrB3wAp0t8APNDaAD7N5wA23OQAQsPfAEvO2AB5wcYAf8DEAHzCygBuw9YAdsTUAGPQ2QB92NwA
      RsTgAE3J4QBH3OsAUtDqAF7b7QBh1ecAWPz/AH3k8QB7//8AgKmqAImtsQCCsrQAhLa9AI67uwCR
      ubwAmMC+AIe+xgCLvMQAgrjKAJy/xgCQv8oAgL7SAIi90ACCwcYAi8PHAITFzACNxMgAi8PNAIjH
      zACEys8AiMnOAJTAwQCXxsgAm8bIAJvIzACGzdIAhs3UAI7J0ACIzNAAjs3SAJjD0QCTyNQAh9DU
      AIvQ0wCI0tYAjdHVAIvS2ACP1NoAjNnfAJHS1gCd09QAk9XZAJvV2gCT2NoAlNjaAJHY3ACU2NwA
      os3ZALLO0gCk09YArNHTAK7W2QCs298AstPWALHV2wCO2uAAk9vgAJXb4ACV3OEAl9/kAJzd5ACq
      0eAAq9XgAKLZ4wCg3+YAqN7hAKjY5ACu2uUAqdznALDT4QC11eMAs9nlAJng5ACd4ucAneTnAJ3h
      6gCe5uoAn+fsAI/s8wCd8vsAm///AKrg5ACi5+wAquToAKzl6wCl6e4AtOPpALrk6wC26+4At+zu
      ALzq7QCk6/AAquvwAKjt8QCx7fEAuO3wAKrw8gCq8PYArPL3AK319gCm8fgArvP4ALXw8wC28/YA
      vfHzALvy9gC09foAs/r+ALz//wDI19wAw9zoAMHq7QDF6e8AwuzuAN7u7wDD7fAAxu3wAMju8ADC
      8fMAx/HzAMjx8gDJ8vUAzPP2AM329gDP+PcAxff6AMP8/wDK/PwA2PD0ANH//wDY//8A6P//APL/
      /wD///8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAC3uW0AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA6MC3ztfa
      pS0JAAAAAAAAAAAAAAAAAAAAAAAAAADAwdHu8u/Z19eTjHlBAAAAAAAAAAAAAAAAAAAAAMC96fL2
      8vLu6dLX15OaoC8rWQAAAAAAAAAAAADAvb3u8vLy8vLy8u7p2Nfak5OcXjJ9WwAAAAAAAAAAAMXa
      4fDy8vLy8vLy8uLXw7NIaZOeoBcwlVkAAAAAAAAAw9fX4fDw8vLy8tTLpZyew8WTXImeh0V7j0GG
      AAAAAADD2tfa5OLS0L2koaXD19rf5eXFa1ycsQ4sooFZAAAAALna18WzpZyJaYeJiYeHh4eVz+Xk
      pVxrlUdDjI9BAAAAw7OTazolIyYnJygpNDtCSkVCw9/l14lpjQs+nH98WgBBEgcDAQEEBggMFRYk
      Gh44rZA5ic/l5bNpRkpHjo8/AAAnAwIFDS57kaytq4lUIRs1qa8+PKXa5dqcSApHnkQAAAAAgWCN
      zvn7+/n45Ml3c05LYbB+NofM3+XFXkSHSAAAAAAAANBrgK739+TeyHd0ZVNMUIWqMjyl1+Xlnio7
      AAAAAAAAAADQjHyk13d1cXBnYlJPTVWYejaHw9fapToAAAAAAAAAAAAA6YlZmfr+/v385sp4dnJs
      g3lCiJKsAAAAAAAAAAAAAAAAAACJhOfs1MNvbmhkZmNRNz0AAAAAAAAAAAAAAAAAAAAAAAAAYBkY
      HB0gIh8UExEPEDMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAX4IAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAP/////////////////////////////////////////////////8f///wB///gAP//AAA/8A
      AAH/AAAAfwAAAB8AAAAPAAAABwAAAAEAAAABgAAAAeAAAAH4AAAB/gAAAf+AAAP/8AAf//wAD///
      /+f/////////////////////KAAAABgAAAAwAAAAAQAIAAAAAABAAgAAAAAAAAAAAAAAAQAAAAEA
      AAAAAAAKNjwAH0tOACFQVgAsWV8AIFdhAClcZAA4ZGwAQGxuAEBtdQARaIMAMnePAEJ0gQBFf4YA
      QnuJAAuBnwAcgp0AH5CuADOMpAA5i6EAKJSvAC2arAAsmbEAIZu7ADuuvgBEgo8AXoqMAFCQmwBV
      k5wAX5CfAF+WnABjhIUAYYeKAG2XmQBmmJsAeZaVAHaZmgBIl6cARp+rAEafrABYkKEAWZWlAGOZ
      pwBgnqoAaKGnAGyqrwB/oKEAeqKlAHSnqwB7o6wAe6WvAHCorQB3q68Abq61AHajswBxqrIAdKyy
      AHevtQByrrgAd6+8AG+yuQB9srYAcLO5AHO0ugB5tLkAeLu/AH64vQACmsUAErjdACi52gBur8AA
      d7fDAHe9wgB3vsQAe7rAAHm9wwB8uMQAfL/GAHq5zAB6vtEAM8XOADzM5gA80uYAfsHFAH7DyAB8
      wcwAf8TOAF/a6wBj2u4AaNnqAHra7QBV9PUAfebwAIyvsQCArr0AgLK2AIqxsgCJsrcAkLW6AJO4
      vwCMvsAAlr7CAI7CwgCBxcoAh8fJAIPHzACKx8wAg8rOAIvKzwCMys0AmMLFAJ7GxQCRxMkAnsLM
      AJrFzACSyc0Als3OAJ3JyACCwNUAgsrRAITL0ACHzNcAi83SAI3N0QCLztQAjM7bAJHP0QCZyNIA
      n8nRAJvJ1ACO0NUAj9PfAIzX3QCe0NAAkdXaAJTX3QCd1NgAn9bfAJTZ2wCR2NwAlNndAKLI0gCj
      ytUApcrUAKzJ0wCwy9YApNDXAKbW2wCq1dwAu9beAIXV4gCB2esAhNrrAIrc6gCa1uAAl9vgAJbc
      4QCZ3+MAm9vlAJjf5ACk3OAAqt3jALXd4wCb4+cAnOLlAJ7k6QCY7vcAgfPzAKfh5QCg4ukAp+fr
      AKbm7ACq4eoApOrvAKft7wCq7O8AtubqALDj7QC44usAueXoALbq6wC37O8Av+nrALvp7wCm6/EA
      pOzwAKju8gCq7/QAs+zwALnv9ACu8PMAq/H0AKzx9QCt9/UAo/D4AK7z+QCv9PgAsvD0ALj3/ACy
      +f0Atfr9ALX8/wDK4OYAxOvuAMXt8QDB8PIAwvP0AMT09QDJ8PEAzPLzAMny9QDM8/cAyPb2AM32
      9gDO+PgAyv3/ANP9+gDT//8A7vHyAPf7+wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP///wAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAlYF5XgAAAAAAAAAAAAAAAAAA
      AAAAkXGBoK6kQh0AAAAAAAAAAAAAAAAAkI2UttHRtbuLQmYqAAAAAAAAAAAAjY2izNHW0dHLvMCL
      QGsjOXYAAAAAAACewM/R0dHR0dHPqptCM1SEJC5PAAAAAAClwMXP0dHMs5NtaXybaTx5aT1gTAAA
      AACqwLuvqIhzanybrrvDyZ4/SHkfZHBOAACqu5tMPzU1P0xTaX6LpcnDUz9rIV91RgBHHA0GAwQH
      CQwOGRxfc4vAyYtIIjNtbzqSBQECCBouXWA5JRATMW98pMm4aS0ggkIAAHFUh7XR2dbGpllFFyli
      cIe7yYssQEwAAAAAtpqhsL3CXFdRREMSMWVtnru4MB0AAAAAAACziISWmZiYWlhSGCtjhXx8bTkA
      AAAAAAAAALB9ytrb2cCnW1AmOoEAAAAAAAAAAAAAAAAArFQmFRYWFBEPCgsAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAADYpAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAD///8A////AP///wD///8A////AP///wD/w/8A/gH/APAA/wCAAD8AAAAfAAAADwAAAAMA
      AAABAAAAAAAAAAAAwAAAAPAAAAD8AAAA/wAHAP/ABwD///MA////AP///wAoAAAAEAAAACAAAAAB
      AAgAAAAAAAABAAAAAAAAAAAAAAABAAAAAQAAAAAAAABfggAljaMALZClADaTpgA/l6gAQouhAEma
      qgBTnqsAXKKtAGalrwBvqbEAeKyyADOixQBBrMsAQ63MAEWvzQBGsM4ASbLPAFC10QBZudMAW7zV
      AGe/1wBowtkAdsXbAHfI3QBo0OYAcNPoAHnW6QCGzN8Ahc7kAIjP4QCC2usAjN3tAJnT4wCb1uUA
      n9XlAKTY5wCq2+gAr93qAJbh7gCf5fAAqejyALLs9AC77/UAvP//AMX//wDP//8A2f//AOL//wDs
      //8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAA////AAAAAQEBAQEBAQEBAQEBAAAAAAYMCwoJCAcFBAMCAQAA
      AAAGDAsKCQgHBQQDAgEAAAAAHiwrKikoISAcGxoNAAAAAB4sKyopKCEgHBsaDQAAAAAeLCsqKSgh
      IBwbGg0AAAAAHiwrKikoISAcGxoNAAAAAB4sKyopKCEgHBsaDQAAAAAeLCIdGBYUExEOGg0AAAAA
      HiwiMjEwLy4tDhoNAAAAAB4sJTIxMC8uLQ4aDQAAAAAeLCYyMTAvLi0RGg0AAAAAHiwmIh8ZFxQT
      EhoNAAAAAAYMCwoJCAcFBAMCAQAAAAAGBgYGBgYGBgYGBgYAAAAAAAAAAAAAAAAAAAAAAADAAwAA
      wAMAAMADAADAAwAAwAMAAMADAADAAwAAwAMAAMADAADAAwAAwAMAAMADAADAAwAAwAMAAMADAAD/
      /wAAKAAAACAAAABAAAAAAQAgAAAAAACAEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAdKbGAYG0zTSOwtRUhLzS
      r5zX4e1qv9HqCF59egAnQg4AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgbHMQI2+1IaY
      xtiyqNTf9Kzl6/+p7vH/qvHy/5PY2v9nlZv/ADtSww9tjTMAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAaJ/CA4y91CR/tM5JjL3U
      pafU4dy45Ov3xu3x/8rx8v/I7vD/uO3w/6nt8f+o7PH/hs7T/4jHzP+Aqqr/YKq67wd5oHsAR2YK
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAbaLBPoa4z26R
      wtasqtjk8cDq7v/K8/X/z/j3/8z29v/J8vP/xu3w/8Pr7f+26+7/qOzw/6jt8f+HztL/h9DU/43a
      3/91lJP/aIeM/yqRsb0AX4s3AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAf7jO
      nJLP3sul2+bzw+3w/8329v/O9vf/zPP2/8ry9f/I8vX/yfL1/8jx8v/H7vH/wuzu/7Ht8f+p7vL/
      qvDz/4bN0P+HztP/idLW/3+9wv91nZ//jru7/2Orve0Dc5t7AE9zCQAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAACd4er/rvX2/7bz9v/D8vP/yfPz/8jy9f/I8fP/yfP1/8ny9f/J8vX/yfHz/8jw
      8v+98fP/quvw/53i5/+V2+D/c7u+/3nBxv+HztP/i9PY/4zZ3/9hfYD/e5ST/47J0P81kK3BAGiQ
      NgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAJvf5/mp7vL/qu7z/7Xw8//B8fP/x/Hz/8rz9v/M8/f/
      yvP2/8jy8/++6+7/quDk/5TY2v+M09b/j9Xb/5ng5f+f5+z/h87U/3S7wf+Ax87/i9LY/4LCx/95
      srf/gbK1/5XBwv9ep7rqFH6kgQAAAAMAAAAAAAAAAAAAAAAAAAAAm97m+anw8v+p7vP/rPL3/7X2
      +P+78vb/t+zu/7Tm6v+o3uH/mdXZ/5HS1v+T2Nv/neTn/6bt8v+q8Pb/r/P4/7L4/v+1/P//n+br
      /3zDyP93vsP/iNLW/47a4P9denv/aYyN/53T1P+Mu8H/Mo+swQBqkzcAAAAAAAAAAAAAAACe3+b5
      q/Dz/6jt8v+f5ur/l9/k/5LY3f+L0NP/hMbK/3/AxP+Dwsb/h8bL/4bHyv+ExMf/hMLG/4PAxf+F
      wsb/js3S/6bp7v+0+///sPf8/5HY3P90u8H/fcbM/4jM0P98tbj/ea6x/43EyP+XwMD/Y6i58QB1
      n38AVnUJAAAAAJ7e5fqT2+D/hs3U/3rCyf9hoqr/S4eQ/0mDjP9Qh5D/UIiS/1OMlf9Vk53/WZSe
      /1+bo/9ppKj/dK6y/365vP95tLf/dK+0/5rc4f+v8/j/t/7//6Xs8P+Axsz/ecHH/4PMz/9YaWn/
      dqqu/47Q1f+YwL7/h7e9/yuLqbMAAAAKaqm4/z94gv8xZG3/G0tT/xA7Qf8VP0L/IUpN/zBXWf8/
      Y2n/Sm50/013gf9LfYv/RIKT/zyGmf8/jaP/Xam7/67W2/+Xxsj/aaGl/4XDyP+k6u7/sfj9/7L5
      /v+V3OH/ecLH/3W0uP9/uLz/fLa7/4jJzv+SwML/d6uv/QAAACqCy9NpPHiA3hhKUf8ZRUr/Ll1e
      /0x+f/9pmZv/g7K0/5vGyP+r0NL/rtbY/6TT1v+Hxs3/XbLB/y6Zsv8Kh6n/Qp+6/6LN2f+y09b/
      dais/2+prv+U2Nz/qvD1/7X7//+r8vf/iNLW/3S6vv9Va23/fLW4/4/V2/9ur7X2AAAAKAAAAACm
      7fMEh8zVZGeqtsJ5v8n/hsjP/6rk6P/J+vr/0P///9L////M/v//w/z//7T3/v+d8vv/f+bz/1LQ
      6v8dtdn/F6PJ/2S30P+x1dv/kbm8/2icof+AwMT/oufs/671+P+1+///nufr/3m7wf9xsbb/hcLH
      /3O3vPZNTU0qAAAAAAAAAAAAAAAAktbfAqLq9UyQ1d6tf8DL/4e+xv+s29//x/f5/8P3+/+38/n/
      pvH4/4/s8/955fD/Xtvt/z7N5/8Yu+D/AKbU/yGjyf+AvtL/ss7S/3Ccn/9vq67/k9fa/6br8P+0
      +f7/svz//4/U2P9hh4n/ZqSp+MHBwUEAAAAAAAAAAAAAAAAAAAAAAAAAAKvt8wKw8PUuktPeqIK+
      yeuCtr3/ntXc/6Do8f9/4vD/YdXn/03J4f9GxOD/QsPf/zrB3/8svd7/Hrvb/xSy1f9MttD/mMPR
      /4mtsf9snqH/gMLG/5rg5P+m7PH/rPX3/5PU2P9kpqv/x8fHHAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAn+LtKI7U4YF4v8voZq/A/5PI1P/Y8PT/8v////P////o////2P///7z///+b
      ////e////1j8//9H3Ov/bsPW/5y/xv9/p6r8b6qv83O3vNJorLKoY6mvhF+lq3MAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAJLi7B6C1uFpc7rI3pC/yv/I19z/3u7v
      /7vq7P+Z4OT/fdjc/2PQ2f9Lztj/PNDa/zbc5P8p0t//OLjO/0yguvg6c4GnRHB1aDxiZwsAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAWsbXZUuqur8ui5n/EoSY/wmGn/QDe5vcAHud0QB5nccAb5fNAGWR1QBplOIAb5f2AGmT/wpr
      k/8pd5DQMm5+eAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAVdLfA0CxxkQmjq5FKIuwNSiGriIukrgaM5vBEiqTuhYig7Ad
      I4ayJwxxoDcOdKRBIIWvWBp8n5Ecf6CMHoGlKgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD/////////////////////////////
      ////////////////////4B///4AP//AAA//AAAH/AAAAfwAAAD8AAAAPAAAABwAAAAEAAAAAAAAA
      AAAAAACAAAAA4AAAAPgAAAD/AAAB/8AAB//4AAf//AAD/////////////////////ygAAAAYAAAA
      MAAAAAEAIAAAAAAAYAkAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABCcHgCQnB4BQAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAgbLNAYCyzRFzpLpLfLC/g4e+zNOBytXyIXGMkQBFbBMAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAGuesySBr7xSe6m7mIe0wc6d
      yND4pNzg/6ft7/+c4uX/fri9/ydpfbwUmMMxAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
      AAAAAAAAQnB4CF+QozJmma1of628pY68ydCq1dz+v+nr/8zy8//K8PH/t+zv/6ru8/+R2N3/fri9
      /47Cwv9Rjp3kE4OvYgB2rgEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABso7R7erHAq5PBztey3OL0
      xOzw/8/29//O+Pj/zPX1/8jw8v/E6+7/s+zw/6zy9v+U2t3/erS5/4PKzf95lpX/da60+jeavJ8A
      dqoaAAAAAAAAAAAAAAAAAAAAAAAAAACZ2+X4rvDz/8T09f/O9/b/y/P2/8ry9f/J8vX/yvL1/8rw
      8v/B8PL/p+fr/5fb4P94u7//cKit/37DyP+M193/dpma/3+gof9csMfPB4GwRAAAAAAAAAAAAAAA
      AAAAAACf5Or+q/H0/7Lw9P/C8/T/yPP1/8zz9//G7/L/ueXo/6bW2/+Mys7/g8fM/4vO1P+W3OH/
      gcXL/2+yuf+CytH/g8TJ/32ytv+KsbL/c7PA7iqUuH4gj7QJAAAAAAAAAACf4un6rfL1/6nv9P+q
      7O//p+Hl/53U2P+Syc3/isfM/4zN0f+X3eD/pOrv/6nt8v+u8/n/tfz//5nf4/9ztLr/d73C/4TL
      0P9jhIX/jL7A/5LEyf88mbWtAYCxKgAAAACl5uz6qO/z/5bd4/98v8b/cLO5/2+vtv9trrX/c7S7
      /3m9w/9+wMT/h8fJ/5HP0f+U2dv/neTp/7X6/f+v9Pj/f8LH/3Czuv+Dy8//bZeZ/4Gztv+dycj/
      WaS33hOIslF3t8P/VZOc/0V/hv8pXGT/IVBW/yxZX/84ZGz/QG11/0J0gf9Ce4n/RIKP/1CQm/+A
      sbb/ls3O/5HY3P+s8fX/s/r+/5TZ3v93vsT/Zpib/3err/+Lys//nsbF/2qptPButL+fH1Zg/Qo2
      PP8fS07/QGxu/16KjP96oqX/jK+x/4myt/9xqrL/SJen/xyCnf85i6H/e6Os/5jCxf+LzdL/m+Pn
      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
  AADAAwAAwAMAAMADAADAAwAAwAMAAP//AAA=')
  #endregion
  $formChooseOU.MaximizeBox = $False
  $formChooseOU.MinimizeBox = $False
  $formChooseOU.Name = "formChooseOU"
  $formChooseOU.StartPosition = 'CenterScreen'
  $formChooseOU.Text = "Choose Active Directory OU"
  $formChooseOU.add_Load($FormEvent_Load)
  #
  # treeview1
  #
  $treeview1.ImageIndex = 0
  $treeview1.ImageList = $imagelist1
  $treeview1.Location = '19, 21'
  $treeview1.Name = "treeview1"
  $System_Windows_Forms_TreeNode_1 = New-Object 'System.Windows.Forms.TreeNode' ("Active Directory Hierarchy", 3, 3)
  $System_Windows_Forms_TreeNode_1.ImageIndex = 3
  $System_Windows_Forms_TreeNode_1.Name = "Active Directory Hierarchy"
  $System_Windows_Forms_TreeNode_1.SelectedImageIndex = 3
  $System_Windows_Forms_TreeNode_1.Tag = "root"
  $System_Windows_Forms_TreeNode_1.Text = "Active Directory Hierarchy"
  [void]$treeview1.Nodes.Add($System_Windows_Forms_TreeNode_1)
  $treeview1.SelectedImageIndex = 0
  $treeview1.Size = '301, 425'
  $treeview1.TabIndex = 1
  $treeview1.add_AfterLabelEdit($CreateOU)
  #
  # buttonOK
  #
  $buttonOK.Anchor = 'Bottom, Right'
  $buttonOK.DialogResult = 'OK'
  $buttonOK.Location = '245, 467'
  $buttonOK.Name = "buttonOK"
  $buttonOK.Size = '75, 23'
  $buttonOK.TabIndex = 0
  $buttonOK.Text = "OK"
  $buttonOK.UseVisualStyleBackColor = $True
  $buttonOK.add_Click($buttonOK_Click)
  #
  # imagelist1
  #
  $Formatter_binaryFomatter = New-Object System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
  #region Binary Data
  $System_IO_MemoryStream = New-Object System.IO.MemoryStream (,[byte[]][System.Convert]::FromBase64String('
        AAEAAAD/////AQAAAAAAAAAMAgAAAFdTeXN0ZW0uV2luZG93cy5Gb3JtcywgVmVyc2lvbj00LjAu
        MC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODkFAQAA
        ACZTeXN0ZW0uV2luZG93cy5Gb3Jtcy5JbWFnZUxpc3RTdHJlYW1lcgEAAAAERGF0YQcCAgAAAAkD
        AAAADwMAAABwCgAAAk1TRnQBSQFMAgEBBAEAAUABAAFAAQABEAEAARABAAT/AQkBAAj/AUIBTQE2
        AQQGAAE2AQQCAAEoAwABQAMAASADAAEBAQABCAYAAQgYAAGAAgABgAMAAoABAAGAAwABgAEAAYAB
        AAKAAgADwAEAAcAB3AHAAQAB8AHKAaYBAAEzBQABMwEAATMBAAEzAQACMwIAAxYBAAMcAQADIgEA
        AykBAANVAQADTQEAA0IBAAM5AQABgAF8Af8BAAJQAf8BAAGTAQAB1gEAAf8B7AHMAQABxgHWAe8B
        AAHWAucBAAGQAakBrQIAAf8BMwMAAWYDAAGZAwABzAIAATMDAAIzAgABMwFmAgABMwGZAgABMwHM
        AgABMwH/AgABZgMAAWYBMwIAAmYCAAFmAZkCAAFmAcwCAAFmAf8CAAGZAwABmQEzAgABmQFmAgAC
        mQIAAZkBzAIAAZkB/wIAAcwDAAHMATMCAAHMAWYCAAHMAZkCAALMAgABzAH/AgAB/wFmAgAB/wGZ
        AgAB/wHMAQABMwH/AgAB/wEAATMBAAEzAQABZgEAATMBAAGZAQABMwEAAcwBAAEzAQAB/wEAAf8B
        MwIAAzMBAAIzAWYBAAIzAZkBAAIzAcwBAAIzAf8BAAEzAWYCAAEzAWYBMwEAATMCZgEAATMBZgGZ
        AQABMwFmAcwBAAEzAWYB/wEAATMBmQIAATMBmQEzAQABMwGZAWYBAAEzApkBAAEzAZkBzAEAATMB
        mQH/AQABMwHMAgABMwHMATMBAAEzAcwBZgEAATMBzAGZAQABMwLMAQABMwHMAf8BAAEzAf8BMwEA
        ATMB/wFmAQABMwH/AZkBAAEzAf8BzAEAATMC/wEAAWYDAAFmAQABMwEAAWYBAAFmAQABZgEAAZkB
        AAFmAQABzAEAAWYBAAH/AQABZgEzAgABZgIzAQABZgEzAWYBAAFmATMBmQEAAWYBMwHMAQABZgEz
        Af8BAAJmAgACZgEzAQADZgEAAmYBmQEAAmYBzAEAAWYBmQIAAWYBmQEzAQABZgGZAWYBAAFmApkB
        AAFmAZkBzAEAAWYBmQH/AQABZgHMAgABZgHMATMBAAFmAcwBmQEAAWYCzAEAAWYBzAH/AQABZgH/
        AgABZgH/ATMBAAFmAf8BmQEAAWYB/wHMAQABzAEAAf8BAAH/AQABzAEAApkCAAGZATMBmQEAAZkB
        AAGZAQABmQEAAcwBAAGZAwABmQIzAQABmQEAAWYBAAGZATMBzAEAAZkBAAH/AQABmQFmAgABmQFm
        ATMBAAGZATMBZgEAAZkBZgGZAQABmQFmAcwBAAGZATMB/wEAApkBMwEAApkBZgEAA5kBAAKZAcwB
        AAKZAf8BAAGZAcwCAAGZAcwBMwEAAWYBzAFmAQABmQHMAZkBAAGZAswBAAGZAcwB/wEAAZkB/wIA
        AZkB/wEzAQABmQHMAWYBAAGZAf8BmQEAAZkB/wHMAQABmQL/AQABzAMAAZkBAAEzAQABzAEAAWYB
        AAHMAQABmQEAAcwBAAHMAQABmQEzAgABzAIzAQABzAEzAWYBAAHMATMBmQEAAcwBMwHMAQABzAEz
        Af8BAAHMAWYCAAHMAWYBMwEAAZkCZgEAAcwBZgGZAQABzAFmAcwBAAGZAWYB/wEAAcwBmQIAAcwB
        mQEzAQABzAGZAWYBAAHMApkBAAHMAZkBzAEAAcwBmQH/AQACzAIAAswBMwEAAswBZgEAAswBmQEA
        A8wBAALMAf8BAAHMAf8CAAHMAf8BMwEAAZkB/wFmAQABzAH/AZkBAAHMAf8BzAEAAcwC/wEAAcwB
        AAEzAQAB/wEAAWYBAAH/AQABmQEAAcwBMwIAAf8CMwEAAf8BMwFmAQAB/wEzAZkBAAH/ATMBzAEA
        Af8BMwH/AQAB/wFmAgAB/wFmATMBAAHMAmYBAAH/AWYBmQEAAf8BZgHMAQABzAFmAf8BAAH/AZkC
        AAH/AZkBMwEAAf8BmQFmAQAB/wKZAQAB/wGZAcwBAAH/AZkB/wEAAf8BzAIAAf8BzAEzAQAB/wHM
        AWYBAAH/AcwBmQEAAf8CzAEAAf8BzAH/AQAC/wEzAQABzAH/AWYBAAL/AZkBAAL/AcwBAAJmAf8B
        AAFmAf8BZgEAAWYC/wEAAf8CZgEAAf8BZgH/AQAC/wFmAQABIQEAAaUBAANfAQADdwEAA4YBAAOW
        AQADywEAA7IBAAPXAQAD3QEAA+MBAAPqAQAD8QEAA/gBAAHwAfsB/wEAAaQCoAEAA4ADAAH/AgAB
        /wMAAv8BAAH/AwAB/wEAAf8BAAL/AgAD//8A/wD/AP8ABQAk/wL0Bv8D9AP/DPQD/wp0BHMC/wp0
        BHMC/wPsAesBbQH3Av8BBwLsAesBcgFtAfQC/wwqA/8BdAGaA3kBegd5AXMC/wF0AZoDeQF6B3kB
        cwL/AfcBBwGYATQBVgH3Av8BvAHvAQcBVgE5AXIB9AL/AVEBHAF0A3MFUQEqA/8BeQKaBUsFmgF0
        Av8BeQyaAXQC/wHvAQcB7wJ4AZIC8QMHAXgBWAHrAfQC/wF0ApkCeQN0A1IBKgP/AXkCmgFLA1EB
        KgWaAXQC/wF5DJoBdAL/Ae8CBwHvAZIC7AFyAe0CBwLvAewB9AL/AZkCGgGgBJoCegF5AVID/wF5
        AaABmgF5AZkCeQFRBZoBdAL/AXkBoAuaAXQC/wEHAe8C9wLtAXgBNQF4Ae8D9wHsAfQC/wGZAhoB
        oASaAnoBeQFSA/8BeQGgAZoCmQGgAXkBUgWaAXQC/wF5AaALmgF0Av8BBwPvAfcB7QGYAXgBmQEH
        A+8B7AP/AZkCGgGgBJoCegF5AVID/wGZAaABmgGZAXkBmgF5AVIFmgF0Av8BmQGgC5oBdAL/AbwD
        8wG8AZIBBwHvAQcB8QLzAfIB7QP/AZkCGgGgBJoCegF5AVID/wGZAaABmgJ5AXQCUgWaAXQC/wGZ
        AaALmgF0Av8BvAEHAu8B9wPtAe8CBwLvAe0D/wGZARoBmgKZBnkBUgP/AZkBwwGaBHQBeQGgBJoB
        dAL/AZkBwwaaAaAEmgF0Av8CvAIHAvcCBwO8AgcBkgP/AZkBGgGZAxoDmgFSAXkBUgP/AZkBwwOa
        AqABmQWaAXQC/wGZAcMDmgKgAZkFmgF0Av8CvAHrAewCBwLzAfABvAHtAW0BBwH3A/8BmQEaAZkC
        9gTDAVIBeQFSA/8BmQWgAZoCdAV5Av8BmQWgAZoCdAV5Av8BvAEHApIB7wH3ApIB7wG8Ae8BkgHv
        AfcD/wGZAhoC9gTDAVgBeQFSA/8BmQGaBBoBdAOaApkBmgF5Av8BeQGaBBoBdAOaApkBmgF5Av8D
        9AHyAbwB8QK8Ae8B8AT0A/8BmQMaApkDeQFYAXkBUgP/ARsGeQGaAvYB1gG0AZoBmQL/AZkGeQGa
        AvYB1gG0AZoBeQX/AfQBvAH3ARIB7AHvAfAH/wFRARwBeQN0AVIEUQEqCf8BwwZ5AcMI/wGaBnkB
        mgX/AfQBvAEHAu8B9wHxB/8MUUL/AUIBTQE+BwABPgMAASgDAAFAAwABIAMAAQEBAAEBBgABARYA
  A///AAIACw=='))
  #endregion
  $imagelist1.ImageStream = $Formatter_binaryFomatter.Deserialize($System_IO_MemoryStream)
  $Formatter_binaryFomatter = $null
  $System_IO_MemoryStream = $null
  $imagelist1.TransparentColor = 'Transparent'
  #
  # ErrorProviderOU
  #
  $ErrorProviderOU.ContainerControl = $formChooseOU
  #endregion Generated Form Code

  #----------------------------------------------

  #Save the initial state of the form
  $InitialFormWindowState = $formChooseOU.WindowState
  #Init the OnLoad event to correct the initial state of the form
  $formChooseOU.add_Load($Form_StateCorrection_Load)
  #Clean up the control events
  $formChooseOU.add_FormClosed($Form_Cleanup_FormClosed)
  #Show the Form
  return $formChooseOU.ShowDialog()

}

#------------------------------------------------------------[Actions]-------------------------------------------------------------
 
 # Call OnApplicationLoad to initialize
   If((OnApplicationLoad) -eq $true) {
       #Call the form
       Call-AD_OU_select_pff | Out-Null
       #Perform cleanup
       OnApplicationExit
                                     }
 
 # Start of log completion
   Start-Transcript $ScriptLogFile | Out-Null

 #Show selected OU on a messagebox
   [System.Windows.Forms.MessageBox]::Show("Selected OU: $objSelectedOU" , "Information" , 0, [Windows.Forms.MessageBoxIcon]::Information)

 # Get all AD groups with CommonName and Description and do an Export-CSV (with specifics parameters)
<#
    # Define the parameters and values to give to Get-ADObject
    $GetADObjectParameters = @{
                CN = $config.CN
                Description = $config.Description
                SamAccountName = $config.SamAccountName
                LogonTimeStamp = $config.LogonTimeStamp
    }
    # Check if $config.XXXXXX is null. If it isn't, add it to the hashtable.
    if ($null -ne $config.Bcc) {
    $parameters.Add("BCC", $config.Bcc)
                           }
    if ($null -ne $config.Bcc) {
    $parameters.Add("BCC", $config.Bcc)
               }
    #>
Get-ADObject -SearchBase $objSelectedOU -Filter * -Property cn, sAMAccountName, homedirectory | Select-Object cn, sAMAccountName, homedirectory | Export-CSV $CSVExportADObjects -NoTypeInformation -Encoding Unicode -Delimiter ';'
 
 # MessageBox who inform of the end of the process
   [System.Windows.Forms.MessageBox]::Show(
  "Process done.
    The log file will be opened when click on 'OK' button.
    Please, check the log file for further informations.
" , "End of process" , 0, [Windows.Forms.MessageBoxIcon]::Information)
 
 # Stop the log transcript
   Stop-TranscriptOnLog

 # Open the log file
   Invoke-Item $ScriptLogFile
