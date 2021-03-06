﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;

namespace FLAGSYSTEMPV_2017
{
    public partial class Consultas : Form
    {


        public Consultas()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Consultas_Load(object sender, EventArgs e)
        {
            
            if (Conexion.data == "Ventas")
            {
                label1.Text = "Listado de Ventas";
                label2.Text = "Detalle de venta seleccionada";
                Conexion.abrir();
                DataTable showv = Conexion.Consultar("nfactura as [N° Fact.], vendedor as Usuario, fechaventa as Fecha, total as Importe, estadoventa as Estado, tipoFactura as Factura", "Ventas", " order by nfactura desc", "", new SqlCeCommand());
                Conexion.cerrar();
                BindingSource SBind = new BindingSource();
                SBind.DataSource = showv;
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = showv;
                dataGridView1.DataSource = SBind;
                dataGridView1.Columns[3].DefaultCellStyle.Format = "c";
                dataGridView1.Refresh();
                if (showv.Rows.Count > 0)
                dataGridView1.DataSource = showv; //mostramos lo que hay
                textBox1.Select();
            }
            if (Conexion.data == "Compras")
            {
                label1.Text = "Listado de Compras";
                label2.Text = "Detalle de compra seleccionada";
                Conexion.abrir();
                DataTable showv = Conexion.Consultar("nfactura as [N° Factura], vendedor as Vendedor, fechacompra as Fecha,proveedor as Proveedor, totalfactura as Total,estadocompra as Estado ", "Compras", " order by nfactura desc", "", new SqlCeCommand());
                Conexion.cerrar();
                BindingSource SBind = new BindingSource();
                SBind.DataSource = showv;
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = showv;
                dataGridView1.DataSource = SBind;
                dataGridView1.Refresh();
                if (showv.Rows.Count > 0)
                    dataGridView1.DataSource = showv; //mostramos lo que hay
                textBox1.Select();
            }
        }

        void get(string what1, string fromwhere1, string where,string valuedata, DataGridView whatview1)
        {
            Conexion.abrir();
            
            DataTable showv = Conexion.Consultar(what1, fromwhere1, where, "", new SqlCeCommand());
            
            Conexion.cerrar();
            BindingSource SBind = new BindingSource();
            SBind.DataSource = showv;
            whatview1.AutoGenerateColumns = true;
            whatview1.DataSource = showv;
            whatview1.DataSource = SBind;
            //whatview1.Columns[3].DefaultCellStyle.Format = "c";
            whatview1.Refresh();

            if (showv.Rows.Count > 0)
                whatview1.DataSource = showv; //mostramos lo que hay
           
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (Conexion.data == "Ventas")
            {
                try
                {
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    string nfactura = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
                    Conexion.abrir();
                    SqlCeCommand data = new SqlCeCommand();
                    data.Parameters.AddWithValue("@nf", nfactura);
                    DataTable showv = Conexion.Consultar("codigoproducto as Codigo, descripproducto as Descripcion,marcaproducto as Marca,cantidproducto as Cantidad, precioproducto as Precio, totalproducto as Total", "DetalleVentas", "WHERE nfactura = @nf", "", data);

                    Conexion.cerrar();
                    BindingSource SBind = new BindingSource();
                    SBind.DataSource = showv;
                    dataGridView2.AutoGenerateColumns = true;

                    dataGridView2.DataSource = showv;
                   
                    dataGridView2.DataSource = SBind;
                   
                    dataGridView2.Refresh();
                    
                    if (showv.Rows.Count > 0)
                        dataGridView2.DataSource = showv; //mostramos lo que hay
                }
                catch (Exception) { }
            }
            if (Conexion.data == "Compras")
            {
                try
                {
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    string nfactura = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
                    Conexion.abrir();
                    SqlCeCommand data = new SqlCeCommand();
                    data.Parameters.AddWithValue("@nf", nfactura);
                    DataTable showv = Conexion.Consultar("nfactura as [N° Factura],descripproducto as Descripcion, marcaproducto as Marca, cantidproducto as Cantidad, precioproducto as [Precio U], totalproducto as Total", "DetalleCompras", "WHERE nfactura = @nf", "", data);

                    Conexion.cerrar();
                    BindingSource SBind = new BindingSource();
                    SBind.DataSource = showv;
                    dataGridView2.AutoGenerateColumns = true;
                    //dataGridView1.Columns[2].DefaultCellStyle.Format = "c";
                    //dataGridView2.Columns[4].DefaultCellStyle.Format = "c";
                    //dataGridView2.Columns[5].DefaultCellStyle.Format = "c";
                    dataGridView2.DataSource = showv;

                    dataGridView2.DataSource = SBind;

                    dataGridView2.Refresh();

                    if (showv.Rows.Count > 0)
                        dataGridView2.DataSource = showv; //mostramos lo que hay
                }
                catch (Exception)
                { }
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
            if (Conexion.data == "Ventas")
            {
                try
                {


                    var bd = dataGridView1.DataSource;
                    var dt = (DataTable)bd;
                    string formatstring = "";
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (i == 0) formatstring += " CONVERT([" + dt.Columns[i].ColumnName + "],System.String) like '%{0}%' ";
                        else formatstring += " or CONVERT([" + dt.Columns[i].ColumnName + "],System.String) like '%{0}%' ";

                    }

                    //MessageBox.Show(formatstring);
                    dt.DefaultView.RowFilter = string.Format(formatstring, textBox1.Text.Trim().Replace("'", "''"));
                    dataGridView1.Refresh();

                }
                catch (Exception) { }
            }
            if (Conexion.data == "Compras")
            {
                try
                {


                    var bd = dataGridView1.DataSource;
                    var dt = (DataTable)bd;
                    string formatstring = "";
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (i == 0) formatstring += " CONVERT([" + dt.Columns[i].ColumnName + "],System.String) like '%{0}%' ";
                        else formatstring += " or CONVERT([" + dt.Columns[i].ColumnName + "],System.String) like '%{0}%' ";

                    }

                    //MessageBox.Show(formatstring);
                    dt.DefaultView.RowFilter = string.Format(formatstring, textBox1.Text.Trim().Replace("'", "''"));
                    dataGridView1.Refresh();

                }
                catch (Exception) { }
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
          
        }

        private void Consultas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
            if (e.KeyCode == Keys.F1) textBox1.Select();
            
            if (e.KeyCode == Keys.Up && dataGridView1.Focused == false && dataGridView2.Focused == false)
            {
                try
                {
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    dataGridView1.Rows[rowIndex - 1].Cells[1].Selected = true;
                }
                catch (Exception)
                {

                }

            }
            if (e.KeyCode == Keys.Down && dataGridView1.Focused == false && dataGridView2.Focused == false)
            {
                try
                {
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    dataGridView1.Rows[rowIndex + 1].Cells[1].Selected = true;
                }
                catch (Exception)
                {
                }

            }
        }
      
    }
}
