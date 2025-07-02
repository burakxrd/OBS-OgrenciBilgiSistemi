using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OBS;

namespace OBS
{
    public partial class Form1 : Form
    {
        private readonly int _kullaniciNo;
        public Form1(int kullaniciNo)
        {
            InitializeComponent();
            _kullaniciNo = kullaniciNo;
            KontrolEtYoneticiRol();
        }
        private void KontrolEtYoneticiRol()
        {
            try
            {
                using (var context = new OBSModel())
                {
                    var kullanici = context.Kullanicilar
                        .FirstOrDefault(k => k.KullaniciNo == _kullaniciNo);
                    if (kullanici == null || kullanici.Rol != "Yonetici")
                    {
                        MessageBox.Show("Bu ekrana sadece yöneticiler erişebilir.", "Yetki Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Yetki kontrolü sırasında hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                using (var context = new OBSModel())
                {
                    // Bölümleri yükle
                    var bolumler = context.Bolumler.ToList();
                    cmbBolum.DataSource = bolumler;
                    cmbBolum.DisplayMember = "BolumAdi";
                    cmbBolum.ValueMember = "BolumNo";
                    cmbBolum.SelectedIndex = -1;
                    // Sınıf ve cinsiyet seçeneklerini yükle
                    cmbSinif.Items.AddRange(new[] { "1. Sınıf", "2. Sınıf", "3. Sınıf", "4. Sınıf", "Hazırlık" });
                    cmbCinsiyet.Items.AddRange(new[] { "Erkek", "Kadın" });
                    // Bölüm adlarının uzunluğuna göre DropDownWidth ayarla
                    if (bolumler.Any())
                    {
                        using (Graphics g = cmbBolum.CreateGraphics())
                        {
                            float maxWidth = bolumler.Max(b => g.MeasureString(b.BolumAdi, cmbBolum.Font).Width);
                            cmbBolum.DropDownWidth = Math.Max(cmbBolum.Width, (int)maxWidth + 20); // 20px boşluk
                        }
                    }
                    // DataGridView’i yükle
                    YukleDataGridView();
                    // Seçimi kaldır ve formu temizle
                    dataGridView1.ClearSelection();
                    FormuTemizle();
                    //DateTimeNow kısmı buraya eklendi
                    dateTimePicker1.Value = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Form yüklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void YukleDataGridView(string aramaMetni = "")
        {

            try
            {
                using (var context = new OBSModel())
                {
                    var query = context.Ogrenciler
                        .Include("Bolumler")
                        .Select(o => new
                        {
                            OgrenciNo = o.OgrenciNo,
                            o.Ad,
                            o.Soyad,
                            BolumAdi = o.Bolumler != null ? o.Bolumler.BolumAdi : null,
                            o.Sinif,
                            o.DogumTarihi,
                            Cinsiyet = o.Cinsiyet == "E" ? "Erkek" : "Kadın",
                            o.TCKimlikNo
                        });

                    if (!string.IsNullOrWhiteSpace(aramaMetni))
                    {
                        query = query.Where(o => o.Ad.Contains(aramaMetni) || o.Soyad.Contains(aramaMetni));
                    }

                    dataGridView1.DataSource = query.ToList();
                    // Sütun modunu Fill yap
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    // Satır yüksekliğini artır (Ad sütununda ikinci satır için)
                    dataGridView1.RowTemplate.Height = 40; // 2 satırlık yükseklik

                    // Sütun başlıklarını Türkçe yap
                    if (dataGridView1.Columns["OgrenciNo"] != null)
                    {
                        dataGridView1.Columns["OgrenciNo"].HeaderText = "Öğrenci No";
                        dataGridView1.Columns["OgrenciNo"].MinimumWidth = 110;
                        dataGridView1.Columns["OgrenciNo"].DefaultCellStyle.WrapMode = DataGridViewTriState.False;
                    }
                    if (dataGridView1.Columns["Ad"] != null)
                    {
                        dataGridView1.Columns["Ad"].HeaderText = "Ad";
                        dataGridView1.Columns["Ad"].MinimumWidth = 120;
                        dataGridView1.Columns["Ad"].DefaultCellStyle.WrapMode = DataGridViewTriState.True; // Uzun isimler sarılacak
                    }
                    if (dataGridView1.Columns["Soyad"] != null)
                    {
                        dataGridView1.Columns["Soyad"].HeaderText = "Soyad";
                        dataGridView1.Columns["Soyad"].MinimumWidth = 120;
                        dataGridView1.Columns["Soyad"].DefaultCellStyle.WrapMode = DataGridViewTriState.False;
                    }
                    if (dataGridView1.Columns["BolumAdi"] != null)
                    {
                        dataGridView1.Columns["BolumAdi"].HeaderText = "Bölüm Adı";
                        dataGridView1.Columns["BolumAdi"].MinimumWidth = 150;
                        dataGridView1.Columns["BolumAdi"].FillWeight = 200;
                        dataGridView1.Columns["BolumAdi"].DefaultCellStyle.WrapMode = DataGridViewTriState.False;
                    }
                    if (dataGridView1.Columns["TCKimlikNo"] != null)
                    {
                        dataGridView1.Columns["TCKimlikNo"].HeaderText = "TC Kimlik No";
                        dataGridView1.Columns["TCKimlikNo"].MinimumWidth = 120; // TC Kimlik No görünmesi için
                        dataGridView1.Columns["TCKimlikNo"].DefaultCellStyle.WrapMode = DataGridViewTriState.False;
                    }
                    if (dataGridView1.Columns["DogumTarihi"] != null)
                    {
                        dataGridView1.Columns["DogumTarihi"].HeaderText = "Doğum Tarihi";
                        dataGridView1.Columns["DogumTarihi"].MinimumWidth = 110;
                        dataGridView1.Columns["DogumTarihi"].DefaultCellStyle.WrapMode = DataGridViewTriState.False;
                    }
                    if (dataGridView1.Columns["Cinsiyet"] != null)
                    {
                        dataGridView1.Columns["Cinsiyet"].HeaderText = "Cinsiyet";
                        dataGridView1.Columns["Cinsiyet"].MinimumWidth = 60;
                        dataGridView1.Columns["Cinsiyet"].DefaultCellStyle.WrapMode = DataGridViewTriState.False;
                    }
                    if (dataGridView1.Columns["Sinif"] != null)
                    {
                        dataGridView1.Columns["Sinif"].HeaderText = "Sınıf";
                        dataGridView1.Columns["Sinif"].MinimumWidth = 50;
                        dataGridView1.Columns["Sinif"].DefaultCellStyle.WrapMode = DataGridViewTriState.False;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"DataGridView yüklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;
            string newValue = e.FormattedValue?.ToString()?.Trim();

            // Boş değer kontrolü
            if (string.IsNullOrWhiteSpace(newValue) && columnName != "DogumTarihi")
            {
                dataGridView1.Rows[e.RowIndex].ErrorText = "Bu alan boş olamaz.";
                e.Cancel = true;
                return;
            }

            try
            {
                switch (columnName)
                {
                    case "OgrenciNo":
                        if (!int.TryParse(newValue, out int ogrenciNo) || ogrenciNo <= 0)
                        {
                            dataGridView1.Rows[e.RowIndex].ErrorText = "Öğrenci No pozitif bir sayı olmalı.";
                            e.Cancel = true;
                        }
                        break;

                    case "Ad":
                    case "Soyad":
                        if (newValue.Length > 50)
                        {
                            dataGridView1.Rows[e.RowIndex].ErrorText = $"{columnName} 50 karakterden uzun olamaz.";
                            e.Cancel = true;
                        }
                        break;

                    case "TCKimlikNo":
                        if (newValue.Length != 11 || !newValue.All(char.IsDigit))
                        {
                            dataGridView1.Rows[e.RowIndex].ErrorText = "TC Kimlik No 11 haneli bir sayı olmalı.";
                            e.Cancel = true;
                        }
                        break;

                    case "Cinsiyet":
                        if (newValue != "Erkek" && newValue != "Kadın")
                        {
                            dataGridView1.Rows[e.RowIndex].ErrorText = "Cinsiyet 'Erkek' veya 'Kadın' olmalı.";
                            e.Cancel = true;
                        }
                        break;

                    case "Sinif":
                        if (!new[] { "1. Sınıf", "2. Sınıf", "3. Sınıf", "4. Sınıf", "Hazırlık" }.Contains(newValue))
                        {
                            dataGridView1.Rows[e.RowIndex].ErrorText = "Geçerli bir sınıf seçin.";
                            e.Cancel = true;
                        }
                        break;

                    case "BolumAdi":
                        using (var context = new OBSModel())
                        {
                            if (!context.Bolumler.Any(b => b.BolumAdi == newValue))
                            {
                                dataGridView1.Rows[e.RowIndex].ErrorText = "Geçerli bir bölüm adı girin.";
                                e.Cancel = true;
                            }
                        }
                        break;

                    case "DogumTarihi":
                        if (!string.IsNullOrWhiteSpace(newValue) && !DateTime.TryParse(newValue, out _))
                        {
                            dataGridView1.Rows[e.RowIndex].ErrorText = "Geçerli bir tarih girin.";
                            e.Cancel = true;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                dataGridView1.Rows[e.RowIndex].ErrorText = $"Doğrulama hatası: {ex.Message}";
                e.Cancel = true;
            }

            if (!e.Cancel)
            {
                dataGridView1.Rows[e.RowIndex].ErrorText = string.Empty;
            }
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0) return;

                string columnName = dataGridView1.Columns[e.ColumnIndex].Name;
                string newValue = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString()?.Trim();

                switch (columnName)
                {
                    case "OgrenciNo":
                        txtOgrenciNo.Text = newValue;
                        break;

                    case "Ad":
                        txtAd.Text = newValue;
                        break;

                    case "Soyad":
                        txtSoyad.Text = newValue;
                        break;

                    case "TCKimlikNo":
                        txtTCKimlikNo.Text = newValue;
                        break;

                    case "Cinsiyet":
                        if (newValue == "Erkek" || newValue == "Kadın")
                        {
                            cmbCinsiyet.Text = newValue;
                        }
                        else
                        {
                            cmbCinsiyet.SelectedIndex = -1; // Geçersizse sıfırla
                        }
                        break;

                    case "Sinif":
                        cmbSinif.Text = newValue;
                        break;

                    case "DogumTarihi":
                        if (DateTime.TryParse(newValue, out DateTime dogumTarihi))
                        {
                            dateTimePicker1.Value = dogumTarihi;
                        }
                        else
                        {
                            dateTimePicker1.Value = DateTime.Now;
                        }
                        break;

                    case "BolumAdi":
                        using (var context = new OBSModel())
                        {
                            var bolum = context.Bolumler.FirstOrDefault(b => b.BolumAdi == newValue);
                            if (bolum != null)
                            {
                                cmbBolum.SelectedValue = bolum.BolumNo;
                            }
                            else
                            {
                                cmbBolum.SelectedIndex = -1;
                            }
                        }
                        break;
                }

                // Güncelleme için formu hazırla
                dataGridView1.ClearSelection();
                dataGridView1.Rows[e.RowIndex].Selected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hücre düzenlenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtAra_TextChanged(object sender, EventArgs e)
        {
            YukleDataGridView(txtAra.Text.Trim());
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOgrenciNo.Text) || string.IsNullOrWhiteSpace(txtAd.Text) ||
                string.IsNullOrWhiteSpace(txtSoyad.Text) || cmbBolum.SelectedValue == null ||
                string.IsNullOrWhiteSpace(cmbSinif.Text) || string.IsNullOrWhiteSpace(cmbCinsiyet.Text) ||
                string.IsNullOrWhiteSpace(txtTCKimlikNo.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(txtOgrenciNo.Text.Trim(), out int ogrenciNo) || ogrenciNo <= 0)
            {
                MessageBox.Show("Öğrenci No geçerli bir pozitif sayı olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtAd.Text.Trim().Length > 50 || txtSoyad.Text.Trim().Length > 50)
            {
                MessageBox.Show("Ad ve Soyad 50 karakterden uzun olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    // Öğrenci numarasının zaten var olup olmadığını kontrol et
                    if (context.Ogrenciler.Any(o => o.OgrenciNo == ogrenciNo))
                    {
                        MessageBox.Show($"Öğrenci No {ogrenciNo} zaten mevcut. Lütfen farklı bir numara girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Kullanıcı adının (öğrenci no) zaten var olup olmadığını kontrol et
                    if (context.Kullanicilar.Any(k => k.KullaniciAdi == ogrenciNo.ToString()))
                    {
                        MessageBox.Show($"Öğrenci No {ogrenciNo} ile kayıtlı bir kullanıcı zaten mevcut.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // TC Kimlik No’nun zaten var olup olmadığını kontrol et
                    if (context.Kullanicilar.Any(k => k.TCKimlikNo == txtTCKimlikNo.Text.Trim()) ||
                        context.Ogrenciler.Any(o => o.TCKimlikNo == txtTCKimlikNo.Text.Trim()))
                    {
                        MessageBox.Show($"Bu TC Kimlik No zaten kayıtlı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int? bolumNo = null;
                    if (cmbBolum.SelectedValue != null)
                    {
                        if (!int.TryParse(cmbBolum.SelectedValue.ToString(), out int selectedBolumNo))
                        {
                            MessageBox.Show("Seçilen bölüm geçersiz. Lütfen bir bölüm seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (!context.Bolumler.Any(b => b.BolumNo == selectedBolumNo))
                        {
                            MessageBox.Show($"Seçilen bölüm (BolumNo: {selectedBolumNo}) veritabanında bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        bolumNo = selectedBolumNo;
                    }

                    string cinsiyet = cmbCinsiyet.Text[0].ToString();
                    if (cinsiyet != "E" && cinsiyet != "K")
                    {
                        MessageBox.Show("Cinsiyet 'Erkek' veya 'Kadın' olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Yeni öğrenci ekle
                    var ogrenci = new Ogrenciler
                    {
                        OgrenciNo = ogrenciNo,
                        Ad = txtAd.Text.Trim(),
                        Soyad = txtSoyad.Text.Trim(),
                        BolumNo = bolumNo,
                        Sinif = cmbSinif.Text.Length > 10 ? cmbSinif.Text.Substring(0, 10) : cmbSinif.Text,
                        DogumTarihi = dateTimePicker1.Value != DateTime.MinValue ? (DateTime?)dateTimePicker1.Value : null,
                        Cinsiyet = cinsiyet,
                        TCKimlikNo = txtTCKimlikNo.Text.Trim()
                    };
                    context.Ogrenciler.Add(ogrenci);

                    // Yeni kullanıcı ekle
                    var yeniKullanici = new Kullanicilar
                    {
                        KullaniciAdi = ogrenciNo.ToString(),
                        Sifre = "12345",
                        Rol = "Ogrenci",
                        TCKimlikNo = txtTCKimlikNo.Text.Trim()
                    };
                    context.Kullanicilar.Add(yeniKullanici);

                    // Denetim kaydı ekle
                    var denetim = new DenetimKayitlari
                    {
                        KullaniciNo = _kullaniciNo,
                        Eylem = $"Öğrenci eklendi: {ogrenci.Ad} {ogrenci.Soyad} (OgrenciNo: {ogrenciNo})",
                        ZamanDamgasi = DateTime.Now
                    };
                    context.DenetimKayitlari.Add(denetim);

                    // Değişiklikleri kaydet
                    context.SaveChanges();

                    // DataGridView’i güncelle ve formu temizle
                    YukleDataGridView();
                    FormuTemizle();
                    MessageBox.Show($"Öğrenci ve kullanıcı eklendi (OgrenciNo: {ogrenciNo}).", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => $"Property: {x.PropertyName}, Error: {x.ErrorMessage}");
                var fullErrorMessage = string.Join("; ", errorMessages);
                MessageBox.Show($"Doğrulama hatası: {fullErrorMessage}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += $"\nInner Exception: {ex.InnerException.Message}";
                    if (ex.InnerException.InnerException != null)
                        errorMessage += $"\nInner Inner Exception: {ex.InnerException.InnerException.Message}";
                }
                MessageBox.Show($"Öğrenci veya kullanıcı eklenirken hata: {errorMessage}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen bir öğrenci seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtOgrenciNo.Text) || string.IsNullOrWhiteSpace(txtAd.Text) ||
                string.IsNullOrWhiteSpace(txtSoyad.Text) || cmbBolum.SelectedValue == null ||
                string.IsNullOrWhiteSpace(cmbSinif.Text) || string.IsNullOrWhiteSpace(cmbCinsiyet.Text) ||
                string.IsNullOrWhiteSpace(txtTCKimlikNo.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(txtOgrenciNo.Text.Trim(), out int yeniOgrenciNo) || yeniOgrenciNo <= 0)
            {
                MessageBox.Show("Öğrenci No geçerli bir pozitif sayı olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtAd.Text.Trim().Length > 50 || txtSoyad.Text.Trim().Length > 50)
            {
                MessageBox.Show("Ad ve Soyad 50 karakterden uzun olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    // Mevcut öğrenci numarasını al
                    int mevcutOgrenciNo = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                    var ogrenci = context.Ogrenciler.FirstOrDefault(o => o.OgrenciNo == mevcutOgrenciNo);
                    if (ogrenci == null)
                    {
                        MessageBox.Show($"Öğrenci (OgrenciNo: {mevcutOgrenciNo}) bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Yeni öğrenci numarasının benzersizliğini kontrol et
                    if (yeniOgrenciNo != mevcutOgrenciNo && context.Ogrenciler.Any(o => o.OgrenciNo == yeniOgrenciNo))
                    {
                        MessageBox.Show($"Öğrenci No {yeniOgrenciNo} zaten mevcut. Lütfen farklı bir numara girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Yeni TC Kimlik No’nun benzersizliğini kontrol et
                    if (ogrenci.TCKimlikNo != txtTCKimlikNo.Text.Trim() &&
                        (context.Ogrenciler.Any(o => o.TCKimlikNo == txtTCKimlikNo.Text.Trim() && o.OgrenciNo != mevcutOgrenciNo) ||
                         context.Kullanicilar.Any(k => k.TCKimlikNo == txtTCKimlikNo.Text.Trim() && k.KullaniciAdi != mevcutOgrenciNo.ToString())))
                    {
                        MessageBox.Show($"Bu TC Kimlik No zaten kayıtlı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Bölüm numarasını doğrula
                    int? bolumNo = null;
                    if (cmbBolum.SelectedValue != null)
                    {
                        if (!int.TryParse(cmbBolum.SelectedValue.ToString(), out int selectedBolumNo))
                        {
                            MessageBox.Show("Seçilen bölüm geçersiz. Lütfen bir bölüm seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (!context.Bolumler.Any(b => b.BolumNo == selectedBolumNo))
                        {
                            MessageBox.Show($"Seçilen bölüm (BolumNo: {selectedBolumNo}) veritabanında bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        bolumNo = selectedBolumNo;
                    }

                    // Cinsiyet doğrulaması
                    string cinsiyet = cmbCinsiyet.Text[0].ToString();
                    if (cinsiyet != "E" && cinsiyet != "K")
                    {
                        MessageBox.Show("Cinsiyet 'Erkek' veya 'Kadın' olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Değişiklikleri kontrol et
                    bool degisiklikVar = false;
                    if (ogrenci.OgrenciNo != yeniOgrenciNo ||
                        ogrenci.Ad != txtAd.Text.Trim() ||
                        ogrenci.Soyad != txtSoyad.Text.Trim() ||
                        ogrenci.BolumNo != bolumNo ||
                        ogrenci.Sinif != (cmbSinif.Text.Length > 10 ? cmbSinif.Text.Substring(0, 10) : cmbSinif.Text) ||
                        ogrenci.DogumTarihi != dateTimePicker1.Value ||
                        ogrenci.Cinsiyet != cinsiyet ||
                        ogrenci.TCKimlikNo != txtTCKimlikNo.Text.Trim())
                    {
                        degisiklikVar = true;
                    }

                    // Kullanıcı değişikliklerini kontrol et
                    var kullanici = context.Kullanicilar.FirstOrDefault(k => k.KullaniciAdi == mevcutOgrenciNo.ToString());
                    bool kullaniciDegisiklikVar = false;
                    if (kullanici != null && (kullanici.KullaniciAdi != yeniOgrenciNo.ToString() || kullanici.TCKimlikNo != txtTCKimlikNo.Text.Trim()))
                    {
                        kullaniciDegisiklikVar = true;
                    }

                    // Eğer hiçbir değişiklik yoksa, mesaj göster ve çık
                    if (!degisiklikVar && !kullaniciDegisiklikVar)
                    {
                        MessageBox.Show("Hiçbir değişiklik yapılmadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Eski öğrenci kaydını sil
                    if (degisiklikVar)
                    {
                        context.Ogrenciler.Remove(ogrenci);
                    }

                    // Yeni öğrenci kaydını ekle
                    var yeniOgrenci = new Ogrenciler
                    {
                        OgrenciNo = yeniOgrenciNo,
                        Ad = txtAd.Text.Trim(),
                        Soyad = txtSoyad.Text.Trim(),
                        BolumNo = bolumNo,
                        Sinif = cmbSinif.Text.Length > 10 ? cmbSinif.Text.Substring(0, 10) : cmbSinif.Text,
                        DogumTarihi = dateTimePicker1.Value,
                        Cinsiyet = cinsiyet,
                        TCKimlikNo = txtTCKimlikNo.Text.Trim()
                    };
                    context.Ogrenciler.Add(yeniOgrenci);

                    // Kullanıcıyı güncelle
                    if (kullaniciDegisiklikVar && kullanici != null)
                    {
                        // Eski kullanıcı kaydını sil
                        context.Kullanicilar.Remove(kullanici);

                        // Yeni kullanıcı kaydını ekle
                        var yeniKullanici = new Kullanicilar
                        {
                            KullaniciAdi = yeniOgrenciNo.ToString(),
                            Sifre = kullanici.Sifre, // Mevcut şifreyi koru
                            Rol = "Ogrenci",
                            TCKimlikNo = txtTCKimlikNo.Text.Trim()
                        };
                        context.Kullanicilar.Add(yeniKullanici);
                    }
                    else if (kullanici == null && yeniOgrenciNo != mevcutOgrenciNo)
                    {
                        // Eğer kullanıcı yoksa ve OgrenciNo değiştiyse, yeni kullanıcı ekle
                        var yeniKullanici = new Kullanicilar
                        {
                            KullaniciAdi = yeniOgrenciNo.ToString(),
                            Sifre = "12345", // Varsayılan şifre
                            Rol = "Ogrenci",
                            TCKimlikNo = txtTCKimlikNo.Text.Trim()
                        };
                        context.Kullanicilar.Add(yeniKullanici);
                    }

                    // Denetim kaydı ekle
                    var denetim = new DenetimKayitlari
                    {
                        KullaniciNo = _kullaniciNo,
                        Eylem = $"Öğrenci güncellendi: {yeniOgrenci.Ad} {yeniOgrenci.Soyad} (OgrenciNo: {yeniOgrenciNo})",
                        ZamanDamgasi = DateTime.Now
                    };
                    context.DenetimKayitlari.Add(denetim);

                    // Değişiklikleri kaydet
                    context.SaveChanges();

                    // DataGridView’i güncelle ve formu temizle
                    YukleDataGridView();
                    FormuTemizle();
                    MessageBox.Show($"Öğrenci ve kullanıcı güncellendi (OgrenciNo: {yeniOgrenciNo}).", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => $"Property: {x.PropertyName}, Error: {x.ErrorMessage}");
                var fullErrorMessage = string.Join("; ", errorMessages);
                MessageBox.Show($"Doğrulama hatası: {fullErrorMessage}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += $"\nInner Exception: {ex.InnerException.Message}";
                    if (ex.InnerException.InnerException != null)
                        errorMessage += $"\nInner Inner Exception: {ex.InnerException.InnerException.Message}";
                }
                MessageBox.Show($"Öğrenci veya kullanıcı güncellenirken hata: {errorMessage}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Lütfen silmek için bir öğrenci seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int ogrenciNo = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["OgrenciNo"].Value);
                string ogrenciAdi = dataGridView1.SelectedRows[0].Cells["Ad"].Value.ToString();
                string ogrenciSoyadi = dataGridView1.SelectedRows[0].Cells["Soyad"].Value.ToString();

                if (MessageBox.Show($"Öğrenci {ogrenciAdi} {ogrenciSoyadi} (OgrenciNo: {ogrenciNo}) silinsin mi?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                using (var context = new OBSModel())
                {
                    var ogrenci = context.Ogrenciler.FirstOrDefault(o => o.OgrenciNo == ogrenciNo);
                    if (ogrenci == null)
                    {
                        MessageBox.Show($"Öğrenci (OgrenciNo: {ogrenciNo}) bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var kullanici = context.Kullanicilar.FirstOrDefault(k => k.KullaniciAdi == ogrenciNo.ToString());
                    if (kullanici != null)
                    {
                        context.Kullanicilar.Remove(kullanici);
                    }

                    context.Ogrenciler.Remove(ogrenci);

                    // Denetim kaydı ekle
                    var denetim = new DenetimKayitlari
                    {
                        KullaniciNo = _kullaniciNo,
                        Eylem = $"Öğrenci silindi: {ogrenciAdi} {ogrenciSoyadi} (OgrenciNo: {ogrenciNo})",
                        ZamanDamgasi = DateTime.Now
                    };
                    context.DenetimKayitlari.Add(denetim);

                    // Değişiklikleri kaydet
                    context.SaveChanges();

                    // DataGridView’i güncelle ve formu temizle
                    YukleDataGridView();
                    FormuTemizle();
                    MessageBox.Show($"Öğrenci ve kullanıcı silindi (OgrenciNo: {ogrenciNo}).", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Silme sırasında hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    using (var context = new OBSModel())
                    {
                        int ogrenciNo = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                        var ogrenci = context.Ogrenciler
                            .Include("Bolumler")
                            .FirstOrDefault(o => o.OgrenciNo == ogrenciNo);
                        if (ogrenci != null)
                        {
                            txtOgrenciNo.Text = ogrenci.OgrenciNo.ToString();
                            txtAd.Text = ogrenci.Ad;
                            txtSoyad.Text = ogrenci.Soyad;
                            cmbBolum.SelectedValue = ogrenci.BolumNo ?? -1;
                            cmbSinif.Text = ogrenci.Sinif;
                            dateTimePicker1.Value = ogrenci.DogumTarihi ?? DateTime.Now;
                            cmbCinsiyet.Text = ogrenci.Cinsiyet == "E" ? "Erkek" : "Kadın";
                            txtTCKimlikNo.Text = ogrenci.TCKimlikNo;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Öğrenci bilgileri yüklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FormuTemizle()
        {
            txtOgrenciNo.Clear();
            txtAd.Clear();
            txtSoyad.Clear();
            txtTCKimlikNo.Clear();
            txtAra.Clear();
            cmbBolum.SelectedIndex = -1;
            cmbSinif.SelectedIndex = -1;
            cmbCinsiyet.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Now;
            dataGridView1.ClearSelection();
        }

        private void txtTCKimlikNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Sadece sayılara izin ver
            }
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            Program.MainGirisForm.Show();
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && Program.MainGirisForm != null)
            {
                Program.MainGirisForm.Show();
            }
        }

        private void txtOgrenciNo_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Sadece sayılara izin ver
            }
        }

        private void btnNotGir_Click(object sender, EventArgs e)
        {
            try
            {
                using (var notGirForm = new NotGirForm())
                {
                    notGirForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Not giriş formu açılırken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

