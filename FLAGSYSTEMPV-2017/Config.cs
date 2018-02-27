﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using System.IO;

namespace FLAGSYSTEMPV_2017
{
    public partial class Config : Form
    {
        public Config()
        {
            InitializeComponent();
        }

        private void Config_Load(object sender, EventArgs e)
        {
            cargadata();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Config_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 4),
                          this.DisplayRectangle);            
        }

        private void cargadata()
        {
            if (Demo.EsDemo == false)
            {
                    Conexion.abrir();
                    DataTable datos = Conexion.Consultar("*", "Configuracion", "", "", new SqlCeCommand());
                    Conexion.cerrar();
                    if (datos.Rows.Count > 0)
                    {
                        textBox1.Text = datos.Rows[0][2].ToString();
                        textBox2.Text = datos.Rows[0][3].ToString();
                        textBox3.Text = datos.Rows[0][4].ToString();
                        textBox4.Text = datos.Rows[0][5].ToString();
                        textBox5.Text = datos.Rows[0][6].ToString();
                        textBox6.Text = datos.Rows[0][8].ToString();
                        textBox7.Text = float.Parse(datos.Rows[0][0].ToString()).ToString("$0.00");
                        textBox14.Text = datos.Rows[0][17].ToString();
                        textBox13.Text = datos.Rows[0][18].ToString();
                        textBox12.Text = datos.Rows[0][19].ToString();
                        textBox11.Text = datos.Rows[0][20].ToString();
                        textBox10.Text = datos.Rows[0][21].ToString();
                        textBox9.Text = datos.Rows[0][22].ToString();
                        textBox8.Text = datos.Rows[0][23].ToString();
                        textBox16.Text = datos.Rows[0][24].ToString();
                        numericUpDown1.Value = decimal.Parse(datos.Rows[0][27].ToString());
                        if (datos.Rows[0][28].ToString() == "si") checkBox1.Checked = true;
                        if (datos.Rows[0][29].ToString() == "si") checkBox2.Checked = true;
                        if (datos.Rows[0][30].ToString() == "si") checkBox3.Checked = true;
                        if (datos.Rows[0][31].ToString() == "si") checkBox4.Checked = true;
                    }
            }
            else
            {
                MessageBox.Show("Esta opcion es para usuarios de licencia solamente");
                this.Close();
            }
        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)(HT_CAPTION);
        }
        private const int WM_NCHITTEST = 0x84;
        private const int HT_CLIENT = 0x1;
        private const int HT_CAPTION = 0x2;

        private void button1_Click(object sender, EventArgs e)
        {
            string box1;
            string box2;
            string box3;
            string box4;
            if (checkBox1.Checked == true) box1 = "si"; else box1 = "no";
            if (checkBox2.Checked == true) box2 = "si"; else box2 = "no";
            if (checkBox3.Checked == true) box3 = "si"; else box3 = "no";
            if (checkBox4.Checked == true) box4 = "si"; else box4 = "no";
            registereduser.closeandbkp = box1;
            registereduser.sololectura = box2;
            registereduser.alwaysprint = box3;
            registereduser.tooltips = box4;
            registereduser.redondeo = numericUpDown1.Value.ToString();
            SqlCeCommand inserto = new SqlCeCommand();
            inserto.Parameters.Clear();
            inserto.Parameters.AddWithValue("b", textBox2.Text);
            inserto.Parameters.AddWithValue("c", textBox3.Text);
            inserto.Parameters.AddWithValue("d", textBox4.Text);
            inserto.Parameters.AddWithValue("e", textBox5.Text);
            inserto.Parameters.AddWithValue("g", textBox7.Text.Replace("$",""));
            inserto.Parameters.AddWithValue("h", textBox14.Text);
            inserto.Parameters.AddWithValue("i", textBox13.Text);
            inserto.Parameters.AddWithValue("j", textBox12.Text);
            inserto.Parameters.AddWithValue("k", textBox11.Text);
            inserto.Parameters.AddWithValue("l", textBox10.Text);
            inserto.Parameters.AddWithValue("m", textBox9.Text);
            inserto.Parameters.AddWithValue("n", textBox8.Text);
            inserto.Parameters.AddWithValue("o", textBox16.Text);
            inserto.Parameters.AddWithValue("p", numericUpDown1.Value);
            inserto.Parameters.AddWithValue("q", box1);
            inserto.Parameters.AddWithValue("r", box2);
            inserto.Parameters.AddWithValue("s", box3);
            inserto.Parameters.AddWithValue("t", box4);
            Conexion.abrir();
            Conexion.Actualizar("Configuracion", "tooltipsON = @t, redondeo = @p,backupearsiemprealcerrardia = @q, nopermitircambiosendiasanteriores = @r, siempreimprimirtickets = @s, DireccionFisica = @b,Email= @c,Telefono1= @d,Localidad= @e,SaldoInicial= @g,SMTP= @h,PUERTO= @i,SSL= @j,MAIL= @k,CLAVE= @l,PARA= @m,TITULO= @n,CUERPO= @o", "", "", inserto);
            Conexion.cerrar();
            registereduser.smtp = textBox14.ToString();
            registereduser.puerto = textBox13.ToString();
            registereduser.ssl = textBox12.ToString();
            registereduser.mail = textBox11.ToString();
            registereduser.clave = textBox10.ToString();
            registereduser.para = textBox9.ToString();
            registereduser.titulo = textBox8.ToString();
            registereduser.cuerpo = textBox16.ToString();    
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Bases de Datos (*.sdf)|*.sdf";
            saveFileDialog1.InitialDirectory = app.dir;
            saveFileDialog1.FileName = "BACKUP" + app.hoy.Replace("/", "")+".sdf";
            saveFileDialog1.ShowDialog();

          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult seguro = MessageBox.Show("Esta función volverá a un backup anterior donde puede que ciertos usuarios o PCs aun no hayan sido registrados, esto puede causar problemas a nivel operativo, está seguro de volver a un backup anterior?", "Seguro de volver a un backup anterior?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (seguro == DialogResult.Yes)
            {
                openFileDialog1.Filter = "Bases de Datos (*.sdf)|*.sdf";
                openFileDialog1.InitialDirectory = app.dir;
                openFileDialog1.ShowDialog();
            }
            
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string archivoabrir = openFileDialog1.FileName;
           
            string nombrebackup = "Backup"+app.hoy.Replace("/","")+DateTime.Now.Hour.ToString()+DateTime.Now.Minute.ToString()+DateTime.Now.Second.ToString()+".sdf";
           // MessageBox.Show(nombrebackup);
            if (!Directory.Exists(app.dir + "\\BackupsAutomaticos\\")) Directory.CreateDirectory(app.dir + "\\BackupsAutomaticos");
            File.Move(app.dir + "\\BACKEND.sdf", app.dir + "\\BackupsAutomaticos\\" + nombrebackup);
         
            File.Copy(archivoabrir, app.dir + "\\BACKEND.sdf");
            MessageBox.Show("Se ha abierto el backup anterior correctamente. Se ha hecho un backup automatico de la base antes de realizar esta accion, la misma se encuentra en BackupsAutomaticos\\" + nombrebackup);
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string rutaynombre = saveFileDialog1.FileName;
            if (!File.Exists(rutaynombre))
                File.Copy(app.dir + "\\BACKEND.sdf", rutaynombre);
            else MessageBox.Show("El archivo ya existe, elimine o mueva el archivo existente para poder guardar el nuevo backup","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            float saldoini = 0;
            try
            {
                saldoini = float.Parse(textBox7.Text.ToString());
                textBox7.Text = saldoini.ToString("$0.00");
            }
            catch
            {
                textBox7.Text = saldoini.ToString("$0.00");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("A continuación le explicaremos los SMTPs mas utilizados para agilizar la introduccion de datos para el envio de emails.\n\nSMTP para GMAIL:\nIngrese los siguientes datos para utilizar el servicio de GMAIL:\nSMTP: smtp.gmail.com\nPuerto: 587 o 465\n\nSMTP para HOTMAIL:\nIngrese los siguientes datos para utilizar el servicio de HOTMAIL:\nSMTP: smtp.live.com\nPuerto: 587\n\nSMTP para YAHOO:\nIngrese los siguientes datos para utilizar el servicio de YAHOO:\nSMTP: smtp.mail.yahoo.com\nPuerto: 465\n\nEmail Enviador: SU EMAIL\nClave Email: SU CLAVE\nPara: EMAIL DE LA PERSONA QUE RECIBIRA EL CORREO\nTitulo Mail: EL TITULO DEL EMAIL\nCuerpo del Mail: UN MENSAJE CORTO\nArchivo Adjunto: El archivo adjunto siempre será el resumen del dia, en el resumen del dia se encuentran todos los movimientos registrados en el dia.");
        }
    }
}
