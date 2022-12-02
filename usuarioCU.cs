﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PulperiaPY
{
    public partial class usuarioCU : Form
    {
        public string operacion;
        public usuarioCU()
        {
            InitializeComponent();
        }
        Conexion conexion = new Conexion();

        private void usuarioCU_Load(object sender, EventArgs e)
        {
            txtId.Enabled = false;
            if (operacion == "Agregar")
            {
                btnAgregarEditar.Text = "Agregar";
            }
            else
            {
                btnAgregarEditar.Text = "Editar";

                conexion.AbrirConexion();
                SqlCommand cmd = new SqlCommand("exec ContraseñaUsuario @idUsuario", conexion.Conectar);
                cmd.Parameters.AddWithValue("@idUsuario", txtId.Text);

                try
                {

                    SqlDataReader lector = cmd.ExecuteReader();


                    if (lector.Read())
                    {
                        txtcontra.Text = Convert.ToString(lector[0]);
                    }
                    conexion.CerrarConexion();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }
        private void btnAgregarEditar_Click(object sender, EventArgs e)
        {
            if (txtusuario.Text != "" && txtcontra.Text != "" && txtnombre.Text != "" && txtapellido.Text != "" && txtcorreo.Text != "" && txttelefono.Text != "")
            {

                if (operacion == "Agregar")
                {

                    conexion.AbrirConexion();
                    SqlCommand cmd = new SqlCommand("exec Crearusuario @user, @contra, @nombre, @apellido, @correo, @telefono", conexion.Conectar);
                    cmd.Parameters.AddWithValue("@user", txtusuario.Text);
                    cmd.Parameters.AddWithValue("@contra", txtcontra.Text);
                    cmd.Parameters.AddWithValue("@nombre", txtnombre.Text);
                    cmd.Parameters.AddWithValue("@apellido", txtapellido.Text);
                    cmd.Parameters.AddWithValue("@correo", txtcorreo.Text);
                    cmd.Parameters.AddWithValue("@telefono", txttelefono.Text);

                    try
                    {
                        int i = cmd.ExecuteNonQuery();
                        if (i != 0)
                        {
                            MessageBox.Show("Usuario Ingresado satisfactoriamente");
                            txtusuario.Text = "";
                            txtcontra.Text = "";
                            txtnombre.Text = "";
                            txtapellido.Text = "";
                            txtcorreo.Text = "";
                            txttelefono.Text = "";
                            conexion.CerrarConexion();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Error al ingresar Datos");

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error en la base de datos: \n" + ex.Message);
                    }
                }
                else
                {

                    conexion.AbrirConexion();
                    SqlCommand cmd = new SqlCommand("exec actualizarUsuario @idUsuario, @user, @contra, @nombre, @apellido, @correo, @telefono", conexion.Conectar);
                    cmd.Parameters.AddWithValue("@idUsuario", txtId.Text);
                    cmd.Parameters.AddWithValue("@user", txtusuario.Text);
                    cmd.Parameters.AddWithValue("@contra", txtcontra.Text);
                    cmd.Parameters.AddWithValue("@nombre", txtnombre.Text);
                    cmd.Parameters.AddWithValue("@apellido", txtapellido.Text);
                    cmd.Parameters.AddWithValue("@correo", txtcorreo.Text);
                    cmd.Parameters.AddWithValue("@telefono", txttelefono.Text);

                    try
                    {
                        int i = cmd.ExecuteNonQuery();
                        if (i != 0)
                        {
                            MessageBox.Show("Usuario actualizado satisfactoriamente");
                            conexion.CerrarConexion();
                            ListadoUsuarios usuarios = new ListadoUsuarios();
                            usuarios.Show();
                            this.Hide();

                        }
                        else
                        {
                            MessageBox.Show("Error al actualizar los  Datos");

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error en la base de datos: \n" + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Ingrese Todos los campos");
            }
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void btnAgregarEditar_MouseHover(object sender, EventArgs e)
        {
            btnAgregarEditar.ForeColor = Color.White;
        }

        private void btnAgregarEditar_MouseLeave(object sender, EventArgs e)
        {
            btnAgregarEditar.ForeColor = Color.FromArgb(32, 43, 76);
        }

        private void btnRegresar_MouseHover(object sender, EventArgs e)
        {
            btnRegresar.ForeColor = Color.White;
        }

        private void btnRegresar_MouseLeave(object sender, EventArgs e)
        {
            btnRegresar.ForeColor = Color.FromArgb(32, 43, 76);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
