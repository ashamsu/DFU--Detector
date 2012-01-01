'
'Copyright (C) <2011>  <The Private Dev Team>
'    This program is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    This program is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <http://www.gnu.org/licenses/>
Imports System.Management
Imports System.IO
Module Module1
    Public DFUConnected As Boolean = False
    Public forever As Boolean = True
    Public text1 As String = ""
    Public WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Public Sub Delay(ByVal dblSecs As Double)
        'iH8sn0w's delay code...
        Const OneSec As Double = 1.0# / (1440.0# * 60.0#)
        Dim dblWaitTil As Date
        Now.AddSeconds(OneSec)
        dblWaitTil = Now.AddSeconds(OneSec).AddSeconds(dblSecs)
        Do Until Now > dblWaitTil
            Application.DoEvents() ' Allow windows messages to be processed
        Loop
    End Sub
    
    Public Sub Search_DFU(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        'iH8sn0w's DFU detector
        DFUConnected = False
        Do Until DFUConnected = True
            text1 = " "
            Dim searcher As New ManagementObjectSearcher( _
                      "root\CIMV2", _
                      "SELECT * FROM Win32_PnPEntity WHERE Description = 'Apple Recovery (DFU) USB Driver'")
            For Each queryObj As ManagementObject In searcher.Get()

                text1 += (queryObj("Description"))
            Next
            If text1.Contains("DFU") Then
                DFUConnected = True
            End If
        Loop
    End Sub
    Sub main(ByVal args() As String)
        If args.Length = 0 Then
            Console.WriteLine()
            Console.Write("Usage: DFU Detector.exe -s")
            Console.WriteLine()
            Exit Sub
        Else
            Console.WriteLine()
            Console.WriteLine("Searching for DFU...")
            BackgroundWorker1 = New System.ComponentModel.BackgroundWorker
            BackgroundWorker1.RunWorkerAsync()
            Do While forever = True
                If DFUConnected = True Then
                    Console.WriteLine("DFU device found!")
                    Exit Sub
                End If
            Loop
        End If
    End Sub
End Module