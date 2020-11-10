Public Class AlterCompany

    Dim con As SqlClient.SqlConnection
    Dim sel As SqlClient.SqlCommand
    Dim da As SqlClient.SqlDataAdapter
    Dim dr As SqlClient.SqlDataReader
    Dim ds As DataSet
    Dim cn As String

    Private Sub AlterCompany_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        con = New SqlClient.SqlConnection("Data Source=.\SQLEXPRESS;AttachDbFilename=D:\Tally Plus\Tally Plus\Database1.mdf;Integrated Security=True;User Instance=True")

        Try
            sel = New SqlClient.SqlCommand("select Company_Name from CompanyData", con)
            da = New SqlClient.SqlDataAdapter(sel)
            ds = New DataSet
            con.Open()
            da.Fill(ds, "CompanyData")
            con.Close()

            DataGridView1.DataSource = ds
            DataGridView1.DataMember = "CompanyData"

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub DataGridView1_CellContentDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentDoubleClick
        Password.cn.Text = DataGridView1.SelectedRows(0).Cells(0).Value.ToString
        Me.Close()
        Password.Show()
    End Sub
End Class