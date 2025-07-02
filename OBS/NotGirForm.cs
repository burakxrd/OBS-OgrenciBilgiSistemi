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
    public partial class NotGirForm : Form
    {
        private ToolTip validationToolTip = new ToolTip();
        private System.Timers.Timer resetColorTimer;
        public NotGirForm()
        {
            InitializeComponent(); 
            YukleBolumler();
            // Renk sıfırlama zamanlayıcısı
            resetColorTimer = new System.Timers.Timer(2000);
            resetColorTimer.Elapsed += (s, e) => BeginInvoke(new Action(ResetCellColors));
            resetColorTimer.AutoReset = false;
        }

        public class NotViewModel
        {
            public string DersKodu { get; set; }
            public string DersAdi { get; set; }
            public int? Vize { get; set; }
            public int? Final { get; set; }
            public int? But { get; set; }
        }
        public class OgrenciViewModel
        {
            public int? OgrenciNo { get; set; }
            public string AdSoyad { get; set; }
            public override string ToString() => AdSoyad;
        }

        public class SinifViewModel
        {
            public string Sinif { get; set; }
            public override string ToString() => Sinif;
        }
        public class BolumViewModel
        {
            public int? BolumNo { get; set; }
            public string BolumAdi { get; set; }
            public override string ToString() => BolumAdi;
        }


        private void YukleBolumler()
        {
            try
            {
                using (var context = new OBSModel())
                {
                    var bolumler = context.Bolumler
                        .Select(b => new BolumViewModel { BolumNo = b.BolumNo, BolumAdi = b.BolumAdi })
                        .OrderBy(b => b.BolumAdi)
                        .ToList();
                    var bolumList = new List<BolumViewModel> { new BolumViewModel { BolumNo = null, BolumAdi = "Seç" } };
                    bolumList.AddRange(bolumler);
                    cmbBolum.SelectedIndexChanged -= cmbBolum_SelectedIndexChanged_1;
                    cmbBolum.DataSource = bolumList;
                    cmbBolum.DisplayMember = "BolumAdi";
                    cmbBolum.ValueMember = "BolumNo";
                    cmbBolum.SelectedIndex = -1;
                    cmbBolum.Text = "Seç";
                    int maxWidth = 0;
                    using (Graphics g = cmbBolum.CreateGraphics())
                    {
                        foreach (var item in bolumList)
                        {
                            int width = (int)g.MeasureString(item.BolumAdi, cmbBolum.Font).Width;
                            maxWidth = Math.Max(maxWidth, width);
                        }
                    }
                    cmbBolum.DropDownWidth = Math.Max(maxWidth + 20, cmbBolum.Width);
                    cmbBolum.SelectedIndexChanged += cmbBolum_SelectedIndexChanged_1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bölümler yüklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (cmbOgrenci.SelectedValue == null || cmbOgrenci.SelectedIndex == 0)
            {
                MessageBox.Show("Lütfen bir öğrenci seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var context = new OBSModel())
                {
                    int ogrenciNo = (int)cmbOgrenci.SelectedValue;
                    bool anyChanges = false;

                    // YENİ: Değişen derslerin listesini tut (denetim kaydı için)
                    List<string> degisenDersler = new List<string>();

                    foreach (DataGridViewRow row in dgvNotlar.Rows)
                    {
                        if (row.IsNewRow || row.Cells["DersKodu"].Value == null) continue;

                        string dersKodu = row.Cells["DersKodu"].Value.ToString();

                        // YENİ: Güvenli int? dönüşümü (C# 7.3 uyumlu)
                        int? GetNullableInt(object cellValue)
                        {
                            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                                return null;
                            if (int.TryParse(cellValue.ToString(), out int result) && result >= 0 && result <= 100)
                                return result;
                            return null;
                        }

                        int? vize = GetNullableInt(row.Cells["Vize"].Value);
                        int? final = GetNullableInt(row.Cells["Final"].Value);
                        int? but = GetNullableInt(row.Cells["But"].Value);

                        var mevcutNot = context.Notlar
                            .FirstOrDefault(n => n.OgrenciNo == ogrenciNo && n.DersKodu == dersKodu);

                        if (mevcutNot == null && (vize.HasValue || final.HasValue || but.HasValue))
                        {
                            context.Notlar.Add(new Notlar
                            {
                                OgrenciNo = ogrenciNo,
                                DersKodu = dersKodu,
                                Vize = vize,
                                Final = final,
                                But = but
                            });
                            anyChanges = true;
                            degisenDersler.Add(dersKodu); // YENİ: Yeni eklenen dersi listeye ekle
                            row.Cells["Vize"].Style.BackColor = Color.LightGreen;
                            row.Cells["Final"].Style.BackColor = Color.LightGreen;
                            row.Cells["But"].Style.BackColor = Color.LightGreen;
                        }
                        else if (mevcutNot != null)
                        {
                            bool changed = false;
                            // DEĞİŞTİ: Değişiklik kontrolü null ve değer eşitsizliğine göre
                            if (!Equals(mevcutNot.Vize, vize)) { mevcutNot.Vize = vize; changed = true; }
                            if (!Equals(mevcutNot.Final, final)) { mevcutNot.Final = final; changed = true; }
                            if (!Equals(mevcutNot.But, but)) { mevcutNot.But = but; changed = true; }

                            if (changed)
                            {
                                anyChanges = true;
                                degisenDersler.Add(dersKodu); // Güncellenen dersi listeye ekle
                                row.Cells["Vize"].Style.BackColor = Color.LightGreen;
                                row.Cells["Final"].Style.BackColor = Color.LightGreen;
                                row.Cells["But"].Style.BackColor = Color.LightGreen;
                            }
                        }
                    }

                    if (anyChanges)
                    {
                        context.SaveChanges();

                        // YENİ: Denetim kaydı ekle
                        var adminKullanici = context.Kullanicilar
                            .FirstOrDefault(k => k.KullaniciAdi == "admin");
                        if (adminKullanici != null)
                        {
                            string eylem = $"Not güncellendi: OgrenciNo={ogrenciNo}, Dersler={string.Join(", ", degisenDersler)}";
                            var denetim = new DenetimKayitlari
                            {
                                KullaniciNo = adminKullanici.KullaniciNo,
                                Eylem = eylem,
                                ZamanDamgasi = DateTime.Now
                            };
                            context.DenetimKayitlari.Add(denetim);
                            context.SaveChanges(); // Denetim kaydını kaydet
                        }
                        else
                        {
                            MessageBox.Show("Admin kullanıcısı bulunamadı, denetim kaydı eklenemedi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        MessageBox.Show("Notlar başarıyla kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        resetColorTimer.Start();
                        YukleNotlar(ogrenciNo);
                    }
                    else
                    {
                        //  Daha açıklayıcı mesaj
                        MessageBox.Show("Hiçbir not değiştirilmedi veya zaten kaydedilmiş.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Notlar kaydedilirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Öğrencinin notlarını yeniden yükler
        private void YukleNotlar(int ogrenciNo)
        {
            try
            {
                using (var context = new OBSModel())
                {
                    var ogrenci = context.Ogrenciler
                        .FirstOrDefault(o => o.OgrenciNo == ogrenciNo);
                    if (ogrenci == null) return;

                    var query = from d in context.Dersler
                                where d.BolumNo == ogrenci.BolumNo || d.BolumNo == null
                                join n in context.Notlar
                                on new { DersKodu = d.DersKodu, OgrenciNo = ogrenciNo }
                                equals new { n.DersKodu, n.OgrenciNo } into notlar
                                from n in notlar.DefaultIfEmpty()
                                select new NotViewModel
                                {
                                    DersKodu = d.DersKodu,
                                    DersAdi = d.DersAdi,
                                    Vize = n != null ? n.Vize : null,
                                    Final = n != null ? n.Final : null,
                                    But = n != null ? n.But : null
                                };

                    var result = query.OrderBy(x => x.DersAdi).ToList();
                    dgvNotlar.DataSource = null;
                    dgvNotlar.Columns.Clear();
                    dgvNotlar.DataSource = result;

                    // Sütun ayarları
                    dgvNotlar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                    if (dgvNotlar.Columns["DersKodu"] != null)
                    {
                        dgvNotlar.Columns["DersKodu"].HeaderText = "Ders Kodu";
                        dgvNotlar.Columns["DersKodu"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                        dgvNotlar.Columns["DersKodu"].ReadOnly = true;
                    }
                    if (dgvNotlar.Columns["DersAdi"] != null)
                    {
                        dgvNotlar.Columns["DersAdi"].HeaderText = "Ders Adı";
                        dgvNotlar.Columns["DersAdi"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dgvNotlar.Columns["DersAdi"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                        dgvNotlar.Columns["DersAdi"].ReadOnly = true;
                    }
                    if (dgvNotlar.Columns["Vize"] != null)
                    {
                        dgvNotlar.Columns["Vize"].HeaderText = "Vize";
                        dgvNotlar.Columns["Vize"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    }
                    if (dgvNotlar.Columns["Final"] != null)
                    {
                        dgvNotlar.Columns["Final"].HeaderText = "Final";
                        dgvNotlar.Columns["Final"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    }
                    if (dgvNotlar.Columns["But"] != null)
                    {
                        dgvNotlar.Columns["But"].HeaderText = "Büt";
                        dgvNotlar.Columns["But"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    }
                    dgvNotlar.RowTemplate.Height = 40;
                    dgvNotlar.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Notlar yüklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbBolum_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cmbBolum.SelectedIndex <= 0 || cmbBolum.SelectedValue == null)
            {
                cmbSinif.DataSource = null;
                cmbSinif.Text = "Seç";
                cmbOgrenci.DataSource = null;
                cmbOgrenci.Text = "Seç";
                dgvNotlar.DataSource = null;
                return;
            }

            try
            {
                using (var context = new OBSModel())
                {
                    if (!int.TryParse(cmbBolum.SelectedValue.ToString(), out int bolumNo))
                    {
                        return;
                    }
                    var siniflar = context.Ogrenciler
                        .Where(o => o.BolumNo == bolumNo && o.Sinif != null)
                        .Select(o => new SinifViewModel { Sinif = o.Sinif })
                        .Distinct()
                        .OrderBy(s => s.Sinif)
                        .ToList();
                    var sinifList = new List<SinifViewModel> { new SinifViewModel { Sinif = "Seç" } };
                    sinifList.AddRange(siniflar);
                    cmbSinif.SelectedIndexChanged -= cmbSinif_SelectedIndexChanged;
                    cmbSinif.DataSource = sinifList;
                    cmbSinif.DisplayMember = "Sinif";
                    cmbSinif.SelectedIndex = -1;
                    cmbSinif.Text = "Seç";
                    cmbSinif.SelectedIndexChanged += cmbSinif_SelectedIndexChanged;
                    cmbOgrenci.DataSource = null;
                    cmbOgrenci.Text = "Seç";
                    dgvNotlar.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Sınıflar yüklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbSinif_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBolum.SelectedValue == null || cmbBolum.SelectedIndex == 0 || cmbSinif.SelectedIndex == 0 || cmbSinif.SelectedItem == null)
            {
                cmbOgrenci.DataSource = null;
                cmbOgrenci.Text = "Seç";
                dgvNotlar.DataSource = null;
                return;
            }

            try
            {
                using (var context = new OBSModel())
                {
                    int bolumNo = (int)cmbBolum.SelectedValue;
                    var selectedSinif = cmbSinif.SelectedItem as SinifViewModel;
                    if (selectedSinif == null || string.IsNullOrEmpty(selectedSinif.Sinif))
                    {
                        MessageBox.Show("Geçersiz sınıf seçimi!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    string sinif = selectedSinif.Sinif;
                    var ogrenciler = context.Ogrenciler
                        .Where(o => o.BolumNo == bolumNo && o.Sinif == sinif)
                        .Select(o => new OgrenciViewModel { OgrenciNo = o.OgrenciNo, AdSoyad = o.Ad + " " + o.Soyad })
                        .OrderBy(o => o.AdSoyad)
                        .ToList();
                    var ogrenciList = new List<OgrenciViewModel> { new OgrenciViewModel { OgrenciNo = null, AdSoyad = "Seç" } };
                    ogrenciList.AddRange(ogrenciler);
                    cmbOgrenci.SelectedIndexChanged -= cmbOgrenci_SelectedIndexChanged;
                    cmbOgrenci.DataSource = ogrenciList;
                    cmbOgrenci.DisplayMember = "AdSoyad";
                    cmbOgrenci.ValueMember = "OgrenciNo";
                    cmbOgrenci.SelectedIndex = -1;
                    cmbOgrenci.Text = "Seç";
                    int maxWidth = 0;
                    using (Graphics g = cmbOgrenci.CreateGraphics())
                    {
                        foreach (var item in ogrenciList)
                        {
                            int width = (int)g.MeasureString(item.AdSoyad, cmbOgrenci.Font).Width;
                            maxWidth = Math.Max(maxWidth, width);
                        }
                    }
                    cmbOgrenci.DropDownWidth = Math.Max(maxWidth + 20, cmbOgrenci.Width);
                    cmbOgrenci.SelectedIndexChanged += cmbOgrenci_SelectedIndexChanged;
                    dgvNotlar.DataSource = null;

                    // Hata ayıklama için öğrenci sayısını kontrol et
                    if (ogrenciler.Count == 0)
                    {
                        MessageBox.Show($"Bölüm No: {bolumNo}, Sınıf: {sinif} için öğrenci bulunamadı. Veritabanını kontrol edin.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Öğrenciler yüklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbOgrenci_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbOgrenci.SelectedValue == null || cmbOgrenci.SelectedIndex == 0)
            {
                dgvNotlar.DataSource = null;
                return;
            }

            try
            {
                using (var context = new OBSModel())
                {
                    int ogrenciNo = (int)cmbOgrenci.SelectedValue;
                    var ogrenci = context.Ogrenciler
                        .FirstOrDefault(o => o.OgrenciNo == ogrenciNo);
                    if (ogrenci == null) return;

                    var query = from d in context.Dersler
                                where d.BolumNo == ogrenci.BolumNo || d.BolumNo == null
                                join n in context.Notlar
                                on new { DersKodu = d.DersKodu, OgrenciNo = ogrenciNo }
                                equals new { n.DersKodu, n.OgrenciNo } into notlar
                                from n in notlar.DefaultIfEmpty()
                                select new NotViewModel
                                {
                                    DersKodu = d.DersKodu,
                                    DersAdi = d.DersAdi,
                                    Vize = n != null ? n.Vize : null,
                                    Final = n != null ? n.Final : null,
                                    But = n != null ? n.But : null
                                };

                    var result = query.OrderBy(x => x.DersAdi).ToList();
                    dgvNotlar.DataSource = null;
                    dgvNotlar.Columns.Clear();
                    dgvNotlar.DataSource = result;

                    // Sütun ayarları
                    dgvNotlar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                    if (dgvNotlar.Columns["DersKodu"] != null)
                    {
                        dgvNotlar.Columns["DersKodu"].HeaderText = "Ders Kodu";
                        dgvNotlar.Columns["DersKodu"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                        dgvNotlar.Columns["DersKodu"].ReadOnly = true;
                    }
                    if (dgvNotlar.Columns["DersAdi"] != null)
                    {
                        dgvNotlar.Columns["DersAdi"].HeaderText = "Ders Adı";
                        dgvNotlar.Columns["DersAdi"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dgvNotlar.Columns["DersAdi"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                        dgvNotlar.Columns["DersAdi"].ReadOnly = true;
                    }
                    if (dgvNotlar.Columns["Vize"] != null)
                    {
                        dgvNotlar.Columns["Vize"].HeaderText = "Vize";
                        dgvNotlar.Columns["Vize"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    }
                    if (dgvNotlar.Columns["Final"] != null)
                    {
                        dgvNotlar.Columns["Final"].HeaderText = "Final";
                        dgvNotlar.Columns["Final"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    }
                    if (dgvNotlar.Columns["But"] != null)
                    {
                        dgvNotlar.Columns["But"].HeaderText = "Büt";
                        dgvNotlar.Columns["But"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    }
                    dgvNotlar.RowTemplate.Height = 40;
                    dgvNotlar.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Notlar yüklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetCellColors()
        {
            foreach (DataGridViewRow row in dgvNotlar.Rows)
            {
                if (row.IsNewRow) continue;
                row.Cells["Vize"].Style.BackColor = Color.White;
                row.Cells["Final"].Style.BackColor = Color.White;
                row.Cells["But"].Style.BackColor = Color.White;
            }
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            if (Program.MainGirisForm != null)
            {
                Program.MainGirisForm.Show();
                this.Close();
            }
        }

        private void dgvNotlar_CellValidating_1(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex < 2) return;
            string value = e.FormattedValue?.ToString();
            string columnName = dgvNotlar.Columns[e.ColumnIndex].Name;
            if (columnName != "Vize" && columnName != "Final" && columnName != "But") return;

            if (string.IsNullOrWhiteSpace(value))
            {
                dgvNotlar.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
                return;
            }

            if (!int.TryParse(value, out int number) || number < 0 || number > 100)
            {
                e.Cancel = true;
                dgvNotlar.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightPink;
                validationToolTip.Show($"{columnName} notu 0-100 arasında bir sayı olmalı!", dgvNotlar, dgvNotlar.PointToClient(Cursor.Position), 3000);
            }
            else
            {
                dgvNotlar.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
            }
        }

        private void dgvNotlar_CellEndEdit_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 2) return;
            if (cmbOgrenci.SelectedValue == null || cmbOgrenci.SelectedIndex == 0) return;

            string columnName = dgvNotlar.Columns[e.ColumnIndex].Name;
            if (columnName != "Vize" && columnName != "Final" && columnName != "But") return;

            // YENİ: Sadece doğrulama ve renk değişimi, kaydetme yok
            try
            {
                string value = dgvNotlar.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
                if (string.IsNullOrWhiteSpace(value))
                {
                    dgvNotlar.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
                    return;
                }

                if (!int.TryParse(value, out int number) || number < 0 || number > 100)
                {
                    dgvNotlar.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightPink;
                    validationToolTip.Show($"{columnName} notu 0-100 arasında bir sayı olmalı!", dgvNotlar, dgvNotlar.PointToClient(Cursor.Position), 3000);
                }
                else
                {
                    dgvNotlar.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightGreen;
                    resetColorTimer.Start();
                }
            }
            catch (Exception ex)
            {
                dgvNotlar.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightPink;
                validationToolTip.Show($"Hata: {ex.Message}", dgvNotlar, dgvNotlar.PointToClient(Cursor.Position), 3000);
            }
        }

        private void cmbSinif_DropDown_1(object sender, EventArgs e)
        {
            if (cmbBolum.SelectedIndex <= 0 || cmbBolum.SelectedValue == null)
            {
                cmbSinif.DataSource = new List<SinifViewModel> { new SinifViewModel { Sinif = "Önce bölüm seç" } };
                cmbSinif.DisplayMember = "Sinif";
            }
        }

        private void cmbOgrenci_DropDown_1(object sender, EventArgs e)
        {
            if (cmbBolum.SelectedIndex <= 0 || cmbBolum.SelectedValue == null)
            {
                cmbOgrenci.DataSource = new List<OgrenciViewModel> { new OgrenciViewModel { OgrenciNo = null, AdSoyad = "Önce bölüm seç" } };
                cmbOgrenci.DisplayMember = "AdSoyad";
                cmbOgrenci.ValueMember = "OgrenciNo";
            }
            else if (cmbSinif.SelectedIndex <= 0 || cmbSinif.SelectedItem == null)
            {
                cmbOgrenci.DataSource = new List<OgrenciViewModel> { new OgrenciViewModel { OgrenciNo = null, AdSoyad = "Önce sınıf seç" } };
                cmbOgrenci.DisplayMember = "AdSoyad";
                cmbOgrenci.ValueMember = "OgrenciNo";
            }
        }
    }
}
