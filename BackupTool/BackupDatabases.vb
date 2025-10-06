Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.IO

Public Class frm_Backup

    Public con As New SqlConnection
    Dim cmd As New SqlCommand
    Dim dread As SqlDataReader
    Dim check As String = String.Empty
    Dim Logsize As Decimal

    Private Sub Instances()
        Dim dt As DataTable = SqlDataSourceEnumerator.Instance.GetDataSources
        Dim dr As DataRow
        For Each dr In dt.Rows
            cmbServers.Items.Add(String.Concat(dr("ServerName"), "\", dr("InstanceName")))
        Next
    End Sub

    Private Sub GetDatabases()
        con = New SqlConnection("Data Source=" & Trim(cmbServers.Text) & ";Database=Master;user id=sa;password=togetherforever;")
        Try
            con.Open()
            chklstDatabases.Items.Clear()
            cmd = New SqlCommand("SELECT [name] FROM sys.databases 
                                  Where [name] 
                                  Not In('master','model','msdb','tempdb','ReportServer','ReportServerTempDB')", con)
            dread = cmd.ExecuteReader()
            While dread.Read
                chklstDatabases.Items.Add(dread(0))
            End While
            dread.Close()
            con.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub FillServerCombobox()
        Instances()
        Dim dr() As DriveInfo = DriveInfo.GetDrives
        cmbDrivers.Items.Clear()

        For i = 0 To dr.Length - 1
            cmbDrivers.Items.Add(dr(i))
        Next
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FillServerCombobox()
    End Sub

    Private Sub CboServer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbServers.SelectedIndexChanged
        GetDatabases()
    End Sub

    Sub DropIndexes()

        If chklstDatabases.CheckedItems.Count = 1 Then

            For Each DatabaseChecked As Object In chklstDatabases.CheckedItems
                Dim con As New SqlConnection("server=" & cmbServers.Text & ";database=" & DatabaseChecked.ToString & ";user id=sa;password=togetherforever")
                Dim cmd As New SqlCommand("declare @qry nvarchar(max);
                                  select @qry = (
    select  'IF EXISTS(SELECT * FROM sys.indexes WHERE name='''+ i.name +''' AND object_id = OBJECT_ID(''['+s.name+'].['+o.name+']''))    drop index ['+i.name+'] ON ['+s.name+'].['+o.name+'];  '
    from sys.indexes i 
        inner join sys.objects o on  i.object_id=o.object_id
        inner join sys.schemas s on o.schema_id = s.schema_id
    where o.type<>'S' and is_primary_key<>1 and index_id>0
    and s.name!='sys' and s.name!='sys' and is_unique_constraint=0
    for xml path(''));
     exec sp_executesql @qry", con)

                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()

            Next
        End If

    End Sub

    Private Sub BtnBak_Click(sender As Object, e As EventArgs) Handles BtnBak.Click

        Dim Yourpath = cmbDrivers.Text & "AllDataBackup" & " " & Today.ToString("dd.MM.yyyy")

        ' Check if drive is selected first
        If cmbDrivers.Text = "" Then
            MsgBox("Please Select Backup Drive", MessageBoxIcon.Information, "Select Drive")
            Exit Sub
        End If

        If chklstDatabases.CheckedItems.Count <> 0 Then

            Dim dr As New DriveInfo(cmbDrivers.Text)
            If dr.IsReady Then
                If cmbDrivers.Text <> "" Then

                    ProgressBar1.Maximum = chklstDatabases.CheckedItems.Count - 1

                    For Each DatabaseChecked As Object In chklstDatabases.CheckedItems

                        Dim i As String = DatabaseChecked.ToString
                        Dim lfolder As String

                        Try
                            If Not Directory.Exists(Yourpath) Then
                                Directory.CreateDirectory(Yourpath)
                            End If
                        Catch ex As Exception
                            MsgBox("You Try To Create Folder In Cdrom Drive OR Google Drive" & Environment.NewLine & "CHOOSE VALID PATH", MsgBoxStyle.Exclamation, "Error")
                            Exit Sub
                        End Try


                        lfolder = Yourpath & "\" & DatabaseChecked.ToString & "--" & Format(DateTime.Now, "dd.MM.yyyy HH.mm") & ".bak"
                        Dim strbak As String = "Backup database " & DatabaseChecked.ToString & " To Disk='" & lfolder & "' With Format,compression "
                        Dim cmd1 As New SqlCommand(strbak, con)

                        Try
                            con.Open()
                            cmd1.ExecuteNonQuery()
                            ProgressBar1.Increment(1)
                        Catch ex As Exception
                            MsgBox("Cannot Backup database" & " " & i & Environment.NewLine & "Check Data After Backup Finished", MsgBoxStyle.Exclamation, "Error")
                        End Try
                        con.Close()

                    Next
                    MsgBox("Backup Sucessfully", MessageBoxIcon.Information, "Backup Successfully")
                    ProgressBar1.Value = 0

                    Process.Start("explorer.exe", Yourpath)
                Else
                    MsgBox("Please Select Backup Drive", MessageBoxIcon.Information, "Select Folder")
                End If
            Else
                MsgBox("Select Data You Want To Backup", MessageBoxIcon.Exclamation, "Check")
            End If
        End If
    End Sub

    Private Sub btn_SelectAll_Click(sender As Object, e As EventArgs) Handles btn_SelectAll.Click

        If chklstDatabases.Items.Count > 0 Then
            If btn_SelectAll.Text = "SelectAll" Then
                For i As Integer = 0 To chklstDatabases.Items.Count - 1
                    chklstDatabases.SetItemChecked(i, True)
                Next
                btn_SelectAll.Text = "DeSelectAll"
                btn_SelectAll.BackColor = Color.Green
            Else
                For i As Integer = 0 To chklstDatabases.Items.Count - 1
                    chklstDatabases.SetItemChecked(i, False)
                    btn_SelectAll.Text = "SelectAll"
                    btn_SelectAll.BackColor = Color.SteelBlue
                Next
            End If
        End If

    End Sub

    Private Sub ShrinkDatabases()

        Const TARGET_LOG_SIZE_MB As Integer = 1
        Dim processedCount As Integer = 0
        Dim errorMessages As New List(Of String)

        ' Get count of checked databases and configure progress bar
        Dim totalDatabases As Integer = chklstDatabases.CheckedItems.Count
        If totalDatabases = 0 Then
            MsgBox("No databases selected to process.", MessageBoxIcon.Warning, "Warning")
            Exit Sub
        End If

        ProgressBar1.Maximum = totalDatabases
        ProgressBar1.Value = 0
        ProgressBar1.Visible = True

        Try
            For Each DatabaseChecked As Object In chklstDatabases.CheckedItems
                Dim databaseName As String = DatabaseChecked.ToString()

                Using con As New SqlConnection(GetConnectionString())
                    con.Open()

                    ' Get all logical log file names
                    Dim logFileNames As New List(Of String)
                    Using cmd As New SqlCommand(
                    $"SELECT name FROM sys.master_files WHERE database_id = DB_ID('{databaseName}') AND type_desc = 'LOG'",
                    con)
                        cmd.CommandTimeout = 300
                        Using reader As SqlDataReader = cmd.ExecuteReader()
                            While reader.Read()
                                logFileNames.Add(reader.GetString(0))
                            End While
                        End Using

                        If logFileNames.Count = 0 Then
                            errorMessages.Add($"Could not find any log files for database '{databaseName}'")
                            Continue For
                        End If
                    End Using

                    ' Array of SQL commands for shrinking
                    Dim setupCommands As String() = {
                    $"USE {databaseName}{vbNewLine}",
                    $"ALTER DATABASE {databaseName} SET RECOVERY SIMPLE {vbNewLine}"
                }

                    Dim recoveryCommand As String = $"ALTER DATABASE {databaseName} SET RECOVERY FULL"

                    ' Execute setup commands
                    For Each commandText In setupCommands
                        Using cmd As New SqlCommand(commandText, con)
                            cmd.CommandTimeout = 300
                            cmd.ExecuteNonQuery()
                        End Using
                    Next

                    ' Shrink each log file
                    For Each logFileName In logFileNames
                        Using cmd As New SqlCommand(
                        $"DBCC SHRINKFILE('{logFileName}', {TARGET_LOG_SIZE_MB})",
                        con)
                            cmd.CommandTimeout = 300
                            cmd.ExecuteNonQuery()
                        End Using
                    Next

                    ' Restore recovery mode
                    Using cmd As New SqlCommand(recoveryCommand, con)
                        cmd.CommandTimeout = 300
                        cmd.ExecuteNonQuery()
                    End Using
                End Using

                processedCount += 1
                ProgressBar1.Value = processedCount
                Application.DoEvents() ' Update UI during processing
            Next

            Dim message As String = $"Successfully processed {processedCount} database(s)"
            If errorMessages.Count > 0 Then
                message &= vbNewLine & "Errors encountered:" & vbNewLine & String.Join(vbNewLine, errorMessages)
            End If
            MsgBox(message, MessageBoxIcon.Information, "Execution Complete")

        Catch ex As SqlException
            errorMessages.Add($"SQL Error: {ex.Message}")
            MsgBox("Errors occurred during processing:" & vbNewLine & String.Join(vbNewLine, errorMessages),
               MessageBoxIcon.Error, "Error")
        Catch ex As Exception
            errorMessages.Add($"General Error: {ex.Message}")
            MsgBox("Errors occurred during processing:" & vbNewLine & String.Join(vbNewLine, errorMessages),
               MessageBoxIcon.Error, "Error")
        Finally
            ProgressBar1.Visible = False
        End Try
    End Sub


    Private Function GetConnectionString(Optional databaseName As String = "") As String
        Dim builder As New SqlConnectionStringBuilder With {
        .DataSource = cmbServers.Text,
        .UserID = "sa",
        .Password = "togetherforever"
    }

        If Not String.IsNullOrEmpty(databaseName) Then
            builder.InitialCatalog = databaseName
        End If

        Return builder.ConnectionString
    End Function

    Private Sub btn_CreateIndexes_Click(sender As Object, e As EventArgs) Handles btn_CreateAccIndexes.Click

        DropIndexes()

        If chklstDatabases.CheckedItems.Count <> 0 Then

            ProgressBar1.Maximum = chklstDatabases.CheckedItems.Count - 1

            For Each DatabaseChecked As Object In chklstDatabases.CheckedItems

                Dim con As New SqlConnection("server=" & cmbServers.Text & ";database=" & DatabaseChecked.ToString & ";user id=sa;password=togetherforever")

                Dim cmdcheck As New SqlCommand("select name from sys.tables where name='items'", con)

                Dim strdate As String = "CREATE NONCLUSTERED INDEX [Index_AccMoveBatch_Date] ON [dbo].[AccMoveBatch] ([Date] ASC)"
                Dim strAccmasterid As String = "CREATE NONCLUSTERED INDEX [Index_AccMoveDetail_AccMasterId] ON [dbo].[AccMoveDetail] ([AccMasterId] ASC)"
                Dim strBatchid As String = "CREATE NONCLUSTERED INDEX [Index_AccMoveInfo_Batchid] ON [dbo].[AccMoveInfo] ([BatchID] ASC)"
                Dim strMovedate As String = "CREATE NONCLUSTERED INDEX [Index_AccMoveInfo_Movedate] ON [dbo].[AccMoveInfo] ([MoveDate] ASC)"
                Dim strMoveInfoId As String = " CREATE NONCLUSTERED INDEX [Index_AccMoveMaster_MoveInfoId] ON [dbo].[AccMoveMaster] ([MoveInfoID] ASC)"
                Dim strInvid As String = " CREATE NONCLUSTERED INDEX [Index_InvDetail_Invid] ON [dbo].[InvDetail] ([InvID] ASC)"
                Dim strItemID As String = " CREATE NONCLUSTERED INDEX [Index_InvDetail_ItemID] ON [dbo].[InvDetail] ([ItemID] ASC)"
                Dim strInvDate As String = "CREATE NONCLUSTERED INDEX [Index_InvMaster_InvDate] ON [dbo].[InvMaster] ([InvDate] Asc)"
                Dim strPoDate As String = "CREATE NONCLUSTERED INDEX [Index_Po_PoDate] ON [dbo].[po]([PoDate] Asc)"
                Dim StrPoid As String = "CREATE NONCLUSTERED INDEX [Index_PoProducts_Poid] ON [dbo].[Poproducts]([POID] ASC)"
                Dim StrItemidPo As String = "CREATE NONCLUSTERED INDEX [Index_PoProducts_ItemID] ON [dbo].[Poproducts]([ItemId] ASC)"
                Dim strItemSign As String = "CREATE NONCLUSTERED INDEX [Index_Items_ItemSign] ON [dbo].[Items]([Sign] ASC)"

                Dim cmd1 As New SqlCommand(strdate, con)
                Dim cmd2 As New SqlCommand(strAccmasterid, con)
                Dim cmd3 As New SqlCommand(strBatchid, con)
                Dim cmd4 As New SqlCommand(strMovedate, con)
                Dim cmd5 As New SqlCommand(strMoveInfoId, con)
                Dim cmd6 As New SqlCommand(strInvid, con)
                Dim cmd7 As New SqlCommand(strItemID, con)
                Dim cmd8 As New SqlCommand(strInvDate, con)
                Dim cmd9 As New SqlCommand(strPoDate, con)
                Dim cmd10 As New SqlCommand(StrPoid, con)
                Dim cmd11 As New SqlCommand(StrItemidPo, con)
                Dim cmd12 As New SqlCommand(strItemSign, con)

                con.Open()
                Dim dr As SqlDataReader = cmdcheck.ExecuteReader
                dr.Read()
                If dr.HasRows Then
                    check = dr(0)
                End If
                dr.Close()
                con.Close()

                If check = "items" Then

                    con.Open()
                    Try
                        cmd1.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd2.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd3.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd4.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd5.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd6.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd7.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd8.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd9.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd10.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd11.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd12.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    ProgressBar1.Increment(1)

                    con.Close()

                    check = String.Empty
                Else
                    MsgBox("Select Account Data", MessageBoxIcon.Error, "Wrong Selection")
                    Return
                End If

            Next
            MsgBox("Indexes Created Succssefully", MessageBoxIcon.Information, "Executed")
            ProgressBar1.Value = 0

            'ElseIf CheckedListBox1.CheckedItems.Count > 1 Then
            '    MsgBox("Select one database only", MessageBoxIcon.Error, "Error")
        Else
            MsgBox("You Dont Select Any Database", MessageBoxIcon.Error, "Check")
        End If


    End Sub

    Private Sub btn_CreatePayIndexes_Click(sender As Object, e As EventArgs) Handles btn_CreatePayIndexes.Click

        ProgressBar1.Maximum = chklstDatabases.CheckedItems.Count - 1
        DropIndexes()


        If chklstDatabases.CheckedItems.Count <> 0 Then

            For Each DatabaseChecked As Object In chklstDatabases.CheckedItems

                Dim con As New SqlConnection("server=" & cmbServers.Text & ";database=" & DatabaseChecked.ToString & ";user id=sa;password=togetherforever")

                Dim cmdcheck As New SqlCommand("select name from sys.tables where name='Empinfo' ", con)

                Dim strManualId As String = "CREATE NONCLUSTERED INDEX [Index_Empinfo_ManualId] ON [dbo].[EmpInfo]([ManualId] ASC)"
                Dim strIndate As String = "CREATE NONCLUSTERED INDEX [Index_Empinfo_InDate] ON [dbo].[EmpInfo]([InDate] ASC)"
                Dim strEmpId As String = "CREATE NONCLUSTERED INDEX [Index_Empmove_EmpId] ON [dbo].[EmpMove]([EmpId] ASC)"
                Dim strMonthf As String = "CREATE NONCLUSTERED INDEX [Index_Empmove_MonthF] ON [dbo].[EmpMove]([MonthF] ASC)"
                Dim strEmpid2 As String = "CREATE NONCLUSTERED INDEX [Index_Empsalaryvar_EmpId] ON [dbo].[EmpSalaryVar]([EmpId] ASC)"
                Dim strMonthf2 As String = "CREATE NONCLUSTERED INDEX [Index_Empsalaryvar_MonthF] ON [dbo].[EmpSalaryVar]([MonthF] ASC)"
                Dim strEmpcode As String = "CREATE NONCLUSTERED INDEX [Index_InOutInfo_Empcode] ON [dbo].[InOutInfo]([EmpCode] ASC)"
                Dim strMonthf3 As String = "CREATE NONCLUSTERED INDEX [Index_InOutInfo_Monthf] ON [dbo].[InOutInfo]([MonthF] ASC)"
                Dim strId As String = "CREATE NONCLUSTERED INDEX [Index_OverTime_Id] ON [dbo].[OverTime]([ID] ASC)"
                Dim strMonthf4 As String = "CREATE NONCLUSTERED INDEX [Index_OverTime_Monthf] ON [dbo].[OverTime]([MonthF] ASC)"


                Dim cmd1 As New SqlCommand(strManualId, con)
                Dim cmd2 As New SqlCommand(strIndate, con)
                Dim cmd3 As New SqlCommand(strEmpId, con)
                Dim cmd4 As New SqlCommand(strMonthf, con)
                Dim cmd5 As New SqlCommand(strEmpid2, con)
                Dim cmd6 As New SqlCommand(strMonthf2, con)
                Dim cmd7 As New SqlCommand(strEmpcode, con)
                Dim cmd8 As New SqlCommand(strMonthf3, con)
                Dim cmd9 As New SqlCommand(strId, con)
                Dim cmd10 As New SqlCommand(strMonthf4, con)

                con.Open()
                Dim dr As SqlDataReader = cmdcheck.ExecuteReader
                dr.Read()
                If dr.HasRows Then
                    check = dr(0)
                End If
                dr.Close()
                con.Close()

                If check = "EmpInfo" Then

                    con.Open()
                    Try
                        cmd1.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd2.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd3.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd4.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd5.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd6.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd7.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd8.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd9.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd10.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try

                    con.Close()
                    ProgressBar1.Increment(1)
                    check = String.Empty
                Else
                    MsgBox("Select Payroll Data", MessageBoxIcon.Error, "Wrong Selection")
                    Return
                End If

            Next
            MsgBox("Indexes Created Succssefully", MessageBoxIcon.Information, "Executed")
            ProgressBar1.Value = 0

            'ElseIf CheckedListBox1.CheckedItems.Count > 1 Then
            '    MsgBox("Select one database only", MessageBoxIcon.Error, "Error")
        Else
            MsgBox("You Dont Select Any Database", MessageBoxIcon.Error, "Check")
        End If
    End Sub

    Private Sub Btn_RebuildIndexesAcc_Click(sender As Object, e As EventArgs) Handles Btn_RebuildIndexesAcc.Click

        If chklstDatabases.CheckedItems.Count <> 0 Then

            ProgressBar1.Maximum = chklstDatabases.CheckedItems.Count - 1
            For Each DatabaseChecked As Object In chklstDatabases.CheckedItems

                Dim con As New SqlConnection("server=" & cmbServers.Text & ";database=" & DatabaseChecked.ToString & ";user id=sa;password=togetherforever")

                Dim cmdcheck As New SqlCommand("select name from sys.tables where name='items'", con)

                Dim RebuildAccMoveBatch As String = "ALTER INDEX ALL ON [dbo].[AccMoveBatch] REBUILD"
                Dim RebuildAccMoveDetail As String = "ALTER INDEX ALL ON [dbo].[AccMoveDetail] REBUILD"
                Dim RebuildAccMoveInfo As String = "ALTER INDEX ALL ON [dbo].[AccMoveInfo] REBUILD"
                Dim RebuildAccMoveMaster As String = "ALTER INDEX ALL ON [dbo].[AccMoveMaster] REBUILD"
                Dim RebuildInvDetail As String = "ALTER INDEX ALL ON [dbo].[InvDetail]  REBUILD"
                Dim RebuildInvMaster As String = "ALTER INDEX ALL ON [dbo].[InvMaster] REBUILD"
                Dim Rebuildpo As String = "ALTER INDEX ALL ON [dbo].[Po] REBUILD"
                Dim RebuildItems As String = "ALTER INDEX ALL ON [dbo].[Items] REBUILD"

                Dim UpdateStatisAccMoveBatch As String = " UPDATE STATISTICS [dbo].[AccMoveBatch] "
                Dim UpdateStatisAccMoveDetail As String = " UPDATE STATISTICS [dbo].[AccMoveDetail] "
                Dim UpdateStatisAccMoveInfo As String = " UPDATE STATISTICS [dbo].[AccMoveInfo] "
                Dim UpdateStatisAccMoveMaster As String = "UPDATE STATISTICS [dbo].[AccMoveMaster] "
                Dim UpdateStatisInvDetail As String = "UPDATE STATISTICS [dbo].[InvDetail]  "
                Dim UpdateStatisInvMaster As String = "UPDATE STATISTICS [dbo].[InvMaster] "
                Dim UpdateStatispo As String = "UPDATE STATISTICS [dbo].[Po] "
                Dim UpdateStatisItems As String = "UPDATE STATISTICS [dbo].[Items] "


                Dim cmd1 As New SqlCommand(RebuildAccMoveBatch, con)
                Dim cmd2 As New SqlCommand(RebuildAccMoveDetail, con)
                Dim cmd3 As New SqlCommand(RebuildAccMoveInfo, con)
                Dim cmd4 As New SqlCommand(RebuildAccMoveMaster, con)
                Dim cmd5 As New SqlCommand(RebuildInvDetail, con)
                Dim cmd6 As New SqlCommand(RebuildInvMaster, con)
                Dim cmd7 As New SqlCommand(Rebuildpo, con)
                Dim cmd8 As New SqlCommand(RebuildItems, con)

                Dim cmd9 As New SqlCommand(UpdateStatisAccMoveBatch, con)
                Dim cmd10 As New SqlCommand(UpdateStatisAccMoveDetail, con)
                Dim cmd11 As New SqlCommand(UpdateStatisAccMoveInfo, con)
                Dim cmd12 As New SqlCommand(UpdateStatisAccMoveMaster, con)
                Dim cmd13 As New SqlCommand(UpdateStatisInvDetail, con)
                Dim cmd14 As New SqlCommand(UpdateStatisInvMaster, con)
                Dim cmd15 As New SqlCommand(UpdateStatispo, con)
                Dim cmd16 As New SqlCommand(UpdateStatisItems, con)

                con.Open()
                Dim dr As SqlDataReader = cmdcheck.ExecuteReader
                dr.Read()
                If dr.HasRows Then
                    check = dr(0)
                End If
                dr.Close()
                con.Close()

                If check <> String.Empty Then

                    con.Open()
                    Try
                        cmd1.ExecuteNonQuery()
                        ProgressBar1.Increment(1)
                    Catch ex As Exception
                    End Try
                    Try
                        cmd2.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd3.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd4.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd5.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd6.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd7.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd8.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd9.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd10.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd11.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd12.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd13.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd14.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd15.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd16.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try

                    con.Close()

                    check = String.Empty
                Else
                    MsgBox("Select Account Data", MessageBoxIcon.Error, "Wrong Selection")
                    Return
                End If

            Next
            MsgBox("Indexes Rebuilded " & Environment.NewLine & "Statistics Updated", MessageBoxIcon.Information, "Executed")
            ProgressBar1.Value = 0

            'ElseIf CheckedListBox1.CheckedItems.Count > 1 Then
            '    MsgBox("Select one database only", MessageBoxIcon.Error, "Error")
        Else
            MsgBox("You Dont Select Any Database", MessageBoxIcon.Error, "Check")
        End If
    End Sub

    Private Sub btn_RebuildPayIndexes_Click(sender As Object, e As EventArgs) Handles btn_RebuildPayIndexes.Click

        If chklstDatabases.CheckedItems.Count <> 0 Then

            ProgressBar1.Maximum = chklstDatabases.CheckedItems.Count - 1

            For Each DatabaseChecked As Object In chklstDatabases.CheckedItems

                Dim con As New SqlConnection("server=" & cmbServers.Text & ";database=" & DatabaseChecked.ToString & ";user id=sa;password=togetherforever")

                Dim cmdcheck As New SqlCommand("select name from sys.tables where name='Empinfo' ", con)

                Dim RebuildEmpinfo As String = "ALTER INDEX ALL ON [dbo].[EmpInfo] REBUILD"
                Dim RebuildEmpMove As String = "ALTER INDEX ALL ON [dbo].[EmpMove] REBUILD"
                Dim RebuildEmpSalaryVar As String = "ALTER INDEX ALL ON [dbo].[EmpSalaryVar] REBUILD"
                Dim RebuildInOutInfo As String = "ALTER INDEX ALL ON [dbo].[InOutInfo] REBUILD"
                Dim RebuildOverTime As String = "ALTER INDEX ALL ON [dbo].[OverTime] REBUILD"

                Dim UpdateStatisEmpinfo As String = "UPDATE STATISTICS [dbo].[EmpInfo] "
                Dim UpdateStatisEmpMove As String = " UPDATE STATISTICS [dbo].[EmpMove] "
                Dim UpdateStatisEmpSalaryVar As String = " UPDATE STATISTICS [dbo].[EmpSalaryVar] "
                Dim UpdateStatisInOutInfo As String = " UPDATE STATISTICS [dbo].[InOutInfo] "
                Dim UpdateStatisOverTime As String = " UPDATE STATISTICS [dbo].[OverTime] "

                Dim cmd1 As New SqlCommand(RebuildEmpinfo, con)
                Dim cmd2 As New SqlCommand(RebuildEmpMove, con)
                Dim cmd3 As New SqlCommand(RebuildEmpSalaryVar, con)
                Dim cmd4 As New SqlCommand(RebuildInOutInfo, con)
                Dim cmd5 As New SqlCommand(RebuildOverTime, con)

                Dim cmd6 As New SqlCommand(UpdateStatisEmpinfo, con)
                Dim cmd7 As New SqlCommand(UpdateStatisEmpMove, con)
                Dim cmd8 As New SqlCommand(UpdateStatisEmpSalaryVar, con)
                Dim cmd9 As New SqlCommand(UpdateStatisInOutInfo, con)
                Dim cmd10 As New SqlCommand(UpdateStatisOverTime, con)

                con.Open()
                Dim dr As SqlDataReader = cmdcheck.ExecuteReader
                dr.Read()
                If dr.HasRows Then
                    check = dr(0)
                End If
                dr.Close()
                con.Close()

                If check <> String.Empty Then

                    con.Open()
                    Try
                        cmd1.ExecuteNonQuery()
                        ProgressBar1.Increment(1)
                    Catch ex As Exception
                    End Try
                    Try
                        cmd2.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd3.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd4.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd5.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd6.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd7.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd8.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd9.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try
                    Try
                        cmd10.ExecuteNonQuery()
                    Catch ex As Exception
                    End Try

                    con.Close()

                    check = String.Empty
                Else
                    MsgBox("Select Payroll Data", MessageBoxIcon.Error, "Wrong Selection")
                    Return
                End If

            Next
            MsgBox("Indexes Rebuilded " & Environment.NewLine & "Statistics Updated", MessageBoxIcon.Information, "Executed")
            ProgressBar1.Value = 0
            'ElseIf CheckedListBox1.CheckedItems.Count > 1 Then
            '    MsgBox("Select one database only", MessageBoxIcon.Error, "Error")
        Else
            MsgBox("You Dont Select Any Database", MessageBoxIcon.Error, "Check")
        End If
    End Sub

    Private Sub btnDeleteAllDatabase_Click(sender As Object, e As EventArgs) Handles btnDeleteAllDatabase.Click

        'message before delete
        Dim result As Integer = MessageBox.Show("Are You Sure You Want To Delete Selected Databases?", "Delete All Databases", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.No Then
            Return
        End If

        Dim path = cmbDrivers.Text & "DeletedDatabases" & " " & Today.ToString("dd.MM.yyyy")

        ' Check if drive is selected first
        If cmbDrivers.Text = "" Then
            MsgBox("Please Select Backup Drive", MessageBoxIcon.Information, "Select Drive")
            cmbDrivers.Focus()
            Exit Sub
        End If

        If chklstDatabases.CheckedItems.Count <> 0 Then
            Dim dr As New DriveInfo(cmbDrivers.Text)
            If dr.IsReady Then
                ProgressBar1.Maximum = chklstDatabases.CheckedItems.Count - 1

                ' First, backup all selected databases
                For Each DatabaseChecked As Object In chklstDatabases.CheckedItems
                    Dim i As String = DatabaseChecked.ToString
                    Dim lfolder As String

                    Try
                        If Not Directory.Exists(path) Then
                            Directory.CreateDirectory(path)
                        End If
                    Catch ex As Exception
                        MsgBox("You Try To Create Folder In Cdrom Drive OR Google Drive" & Environment.NewLine & "CHOOSE VALID PATH", MsgBoxStyle.Exclamation, "Error")
                        Exit Sub
                    End Try

                    lfolder = path & "\" & DatabaseChecked.ToString & "--" & Format(DateTime.Now, "dd.MM.yyyy HH.mm") & ".bak"
                    Dim strbak As String = "Backup database " & DatabaseChecked.ToString & " To Disk='" & lfolder & "' With Format,compression "
                    Dim cmd1 As New SqlCommand(strbak, con)

                    Try
                        con.Open()
                        cmd1.ExecuteNonQuery()
                        ProgressBar1.Increment(1)
                    Catch ex As Exception
                        MsgBox("Cannot Backup database" & " " & i & Environment.NewLine & "Check Data After Backup Finished", MsgBoxStyle.Exclamation, "Error")
                    End Try
                    con.Close()
                Next

                ' After all backups are complete, drop the databases
                For Each DatabaseChecked As Object In chklstDatabases.CheckedItems
                    Dim i As String = DatabaseChecked.ToString
                    Dim strDrop As String = "ALTER DATABASE [" & DatabaseChecked.ToString & "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;" &
                                            "DROP DATABASE [" & DatabaseChecked.ToString & "]"
                    Dim cmdDrop As New SqlCommand(strDrop, con)

                    Try
                        con.Open()
                        cmdDrop.ExecuteNonQuery()
                    Catch ex As Exception
                        MsgBox("Cannot Drop database" & " " & i & Environment.NewLine & "Check Server After Operation", MsgBoxStyle.Exclamation, "Error")
                    End Try
                    con.Close()
                Next

                MsgBox("Backup and Drop Successfully Completed", MessageBoxIcon.Information, "Operation Successfully")
                ProgressBar1.Value = 0
                CboServer_SelectedIndexChanged(Nothing, Nothing)
                Process.Start("explorer.exe", path)
            Else
                MsgBox("Selected Drive is Not Ready" & Environment.NewLine & "Please Check Drive", MsgBoxStyle.Exclamation, "Drive Error")
            End If
        Else
            MsgBox("Select Data You Want To Delete", MessageBoxIcon.Exclamation, "Info")
        End If
    End Sub

    Private Sub btnLoadServers_Click(sender As Object, e As EventArgs) Handles btnLoadDatabases.Click
        GetDatabases()
    End Sub

    Private Sub btn_Shrink_Click(sender As Object, e As EventArgs) Handles btn_Shrink.Click
        ShrinkDatabases()
    End Sub
End Class
