﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using PulperiaPY.Utilidades;

namespace PulperiaPY
{
    public partial class Venta : Form
    {
        CultureInfo culturaHN = new CultureInfo("es-HN");
        Conexion conexionDb = new Conexion();
        string accion = "Agregar";
        string accionVenta = "Nueva Venta";
        DataTable TablaDetalleVentaMod = new DataTable("DetalleVentaMod");

        DataTable TablaProductosEliminados = new DataTable("ProductosEliminados");
        DataColumn codigoProductoEliminar = new DataColumn("CodigoProductoEliminar");
        DataColumn nombreProductoEliminar = new DataColumn("ProductoEliminar");
        DataColumn precioProductoEliminar = new DataColumn("PrecioProductoEliminar");
        DataColumn cantidadProductoEliminar = new DataColumn("CantidadProductoEliminar");

        DataTable TablaProductosNuevosMod = new DataTable("ProductosNuevosMod");
        DataColumn codigoProductoNuevosMod = new DataColumn("CodigoProductoNuevosMod");
        DataColumn nombreProductoNuevosMod = new DataColumn("ProductoNuevosMod");
        DataColumn precioProductoNuevosMod = new DataColumn("PrecioNuevosMod");
        DataColumn cantidadProductoNuevosMod = new DataColumn("CantidadNuevosMod");

        DataTable TablaDetalleVenta = new DataTable("DetalleVenta");
        DataColumn codigoProducto = new DataColumn("Codigo");
        DataColumn nombreProducto = new DataColumn("Producto");
        DataColumn precio = new DataColumn("Precio");
        DataColumn cantidad = new DataColumn("Cantidad");
        DataColumn subTotal = new DataColumn("SubTotal");
        DataColumn Eliminar = new DataColumn(" ");

        public Venta()
        {
            InitializeComponent();
        }

        private void Venta_Load(object sender, EventArgs e)
        {
            accionVenta = "Nueva Venta";
            lblTipoAccion.Text = accionVenta;
            dtpFecha.Value = DateTime.Now;
            txtCambio.Text = "0.00";
            txtTotalPagar.Text = "0.00";
            reiniciarNumVenta();
            accionVenta = "Nueva Venta";
            lblTipoAccion.Text = accionVenta;
            limpiarVenta();

            //Textbox
            txtNumVenta.ReadOnly = true;
            txtCodProducto.ReadOnly = true;
            txtNombreProducto.ReadOnly = true;
            txtPrecio.ReadOnly = false;
            txtStock.ReadOnly = true;
            nudCantidad.ReadOnly = false;
            txtTotalPagar.ReadOnly = true;
            txtPagaCon.ReadOnly = false;
            txtCambio.ReadOnly = true;

            //Buttons
            btnBuscarVenta.Enabled = false;
            btnBuscarVenta.Visible = false;
            btnBuscar.Visible = true;
            btnBuscar.Enabled = true;
            btnAgregarProducto.Visible = true;
            btnAgregarProducto.Enabled = true;

            //DataGridView
            dgvDetalleVenta.Enabled = true;

            //Cambio texto Finalizar Venta
            btnFinalizarVenta.Text = "Finalizar Venta";
            btnAgregarProducto.Text = "Agregar Producto";

            TablaDetalleVenta.Columns.Add(codigoProducto);
            TablaDetalleVenta.Columns.Add(nombreProducto);
            TablaDetalleVenta.Columns.Add(precio);
            TablaDetalleVenta.Columns.Add(cantidad);
            TablaDetalleVenta.Columns.Add(subTotal);
            TablaDetalleVenta.Columns.Add(Eliminar);

            TablaProductosEliminados.Columns.Add(codigoProductoEliminar);
            TablaProductosEliminados.Columns.Add(nombreProductoEliminar);
            TablaProductosEliminados.Columns.Add(precioProductoEliminar);
            TablaProductosEliminados.Columns.Add(cantidadProductoEliminar);

            TablaProductosNuevosMod.Columns.Add(codigoProductoNuevosMod);
            TablaProductosNuevosMod.Columns.Add(nombreProductoNuevosMod);
            TablaProductosNuevosMod.Columns.Add(precioProductoNuevosMod);
            TablaProductosNuevosMod.Columns.Add(cantidadProductoNuevosMod);


            dgvDetalleVenta.DataSource = TablaDetalleVenta;
        }


