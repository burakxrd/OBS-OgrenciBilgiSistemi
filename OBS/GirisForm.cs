using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OBS
{

    public partial class GirisForm : Form
    {
        private int _girisDenemeSayisi = 0;
        public GirisForm()
        {
            InitializeComponent();
            ButtonAyarla(btnGiris, Color.DodgerBlue);
        }

        private void ButtonAyarla(Button btn, Color backColor)
        {
            btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Dark(backColor, 0.1F);
            btn.MouseLeave += (s, e) => btn.BackColor = backColor;
        }
        private void btnGiris_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtKullaniciAdi.Text) || string.IsNullOrWhiteSpace(txtSifre.Text))
            {
                MessageBox.Show("Lütfen kullanıcı adı ve şifreyi doldurun.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var context = new OBSModel())
                {
                    var kullanici = context.Kullanicilar
                        .FirstOrDefault(k => k.KullaniciAdi == txtKullaniciAdi.Text.Trim() && k.Sifre == txtSifre.Text.Trim());

                    if (kullanici == null)
                    {
                        _girisDenemeSayisi++;
                        if (_girisDenemeSayisi >= 3)
                        {
                            MessageBox.Show("Çok fazla başarısız deneme. Lütfen daha sonra tekrar deneyin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        MessageBox.Show($"Kullanıcı adı veya şifre yanlış. Kalan deneme: {3 - _girisDenemeSayisi}", "Giriş Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    _girisDenemeSayisi = 0;

                    var denetim = new DenetimKayitlari
                    {
                        KullaniciNo = kullanici.KullaniciNo,
                        Eylem = $"Kullanıcı giriş yaptı: {kullanici.KullaniciAdi} (Rol: {kullanici.Rol})".Substring(0, Math.Min(200, $"Kullanıcı giriş yaptı: {kullanici.KullaniciAdi} (Rol: {kullanici.Rol})".Length)),
                        ZamanDamgasi = DateTime.Now
                    };
                    context.DenetimKayitlari.Add(denetim);
                    context.SaveChanges();

                    if (kullanici.Rol == "Yonetici")
                    {
                        Form1 anaForm = new Form1(kullanici.KullaniciNo);
                        anaForm.Show();
                        this.Hide();
                    }
                    else if (kullanici.Rol == "Ogrenci")
                    {
                        OgrenciForm ogrenciForm = new OgrenciForm(kullanici.KullaniciAdi);
                        ogrenciForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Geçersiz kullanıcı rolü.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Giriş sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtSifre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnGiris.PerformClick();
            }
        }

        private void chkSifreyiGoster_CheckedChanged_1(object sender, EventArgs e)
        {
            txtSifre.PasswordChar = chkSifreyiGoster.Checked ? '\0' : '*';
        }
        private void GirisForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        

        private void lnkSifremiUnuttum_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var sifremiUnuttumForm = new Form())
            {
                // Form ayarları
                sifremiUnuttumForm.Text = "Şifremi Unuttum";
                sifremiUnuttumForm.Size = new Size(350, 220); // Daha ferah bir boyut
                sifremiUnuttumForm.StartPosition = FormStartPosition.CenterParent;
                sifremiUnuttumForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                sifremiUnuttumForm.MaximizeBox = false;
                sifremiUnuttumForm.MinimizeBox = false;
                sifremiUnuttumForm.BackColor = Color.FromArgb(245, 245, 245); // Açık gri, modern arka plan

                // Başlık etiketi
                Label lblBaslik = new Label
                {
                    Text = "Şifrenizi Sıfırlayın",
                    Location = new Point(20, 20),
                    Size = new Size(300, 30),
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                // TC Kimlik No etiketi
                Label lblTCKimlikNo = new Label
                {
                    Text = "TC Kimlik No:",
                    Location = new Point(20, 60),
                    Size = new Size(100, 20),
                    Font = new Font("Segoe UI", 10)
                };

                // TC Kimlik No TextBox
                TextBox txtTCKimlikNo = new TextBox
                {
                    Location = new Point(120, 60),
                    Size = new Size(200, 25),
                    MaxLength = 11,
                    Font = new Font("Segoe UI", 10),
                    BorderStyle = BorderStyle.FixedSingle
                };

                // Şifreyi Sıfırla butonu
                Button btnSifirla = new Button
                {
                    Text = "Şifreyi Sıfırla",
                    Location = new Point(120, 100),
                    Size = new Size(110, 40),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.SeaGreen,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                };

                // İptal butonu
                Button btnIptal = new Button
                {
                    Text = "İptal",
                    Location = new Point(230, 100),
                    Size = new Size(100, 40),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.Crimson,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                };

                // Kontrolleri forma ekle
                sifremiUnuttumForm.Controls.AddRange(new Control[] { lblBaslik, lblTCKimlikNo, txtTCKimlikNo, btnSifirla, btnIptal });

                // TC Kimlik No için sadece sayı girişi
                txtTCKimlikNo.KeyPress += (s, args) =>
                {
                    if (!char.IsControl(args.KeyChar) && !char.IsDigit(args.KeyChar))
                    {
                        args.Handled = true; // Sadece sayılara izin ver
                    }
                };

                // Şifreyi Sıfırla butonu mantığı
                btnSifirla.Click += (s, args) =>
                {
                    if (string.IsNullOrWhiteSpace(txtTCKimlikNo.Text))
                    {
                        MessageBox.Show("Lütfen TC Kimlik No girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (txtTCKimlikNo.Text.Trim().Length != 11 || !txtTCKimlikNo.Text.Trim().All(char.IsDigit))
                    {
                        MessageBox.Show("TC Kimlik No 11 haneli bir sayı olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    try
                    {
                        using (var context = new OBSModel())
                        {
                            var kullanici = context.Kullanicilar
                                .FirstOrDefault(k => k.TCKimlikNo == txtTCKimlikNo.Text.Trim());

                            if (kullanici == null)
                            {
                                MessageBox.Show("Bu TC Kimlik No ile kayıtlı kullanıcı bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            // Şifreyi sıfırla
                            kullanici.Sifre = "12345";

                            // Denetim kaydı ekle
                            var denetim = new DenetimKayitlari
                            {
                                KullaniciNo = kullanici.KullaniciNo,
                                Eylem = $"Şifre sıfırlandı: KullaniciAdi: {kullanici.KullaniciAdi}",
                                ZamanDamgasi = DateTime.Now
                            };
                            context.DenetimKayitlari.Add(denetim);

                            context.SaveChanges();

                            MessageBox.Show("Şifreniz sıfırlandı. Yeni şifre: 12345", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            sifremiUnuttumForm.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Şifre sıfırlama sırasında hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                // İptal butonu mantığı
                btnIptal.Click += (s, args) => sifremiUnuttumForm.Close();

                // Formu göster
                sifremiUnuttumForm.ShowDialog();
            }
        }
    }
    }

