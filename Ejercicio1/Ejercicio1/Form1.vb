Public Class Form1

    Private Function CalcularPago(ByVal horasTrabajadas As Integer) As Double
        Dim total As Double

        If horasTrabajadas <= 40 Then
            total = horasTrabajadas * 7.5
        Else
            total = (40 * 7.5) + ((horasTrabajadas - 40) * 9.5)
        End If

        Return total
    End Function

    ' Evento para el botón "Calcular".
    Private Sub btnCalcular_Click_1(sender As Object, e As EventArgs) Handles btnCalcular.Click
        ' Intentar capturar las horas trabajadas del TextBox.
        Try
            Dim horasTrabajadas As Integer = Convert.ToInt32(txtHoras.Text)

            If horasTrabajadas < 0 Then
                MessageBox.Show("Las horas trabajadas no pueden ser negativas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim totalPago As Double = CalcularPago(horasTrabajadas)
            txtTotal.Text = totalPago.ToString("F2") & " Bs."
        Catch ex As Exception
            MessageBox.Show("Por favor, ingrese un número válido para las horas trabajadas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Evento para el botón "Borrar".
    Private Sub btnBorrar_Click(sender As Object, e As EventArgs) Handles btnBorrar.Click
        ' Limpiar todos los campos del formulario.
        txtNombre.Text = ""
        txtCi.Text = ""
        txtHoras.Text = ""
        txtTotal.Text = ""
    End Sub

    ' Evento para el botón "Salir".
    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        ' Cerrar la aplicación.
        Me.Close()
    End Sub

End Class
