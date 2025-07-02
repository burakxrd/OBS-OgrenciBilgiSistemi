📚 OBS - Öğrenci Bilgi Sistemi 🎓
✨ Proje Hakkında
Bu proje, temel öğrenci, ders, not ve kullanıcı yönetimi işlevlerini sunan bir masaüstü (Windows Forms) uygulamasıdır. Veritabanı etkileşimi için Entity Framework kullanılmıştır.

🚀 Özellikler
Öğrenci bilgilerini yönetme (Ekleme, Güncelleme, Silme).

Ders ve not girişi işlemleri.

Kullanıcı kimlik doğrulama ve rol tabanlı erişim.

Sistemde yapılan önemli eylemlerin denetim kaydı.

🛠️ Kullanılan Teknolojiler
C#

Windows Forms

Entity Framework 6 (Database First)

Microsoft SQL Server

📦 Kurulum ve Çalıştırma
Veritabanı Kurulumu
SQL Server'ınızda OBS_VeriTabani adında yeni bir veritabanı oluşturun.

Proje ana dizininde bulunan OBSScriptFile.sql dosyasını bu veritabanında çalıştırarak tabloları ve başlangıç verilerini oluşturun.

Uygulama Kurulumu
Projeyi Visual Studio ile açın.

App.config dosyasındaki bağlantı dizesini kendi SQL Server kurulumunuza uygun şekilde güncelleyin.

OBS/App.config dosyasında <connectionStrings> bölümündeki OBSModel bağlantı dizesindeki (data source=YOUR_SERVER_NAME\SQLEXPRESS;...) YOUR_SERVER_NAME\SQLEXPRESS kısmını kendi sunucu adınızla değiştirmelisiniz. Örneğin: data source=.(localdb)\MSSQLLocalDB; veya data source=BILGISAYAR_ADINIZ\SQLEXPRESS;.

Projeyi derleyin ve çalıştırın.

Kullanıcı Rolleri Hakkında
Uygulamada iki temel kullanıcı rolü bulunmaktadır:

Yönetici (Yonetici rolü): Sistemi yöneten, tüm işlemlere erişimi olan kişiler için atanmalıdır.

Öğrenci (Ogrenci rolü): Sisteme öğrenci olarak giriş yapacak kişiler için atanmalıdır.

📸 Ekran Görüntüleri
![giris_ekrani](https://github.com/user-attachments/assets/1bd20609-6728-42f9-9c3b-450157769495)
![admin_paneli](https://github.com/user-attachments/assets/5f7f6f2b-4283-414e-a3b6-7eff52d673e9)
![not_giris_ekrani](https://github.com/user-attachments/assets/347ba1d3-d676-40cf-8f3a-242928834ded)
![ogrenci_paneli](https://github.com/user-attachments/assets/713bb301-9f9b-43eb-a660-8abf9112b28a)



📜 Lisans
Bu proje MIT Lisansı altında lisanslanmıştır. Daha fazla bilgi için LICENSE dosyasına bakın.
