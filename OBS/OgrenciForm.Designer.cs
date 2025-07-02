namespace OBS
{
    partial class OgrenciForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblBilgiler = new System.Windows.Forms.Label();
            this.txtOgrenciNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAd = new System.Windows.Forms.TextBox();
            this.txtSoyad = new System.Windows.Forms.TextBox();
            this.txtSinif = new System.Windows.Forms.TextBox();
            this.txtBolum = new System.Windows.Forms.TextBox();
            this.txtDogumTarihi = new System.Windows.Forms.TextBox();
            this.txtCinsiyet = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnCikis = new System.Windows.Forms.Button();
            this.btnSifreDegistir = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtTCKimlikNo = new System.Windows.Forms.TextBox();
            this.dgvNotlar = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNotlar)).BeginInit();
            this.SuspendLayout();
            // 
            // lblBilgiler
            // 
            this.lblBilgiler.AutoSize = true;
            this.lblBilgiler.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblBilgiler.Location = new System.Drawing.Point(20, 20);
            this.lblBilgiler.Name = "lblBilgiler";
            this.lblBilgiler.Size = new System.Drawing.Size(199, 32);
            this.lblBilgiler.TabIndex = 0;
            this.lblBilgiler.Text = "Öğrenci Bilgileri";
            this.lblBilgiler.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtOgrenciNo
            // 
            this.txtOgrenciNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOgrenciNo.Enabled = false;
            this.txtOgrenciNo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtOgrenciNo.Location = new System.Drawing.Point(150, 70);
            this.txtOgrenciNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtOgrenciNo.MaxLength = 10;
            this.txtOgrenciNo.Name = "txtOgrenciNo";
            this.txtOgrenciNo.ReadOnly = true;
            this.txtOgrenciNo.Size = new System.Drawing.Size(200, 34);
            this.txtOgrenciNo.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 28);
            this.label1.TabIndex = 2;
            this.label1.Text = "Öğrenci No";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(107, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 28);
            this.label2.TabIndex = 3;
            this.label2.Text = "Ad";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(77, 152);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 28);
            this.label3.TabIndex = 4;
            this.label3.Text = "Soyad";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(94, 276);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 28);
            this.label4.TabIndex = 5;
            this.label4.Text = "Sınıf";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(76, 230);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 28);
            this.label5.TabIndex = 6;
            this.label5.Text = "Bölüm";
            // 
            // txtAd
            // 
            this.txtAd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAd.Enabled = false;
            this.txtAd.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtAd.Location = new System.Drawing.Point(150, 110);
            this.txtAd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAd.Name = "txtAd";
            this.txtAd.ReadOnly = true;
            this.txtAd.Size = new System.Drawing.Size(200, 34);
            this.txtAd.TabIndex = 7;
            // 
            // txtSoyad
            // 
            this.txtSoyad.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSoyad.Enabled = false;
            this.txtSoyad.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtSoyad.Location = new System.Drawing.Point(150, 150);
            this.txtSoyad.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSoyad.Name = "txtSoyad";
            this.txtSoyad.ReadOnly = true;
            this.txtSoyad.Size = new System.Drawing.Size(200, 34);
            this.txtSoyad.TabIndex = 8;
            // 
            // txtSinif
            // 
            this.txtSinif.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSinif.Enabled = false;
            this.txtSinif.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtSinif.Location = new System.Drawing.Point(150, 270);
            this.txtSinif.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSinif.Name = "txtSinif";
            this.txtSinif.ReadOnly = true;
            this.txtSinif.Size = new System.Drawing.Size(200, 34);
            this.txtSinif.TabIndex = 9;
            // 
            // txtBolum
            // 
            this.txtBolum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBolum.Enabled = false;
            this.txtBolum.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtBolum.Location = new System.Drawing.Point(150, 230);
            this.txtBolum.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBolum.Name = "txtBolum";
            this.txtBolum.ReadOnly = true;
            this.txtBolum.Size = new System.Drawing.Size(200, 34);
            this.txtBolum.TabIndex = 10;
            // 
            // txtDogumTarihi
            // 
            this.txtDogumTarihi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDogumTarihi.Enabled = false;
            this.txtDogumTarihi.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtDogumTarihi.Location = new System.Drawing.Point(150, 350);
            this.txtDogumTarihi.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDogumTarihi.Name = "txtDogumTarihi";
            this.txtDogumTarihi.ReadOnly = true;
            this.txtDogumTarihi.Size = new System.Drawing.Size(200, 34);
            this.txtDogumTarihi.TabIndex = 11;
            // 
            // txtCinsiyet
            // 
            this.txtCinsiyet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCinsiyet.Enabled = false;
            this.txtCinsiyet.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtCinsiyet.Location = new System.Drawing.Point(150, 310);
            this.txtCinsiyet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCinsiyet.Name = "txtCinsiyet";
            this.txtCinsiyet.ReadOnly = true;
            this.txtCinsiyet.Size = new System.Drawing.Size(200, 34);
            this.txtCinsiyet.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 350);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(129, 28);
            this.label6.TabIndex = 13;
            this.label6.Text = "Doğum Tarihi";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(64, 318);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 28);
            this.label7.TabIndex = 14;
            this.label7.Text = "Cinsiyet";
            // 
            // btnCikis
            // 
            this.btnCikis.BackColor = System.Drawing.Color.Red;
            this.btnCikis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCikis.Location = new System.Drawing.Point(44, 402);
            this.btnCikis.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCikis.Name = "btnCikis";
            this.btnCikis.Size = new System.Drawing.Size(100, 35);
            this.btnCikis.TabIndex = 15;
            this.btnCikis.Text = "Çıkış";
            this.btnCikis.UseVisualStyleBackColor = false;
            this.btnCikis.Click += new System.EventHandler(this.btnCikis_Click);
            // 
            // btnSifreDegistir
            // 
            this.btnSifreDegistir.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnSifreDegistir.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSifreDegistir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSifreDegistir.Location = new System.Drawing.Point(44, 445);
            this.btnSifreDegistir.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSifreDegistir.Name = "btnSifreDegistir";
            this.btnSifreDegistir.Size = new System.Drawing.Size(100, 70);
            this.btnSifreDegistir.TabIndex = 16;
            this.btnSifreDegistir.Text = "Şifre Değiştir";
            this.btnSifreDegistir.UseVisualStyleBackColor = false;
            this.btnSifreDegistir.Click += new System.EventHandler(this.btnSifreDegistir_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(20, 192);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(124, 28);
            this.label8.TabIndex = 17;
            this.label8.Text = "TC Kimlik No";
            // 
            // txtTCKimlikNo
            // 
            this.txtTCKimlikNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTCKimlikNo.Enabled = false;
            this.txtTCKimlikNo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtTCKimlikNo.Location = new System.Drawing.Point(150, 190);
            this.txtTCKimlikNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTCKimlikNo.Name = "txtTCKimlikNo";
            this.txtTCKimlikNo.ReadOnly = true;
            this.txtTCKimlikNo.Size = new System.Drawing.Size(200, 34);
            this.txtTCKimlikNo.TabIndex = 18;
            // 
            // dgvNotlar
            // 
            this.dgvNotlar.AllowUserToAddRows = false;
            this.dgvNotlar.AllowUserToDeleteRows = false;
            this.dgvNotlar.BackgroundColor = System.Drawing.Color.White;
            this.dgvNotlar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNotlar.Location = new System.Drawing.Point(392, 70);
            this.dgvNotlar.Name = "dgvNotlar";
            this.dgvNotlar.ReadOnly = true;
            this.dgvNotlar.RowHeadersVisible = false;
            this.dgvNotlar.RowHeadersWidth = 51;
            this.dgvNotlar.RowTemplate.Height = 24;
            this.dgvNotlar.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvNotlar.Size = new System.Drawing.Size(820, 440);
            this.dgvNotlar.TabIndex = 2000;
            this.dgvNotlar.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvNotlar_CellFormatting);
            // 
            // OgrenciForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1242, 539);
            this.Controls.Add(this.dgvNotlar);
            this.Controls.Add(this.txtTCKimlikNo);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnSifreDegistir);
            this.Controls.Add(this.btnCikis);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtCinsiyet);
            this.Controls.Add(this.txtDogumTarihi);
            this.Controls.Add(this.txtBolum);
            this.Controls.Add(this.txtSinif);
            this.Controls.Add(this.txtSoyad);
            this.Controls.Add(this.txtAd);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtOgrenciNo);
            this.Controls.Add(this.lblBilgiler);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimizeBox = false;
            this.Name = "OgrenciForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Öğrenci Paneli";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OgrenciForm_FormClosing_1);
            this.Load += new System.EventHandler(this.OgrenciForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNotlar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblBilgiler;
        private System.Windows.Forms.TextBox txtOgrenciNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtAd;
        private System.Windows.Forms.TextBox txtSoyad;
        private System.Windows.Forms.TextBox txtSinif;
        private System.Windows.Forms.TextBox txtBolum;
        private System.Windows.Forms.TextBox txtDogumTarihi;
        private System.Windows.Forms.TextBox txtCinsiyet;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnCikis;
        private System.Windows.Forms.Button btnSifreDegistir;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtTCKimlikNo;
        private System.Windows.Forms.DataGridView dgvNotlar;
    }
}