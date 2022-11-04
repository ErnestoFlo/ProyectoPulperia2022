﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PulperiaPY
{
    public partial class ListadoUsuarios : Form
    {
        public string idUser;
        public ListadoUsuarios()
        {
            InitializeComponent();
        }
        Conexion conexion = new Conexion();
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        private void ListadoUsuarios_Load(object sender, EventArgs e)
        {
            conexion.AbrirConexion();
            SqlCommand cmd = new SqlCommand("exec VerUsuarios", conexion.Conectar);
            adapter.SelectCommand = cmd;
            var datoscbox = new[] { "idUsuario", "username", "nombre", "apellido", "correo", "telefono" };
            try
            {
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                adapter.Fill(table);
                dgvUsers.DataSource = table;
                cboxfilter.DataSource = datoscbox;
                cboxfilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                conexion.CerrarConexion();
            }catch(Exception ex)
            {
                MessageBox.Show("Error al traer los datos" + ex.Message);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            conexion.AbrirConexion();
            string filtro = cboxfilter.Text;
            if(filtro == "idUsuario")
            {
                SqlCommand cmd = new SqlCommand("Select idUsuario, username,nombre, apellido, correo, telefono from Usuarios WHERE idUsuario = @valor", conexion.Conectar);
                cmd.Parameters.AddWithValue("@valor", txtvalorbuscar.Text);
                adapter.SelectCommand = cmd;
                table.Rows.Clear();
                adapter.Fill(table);
                dgvUsers.DataSource = table;
                conexion.CerrarConexion();
            }
            else if (filtro == "username")
            {
                SqlCommand cmd = new SqlCommand("Select idUsuario, username,nombre, apellido, correo, telefono from Usuarios WHERE username LIKE @valor", conexion.Conectar);
                cmd.Parameters.AddWithValue("@valor", txtvalorbuscar.Text);
                adapter.SelectCommand = cmd;
                table.Rows.Clear();
                adapter.Fill(table);
                dgvUsers.DataSource = table;
                conexion.CerrarConexion();
            }
            else if (filtro == "nombre")
            {
                SqlCommand cmd = new SqlCommand("Select idUsuario, username,nombre, apellido, correo, telefono from Usuarios WHERE nombre LIKE @valor", conexion.Conectar);
                cmd.Parameters.AddWithValue("@valor", txtvalorbuscar.Text);
                adapter.SelectCommand = cmd;
                table.Rows.Clear();
                adapter.Fill(table);
                dgvUsers.DataSource = table;
                conexion.CerrarConexion();
            }
            else if (filtro == "apellido")
                {
                    SqlCommand cmd = new SqlCommand("Select idUsuario, username,nombre, apellido, correo, telefono from Usuarios WHERE apellido LIKE @valor", conexion.Conectar);
                    cmd.Parameters.AddWithValue("@valor", txtvalorbuscar.Text);
                    adapter.SelectCommand = cmd;
                    table.Rows.Clear();
                    adapter.Fill(table);
                    dgvUsers.DataSource = table;
                    conexion.CerrarConexion();
                }
            else if (filtro == "correo")
            {
                SqlCommand cmd = new SqlCommand("Select idUsuario, username,nombre, apellido, correo, telefono from Usuarios WHERE correo LIKE @valor", conexion.Conectar);
                cmd.Parameters.AddWithValue("@valor", txtvalorbuscar.Text);
                adapter.SelectCommand = cmd;
                table.Rows.Clear();
                adapter.Fill(table);
                dgvUsers.DataSource = table;
                conexion.CerrarConexion();
            }
            else if (filtro == "telefono")
            {
                SqlCommand cmd = new SqlCommand("Select idUsuario, username,nombre, apellido, correo, telefono from Usuarios WHERE telefono LIKE @valor", conexion.Conectar);
                cmd.Parameters.AddWithValue("@valor", txtvalorbuscar.Text);
                adapter.SelectCommand = cmd;
                table.Rows.Clear();
                adapter.Fill(table);
                dgvUsers.DataSource = table;
                conexion.CerrarConexion();
            }

        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("exec VerUsuarios", conexion.Conectar);
            adapter.SelectCommand = cmd;
            table.Rows.Clear();
            adapter.Fill(table);
            dgvUsers.DataSource = table;
            conexion.CerrarConexion();
        }

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try{
                idUser = Convert.ToString(this.dgvUsers.SelectedRows[0].Cells[0].Value);
                string usuario = Convert.ToString(this.dgvUsers.SelectedRows[0].Cells[1].Value);
                string contraseña = Convert.ToString(this.dgvUsers.SelectedRows[0].Cells[2].Value);
                string nombre = Convert.ToString(this.dgvUsers.SelectedRows[0].Cells[3].Value);
                string apellido = Convert.ToString(this.dgvUsers.SelectedRows[0].Cells[4].Value);
                string correo = Convert.ToString(this.dgvUsers.SelectedRows[0].Cells[5].Value);
                string telefono = Convert.ToString(this.dgvUsers.SelectedRows[0].Cells[6].Value);
            }
            catch (Exception){
                throw;
            }
        }
        private void btnEditar_Click(object sender, EventArgs e)
        {
            Usuario editar = new Usuario();
            //igualar variables a las demas delsiguiente formulario

            editar.operacion = "Editar";
            editar.Show();
        }

        private void btnagregar_Click(object sender, EventArgs e)
        {
            Usuario agregar = new Usuario();
            agregar.operacion = "Agregar";
            agregar.Show();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idUser != "")
            {
                conexion.AbrirConexion();
                SqlCommand cmd = new SqlCommand("exec EliminarUsuario @idUsuario", conexion.Conectar);
                cmd.Parameters.AddWithValue("@idUsuario", idUser);

                try
                {
                    int i = cmd.ExecuteNonQuery();
                    if (i != 0)
                    {
                        MessageBox.Show("Usuario Eliminado Satisfactoriamente");
                        conexion.CerrarConexion();
                    }
                    else
                    {
                        MessageBox.Show("Error al eliminar el usuario");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error en la base de datos: \n" + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Primero debe seleccionar un usuario de la tabla", "Advertencia");
            }
        }

    }

}

