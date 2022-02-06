using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;   // Entity fremework kullanılsaydı otm. gelecekti. 

namespace EntityOrnek
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DbSinavOgrenciEntities db = new DbSinavOgrenciEntities();  // DbSinavOgrenciEntities model aracılı ile db nesnesini ürettik.
        private void btnDersListesi_Click(object sender, EventArgs e)
        {
            SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-E4ARC7I;Initial Catalog=DbSinavOgrenci;Integrated Security=True");
            SqlCommand komut = new SqlCommand("Select * From tbldersler",baglanti); // veriyi "baglanti"'daki tbldersler'den çekecek.
            SqlDataAdapter da = new SqlDataAdapter(komut);// bağlayıcımızı oluşturduk. Bağlayıcı içine "komut" gönderdik.
            DataTable dt = new DataTable(); // Veri tablosu oluşturduk
            da.Fill(dt); // Oluşturduğumuz  veri tablosu içerisini doldurduk.
            dataGridView1.DataSource = dt;

        }

        private void btnOgrenciListele_Click(object sender, EventArgs e)
        {
           
            dataGridView1.DataSource = db.TBLOGRENCI.ToList(); // TblOgrenci'ye ait bütün verileri listeleyecek.
            dataGridView1.Columns[3].Visible = false; // çalıştırdığımızda boş sütunu getirmez.
            dataGridView1.Columns[4].Visible = false;
        }

        private void btnNotListesi_Click(object sender, EventArgs e)
        {
            //dataGridView1.DataSource = db.TBLNOTLAR.ToList();
            var query = from item in db.TBLNOTLAR               // "item" değişken adıdır.
                        select new { item.NOTID, item.OGR, item.DERS, item.SINAV1, item.SINAV2, 
                            item.SINAV3,item.ORTALAMA , item.DURUM };  //süslü prnt. içerisinde bulunan alanları seç
            dataGridView1.DataSource = query.ToList(); // query'den gelen listeyi yazdır

        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            TBLOGRENCI t = new TBLOGRENCI();
            t.AD = txtAd.Text;
            t.SOYAD = txtSoyad.Text;
            db.TBLOGRENCI.Add(t);  
            db.SaveChanges(); // değişiklikleri kaydet bunları veritabanına yansıt.
            MessageBox.Show("Öğrenci Listeye eklenmiştir.");
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtOgrenciId.Text);
            var x = db.TBLOGRENCI.Find(id);  //ilk önce silmek istediğimiz ID'yi tutmamız gerekiyor.
            db.TBLOGRENCI.Remove(x);
            db.SaveChanges();
            MessageBox.Show("Öğrenci sistemden silindi");

        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtOgrenciId.Text);
            var x = db.TBLOGRENCI.Find(id);  //txtOgrenciId'ye girmiş olduğumuz Id değerine göre ilgili satırın kaydını tutuyor
            x.AD = txtAd.Text;
            x.SOYAD = txtSoyad.Text;
            x.FOTOGRAF = txtFoto.Text;
            db.SaveChanges();
            MessageBox.Show("Öğrenci bilgileri başarıyla güncellendi.");

        }

        private void btnProsedur_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.NOTLISTESI();
        }

        private void btnBul_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.TBLOGRENCI.Where(x => x.AD == txtAd.Text & x.SOYAD == txtSoyad.Text).ToList(); //x'e göndereceğimiz ifade; Adı, txtAd'a eşit olan değerleri listeleyecek.
        }

        private void txtAd_TextChanged(object sender, EventArgs e)
        {
            string aranan = txtAd.Text;
            var degerler = from item in db.TBLOGRENCI
                           where item.AD.Contains(aranan) //contains methotu; ifadeyi içerenleri getirir.
                           select item;  // txtAranan ifadesine eşit olan item'ları getirip, bu item'leri değerlere aktar.Sonrada dtGridte bu değerleri listele.
            dataGridView1.DataSource = degerler.ToList();
        }
    }
}
