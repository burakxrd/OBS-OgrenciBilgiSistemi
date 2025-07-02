using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace OBS
{
    public partial class OgrenciForm : Form
    {
        private readonly string _kullaniciAdi;
        public OgrenciForm(string kullaniciAdi)
        {
            InitializeComponent();
            _kullaniciAdi = kullaniciAdi;
            YukleOgrenciBilgileri();
            YukleNotlar();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ActiveControl = null;
            dgvNotlar.ClearSelection();
        }

        private void YukleOgrenciBilgileri()
        {
            try
            {
                using (var context = new OBSModel())
                {
                    var ogrenci = context.Ogrenciler
                        .Include("Bolumler")
                        .FirstOrDefault(o => o.OgrenciNo.ToString() == _kullaniciAdi);

                    if (ogrenci == null)
                    {
                        MessageBox.Show("Öğrenci bilgileri bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    txtOgrenciNo.Text = ogrenci.OgrenciNo.ToString();
                    txtAd.Text = ogrenci.Ad;
                    txtSoyad.Text = ogrenci.Soyad;
                    txtBolum.Text = ogrenci.Bolumler != null ? ogrenci.Bolumler.BolumAdi : "Bölüm atanmamış";
                    txtSinif.Text = ogrenci.Sinif ?? "Belirtilmemiş";
                    txtDogumTarihi.Text = ogrenci.DogumTarihi.HasValue ? ogrenci.DogumTarihi.Value.ToString("dd.MM.yyyy") : "Belirtilmemiş";
                    txtCinsiyet.Text = ogrenci.Cinsiyet == "E" ? "Erkek" : ogrenci.Cinsiyet == "K" ? "Kadın" : "Belirtilmemiş";
                    txtTCKimlikNo.Text = ogrenci.TCKimlikNo ?? "Belirtilmemiş";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtOgrenciNo.Text = "[Hata]";
                txtAd.Text = "[Hata]";
                txtSoyad.Text = "[Hata]";
                txtTCKimlikNo.Text = "[Hata]";
                txtBolum.Text = "[Hata]";
                txtSinif.Text = "[Hata]";
                txtDogumTarihi.Text = "[Hata]";
                txtCinsiyet.Text = "[Hata]";
            }
        }

        private void YukleNotlar()
        {
            try
            {
                using (var context = new OBSModel())
                {
                    var ogrenci = context.Ogrenciler
                        .FirstOrDefault(o => o.OgrenciNo.ToString() == _kullaniciAdi);
                    if (ogrenci == null) return;

                    // DersKayitlari tablosu yoksa, BolumNo’ya göre dersleri al
                    var query = from d in context.Dersler
                                where d.BolumNo == ogrenci.BolumNo || d.BolumNo == null
                                join n in context.Notlar
                                on new { DersKodu = d.DersKodu, OgrenciNo = ogrenci.OgrenciNo }
                                equals new { n.DersKodu, n.OgrenciNo } into notlar
                                from n in notlar.DefaultIfEmpty()
                                select new
                                {
                                    DersAdi = d.DersAdi,
                                    Vize = n != null ? n.Vize : null,
                                    Final = n != null ? n.Final : null,
                                    But = n != null ? n.But : null
                                };

                    var result = query.ToList().Select(x => new
                    {
                        x.DersAdi,
                        x.Vize,
                        x.Final,
                        x.But,
                        Ortalama = HesaplaOrtalama(x.Vize, x.Final, x.But)?.ToString("F2") ?? "-",
                        HarfNotu = HesaplaOrtalama(x.Vize, x.Final, x.But).HasValue
                            ? HesaplaHarfNotu(HesaplaOrtalama(x.Vize, x.Final, x.But).Value)
                            : "-",
                        Durum = HesaplaOrtalama(x.Vize, x.Final, x.But).HasValue
                            ? (HesaplaOrtalama(x.Vize, x.Final, x.But).Value >= 50 ? "Başarılı" : "Başarısız")
                            : "-"
                    }).OrderBy(x => x.DersAdi).ToList();

                    // DataGridView ayarları
                    dgvNotlar.DataSource = null; // Önce sıfırla
                    dgvNotlar.Columns.Clear();
                    dgvNotlar.DataSource = result;

                    // YENİ: CellFormatting olayına abone ol
                    dgvNotlar.CellFormatting -= dgvNotlar_CellFormatting; // Önceki abonelikleri kaldır
                    dgvNotlar.CellFormatting += dgvNotlar_CellFormatting;

                    // Sütun modunu Fill yap
                    dgvNotlar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    // Sütun isimlerini ve ayarlarını özelleştir
                    if (dgvNotlar.Columns["Vize"] != null)
                    {
                        dgvNotlar.Columns["Vize"].HeaderText = "Vize";
                        dgvNotlar.Columns["Vize"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                        dgvNotlar.Columns["Vize"].ReadOnly = true;
                    }
                    if (dgvNotlar.Columns["Final"] != null)
                    {
                        dgvNotlar.Columns["Final"].HeaderText = "Final";
                        dgvNotlar.Columns["Final"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                        dgvNotlar.Columns["Final"].ReadOnly = true;
                    }
                    if (dgvNotlar.Columns["But"] != null)
                    {
                        dgvNotlar.Columns["But"].HeaderText = "Büt";
                        dgvNotlar.Columns["But"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                        dgvNotlar.Columns["But"].ReadOnly = true;
                    }
                    if (dgvNotlar.Columns["Ortalama"] != null)
                    {
                        dgvNotlar.Columns["Ortalama"].HeaderText = "Ortalama";
                        dgvNotlar.Columns["Ortalama"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        dgvNotlar.Columns["Ortalama"].ReadOnly = true;
                    }
                    if (dgvNotlar.Columns["HarfNotu"] != null)
                    {
                        dgvNotlar.Columns["HarfNotu"].HeaderText = "Harf Notu";
                        dgvNotlar.Columns["HarfNotu"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        dgvNotlar.Columns["HarfNotu"].ReadOnly = true;
                    }
                    if (dgvNotlar.Columns["Durum"] != null)
                    {
                        dgvNotlar.Columns["Durum"].HeaderText = "Durum";
                        dgvNotlar.Columns["Durum"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                        dgvNotlar.Columns["Durum"].ReadOnly = true;
                    }
                    if (dgvNotlar.Columns["DersAdi"] != null)
                    {
                        dgvNotlar.Columns["DersAdi"].HeaderText = "Ders Adı";
                        dgvNotlar.Columns["DersAdi"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Kalan alanı doldur
                        dgvNotlar.Columns["DersAdi"].DefaultCellStyle.WrapMode = DataGridViewTriState.True; // Sarma etkin
                        dgvNotlar.Columns["DersAdi"].ReadOnly = true;
                    }

                    // Görsel güncellemeyi zorla
                    dgvNotlar.Refresh();
                    dgvNotlar.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Notlar yüklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private double? HesaplaOrtalama(int? vize, int? final, int? but)
        {
            if (!vize.HasValue || (!final.HasValue && !but.HasValue)) return null;
            var etkiliNot = but.HasValue ? but.Value : final.Value;
            return vize.Value * 0.3 + etkiliNot * 0.7;
        }
        private string HesaplaHarfNotu(double ortalama)
        {
            if (ortalama >= 90) return "AA";
            if (ortalama >= 80) return "BA";
            if (ortalama >= 75) return "BB";
            if (ortalama >= 70) return "CB";
            if (ortalama >= 60) return "CC";
            if (ortalama >= 50) return "DC";
            if (ortalama >= 40) return "DD";
            if (ortalama >= 30) return "FD";
            return "FF";
        }
        private void btnCikis_Click(object sender, EventArgs e)
        {
            if (Program.MainGirisForm != null)
            {
                Program.MainGirisForm.Show();
                this.Hide();
            }
        }

        private void OgrenciForm_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (Program.MainGirisForm != null)
                {
                    Program.MainGirisForm.Show();
                }
            }
            else if (Program.MainGirisForm != null && !Program.MainGirisForm.Visible)
            {
                Program.MainGirisForm.Show();
            }
        }

        private void btnSifreDegistir_Click(object sender, EventArgs e)
        {
            using (var sifreForm = new Form())
            {
                sifreForm.Text = "Şifre Değiştir";
                sifreForm.Size = new Size(300, 200);
                sifreForm.StartPosition = FormStartPosition.CenterParent;
                sifreForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                sifreForm.MaximizeBox = false;
                sifreForm.MinimizeBox = false;

                Label lblMevcutSifre = new Label { Text = "Mevcut Şifre:", Location = new Point(20, 20), Size = new Size(100, 20) };
                TextBox txtMevcutSifre = new TextBox { Location = new Point(120, 20), Size = new Size(150, 20), UseSystemPasswordChar = true };

                Label lblYeniSifre = new Label { Text = "Yeni Şifre:", Location = new Point(20, 50), Size = new Size(100, 20) };
                TextBox txtYeniSifre = new TextBox { Location = new Point(120, 50), Size = new Size(150, 20), UseSystemPasswordChar = true };

                Label lblYeniSifreOnay = new Label { Text = "Yeni Şifre (Onay):", Location = new Point(20, 80), Size = new Size(100, 20) };
                TextBox txtYeniSifreOnay = new TextBox { Location = new Point(120, 80), Size = new Size(150, 20), UseSystemPasswordChar = true };

                Button btnKaydet = new Button { Text = "Kaydet", Location = new Point(120, 110), Size = new Size(75, 30) };
                Button btnIptal = new Button { Text = "İptal", Location = new Point(200, 110), Size = new Size(75, 30) };

                sifreForm.Controls.AddRange(new Control[] { lblMevcutSifre, txtMevcutSifre, lblYeniSifre, txtYeniSifre, lblYeniSifreOnay, txtYeniSifreOnay, btnKaydet, btnIptal });

                btnKaydet.Click += (s, args) =>
                {
                    try
                    {
                        string mevcutSifre = txtMevcutSifre.Text.Trim();
                        string yeniSifre = txtYeniSifre.Text.Trim();
                        string yeniSifreOnay = txtYeniSifreOnay.Text.Trim();

                        if (string.IsNullOrEmpty(mevcutSifre) || string.IsNullOrEmpty(yeniSifre) || string.IsNullOrEmpty(yeniSifreOnay))
                        {
                            MessageBox.Show("Tüm alanları doldurun!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (yeniSifre != yeniSifreOnay)
                        {
                            MessageBox.Show("Yeni şifreler eşleşmiyor!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (yeniSifre.Length < 5)
                        {
                            MessageBox.Show("Yeni şifre en az 5 karakter olmalı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        using (var context = new OBSModel())
                        {
                            var kullanici = context.Kullanicilar
                                .FirstOrDefault(k => k.KullaniciAdi == _kullaniciAdi && k.Sifre == mevcutSifre);
                            if (kullanici == null)
                            {
                                MessageBox.Show("Mevcut şifre yanlış!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            kullanici.Sifre = yeniSifre;

                            var denetim = new DenetimKayitlari
                            {
                                KullaniciNo = kullanici.KullaniciNo,
                                Eylem = $"Şifre değiştirildi: {_kullaniciAdi}",
                                ZamanDamgasi = DateTime.Now
                            };
                            context.DenetimKayitlari.Add(denetim);

                            context.SaveChanges();
                            MessageBox.Show("Şifreniz başarıyla değiştirildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            sifreForm.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                btnIptal.Click += (s, args) => sifreForm.Close();
                sifreForm.ShowDialog();
            }
        }

        private void OgrenciForm_Load(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void dgvNotlar_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvNotlar.Columns[e.ColumnIndex].Name == "Durum" && e.Value != null)
            {
                string durum = e.Value.ToString();
                if (durum == "Başarılı")
                {
                    e.CellStyle.BackColor = Color.LightGreen;
                }
                else if (durum == "Başarısız")
                {
                    e.CellStyle.BackColor = Color.LightCoral; // Kırmızı yerine LightCoral, daha yumuşak
                }
                else
                {
                    e.CellStyle.BackColor = Color.White; // Varsayılan (örn: "-")
                }
            }
        }
    }
}
