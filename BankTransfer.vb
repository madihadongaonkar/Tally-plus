﻿Public Class BankTransfer

    Dim con, con1, con2, con3, con4, con5 As SqlClient.SqlConnection
    Dim sel, upd, ins As SqlClient.SqlCommand
    Dim da As SqlClient.SqlDataAdapter
    Dim ds As DataSet
    Dim dr As SqlClient.SqlDataReader
    Dim dt As DataTable

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        LedgerCreation.ShowDialog()
        BankTransfer_Load(e, e)
    End Sub

    Private Sub BankTransfer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        con = New SqlClient.SqlConnection("Data Source=.\SQLEXPRESS;AttachDbFilename=D:\Tally Plus\Tally Plus\Database1.mdf;Integrated Security=True;User Instance=True")
        con1 = New SqlClient.SqlConnection("Data Source=.\SQLEXPRESS;AttachDbFilename=D:\Tally Plus\Tally Plus\Database1.mdf;Integrated Security=True;User Instance=True")
        con2 = New SqlClient.SqlConnection("Data Source=.\SQLEXPRESS;AttachDbFilename=D:\Tally Plus\Tally Plus\Database1.mdf;Integrated Security=True;User Instance=True")
        con3 = New SqlClient.SqlConnection("Data Source=.\SQLEXPRESS;AttachDbFilename=D:\Tally Plus\Tally Plus\Database1.mdf;Integrated Security=True;User Instance=True")
        con4 = New SqlClient.SqlConnection("Data Source=.\SQLEXPRESS;AttachDbFilename=D:\Tally Plus\Tally Plus\Database1.mdf;Integrated Security=True;User Instance=True")
        con5 = New SqlClient.SqlConnection("Data Source=.\SQLEXPRESS;AttachDbFilename=D:\Tally Plus\Tally Plus\Database1.mdf;Integrated Security=True;User Instance=True")

        Try
            sel = New SqlClient.SqlCommand("Select * from ContraDetails where Company_Name=@cn", con)
            sel.Parameters.Add("@cn", SqlDbType.Char).Value = Convert.ToString(Company_Information.CompanyName.Text)
            da = New SqlClient.SqlDataAdapter(sel)
            ds = New DataSet

            con.Open()
            da.Fill(ds, "ReceiptDetails")
            con.Close()

            DataGridView1.DataSource = ds
            DataGridView1.DataMember = "ReceiptDetails"
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        con.Close()

        sr.Text = (DataGridView1.RowCount + 1).ToString

        Try
            sel = New SqlClient.SqlCommand("select * from Ledger where Ledger_Group=@gn and Company_Name=@cn", con)
            sel.Parameters.Add("@gn", SqlDbType.Char).Value = Convert.ToString("Bank Account")
            sel.Parameters.Add("@cn", SqlDbType.Char).Value = Convert.ToString(Company_Information.CompanyName.Text)
            dt = New DataTable
            con.Open()
            dr = sel.ExecuteReader
            dt.Load(dr)
            con.Close()

            ComboBox4.DataSource = dt
            ComboBox4.DisplayMember = "Ledger_Name"

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        con.Close()

        Try
            sel = New SqlClient.SqlCommand("select * from Ledger where Ledger_Group=@gn and Company_Name=@cn", con)
            sel.Parameters.Add("@gn", SqlDbType.Char).Value = Convert.ToString("Bank Account")
            sel.Parameters.Add("@cn", SqlDbType.Char).Value = Convert.ToString(Company_Information.CompanyName.Text)
            dt = New DataTable
            con.Open()
            dr = sel.ExecuteReader
            dt.Load(dr)
            con.Close()

            ComboBox3.DataSource = dt
            ComboBox3.DisplayMember = "Ledger_Name"

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        con.Close()
    End Sub

    Private Sub ComboBox4_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox4.SelectedIndexChanged
        Try
            sel = New SqlClient.SqlCommand("select * from Ledger where Ledger_Name=@ln and Company_Name=@cn", con)
            sel.Parameters.Add("@ln", SqlDbType.Char).Value = Convert.ToString(ComboBox4.Text)
            sel.Parameters.Add("@cn", SqlDbType.Char).Value = Convert.ToString(Company_Information.CompanyName.Text)
            con.Open()
            dr = sel.ExecuteReader
            If dr.Read Then
                txtfbb.Text = dr("Opening_Balance").ToString
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        con.Close()
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox3.SelectedIndexChanged
        Try
            sel = New SqlClient.SqlCommand("select * from Ledger where Ledger_Name=@ln and Company_Name=@cn", con)
            sel.Parameters.Add("@ln", SqlDbType.Char).Value = Convert.ToString(ComboBox3.Text)
            sel.Parameters.Add("@cn", SqlDbType.Char).Value = Convert.ToString(Company_Information.CompanyName.Text)
            con.Open()
            dr = sel.ExecuteReader
            If dr.Read Then
                txttbb.Text = dr("Opening_Balance").ToString
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        con.Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim bal As Integer

        If (Integer.Parse(txtfbb.Text) < Integer.Parse(txttamt.Text)) Then
            MsgBox("Transaction Not Done", MsgBoxStyle.Critical, "Negative Cash")
        Else

            Try
                sel = New SqlClient.SqlCommand("select * from Ledger where Company_Name=@cn and Ledger_Name=@ln", con)
                sel.Parameters.Add("@cn", SqlDbType.Char).Value = Convert.ToString(Company_Information.CompanyName.Text)
                sel.Parameters.Add("@ln", SqlDbType.Char).Value = Convert.ToString(ComboBox4.Text)
                con.Open()
                dr = sel.ExecuteReader
                If dr.Read Then
                    bal = Integer.Parse(dr("Opening_Balance"))

                    ' Code to Update Bank Account Balance
                    Try
                        upd = New SqlClient.SqlCommand("update Ledger set Opening_Balance=@op where Company_Name=@cn and Ledger_Name=@ln", con1)
                        upd.Parameters.Add("@op", SqlDbType.Char).Value = Convert.ToString(Val(txtfbb.Text) - Val(txttamt.Text))
                        upd.Parameters.Add("@cn", SqlDbType.Char).Value = Convert.ToString(Company_Information.CompanyName.Text)
                        upd.Parameters.Add("@ln", SqlDbType.Char).Value = Convert.ToString(ComboBox4.Text)
                        con1.Open()
                        upd.ExecuteNonQuery()

                        ' Code to Update Cash Account Balance
                        Try
                            upd = New SqlClient.SqlCommand("update Ledger set Opening_Balance=@op where Company_Name=@cn and Ledger_Name=@ln", con2)
                            upd.Parameters.Add("@op", SqlDbType.Char).Value = Convert.ToString(Val(txttbb.Text) + Val(txttamt.Text))
                            upd.Parameters.Add("@cn", SqlDbType.Char).Value = Convert.ToString(Company_Information.CompanyName.Text)
                            upd.Parameters.Add("@ln", SqlDbType.Char).Value = Convert.ToString(ComboBox3.Text)
                            con2.Open()
                            upd.ExecuteNonQuery()

                            ' Code to Insert Record in Contra Table
                            Try
                                ins = New SqlClient.SqlCommand("insert into ContraDetails values(@Company_Name,@Voucher_No,@Date,@Particulars,@Voucher_Type,@Debit_Amount,@Credit_Amount)", con5)
                                ins.Parameters.AddWithValue("@Company_Name", Company_Information.CompanyName.Text)
                                ins.Parameters.AddWithValue("@Voucher_No", sr.Text)
                                ins.Parameters.AddWithValue("@Date", DateTimePicker1.Text)
                                ins.Parameters.AddWithValue("@Particulars", ComboBox4.Text)
                                ins.Parameters.AddWithValue("@Voucher_Type", vt.Text)
                                ins.Parameters.AddWithValue("@Debit_Amount", "0")
                                ins.Parameters.AddWithValue("@Credit_Amount", txttamt.Text)
                                con5.Open()
                                ins.ExecuteNonQuery()

                                ' Code to Insert Data in Transaction Table
                                Try
                                    ins = New SqlClient.SqlCommand("insert into Trans values(@Company_Name,@Voucher_No,@Date,@Particulars,@Voucher_Type,@Debit_Amount,@Credit_Amount)", con3)
                                    ins.Parameters.AddWithValue("@Company_Name", Company_Information.CompanyName.Text)
                                    ins.Parameters.AddWithValue("@Voucher_No", sr.Text)
                                    ins.Parameters.AddWithValue("@Date", DateTimePicker1.Text)
                                    ins.Parameters.AddWithValue("@Particulars", ComboBox4.Text)
                                    ins.Parameters.AddWithValue("@Voucher_Type", vt.Text)
                                    ins.Parameters.AddWithValue("@Debit_Amount", "0")
                                    ins.Parameters.AddWithValue("@Credit_Amount", txttamt.Text)
                                    con3.Open()
                                    ins.ExecuteNonQuery()

                                    Try
                                        ins = New SqlClient.SqlCommand("insert into Trans values(@Company_Name,@Voucher_No,@Date,@Particulars,@Voucher_Type,@Debit_Amount,@Credit_Amount)", con4)
                                        ins.Parameters.AddWithValue("@Company_Name", Company_Information.CompanyName.Text)
                                        ins.Parameters.AddWithValue("@Voucher_No", sr.Text)
                                        ins.Parameters.AddWithValue("@Date", DateTimePicker1.Text)
                                        ins.Parameters.AddWithValue("@Particulars", ComboBox3.Text)
                                        ins.Parameters.AddWithValue("@Voucher_Type", vt.Text)
                                        ins.Parameters.AddWithValue("@Debit_Amount", txttamt.Text)
                                        ins.Parameters.AddWithValue("@Credit_Amount", "0")
                                        con4.Open()
                                        ins.ExecuteNonQuery()

                                        MsgBox("Transaction Done Successfully", MsgBoxStyle.Information, "Transaction Success")
                                        txttamt.Text = ""
                                    Catch ex As Exception
                                        MsgBox(ex.Message)
                                    End Try
                                    con4.Close()


                                    'MsgBox("Transaction Done Successfully", MsgBoxStyle.Information, "Transaction Success")
                                    txttamt.Text = ""
                                Catch ex As Exception
                                    MsgBox(ex.Message)
                                End Try
                                con3.Close()

                            Catch ex As Exception
                                MsgBox(ex.Message)
                            End Try
                            con5.Close()

                        Catch ex As Exception
                            MsgBox(ex.Message)
                        End Try
                        con2.Close()

                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try
                    con1.Close()

                    BankTransfer_Load(e, e)

                End If
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
            con.Close()
        End If
    End Sub

    Private Sub txttamt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txttamt.TextChanged
        If txttamt.Text = "" Then
            Button2.Enabled = False
        Else
            Button2.Enabled = True
        End If
    End Sub
End Class