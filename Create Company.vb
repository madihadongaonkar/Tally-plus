
Imports System.Data.SqlClient

Public Class Create_Company

    Dim con, con1, con2 As SqlClient.SqlConnection
    Dim sel, ins, upd As SqlClient.SqlCommand
    Dim dt As DataTable
    Dim dr As SqlClient.SqlDataReader

    Private Sub Create_Company_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        path.Text = AppDomain.CurrentDomain.BaseDirectory
        con = New SqlClient.SqlConnection("Data Source=.\SQLEXPRESS;AttachDbFilename=D:\Tally Plus\Tally Plus\Database1.mdf;Integrated Security=True;User Instance=True")
        con1 = New SqlClient.SqlConnection("Data Source=.\SQLEXPRESS;AttachDbFilename=D:\Tally Plus\Tally Plus\Database1.mdf;Integrated Security=True;User Instance=True")
        con2 = New SqlClient.SqlConnection("Data Source=.\SQLEXPRESS;AttachDbFilename=D:\Tally Plus\Tally Plus\Database1.mdf;Integrated Security=True;User Instance=True")

        Try
            sel = New SqlClient.SqlCommand("select * from Country", con)
            dt = New DataTable
            con.Open()
            dr = sel.ExecuteReader
            dt.Load(dr)
            con.Close()

            ComboBox1.DataSource = dt
            ComboBox1.DisplayMember = "Country_Name"
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Try
            sel = New SqlClient.SqlCommand("select * from State where Country_Name=@cn", con)
            sel.Parameters.Add("@cn", SqlDbType.Char).Value = Convert.ToString(ComboBox1.Text)
            dt = New DataTable
            con.Open()
            dr = sel.ExecuteReader
            dt.Load(dr)
            con.Close()

            ComboBox2.DataSource = dt
            ComboBox2.DisplayMember = "State_Name"
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub Create_Company_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Company_Information.Panel1.Visible = True
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        ' Company Name
        If TextBox15.Text = "" Then
            ep.SetError(TextBox15, "Enter Company Name")
        Else
            ep.SetError(TextBox15, "")
        End If

        'Mailing Name
        If TextBox1.Text = "" Then
            ep.SetError(TextBox1, "Enter Mailing Name")
        Else
            ep.SetError(TextBox1, "")
        End If

        'Address
        If TextBox2.Text = "" Then
            ep.SetError(TextBox2, "Enter Address")
        Else
            ep.SetError(TextBox2, "")
        End If

        'Pin Code
        If TextBox5.Text = "" Then
            ep.SetError(TextBox5, "Enter Pin Code")
        Else
            ep.SetError(TextBox5, "")
        End If

        'Mobile No
        If MaskedTextBox1.Text = "( +   ) " Then
            ep.SetError(MaskedTextBox1, "Enter Mobile No")
        Else
            ep.SetError(MaskedTextBox1, "")
        End If

        'E-mail ID
        If TextBox7.Text = "" Then
            ep.SetError(TextBox7, "Enter E-mail ID")
        Else
            ep.SetError(TextBox7, "")
        End If

        'Currency Symbol
        If TextBox8.Text = "" Then
            ep.SetError(TextBox8, "Enter Currency Symbol")
        Else
            ep.SetError(TextBox8, "")
        End If

        'Vault Password
        If TextBox12.Text = "" Then
            ep.SetError(TextBox12, "Enter Vault Password")
        Else
            ep.SetError(TextBox12, "")
        End If

        'Confirm Password
        If TextBox13.Text = "" Then
            ep.SetError(TextBox13, "Enter Confirm Password")
        Else
            ep.SetError(TextBox13, "")
        End If

        Dim dateone = DateTimePicker1.Value
        Dim datetwo = DateTimePicker2.Value

        Dim diff As TimeSpan = datetwo - dateone

        Dim month = (datetwo.Month - dateone.Month) + 12 * (datetwo.Year - dateone.Year)

        If month.ToString > 11 Then
            'MsgBox(month.ToString)
            ep.SetError(DateTimePicker2, "Check Date")
        Else
            'MsgBox(month.ToString)
            ep.SetError(DateTimePicker2, "")
        End If

        If TextBox15.Text <> "" And TextBox1.Text <> "" And TextBox2.Text <> "" And TextBox5.Text <> "" And MaskedTextBox1.Text <> "( +   ) " And TextBox7.Text <> "" And TextBox8.Text <> "" And TextBox12.Text <> "" And TextBox13.Text <> "" And month.ToString < 12 Then
            If TextBox12.Text <> TextBox13.Text Then
                ep.SetError(TextBox13, "Password Not Match")
            Else
                ep.SetError(TextBox13, "")

                Try
                    sel = New SqlClient.SqlCommand("select * from CompanyData where Company_Name=@cn", con)
                    sel.Parameters.Add("@cn", SqlDbType.Char).Value = Convert.ToString(TextBox15.Text)
                    con.Open()
                    dr = sel.ExecuteReader
                    If dr.Read Then
                        MsgBox("Company Name Adready Exist", MsgBoxStyle.Critical, "Company Name Exist")
                        TextBox15.Focus()
                    Else

                        Try
                            ins = New SqlClient.SqlCommand("insert into CompanyData values(@Company_Name,@Mailing_Name,@Address,@Country,@State,@Pin_Code,@Telephone_No,@E_Mail_ID,@Currency_Sign,@Data_Manage_Type,@Financial_Year,@Company_Start_Year,@Password,@Confirm_Password)", con1)
                            ins.Parameters.AddWithValue("@Company_Name", TextBox15.Text)
                            ins.Parameters.AddWithValue("@Mailing_Name", TextBox1.Text)
                            ins.Parameters.AddWithValue("@Address", TextBox2.Text)
                            ins.Parameters.AddWithValue("@Country", ComboBox1.Text)
                            ins.Parameters.AddWithValue("@State", ComboBox2.Text)
                            ins.Parameters.AddWithValue("@Pin_Code", TextBox5.Text)
                            ins.Parameters.AddWithValue("@Telephone_No", MaskedTextBox1.Text)
                            ins.Parameters.AddWithValue("@E_Mail_ID", TextBox7.Text)
                            ins.Parameters.AddWithValue("@Currency_Sign", TextBox8.Text)
                            ins.Parameters.AddWithValue("@Data_Manage_Type", ComboBox3.Text)
                            ins.Parameters.AddWithValue("@Financial_Year", DateTimePicker1.Text)
                            ins.Parameters.AddWithValue("@Company_Start_Year", DateTimePicker2.Text)
                            ins.Parameters.AddWithValue("@Password", TextBox12.Text)
                            ins.Parameters.AddWithValue("@Confirm_Password", TextBox13.Text)
                            con1.Open()
                            ins.ExecuteNonQuery()

                            Try
                                ins = New SqlClient.SqlCommand("insert into Ledger values(@Company_Name,@Ledger_Name,@Ledger_Group,@Description,@Opening_Balance)", con2)
                                ins.Parameters.AddWithValue("@Company_Name", TextBox15.Text)
                                ins.Parameters.AddWithValue("@Ledger_Name", "Cash")
                                ins.Parameters.AddWithValue("@Ledger_Group", "Current Assets")
                                ins.Parameters.AddWithValue("@Description", "Cash in Hand")
                                ins.Parameters.AddWithValue("@Opening_Balance", 0)
                                con2.Open()
                                ins.ExecuteNonQuery()

                            Catch ex As Exception
                                MsgBox(ex.Message)
                            End Try
                            con2.Close()

                            MsgBox("Company Created Successfully", MsgBoxStyle.Information, "Company Created")
                            TextBox15.Text = ""
                            TextBox1.Text = ""
                            TextBox2.Text = ""
                            TextBox5.Text = ""
                            MaskedTextBox1.Text = "( +   ) "
                            TextBox7.Text = ""
                            TextBox8.Text = ""
                            ComboBox3.Text = "----- Choose Type ------"
                            DateTimePicker1.Text = Now
                            DateTimePicker2.Text = Now
                            TextBox12.Text = ""
                            TextBox13.Text = ""
                            TextBox15.Focus()
                        Catch ex As Exception
                            MsgBox(ex.Message)
                        End Try
                        con1.Close()

                    End If
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try
                con.Close()
            End If
        End If


        

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Try
            sel = New SqlClient.SqlCommand("select * from State where Country_Name=@cn", con)
            sel.Parameters.Add("@cn", SqlDbType.Char).Value = Convert.ToString(ComboBox1.Text)
            dt = New DataTable
            con.Open()
            dr = sel.ExecuteReader
            dt.Load(dr)
            con.Close()

            ComboBox2.DataSource = dt
            ComboBox2.DisplayMember = "State_Name"
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub DateTimePicker2_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker2.ValueChanged
        Dim dateone = DateTimePicker1.Value
        Dim datetwo = DateTimePicker2.Value

        Dim diff As TimeSpan = datetwo - dateone

        Dim month = (datetwo.Month - dateone.Month) + 12 * (datetwo.Year - dateone.Year)

        'MsgBox(month.ToString)
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Try
            upd = New SqlClient.SqlCommand("update CompanyData set(Mailing_Name=@mn,Address=@aas,Country=@coun,State=@sta,Pin_Code=@pin,Telephone_No=@tn,E_Mail_ID=@eid,Currency_Sign=@cse,Data_Manage_Type=dm,Financial_Year=@fy,Company_Start_Year=@cs,Password=@ps,Confirm_Password=@cp where company where company_name=@company_name)", con1)
            upd.Parameters.Add("@company_name", SqlDbType.Char).Value = Convert.ToString(TextBox15.Text)
            upd.Parameters.Add("@Mailing_Name", SqlDbType.Char).Value = Convert.ToString(TextBox1.Text)
            upd.Parameters.Add("@Address", SqlDbType.Char).Value = Convert.ToString(TextBox2.Text)
            upd.Parameters.Add("@Country", SqlDbType.Char).Value = Convert.ToString(ComboBox1.Text)
            upd.Parameters.Add("@State", SqlDbType.Char).Value = Convert.ToString(ComboBox2.Text)
            upd.Parameters.Add("@Pin_Code", SqlDbType.Char).Value = Convert.ToString(TextBox5.Text)
            upd.Parameters.Add("@Telephone_No", SqlDbType.Char).Value = Convert.ToString(MaskedTextBox1.Text)
            upd.Parameters.Add("@E_Mail_ID", SqlDbType.Char).Value = Convert.ToString(TextBox7.Text)
            upd.Parameters.Add("@Currency_Sign", SqlDbType.Char).Value = Convert.ToString(TextBox8.Text)
            upd.Parameters.AddWithValue("@Data_Manage_Type", ComboBox3.Text)
            upd.Parameters.AddWithValue("@Financial_Year", DateTimePicker1.Text)
            upd.Parameters.AddWithValue("@Company_Start_Year", DateTimePicker2.Text)
            upd.Parameters.AddWithValue("@Password", TextBox12.Text)
            upd.Parameters.AddWithValue("@Confirm_Password", TextBox13.Text)
            con1.Open()
            upd.ExecuteNonQuery()
            MsgBox("Record updated successfully", MsgBoxStyle.Information, "Record Updated")

        Catch ex As Exception
            MsgBox(ex.Message)

        End Try
        con1.Close()
    End Sub
End Class