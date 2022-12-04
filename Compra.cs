﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.Configuration;

namespace PulperiaPY
{
    public partial class Compra : Form
    {
        Conexion conexion = new Conexion();
        public Compra()
        {
            InitializeComponent();

            llenarDGVproveedor();
            llenarDGVcompra();

            // para que no pueda ingresar fechas anteriores a las de "hoy"
            dtpFechaAdquisicion.MinDate = DateTime.Today;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            llenarDGVproveedor();
            llenarDGVcompra();
            // para que no pueda ingresar fechas anteriores a las de "hoy"
            dtpFechaAdquisicion.MinDate = DateTime.Today;
        }

        public void llenarDGVproveedor()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Select * from Proveedor", conexion.Conectar);
                conexion.AbrirConexion();

                cmd.ExecuteNonQuery();

                SqlDataAdapter sqladapter = new SqlDataAdapter(cmd);

                DataTable datatable = new DataTable("Proveedor");

                sqladapter.Fill(datatable);
                dgvProveedores.DataSource = datatable.DefaultView;
                sqladapter.Update(datatable);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                conexion.CerrarConexion();
            }
        }

        public void llenarDGVcompra()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Select * from ListaCompras", conexion.Conectar);
                conexion.AbrirConexion();

                cmd.ExecuteNonQuery();

                SqlDataAdapter sqladapter = new SqlDataAdapter(cmd);

                DataTable datatable = new DataTable("Compra");

                sqladapter.Fill(datatable);
                dgvCompra.DataSource = datatable.DefaultView;
                sqladapter.Update(datatable);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                conexion.CerrarConexion();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void dtpFechaAdquisicion_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        public void LimpiarCasillas()
        {
            txtNombreProveedor.Clear();
            dtpFechaAdquisicion.ResetText();
            cmbEstado.ResetText();
        }

        private void txtLimpiarProveedor_Click(object sender, EventArgs e)
        {
            LimpiarCasillas();
        }

        public bool VerificarDatos()
        {
            if (txtNombreProveedor.Text == string.Empty || cmbEstado.SelectedIndex == null || dtpFechaAdquisicion.Text == string.Empty)
            {
                MessageBox.Show("Verifique los datos solicitados", "Verificar Datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                return true;
            }
        }

        public void InsertarCompra()
        {
            int IdProveedor;
            string Fecha;
            int Estado;

            IdProveedor = Convert.ToInt32(txtIdProveedor.Text);
            Fecha = dtpFechaAdquisicion.Text.ToString();
            //Validamos combobox con switch para que funcione jsjs
            switch (cmbEstado.SelectedIndex)
            {
                case 0:
                    Estado = 1;
                    break;

                case 1:
                    Estado = 2;
                    break;

                case 2:
                    Estado = 3;
                    break;

                case 3:
                    Estado = 4;
                    break;

                case 4:
                    Estado = 5;
                    break;

                case 5:
                    Estado = 6;
                    break;

                case 6:
                    Estado = 7;
                    break;

                case 7:
                    Estado = 8;
                    break;

                default:
                    Estado = 5;
                    break;
            }
                

            try
            {
                SqlCommand cmd = new SqlCommand("InsertarCompra", conexion.Conectar);
                cmd.CommandType = CommandType.StoredProcedure;

                conexion.AbrirConexion();

                cmd.Parameters.AddWithValue("@idProveedor", IdProveedor);
                cmd.Parameters.AddWithValue("@fechaAdquisicion", Fecha);
                cmd.Parameters.AddWithValue("@idEstado", Estado);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                conexion.CerrarConexion();
            }

        }

        public void ModificarCompra()
        {
            int idCompra = Convert.ToInt32(txtIdCompra.Text);
            int idProveedor = Convert.ToInt32(txtIdProveedor.Text);
            string Fecha = dtpFechaAdquisicion.Text.ToString();
            int Estado;

            switch (cmbEstado.SelectedIndex)
            {
                case 0:
                    Estado = 1;
                    break;

                case 1:
                    Estado = 2;
                    break;

                case 2:
                    Estado = 3;
                    break;

                case 3:
                    Estado = 4;
                    break;

                case 4:
                    Estado = 5;
                    break;

                case 5:
                    Estado = 6;
                    break;

                case 6:
                    Estado = 7;
                    break;

                case 7:
                    Estado = 8;
                    break;

                default:
                    Estado = 5;
                    break;
            }

            try
            {
                SqlCommand cmd = new SqlCommand("modificarCompra ", conexion.Conectar);
                cmd.CommandType = CommandType.StoredProcedure;

                conexion.AbrirConexion();

                cmd.Parameters.AddWithValue("@idCompra", idCompra);
                cmd.Parameters.AddWithValue("@idProveedor", idProveedor);
                cmd.Parameters.AddWithValue("@fecha", Fecha);
                cmd.Parameters.AddWithValue("@idEstado", Estado);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                conexion.CerrarConexion();
            }
        }

        public void EliminarCompra()
        {
            int idCompra = Convert.ToInt32(txtIdCompra.Text);

            try
            {
                SqlCommand cmd = new SqlCommand("eliminarCompra  ", conexion.Conectar);
                cmd.CommandType = CommandType.StoredProcedure;

                conexion.AbrirConexion();

                cmd.Parameters.AddWithValue("@idCompra", idCompra);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                conexion.CerrarConexion();
            }
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {
            
        }

        private void cmbEstado_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnReporte_Click(object sender, EventArgs e)
        {
            detalleCompra detalleCompra = new detalleCompra();
            detalleCompra.Show();
            //this.Hide();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (VerificarDatos())
            {
                try
                {
                    InsertarCompra();
                    MessageBox.Show("Compra iniciada", "Éxito!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    llenarDGVcompra();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    MessageBox.Show("Ha ocurrido un error...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    LimpiarCasillas();
                    llenarDGVcompra();
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCasillas();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Esta seguro que desea eliminar?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    EliminarCompra();
                    MessageBox.Show("Compra Eliminada", "Éxito!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    llenarDGVcompra();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    MessageBox.Show("Ha ocurrido un error...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    LimpiarCasillas();
                    llenarDGVcompra();
                }
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (VerificarDatos())
            {
                try
                {
                    ModificarCompra();
                    MessageBox.Show("Compra Editada", "Éxito!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    llenarDGVcompra();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    MessageBox.Show("Ha ocurrido un error...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    LimpiarCasillas();
                    llenarDGVcompra();
                }
            }
        }

        private void dgvProveedores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProveedores.Rows[e.RowIndex];
                txtIdProveedor.Text = row.Cells[0].Value.ToString();
                txtNombreProveedor.Text = row.Cells[1].Value.ToString();
            }
        }

        private void dgvCompra_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCompra.Rows[e.RowIndex];
                txtNombreProveedor.Text = row.Cells[0].Value.ToString();
                txtIdProveedor.Text = row.Cells[1].Value.ToString();
                //dtpFechaAdquisicion.Text =  row.Cells[2].Value.ToString();
                txtIdCompra.Text = row.Cells[3].Value.ToString();
                cmbEstado.SelectedIndex = Convert.ToInt32( row.Cells[5].Value.ToString()) - 1;
            }
        }

        private void Busqueda(string Comando, DataGridView grid, string columna)
        {
            DataSet dsa = new DataSet();
            BindingSource bs = new BindingSource();
            DataTable dt = new DataTable();

            string cnn = "Server=tcp:gestiong1.database.windows.net,1433;Initial Catalog=pulperiaproyect;Persist Security Info=False;User ID=AdminUnicah;Password=Gestiongrup01;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            SqlConnection SqlCon = new SqlConnection(cnn);

            SqlDataAdapter da = new SqlDataAdapter(Comando, SqlCon);

            da.Fill(dt);
            bs.DataSource = dt.DefaultView;

            grid.DataSource = bs;

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            DataView dv = new DataView(ds.Tables[0]);

            dv.RowFilter = columna;

            if (dv.Count != 0)
            {
                grid.DataSource = dv;
            }
            else
            {
                grid.DataSource = null;
            }

        }

        private void txtBuscarProveedor_TextChanged(object sender, EventArgs e)
        {
            Busqueda("Select * from Proveedor ", dgvProveedores, "nombreProveedor LIKE '%" + txtBuscarProveedor.Text + "%'");
        }

        private void txtBuscarCompra_TextChanged(object sender, EventArgs e)
        {
            Busqueda("Select * from ListaCompras ", dgvCompra, "Proveedor LIKE '%" + txtBuscarCompra.Text + "%'");
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void dgvProveedores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnIngresar_MouseHover(object sender, EventArgs e)
        {
            btnIngresar.ForeColor = Color.White;
        }
       

        private void btnIngresar_MouseLeave(object sender, EventArgs e)
        {
            btnIngresar.ForeColor = Color.FromArgb(32, 43, 76);
        }

        private void btnModificar_MouseHover(object sender, EventArgs e)
        {
            btnModificar.ForeColor = Color.White;
        }

        private void btnModificar_MouseLeave(object sender, EventArgs e)
        {
            btnModificar.ForeColor = Color.FromArgb(32, 43, 76);
        }

        private void btnEliminar_MouseHover(object sender, EventArgs e)
        {
            btnEliminar.ForeColor = Color.White;
        }

        private void btnEliminar_MouseLeave(object sender, EventArgs e)
        {
            btnEliminar.ForeColor = Color.FromArgb(32, 43, 76);
        }

        private void btnLimpiar_MouseHover(object sender, EventArgs e)
        {
            btnLimpiar.ForeColor = Color.White;
        }

        private void btnLimpiar_MouseLeave(object sender, EventArgs e)
        {
            btnLimpiar.ForeColor = Color.FromArgb(32, 43, 76);
        }

        private void btnReporte_MouseHover(object sender, EventArgs e)
        {
            btnReporte.ForeColor = Color.White;
        }

        private void btnReporte_MouseLeave(object sender, EventArgs e)
        {
            btnReporte.ForeColor = Color.FromArgb(32, 43, 76);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReporteCompra detalleCompra = new ReporteCompra();
            detalleCompra.Show();
        }
    }
}
