using System;
using System.Linq;
using System.Windows.Forms;

namespace PhoneBook
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        PhoneBookEntities db = new PhoneBookEntities();

        #region Kişi Listesi
        void GetList()
        {
            lviPeople.Items.Clear();
            var people = db.People.ToList();
            foreach (Person person in people)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = person.FirstName;
                lvi.SubItems.Add(person.LastName);
                lvi.SubItems.Add(person.PhoneNumber);
                lvi.SubItems.Add(person.Mail);
                lvi.Tag = person.Id; // liste içerisindeki her bir satırın arka planında gizli olarak ID değerini tutuyoruz.

                lviPeople.Items.Add(lvi);
            }
        }

        // select * from People where FirstName like 'ab%'  => startswidth
        // select * from People where FirstName like '%ab'  => endswidth
        // select * from People where FirstName like '%ab%' => contains


             


        void GetList(string param)
        {
            lviPeople.Items.Clear();
            var people = db.People.Where(x =>
                                         x.Mail.Contains(param)        ||
                                         x.LastName.Contains(param)    ||
                                         x.FirstName.Contains(param)   || 
                                         x.PhoneNumber.Contains(param) 
                                                 
            ).ToList();
             
            foreach (Person person in people)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = person.FirstName;
                lvi.SubItems.Add(person.LastName);
                lvi.SubItems.Add(person.PhoneNumber);
                lvi.SubItems.Add(person.Mail);
                lvi.Tag = person.Id; // liste içerisindeki her bir satırın arka planında gizli olarak ID değerini tutuyoruz.

                lviPeople.Items.Add(lvi);
            }

        }
        #endregion

        #region Form1_Load
        private void Form1_Load(object sender, EventArgs e)
        {
            GetList();
        }
        #endregion

        #region Kişi Kaydet
        private void btnSave_Click(object sender, EventArgs e)
        {
            Person person = new Person();
            person.FirstName = txtFirstName.Text;
            person.LastName = txtLastName.Text;
            person.PhoneNumber = txtPhone.Text;
            person.Mail = txtMail.Text;

            db.People.Add(person);
            bool result = db.SaveChanges() > 0;
            MessageBox.Show(result ? "Kayıt Eklendi" : "Kayıt Ekleme Hatası");
            GetList();
            Clean();
        }
        #endregion

        #region Temizleme Metodu
        void Clean()
        {
            foreach (Control item in groupBox1.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
        }
        #endregion

        #region Random Fake User
        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            txtFirstName.Text = FakeData.NameData.GetFirstName();
            txtLastName.Text = FakeData.NameData.GetSurname();
            txtPhone.Text = FakeData.PhoneNumberData.GetPhoneNumber();
            txtMail.Text = FakeData.NetworkData.GetEmail().ToLower();
        }
        #endregion

        #region Kişi Silme İşlemi
        private void tsmSil_Click(object sender, EventArgs e)
        {
            // listview üzerinden seçtiğimiz her bir satırdaki gizli olarak yer alan Id değerini alıyoruz.

            if (lviPeople.SelectedItems.Count == 0)
            {
                MessageBox.Show("Lütfen silme işlemi için bir eleman seçiniz!");
                return;
            }
            DialogResult dr = MessageBox.Show("Kayıt kalıcı olarak silinecektir.\nİşleme devam etmek istiyormusunuz", "Kayıt silme bildirimi", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (dr == DialogResult.Yes)
            {
                int id = (int)lviPeople.SelectedItems[0].Tag;

                // FirstOrDefault() => parametrede verdiğiniz koşula göre size bulduğu ilk nesneyi teslim eder, eğer nesne bulamazsa default olarak null döner.  Where koşulunun tekil nesne teslim eden halidir.

                Person person = db.People.FirstOrDefault(x => x.Id == id);
                if (person != null)
                {
                    db.People.Remove(person);
                    db.SaveChanges();
                    GetList();
                }
            }

        }
        #endregion

        #region Kişi Düzenleme İşlemi
        Person guncellenecek;
        private void tsmDuzenle_Click(object sender, EventArgs e)
        {
            if (lviPeople.SelectedItems.Count == 0)
            {
                MessageBox.Show("Lütfen güncelleme işlemi için bir eleman seçiniz!");
                return;
            }

            int id = (int)lviPeople.SelectedItems[0].Tag;
            guncellenecek = db.People.FirstOrDefault(x => x.Id == id);
            if (guncellenecek != null)
            {
                txtFirstName.Text = guncellenecek.FirstName;
                txtLastName.Text = guncellenecek.LastName;
                txtMail.Text = guncellenecek.Mail;
                txtPhone.Text = guncellenecek.PhoneNumber;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (guncellenecek == null)
            {
                MessageBox.Show("Bu kaydı güncelleyemezsiniz!");
                return;
            }

            guncellenecek.FirstName = txtFirstName.Text;
            guncellenecek.LastName = txtLastName.Text;
            guncellenecek.Mail = txtMail.Text;
            guncellenecek.PhoneNumber = txtPhone.Text;

            db.SaveChanges();  // güncelleme işlemi için bir metot yer almamaktadır. savechange yapmanız yeterlidir.
            GetList();
            Clean();
        }
        #endregion

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            GetList(txtSearch.Text);
        }
    }
}
































/*
  
CREATE DATABASE PhoneBook;
GO

USE PhoneBook;
GO

CREATE TABLE People ( 
             Id          INT PRIMARY KEY IDENTITY(1 , 1) , 
             FirstName   NVARCHAR(50) NOT NULL , 
             LastName    NVARCHAR(50) NOT NULL , 
             PhoneNumber NVARCHAR(24) NULL , 
             Mail        NVARCHAR(150) NULL
                    );    
    
 */
