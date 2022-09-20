using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TelebeProqrami
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        dbTelebelerEntities db = new dbTelebelerEntities();
        private void btnTelebeListele_Click(object sender, EventArgs e)
        {

            GetTelebe();
        }
        private void GetTelebe()
        {
            dataGridView1.DataSource = db.TBL_Telebeler.ToList();
            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[4].Visible = false;
        }
        private void getDersler()
        {
            dataGridView1.DataSource = db.TBL_Dersler.ToList();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            getDersler();


        }


        private void button2_Click(object sender, EventArgs e)
        {
            var sorgu = from item in db.TBL_Qeydler
                        select new {
                            item.TBL_Telebeler.Ad,
                            item.TBL_Telebeler.Soyad,
                            item.QeydID,
                            item.TBL_Dersler.DersAD,
                            item.Imtahan1,
                            item.Imtahan2,
                            item.Imtahan3,
                            item.Ortalama,
                            item.Veziyyet
                                      };
            dataGridView1.DataSource = sorgu.ToList();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtAd.Text != "" && txtSoyad.Text != "")
            {
                TBL_Telebeler tlb = new TBL_Telebeler();
                tlb.Ad = txtAd.Text;
                tlb.Soyad = txtSoyad.Text;
                db.TBL_Telebeler.Add(tlb);
                db.SaveChanges();
                txtAd.Text = "";
                txtSoyad.Text = "";
                MessageBox.Show("Tələbə uğurla əlavə olundu");
                lblİNFO.Text = "Bazaya tələbə əlavə olundu";
                GetTelebe();
            }

            if (txtDersAD.Text != "")
            {
                TBL_Dersler drs = new TBL_Dersler();
                drs.DersAD = txtDersAD.Text;
                db.TBL_Dersler.Add(drs);
                db.SaveChanges();
                txtDersAD.Text = "";
                MessageBox.Show("Dərs uğurla əlavə olundu");
                lblİNFO.Text = "Bazaya dərs əlavə olundu";
                getDersler();
            }


        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtID.Text);
            var x = db.TBL_Telebeler.Find(id);
            db.TBL_Telebeler.Remove(x);
            db.SaveChanges();
            MessageBox.Show("Tələbə uğurla silindi");
            lblİNFO.Text = "Tələbə bazadan silindi";
        }

        private void btnProsedur_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.TelebeQeydleri();
        }

        private void btnSeach_Click(object sender, EventArgs e)
        {
            if (chkSearch.Checked)
            {
                dataGridView1.DataSource = db.TBL_Telebeler.Where(x => x.Ad == txtAd.Text & x.Soyad == txtSoyad.Text).ToList();
            }
            else
            {
                dataGridView1.DataSource = db.TBL_Telebeler.Where(x => x.Ad == txtAd.Text | x.Soyad == txtSoyad.Text).ToList();
            }

        }

        private void txtAd_TextChanged(object sender, EventArgs e)
        {
            string axtarilan = txtAd.Text;
            var deyeler = from item in db.TBL_Telebeler
                          where item.Ad.Contains(axtarilan)
                          select item;
            dataGridView1.DataSource = deyeler.ToList();

        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (radioMIN.Checked)
            {
                var min = db.TBL_Qeydler.Min(p => p.Imtahan1);
                MessageBox.Show("Imtahan ən aşağı  bal " + min.ToString());
                var sorgu = from item in db.TBL_Qeydler
                            where item.Imtahan1 == min
                            select new
                            {
                                item.TBL_Telebeler.Ad,
                                item.TBL_Telebeler.Soyad,
                                item.QeydID,
                                item.TBL_Dersler.DersAD,
                                item.Imtahan1,
                                item.Imtahan2,
                                item.Imtahan3,
                                item.Ortalama,
                                item.Veziyyet
                            };
                dataGridView1.DataSource = sorgu.ToList();
                //List<TBL_Qeydler> listmax = db.TBL_Qeydler.Where(p => p.Imtahan1 == min).ToList();
                //dataGridView1.DataSource = listmax;
            }
            if (radioEnYuxari.Checked)
            {
                var max = db.TBL_Qeydler.Max(p => p.Imtahan1);
                MessageBox.Show("Imtahan ən yuxarı  bal " + max.ToString());
                var sorgu = from item in db.TBL_Qeydler
                            where item.Imtahan1==max
                            select new
                            {
                                item.TBL_Telebeler.Ad,
                                item.TBL_Telebeler.Soyad,
                                item.QeydID,
                                item.TBL_Dersler.DersAD,
                                item.Imtahan1,
                                item.Imtahan2,
                                item.Imtahan3,
                                item.Ortalama,
                                item.Veziyyet
                            };
                dataGridView1.DataSource = sorgu.ToList();
                //List<TBL_Qeydler> listmax = db.TBL_Qeydler.Where(p => p.Imtahan1 == max).ToList();
            
            }
            if (Radio_ortalama.Checked)
            {

                var ortalama = db.TBL_Qeydler.Average(p => p.Imtahan1);
                MessageBox.Show("Imtahan ortalaması " + ortalama.ToString());
                var sorgu = from item in db.TBL_Qeydler
                            where item.Imtahan1 > ortalama
                            select new
                            {
                                item.TBL_Telebeler.Ad,
                                item.TBL_Telebeler.Soyad,
                                item.TBL_Dersler.DersAD,
                                item.Imtahan1,
                                item.Imtahan2,
                                item.Imtahan3,
                                item.Ortalama,
                                item.Veziyyet
                            };
                dataGridView1.DataSource = sorgu.ToList();
                //List<TBL_Qeydler> listeleAZ = db.TBL_Qeydler.Where(p => p.Imtahan1 > ortalama).ToList();
                //dataGridView1.DataSource = listeleAZ;



            }
            if (radioAZ.Checked)
            {
                List<TBL_Telebeler> listeleAZ = db.TBL_Telebeler.OrderBy(p => p.Ad).ToList();
                dataGridView1.DataSource = listeleAZ;
            }
            if (radioilk3.Checked)
            {
                List<TBL_Telebeler> Listeleilk3 = db.TBL_Telebeler.OrderBy(p => p.Ad).Take(3).ToList();
                dataGridView1.DataSource = Listeleilk3;
            }
            if (radioZA.Checked)
            {
                List<TBL_Telebeler> listeleZA = db.TBL_Telebeler.OrderByDescending(p => p.Ad).ToList();
                dataGridView1.DataSource = listeleZA;
            }
            if (radioMelumat.Checked)
            {
                bool melumat = db.TBL_Klublar.Any();
                if (melumat.ToString() == "True")
                {

                    MessageBox.Show("Məlumat mövcuddur" + " ✓ ", " İnformasiya", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {

                    MessageBox.Show("Məlumat mövcud deyil" + " ✖ ", " İnformasiya", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }

            if (radioCount.Checked)
            {
                int telebesayi = db.TBL_Telebeler.Count();
                MessageBox.Show(telebesayi.ToString() + " tələbə var", " İnformasiya", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
        }

        private void radioZA_CheckedChanged(object sender, EventArgs e)
        {
            List<TBL_Telebeler> listeleZA = db.TBL_Telebeler.OrderByDescending(p => p.Ad).ToList();
            dataGridView1.DataSource = listeleZA;
        }

    }
}
