﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using FiscalPrinterLib;
namespace FLAGSYSTEMPV_2017
{
    public partial class Ventas : Form
    {
        public Ventas()
        {
            InitializeComponent();
        }
        public string totventa;
        private void Ventas_Load(object sender, EventArgs e)
        {
            textBox4.Select();
            totalventa.compraoventa = "Ventas";
            if (dataGridView1.Rows.Count == 0)
            {
                float cero = 0;
                textBox5.Text = cero.ToString("$0.00");
            }
            if (Demo.EsDemo == false)
                textBox3.Text = app.hoy;
            else
                textBox3.Text = DateTime.Now.ToShortDateString();
            Conexion.abrir();
            DataTable nextid = new DataTable();
            nextid = Conexion.Consultar("AUTOINC_NEXT", "INFORMATION_SCHEMA.COLUMNS", " WHERE (TABLE_NAME = 'Ventas') AND (COLUMN_NAME = 'idventa')", "", new SqlCeCommand());
            Conexion.cerrar();
            textBox1.Text = nextid.Rows[0][0].ToString();
            totalventa.idventa = Convert.ToInt32(nextid.Rows[0][0].ToString());
            if (Demo.EsDemo == true) textBox2.Text = Demo.demouser;
            else textBox2.Text = registereduser.reguser;
            float idv = float.Parse(textBox1.Text);
            if (idv >= 50 && Demo.EsDemo == true)
            {
                this.Close();
                MessageBox.Show("Lo sentimos pero esta es la versión demo del producto y solo se permiten ingresar hasta 50 registros de venta. Si quiere habilitar todo el sistema, deberá comprar la licencia.");            
            }
        }
     
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            if (Application.OpenForms.OfType<Inicio>().Count() == 1)
                Application.OpenForms.OfType<Inicio>().First().Focus();
        }

      

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    int  Cantidad = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells[1].Value);
                    float precio = float.Parse(dataGridView1.Rows[rowIndex].Cells[5].Value.ToString().Replace("$", ""));
                    if (Cantidad > 0)
                    {
                        dataGridView1.Rows[rowIndex].Cells[1].Value = (Cantidad + 1).ToString();
                        dataGridView1.Rows[rowIndex].Cells[6].Value = ((Cantidad + 1) * precio).ToString("$0.00") ;
                        chequeartotal();
                    }
                }
            }
            if (e.KeyCode == Keys.Left)
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    int Cantidad = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells[1].Value);
                    float precio = float.Parse(dataGridView1.Rows[rowIndex].Cells[5].Value.ToString().Replace("$", ""));

                    if (Cantidad > 1)
                    {
                        dataGridView1.Rows[rowIndex].Cells[1].Value = (Cantidad - 1).ToString();
                        dataGridView1.Rows[rowIndex].Cells[6].Value = ((Cantidad - 1) * precio).ToString("$0.00");
                        chequeartotal();
                    }
                }
            }
           
            if (e.KeyCode == Keys.Up)
            {
                try
                {
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    dataGridView1.Rows[rowIndex -1].Cells[1].Selected = true;
                }
                catch (Exception)
                {

                }

            }
            if (e.KeyCode == Keys.Down)
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
           
            if (e.KeyCode == Keys.Enter)
            {
                if (dataGridView1.Rows.Count < 1 && textBox4.Text.Length == 0) MessageBox.Show("No hay ningún artículo vendido");
                if (dataGridView1.Rows.Count > 0 && textBox4.Text.Length == 0)
                {

                    DataTable dt = new DataTable(); //agarramos el gridview y le metemos los datos a un datatable.
                    //no hay forma mejor que hacerlo de esta manera porque no funciona referenciando el DT del dgridview

                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        dt.Columns.Add("column" + i.ToString());
                    }
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < dataGridView1.Columns.Count; j++)
                        {
                            dr["column" + j.ToString()] = row.Cells[j].Value;
                        }

                        dt.Rows.Add(dr);
                    }
                    totalventa.detalle = dt;
                    totalventa.totventa = textBox5.Text;
                    Total total = new Total();
                    total.Show();
                   

                }//mostrar la pantalla que ingresa plata clente
                if (textBox4.Text.Length > 0)
                {
                    Conexion.abrir();
                    SqlCeCommand metocodigo = new SqlCeCommand();
                    metocodigo.Parameters.AddWithValue("@cod", textBox4.Text);
                    metocodigo.Parameters.AddWithValue("@elim", "Eliminado");
                    DataTable producto = Conexion.Consultar("idarticulo,codigoart,descripcion,marca,precio,tipo", "Articulos", "WHERE codigoart = @cod and eliminado !=@elim", "", metocodigo);
                    Conexion.cerrar();
                    if (producto.Rows.Count > 0)
                    {
                        int cantidad = 1;
                        string idproducto = producto.Rows[0][0].ToString();
                        string codigo = producto.Rows[0][1].ToString();
                        string desc = producto.Rows[0][2].ToString();
                        string mca = producto.Rows[0][3].ToString();
                        float prec = float.Parse(producto.Rows[0][4].ToString());
                        string tipo = producto.Rows[0][5].ToString();
                        float total = prec * cantidad;
                        dataGridView1.Rows.Add(idproducto,cantidad, codigo, desc, mca, prec.ToString("$0.00"), total.ToString("$0.00"),tipo);
                        textBox4.Text = "";
                        dataGridView1.Rows[(dataGridView1.Rows.Count - 1)].Cells[1].Selected = true;
                    }
                    else
                    {
                        MessageBox.Show("No se ha encontrado el artículo");
                        textBox4.Text = "";
                    }
                }//chequear si existe el codigo de ese producto y de ser asi agregarlo al DGV.
            }
            if (e.KeyCode == Keys.F5)
            {
                //abrir busqueda de articulo
                Buscarticulo bus = new Buscarticulo();
                bus.ShowDialog();
                if (totalventa.codprodbuscado != "")
                {
                    Conexion.abrir();
                    SqlCeCommand metocodigo = new SqlCeCommand();
                    metocodigo.Parameters.AddWithValue("@cod", totalventa.codprodbuscado);
                    DataTable producto = Conexion.Consultar("idarticulo,codigoart,descripcion,marca,precio,tipo", "Articulos", "WHERE codigoart = @cod", "", metocodigo);
                    Conexion.cerrar();
                    if (producto.Rows.Count > 0)
                    {
                        int cantidad = 1;
                        string idproducto = producto.Rows[0][0].ToString();
                        string codigo = producto.Rows[0][1].ToString();
                        string desc = producto.Rows[0][2].ToString();
                        string mca = producto.Rows[0][3].ToString();
                        float prec = float.Parse(producto.Rows[0][4].ToString());
                        string tipo = producto.Rows[0][5].ToString();
                        float total = prec * cantidad;
                        dataGridView1.Rows.Add(idproducto, cantidad, codigo, desc, mca, prec.ToString("$0.00"), total.ToString("$0.00"),tipo.ToString());
                        textBox4.Text = "";
                        //MessageBox.Show(dataGridView1.Rows[0].Cells[7].Value.ToString());
                        dataGridView1.Rows[(dataGridView1.Rows.Count - 1)].Cells[1].Selected = true;
                    }
                }
                
            }
            if (e.KeyCode == Keys.F4 && dataGridView1.Rows.Count > 0)
            {
                //abrir busqueda de articulo
                IngreseUnidades ing = new IngreseUnidades();
                ing.ShowDialog();
                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                decimal Cantidad = totalventa.cantidad;
                float precio = float.Parse(dataGridView1.Rows[rowIndex].Cells[5].Value.ToString().Replace("$", ""));
                if (Cantidad > 0)
                {
                    dataGridView1.Rows[rowIndex].Cells[1].Value = Cantidad.ToString();
                    dataGridView1.Rows[rowIndex].Cells[6].Value = ((float.Parse(Cantidad.ToString())  * precio).ToString("$0.00"));
                    chequeartotal();
                }
            }
            if (e.KeyCode == Keys.Delete)
            {

                if (dataGridView1.Rows.Count > 0)
                {
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;

                    dataGridView1.Rows.Remove(dataGridView1.Rows[rowIndex]);
                    
                }
            }
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            chequeartotal();
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {

            chequeartotal();
        }

        void chequeartotal()
        {
            if (dataGridView1.Rows.Count == 0)
            {
                float cero = 0;
                textBox5.Text = cero.ToString("$0.00");
            }
            else
            {
                float totalgral = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    var row = this.dataGridView1.Rows[i];
                    float total = float.Parse(row.Cells["Total"].Value.ToString().Replace("$", ""));
                    totalgral += total;
                }
                textBox5.Text = totalgral.ToString("$0.00");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox4.Focus();
            MessageBox.Show("Enter: Finaliza Venta (si se cargaron articulos)\nEscape: Cancela toda la venta si hay articulos cargados, de lo contrario cierra la ventana\nAbajo: Mueve el cursor de la lista de productos hacia abajo (si hay articulos cargados)\nArriba: Idem anterior pero para arriba.\nIzquierda: Disminuye en 1 las unidades del producto seleccionado.\nDerecha Aumenta en 1 las unidades del producto seleccionado.\nF5: Abre panel de busqueda de articulos.\nF4: abre una ventana en donde se puede escribir cuantas unidades asignar a un articulo.","Atajos del teclado",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void rectangleShape1_Click(object sender, EventArgs e)
        {
            textBox4.Focus();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox4.Focus();
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox4.Focus();
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            textBox4.Focus();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            textBox4.Focus();
        }

        private void textBox5_Click(object sender, EventArgs e)
        {
            textBox4.Focus();

            //FiscalPrinterLib.HASAR prntr = new FiscalPrinterLib.HASAR();
            
        }

        private void Ventas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    DialogResult result = MessageBox.Show("Está seguro de cancelar la venta?", "Atención",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        this.Close();
                        Ventas frm = new Ventas();
                        frm.Show();
                        
                    }
                }
                else
                {
                    this.Close();
                    if (Application.OpenForms.OfType<Inicio>().Count() == 1)
                        Application.OpenForms.OfType<Inicio>().First().Focus();
                }
            }
        }

    }
}
