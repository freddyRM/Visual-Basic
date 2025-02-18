Imports System.Data.OleDb
Imports System.IO

Public Class Form1

    ' Ruta de la base de datos Access
    Private dataBase As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\..\..\Database.accdb")
    ' Cadena de conexión a la base de datos Access
    Private connectionString As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dataBase};"

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Ajustar el DataGridView al tamaño del formulario
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        CargarDatos()
    End Sub

    Private Sub CargarDatos()
        ' Consulta SQL optimizada con alias para las columnas
        Dim query As String = "SELECT Laboratorio.LabCodigo AS Codigo, " &
                           "Curso.CurNombre AS Nombre, " &
                           "Laboratorio.LabHora AS Horario, " &
                           "Laboratorio.LabProfe AS Jefe_de_Practica, " &
                           "Curso.CurProfe AS Profesor " &
                           "FROM Laboratorio " &
                           "LEFT JOIN Curso ON Laboratorio.LabCodigo = Curso.CurCodigo"

        Try
            Using connection As New OleDbConnection(connectionString)
                Using command As New OleDbCommand(query, connection)
                    connection.Open()

                    ' Crear un DataTable para almacenar los datos
                    Dim dataTable As New DataTable()

                    ' Llenar el DataGridView directamente desde el DataReader
                    Using reader As OleDbDataReader = command.ExecuteReader()
                        dataTable.Load(reader)
                    End Using

                    ' Asignar el DataTable como fuente de datos del DataGridView
                    DataGridView1.DataSource = dataTable

                    ' Personalizar los encabezados de las columnas
                    DataGridView1.Columns("Codigo").HeaderText = "Código"
                    DataGridView1.Columns("Nombre").HeaderText = "Nombre"
                    DataGridView1.Columns("Horario").HeaderText = "Horario"
                    DataGridView1.Columns("Jefe_de_Practica").HeaderText = "Jefe de práctica"
                    DataGridView1.Columns("Profesor").HeaderText = "Profesor"
                End Using
            End Using
        Catch ex As Exception
            ' Manejar diferentes tipos de excepciones si es necesario
            MessageBox.Show("Error al cargar los datos: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Sub EliminarLinea(codigo As String)
        Using connection As New OleDbConnection(connectionString)
            connection.Open()

            ' Eliminar de la tabla Laboratorio
            Dim queryLaboratorio As String = "DELETE FROM Laboratorio WHERE LabCodigo = ?"
            Using commandLaboratorio As New OleDbCommand(queryLaboratorio, connection)
                commandLaboratorio.Parameters.AddWithValue("@LabCodigo", codigo)
                commandLaboratorio.ExecuteNonQuery()
            End Using

            ' Eliminar de la tabla Curso
            Dim queryCurso As String = "DELETE FROM Curso WHERE CurCodigo = ?"
            Using commandCurso As New OleDbCommand(queryCurso, connection)
                commandCurso.Parameters.AddWithValue("@CurCodigo", codigo)
                commandCurso.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Sub btnBorrar_Click(sender As Object, e As EventArgs) Handles btnBorrar.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim confirmacion As DialogResult = MessageBox.Show(
            "¿Está seguro de que desea eliminar esta fila?",
            "Confirmación",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning
        )

            If confirmacion = DialogResult.Yes Then
                Dim filaIndex As Integer = DataGridView1.SelectedRows(0).Index
                Dim codigo As String = DataGridView1.SelectedRows(0).Cells(0).Value.ToString()

                ' Eliminar la fila del DataGridView
                DataGridView1.Rows.RemoveAt(filaIndex)

                ' Eliminar la fila en ambas tablas de la base de datos
                EliminarLinea(codigo)
            End If
        Else
            MessageBox.Show(
            "Seleccione una fila para eliminar.",
            "Advertencia",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning
        )
        End If
    End Sub

    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        Application.Exit()
    End Sub
End Class