        private void txtNumVenta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtTotalPagar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtPagaCon_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtPagaCon_TextChanged(object sender, EventArgs e)
        {
            decimal monto = 0, cambio = 0, cantidadPago = 0;

            if (txtPagaCon.Text != string.Empty)
            {


                if (txtPagaCon.Text.Length >= 1 && txtPagaCon.Text[0] == '.')
                {

                    int select = txtPagaCon.SelectionStart;
                    txtPagaCon.Text = txtPagaCon.Text.Insert(0, "0");
                    txtPagaCon.SelectionStart = ++select;

                }
                else
                {
                    monto = Convert.ToDecimal(txtTotalPagar.Text);
                    cantidadPago = Convert.ToDecimal(txtPagaCon.Text);
                    if (cantidadPago >= 0 && cantidadPago >= monto)
                    {
                        txtPagaCon.ForeColor = Color.Green;
                        cambio = cantidadPago - monto;
                        txtCambio.Text = cambio.ToString("N2", culturaHN);
                    }
                    else
                    {
                        txtPagaCon.ForeColor = Color.Red;
                    }
                }

            }
            else
            {
                txtCambio.Text = "0.00";
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ProductosVenta frm = new ProductosVenta();
            frm.FormClosing += ProductosVenta_FormClosing;
            frm.Show();
        }

        public void ProductosVenta_FormClosing(object sender, FormClosingEventArgs e)
        {
            txtCodProducto.Text = clsVariableGlobales.CodigoProductoV;
            txtNombreProducto.Text = clsVariableGlobales.NombreProductoV;
            txtPrecio.Text = clsVariableGlobales.PrecioProductoV.ToString();
            txtStock.Text = clsVariableGlobales.StockProductoV.ToString();
            clsVariableGlobales.CodigoProductoV = "";
            clsVariableGlobales.NombreProductoV = "";
            clsVariableGlobales.PrecioProductoV = 0;
            clsVariableGlobales.StockProductoV = 0;
            nudCantidad.Value = 1;
            accion = "Agregar";

            btnAgregarProducto.Text = "Agregar Producto";

        }

        private void totalPago()
        {
            decimal montoPagar = 0;


            for (int i = 0; i < dgvDetalleVenta.Rows.Count; i++)
            {
                montoPagar += Convert.ToDecimal(dgvDetalleVenta.Rows[i].Cells["SubTotal"].Value);
            }

            txtTotalPagar.Text = montoPagar.ToString("N2", culturaHN);

        }

        private void AgregarProductoAlDetalle(DataTable TablaDatos)
        {
            bool existe;
            existe = false;
            if (TablaDatos.Rows.Count == 0)
            {
                object[] productos = { txtCodProducto.Text, txtNombreProducto.Text, txtPrecio.Text, nudCantidad.Value, (Convert.ToDecimal(txtPrecio.Text) * Convert.ToInt32(nudCantidad.Value)), "Eliminar"};
                TablaDatos.Rows.Add(productos);
                if (accionVenta == "Editar Venta")
                {
                    object[] productoNuevo = { txtCodProducto.Text, txtNombreProducto.Text, txtPrecio.Text, nudCantidad.Value };

                    TablaProductosNuevosMod.Rows.Add(productoNuevo);
                }
            }
            else
            {
                foreach (DataRow Row in TablaDatos.Rows)
                {
                    string CodProdEvaluar = Convert.ToString(Row[0]);

                    if (CodProdEvaluar == txtCodProducto.Text)
                    {
                        int nuevaCantidad = Convert.ToInt32(Row[3]) + Convert.ToInt32(nudCantidad.Value);
                        if (Convert.ToInt32(txtStock.Text) >= nuevaCantidad)
                        {
                            Row[3] = nuevaCantidad;
                            Row[4] = Convert.ToInt32(Row[3]) * Convert.ToDecimal(Row[2]);
                            existe = true;
                        }
                        else
                        {
                            existe = true;
                            MessageBox.Show("No hay suficiente stock disponible", "Error al agregar producto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }          
                }
                if (!existe)
                {
                    object[] productos = { txtCodProducto.Text, txtNombreProducto.Text, txtPrecio.Text, nudCantidad.Value, (Convert.ToDecimal(txtPrecio.Text) * Convert.ToInt32(nudCantidad.Value)), "Eliminar" };
                    TablaDatos.Rows.Add(productos);
                    if (accionVenta == "Editar Venta")
                    {
                        object[] productoNuevo = { txtCodProducto.Text, txtNombreProducto.Text, txtPrecio.Text, nudCantidad.Value};

                        TablaProductosNuevosMod.Rows.Add(productoNuevo);
                    }
                }
            }
        
            ActualizarTotalPagar();
            

        }

        private void ActualizarProductoDetalle(DataTable tablaDatos)
        {
            foreach (DataRow Row in tablaDatos.Rows)
            {
                string CodProdEvaluar = Convert.ToString(Row[0]);

                if (CodProdEvaluar == txtCodProducto.Text)
                {
                    
                    Row[2] = txtPrecio.Text;
                    Row[3] = nudCantidad.Value;
                    Row[4] = Convert.ToInt32(Row[3]) * Convert.ToDecimal(Row[2]);
                }
            }
            ActualizarTotalPagar();
        }

        private void ActualizarTotalPagar()
        {
            Decimal totalPagar = 0;
            
            foreach(DataGridViewRow row in dgvDetalleVenta.Rows)
            {
                totalPagar += Convert.ToDecimal(row.Cells[4].Value);
            }
            txtTotalPagar.Text = totalPagar.ToString();
        }

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            if (txtCodProducto.Text != string.Empty && txtNombreProducto.Text != string.Empty && txtPrecio.Text != string.Empty 
                && txtStock.Text != string.Empty)
            {
                if (accion == "Agregar")
                {
                    if(nudCantidad.Value <= Convert.ToInt32(txtStock.Text))
                    {
                        if (accionVenta == "Nueva Venta")
                        {
                            AgregarProductoAlDetalle(TablaDetalleVenta);
                        }
                        else
                        {
                            AgregarProductoAlDetalle(TablaDetalleVentaMod);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No hay suficiente stock disponible", "Error al agregar producto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    if (nudCantidad.Value <= Convert.ToInt32(txtStock.Text))
                    {
                        if (accionVenta == "Nueva Venta")
                        {
                            ActualizarProductoDetalle(TablaDetalleVenta);
                        }
                        else
                        {
                            ActualizarProductoDetalle(TablaDetalleVentaMod);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No hay suficiente stock disponible", "Error al agregar producto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe buscar y seleccionar el producto que agregara\nO asegurarse de que todos los campos estan completos", "Error al agregar producto", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
   
        private void reiniciarNumVenta()
        {
            int NumVenta;
            NumVenta = conexionDb.obtenerVariableEntera("SELECT [IdVenta] FROM [dbo].[Venta] WHERE [IdVenta]=(SELECT max([IdVenta]) FROM [dbo].[Venta]);");
            NumVenta += 1;
            txtNumVenta.Text = NumVenta.ToString(); 

        }

        public void limpiarTextBox(Control cont)
        {
            foreach (Control control in cont.Controls)
            {
                if (control is TextBox)
                    control.Text = string.Empty;
                if (control is MaskedTextBox)
                    control.Text = string.Empty;
            }
        }

        private void limpiarVenta()
        {
            limpiarTextBox(gbInformacionVenta);
            limpiarTextBox(gbInformacionProducto);
            limpiarTextBox(panelTotales);
            nudCantidad.Value = 1;
            TablaDetalleVenta.Clear();
            TablaProductosEliminados.Clear();
            TablaDetalleVentaMod.Clear();
            TablaProductosNuevosMod.Clear();
            dgvDetalleVenta.DataSource = TablaDetalleVenta;
            reiniciarNumVenta();
            dtpFecha.Value = DateTime.Now;
            txtCambio.Text = "0.00";
            txtTotalPagar.Text = "0.00";
            reiniciarNumVenta();

        }

        private int SeleccionarStockProducto()
        {
            return conexionDb.obtenerVariableEntera("SELECT dbo.Inventario.CantidadDisponible AS [En Stock] " +
                "FROM dbo.Inventario " +
                "INNER JOIN dbo.Producto ON dbo.Inventario.IdProducto = dbo.Producto.IdProducto " +
                "Where dbo.Producto.CodigoProducto = "+ txtCodProducto.Text +";");
        }

        private void dgvDetalleVenta_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 5)
            {
                if (accionVenta == "Nueva Venta")
                {
                    TablaDetalleVenta.Rows.RemoveAt(dgvDetalleVenta.CurrentRow.Index);
                    ActualizarTotalPagar();
                }
                else
                {
                    object[] productosEliminar = { dgvDetalleVenta.Rows[e.RowIndex].Cells[0].Value.ToString(),
                        dgvDetalleVenta.Rows[e.RowIndex].Cells[1].Value.ToString(),
                        dgvDetalleVenta.Rows[e.RowIndex].Cells[2].Value.ToString(),
                        dgvDetalleVenta.Rows[e.RowIndex].Cells[3].Value.ToString(),};
                    TablaProductosEliminados.Rows.Add(productosEliminar);
                    TablaDetalleVentaMod.Rows.RemoveAt(dgvDetalleVenta.CurrentRow.Index);
                    ActualizarTotalPagar();
                }
                limpiarTextBox(gbInformacionProducto);
                nudCantidad.Value = 1;
                btnAgregarProducto.Text = "Agregar Producto";

            }
            else
            {
                limpiarTextBox(gbInformacionProducto);
                accion = "Actualizar";
                btnAgregarProducto.Text = "Editar Producto";
                txtCodProducto.Text = dgvDetalleVenta.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtNombreProducto.Text = dgvDetalleVenta.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtPrecio.Text = dgvDetalleVenta.Rows[e.RowIndex].Cells[2].Value.ToString();
                nudCantidad.Value = Convert.ToInt32(dgvDetalleVenta.Rows[e.RowIndex].Cells[3].Value);
                txtStock.Text = SeleccionarStockProducto().ToString();
            }
        }

        private void btnNuevaVenta_Click(object sender, EventArgs e)
        {
            accionVenta = "Nueva Venta";
            lblTipoAccion.Text = accionVenta;
            limpiarVenta();
            dgvDetalleVenta.DataSource = TablaDetalleVenta;

            //Textbox
            txtNumVenta.ReadOnly = true;
            txtCodProducto.ReadOnly = true;
            txtNombreProducto.ReadOnly = true;
            txtPrecio.ReadOnly = false;
            txtStock.ReadOnly = true;
            nudCantidad.ReadOnly = false;
            txtTotalPagar.ReadOnly = true;
            txtPagaCon.ReadOnly = false;
            txtCambio.ReadOnly = true;

            //Buttons
            btnBuscarVenta.Enabled = false;
            btnBuscarVenta.Visible = false;
            btnBuscar.Visible = true;
            btnBuscar.Enabled = true;
            btnAgregarProducto.Visible = true;
            btnAgregarProducto.Enabled = true;

            //DataGridView
            dgvDetalleVenta.Enabled = true;

            //Cambio texto Finalizar Venta
            btnFinalizarVenta.Text = "Finalizar Venta";
            
        }

        private void btnEditarVenta_Click(object sender, EventArgs e)
        {
            accionVenta = "Editar Venta";
            lblTipoAccion.Text = accionVenta;
            limpiarVenta();
            txtNumVenta.Clear();


            //Textbox
            txtNumVenta.ReadOnly = false;
            txtCodProducto.ReadOnly = true;
            txtNombreProducto.ReadOnly = true;
            txtPrecio.ReadOnly = false;
            txtStock.ReadOnly = true;
            nudCantidad.ReadOnly = false;
            txtTotalPagar.ReadOnly = true;
            txtPagaCon.ReadOnly = true;
            txtCambio.ReadOnly = true;

            //Buttons
            btnBuscarVenta.Enabled = true;
            btnBuscarVenta.Visible = true;
            btnBuscar.Visible = true;
            btnBuscar.Enabled = true;
            btnAgregarProducto.Visible = true;
            btnAgregarProducto.Enabled = true;

            //DataGridView
            dgvDetalleVenta.Enabled = true;

            //Cambio texto Finalizar Venta
            btnFinalizarVenta.Text = "Actualizar Venta";
        }
        private void btnCancelarVenta_Click(object sender, EventArgs e)
        {
            accionVenta = "Cancelar Venta";
            lblTipoAccion.Text = accionVenta;
            limpiarVenta();
            txtNumVenta.Clear();

            //Textbox
            txtNumVenta.ReadOnly = false;
            txtCodProducto.ReadOnly = true;
            txtNombreProducto.ReadOnly = true;
            txtPrecio.ReadOnly = true;
            txtStock.ReadOnly = true;
            nudCantidad.ReadOnly = true;
            txtTotalPagar.ReadOnly = true;
            txtPagaCon.ReadOnly = true;
            txtCambio.ReadOnly = true;
            

            //Buttons
            btnBuscarVenta.Enabled = true;
            btnBuscarVenta.Visible = true;
            btnBuscar.Visible = false;
            btnBuscar.Enabled = false;
            btnAgregarProducto.Visible = true;
            btnAgregarProducto.Enabled = false;

            //DataGridView
            dgvDetalleVenta.Enabled = false;

            //Cambio texto Finalizar Venta
            btnFinalizarVenta.Text = "Cancelar Venta";
        }
        private void FinalizarNuevaVenta()
        {
            string fecha = dtpFecha.Value.ToString("yyyy-MM-dd");
            int TotalProductosDetalle = dgvDetalleVenta.Rows.Count;
            string detalleVenta = "";
            for (int i = 0; TotalProductosDetalle > i; i++)
            {
                if (i == 0)
                {
                    detalleVenta += "('" + txtNumVenta.Text + "',(SELECT idProducto from [dbo].[Producto] WHERE CodigoProducto = '" + dgvDetalleVenta.Rows[i].Cells[0].Value.ToString() + "'),'" + dgvDetalleVenta.Rows[i].Cells[3].Value.ToString() + "','" + dgvDetalleVenta.Rows[i].Cells[2].Value.ToString() + "','Finalizada')";
                }
                else
                {
                    detalleVenta += ",('" + txtNumVenta.Text + "',(SELECT idProducto from [dbo].[Producto] WHERE CodigoProducto = '" + dgvDetalleVenta.Rows[i].Cells[0].Value.ToString() + "'),'" + dgvDetalleVenta.Rows[i].Cells[3].Value.ToString() + "','" + dgvDetalleVenta.Rows[i].Cells[2].Value.ToString() + "','Finalizada')";

                }
            }
            if (conexionDb.ejecutarComandoSQL("INSERT INTO [dbo].[Venta]([FechaVenta],[Estado]) VALUES('" + fecha + "','Finalizada'); " +
                "INSERT INTO [dbo].[DetalleVenta]([IdVenta],[IdProducto],[Cantidad],[PrecioSugerido],[Estado])" +
                " VALUES" +
                detalleVenta +
                ";"))
            {
                MessageBox.Show("¡Venta Finalizada con Exito!", "VENTA FINALIZADA", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("¡Error al Realizar la Venta!", "ERROR EN LA VENTA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ActualizarVenta()
        {
            
            string detalleVenta = "";
            string productosEliminados = "";
            string productosNuevos = "";

            foreach (DataRow rowProdsMod in TablaDetalleVentaMod.Rows)
            {
                foreach (DataRow rowProdsNuevos in TablaProductosNuevosMod.Rows)
                {
                    if (rowProdsMod[0] == rowProdsNuevos)
                    {
                        rowProdsMod.Delete();
                    }
                }
            }

            foreach(DataRow row in TablaDetalleVentaMod.Rows)
            {
                detalleVenta += "UPDATE [dbo].[DetalleVenta] SET [PrecioSugerido] = " + Convert.ToDecimal(row[2]) + ", [Cantidad] = "+ Convert.ToInt32(row[3]) +", [Estado] = 'Editada' WHERE ([IdVenta] = " + Convert.ToInt32(txtNumVenta.Text) + ") AND (SELECT [IdProducto] FROM [dbo].[Producto] WHERE [CodigoProducto] = '" + row[0] + "') = [IdProducto] AND [dbo].[DetalleVenta].[Estado] <> 'Cancelada'";
            }

            if (TablaProductosEliminados.Rows.Count != 0)
            {
                foreach (DataRow row in TablaProductosEliminados.Rows)
                {
                    productosEliminados += "UPDATE [dbo].[DetalleVenta] SET [PrecioSugerido] = " + Convert.ToDecimal(row[2]) + ", [Cantidad] = " + Convert.ToInt32(row[3]) + ", [Estado] = 'Cancelada' WHERE ([IdVenta] = " + Convert.ToInt32(txtNumVenta.Text) + ") AND (SELECT [IdProducto] FROM [dbo].[Producto] WHERE [CodigoProducto] = '" + row[0] + "') = [IdProducto]; ";
                }
            }
            else
            {
                productosEliminados = "";
            }

            if (TablaProductosNuevosMod.Rows.Count != 0)
            {
                foreach (DataRow row in TablaProductosNuevosMod.Rows)
                {
                    productosNuevos += "INSERT INTO [dbo].[DetalleVenta]([IdVenta],[IdProducto],[Cantidad],[PrecioSugerido],[Estado]) VALUES('" + txtNumVenta.Text + "',(SELECT idProducto from [dbo].[Producto] WHERE CodigoProducto = '" + row[0] + "'),'" + row[3] + "','" + row[2] + "','Agregada'); ";
                }
            }
            else
            {
                productosNuevos += "";
            }

            if (conexionDb.ejecutarComandoSQL("UPDATE [dbo].[Venta] SET [Estado] = 'Editada' WHERE [IdVenta] = " + Convert.ToInt32(txtNumVenta.Text) + "; " +
                detalleVenta + productosEliminados + productosNuevos))
            {
                MessageBox.Show("¡Venta Actualizada con Exito!", "VENTA ACTUALIZADA", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("¡Error al Actalizar la Venta!", "ERROR EN LA VENTA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CancelarVenta()
        {
            if (conexionDb.ejecutarComandoSQL("UPDATE [dbo].[Venta] SET [Estado] = 'Cancelada' WHERE [IdVenta] = " + Convert.ToInt32(txtNumVenta.Text) + "; " +
                "UPDATE [dbo].[DetalleVenta] SET [Estado] = 'Cancelada' WHERE [IdVenta] = " + Convert.ToInt32(txtNumVenta.Text) + "; "))
            {
                MessageBox.Show("¡Venta Cancelada con Exito!", "VENTA CANCELADA", MessageBoxButtons.OK, MessageBoxIcon.Information);
                limpiarVenta();
                txtNumVenta.Text = "";
            }
            else
            {
                MessageBox.Show("¡Error al Cancelar la Venta!", "ERROR EN LA VENTA", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void btnFinalizarVenta_Click(object sender, EventArgs e)
        {
            switch (accionVenta)
            {
                case "Nueva Venta":
                    if (dgvDetalleVenta.Rows.Count > 0)
                    {
                        if (txtPagaCon.Text != String.Empty)
                        {
                            if (Convert.ToDecimal(txtTotalPagar.Text) <= Convert.ToDecimal(txtPagaCon.Text))
                            {
                                FinalizarNuevaVenta();
                                limpiarVenta();
                            }
                            else
                            {
                                MessageBox.Show("Debe agregar un monto con el que paga\nmayor o igual al Total a pagar ", "Error al realizar venta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Debe ingresar el monto con el que se pagará", "Error al realizar venta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Debe agregar productos a la venta", "Error al realizar venta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;

                case "Editar Venta":
                    if(txtNumVenta.Text != string.Empty)
                    {
                        if (dgvDetalleVenta.Rows.Count > 0)
                        {
                            ActualizarVenta();
                            limpiarVenta();
                            btnNuevaVenta.PerformClick();

                        }
                        else
                        {
                            MessageBox.Show("No puede dejar sin productos la venta", "Error al realizar venta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Debe buscar una venta", "ERROR EN VENTA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;

                case "Cancelar Venta":
                    if (txtNumVenta.Text!= string.Empty)
                    {
                        CancelarVenta();
                        limpiarVenta();
                        btnNuevaVenta.PerformClick();

                    }
                    else
                    {
                        MessageBox.Show("Debe buscar una venta", "ERROR EN VENTA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;

                default:
                    MessageBox.Show("Ha ocurrido un error inesperado:\nLa acción seleccionada en la venta tuvo un problema","ERROR EN VENTA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }

        private void btnBuscarVenta_Click(object sender, EventArgs e)
        {
            if (txtNumVenta.Text != String.Empty)
            {
                if (conexionDb.obtenerVariableEntera("Select [IdVenta] from [dbo].[Venta] Where [IdVenta] = "+ Convert.ToInt32(txtNumVenta.Text)) > 0)
                {
                    TablaDetalleVentaMod = conexionDb.llenarDT("SELECT dbo.Producto.CodigoProducto AS Codigo, " +
                        "dbo.Producto.NombreProducto AS [Nombre Producto], " +
                        "dbo.DetalleVenta.PrecioSugerido AS Precio," +
                        " dbo.DetalleVenta.Cantidad, dbo.DetalleVenta.PrecioSugerido * dbo.DetalleVenta.Cantidad AS SubTotal " +
                        "FROM dbo.Venta " +
                        "INNER JOIN dbo.DetalleVenta ON dbo.Venta.IdVenta = dbo.DetalleVenta.IdVenta " +
                        "INNER JOIN dbo.Producto ON dbo.DetalleVenta.IdProducto = dbo.Producto.IdProducto " +
                        "WHERE dbo.Venta.IdVenta = "+ Convert.ToInt32(txtNumVenta.Text) + " and dbo.DetalleVenta.Estado <> 'Cancelada';");
                    dgvDetalleVenta.DataSource = TablaDetalleVentaMod;
                    if (accionVenta == "Editar Venta")
                    {
                        TablaDetalleVentaMod.Columns.Add(" ", typeof(string));
                        foreach (DataRow row in TablaDetalleVentaMod.Rows)
                        {
                            row[5] = "Eliminar";
                        }
                    }
                    txtNumVenta.ReadOnly = true;
                    ActualizarTotalPagar();

                }
                else
                {
                    MessageBox.Show("Venta no encontrada", "Error al buscar venta", MessageBoxButtons.OK, MessageBoxIcon.Error);  
                }

            }
            else
            {
                MessageBox.Show("Debe ingresar el numero de venta a buscar", "Error al buscar venta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVerVentas_Click(object sender, EventArgs e)
        {
            VerVentas verVentas = new VerVentas();
            verVentas.Show();
        }
    }
}
