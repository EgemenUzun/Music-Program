using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Windows;

namespace _9._11._18BttonStill
{
    class vtClass
    {
        public OleDbConnection baglanti;
        public string vt, vtYolu;
        public ArrayList tablolar = new ArrayList();
        private OleDbCommand kmt = new OleDbCommand();
        string baseDir = Environment.CurrentDirectory;

        public vtClass(string vt)//kurucu metod
        {
            this.vt = vt;
            try
            {
                this.vtYolu = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + baseDir /*Application.StartupPath*/ + "\\" + this.vt;
                this.baglanti = new OleDbConnection(vtYolu);
                this.vtBaglan();
                DataTable dt1 = baglanti.GetSchema("Tables");                         
                for (int i = 0; i < dt1.Rows.Count; i++) //tablo adları arraylist' e ekleniyor.
                {
                    if (dt1.Rows[i]["TABLE_TYPE"].ToString() == "TABLE")
                    {
                        tablolar.Add(dt1.Rows[i]["TABLE_NAME"]);
                    }
                }
                this.vtBaglantiKes();
            }
            catch (Exception h)
            {
                MessageBox.Show(h.Message);
            }
        }

        public bool kayitSil(string tabloAdi,string alanAdi,  object deger)//kayıt silme metodu
        {
            this.baglanti.Open();
            kmt.Connection = baglanti;
            kmt.CommandText = "";
            if (deger is int)
            {
                kmt.CommandText = "delete from " + tabloAdi + " where " + alanAdi + "=" + (int)deger;
            }
            else
            {
                kmt.CommandText = "delete from " + tabloAdi + " where " + alanAdi + "=" + (string)deger;
            }
                if (kmt.ExecuteNonQuery()==1)
                {
                    this.baglanti.Close();
                    return false;
                }
                else
                {
                    this.baglanti.Close();
                    return true;
                }
        }
        public bool kayitGuncelle(string tabloAdi, string alanAdi, object deger, params object[] liste)
        {

            //// parametre listeli sorgu oluşturuyor
            string ekleSorgu = "update " + tabloAdi + " set ";
            ArrayList KolonAdlariListGelen;
            this.baglanti.Open(); 

            ////alan adları alınıyor        
          OleDbDataAdapter da = new OleDbDataAdapter("select * from " + tabloAdi, this.baglanti);
            DataTable dt2 = new DataTable();
            da.Fill(dt2);
            KolonAdlariListGelen = new ArrayList();
            //array list içeirisine alan adlarını aktarıyoruz

            for (int i = 1; i < dt2.Columns.Count; i++)
            {
                DataColumn dr = dt2.Columns[i];
                KolonAdlariListGelen.Add(dr.ToString());
               
            } 
            //alanadlarını array liste e ekleme işlemi bitti
            for (int i = 0; i < KolonAdlariListGelen.Count; i++)
            {
                
                if (i != KolonAdlariListGelen.Count - 1)
                {
                    if (liste[i] is string)
                    {
                        if (liste[i] == null)
                            liste[i] = "";
                        kmt.Parameters.Add("@" + KolonAdlariListGelen[i].ToString(), liste[i].ToString());
                        ekleSorgu = ekleSorgu + KolonAdlariListGelen[i].ToString() +"=" + "@" + KolonAdlariListGelen[i] + ",";
                    }
                    else if (liste[i] is int)
                    {
                         if (liste[i]==null)
                            liste[i]=0;
                        kmt.Parameters.Add("@" + KolonAdlariListGelen[i].ToString() , liste[i].ToString());
                        ekleSorgu = ekleSorgu + KolonAdlariListGelen[i].ToString() + "=" + "@" + KolonAdlariListGelen[i] + ",";
                    }
                }
                else
                {
                    if (liste[i] is string)
                    {
                         if (liste[i]==null)
                            liste[i]="";
                        kmt.Parameters.Add("@" + KolonAdlariListGelen[i].ToString(), liste[i].ToString());
                        ekleSorgu = ekleSorgu + KolonAdlariListGelen[i].ToString() + "=" + "@" + KolonAdlariListGelen[i];
                    }
                    else if (liste[i] is int)
                    {
                        if (liste[i] == null)
                            liste[i] = 0;
                        kmt.Parameters.Add("@" + KolonAdlariListGelen[i].ToString(), liste[i].ToString());
                        ekleSorgu = ekleSorgu + KolonAdlariListGelen[i].ToString() + "=" + "@" + KolonAdlariListGelen[i];
                    }
                }

            }
            ekleSorgu = ekleSorgu + " where " + alanAdi +"="+deger;
            ///////////////////sorgu oluşturuldu
            this.kmt.Connection = this.baglanti;
            this.kmt.CommandText = ekleSorgu;
            MessageBox.Show(ekleSorgu);
            this.kmt.ExecuteNonQuery();
            kmt.Dispose();
            this.baglanti.Close();

            return true;
        }
        public void vtBaglan()
        {
            baglanti.Open();
        }

        public void vtBaglantiKes()
        {
            baglanti.Close();
        }

        public DataTable veriAl(string sorgu)
        {
          DataTable dt = new DataTable();
            if (sorgu != "")
            { 
                this.baglanti.Open();
                OleDbDataAdapter da = new OleDbDataAdapter(sorgu, this.baglanti);                    
                da.Fill(dt);
                this.baglanti.Close();
                return dt;                
            }
            else
            {                
                return null;
            }
           
        }
       
        public bool kayitEkle(string tabloAdi, params object[] liste)
        {
            //// parametre listeli sorgu oluşturuyor
           string ekleSorgu="insert into "+ tabloAdi  +" (";
            ArrayList KolonAdlariList;
            this.baglanti.Open();
            OleDbDataAdapter da = new OleDbDataAdapter("select * from "+tabloAdi, this.baglanti); 
            DataTable dt2 = new DataTable();
            da.Fill(dt2);
            KolonAdlariList = new ArrayList();
            for (int i = 1; i < dt2.Columns.Count; i++)
            {         
                DataColumn dr = dt2.Columns[i];  
                KolonAdlariList.Add(dr.ToString());
            }
            for (int k = 0; k < KolonAdlariList.Count;k++)
            {
                if (k != KolonAdlariList.Count - 1)
                {
                    ekleSorgu = ekleSorgu + " " + KolonAdlariList[k] + ",";
                }
                else
                {
                    ekleSorgu = ekleSorgu + " " + KolonAdlariList[k] ;
                }
            }
            ekleSorgu = ekleSorgu + ") values (";
            for (int i = 0; i < KolonAdlariList.Count; i++)
            {
               
                if (i != KolonAdlariList.Count - 1)
                {
                    if (liste[i] is string)
                    {
                        kmt.Parameters.Add("@" + KolonAdlariList[i].ToString() + "", liste[i].ToString());
                        ekleSorgu = ekleSorgu +  "@" + KolonAdlariList[i] + ",";
                    }
                    else if (liste[i] is int)
                    {
                        kmt.Parameters.Add("@" + KolonAdlariList[i].ToString() + "", liste[i].ToString());
                        ekleSorgu = ekleSorgu + "@" + KolonAdlariList[i] + ",";
                    }
                }
                else
                {
                    if (liste[i] is string)
                    {
                        kmt.Parameters.Add("@" + KolonAdlariList[i].ToString() + "", liste[i]);
                        ekleSorgu = ekleSorgu + "@" + KolonAdlariList[i];
                    }
                    else if (liste[i] is int)
                    {
                        kmt.Parameters.Add("@" + KolonAdlariList[i].ToString() + "", liste[i]);
                        ekleSorgu = ekleSorgu + "@" + KolonAdlariList[i];
                    }
                }
                
            }

            ekleSorgu = ekleSorgu + ")";
            ///////////////////sorgu oluşturuldu
            this.kmt.Connection = this.baglanti;
            this.kmt.CommandText = ekleSorgu;
            //MessageBox.Show(ekleSorgu);
            this.kmt.ExecuteNonQuery();
            this.baglanti.Close();
            kmt.Dispose();
            return true;
        }
      
        public bool sorguGonder(string sorgu)
        {
            try
            {
                this.baglanti.Open();
                this.kmt.Connection = baglanti;
                this.kmt.CommandText = sorgu;
                //MessageBox.Show(sorgu.ToString());
                if (this.kmt.ExecuteNonQuery() == 1)
                {
                    this.baglanti.Close();
                    this.kmt.Dispose();
                    return false;
                }
                else
                {
                    this.baglanti.Close();
                    this.kmt.Dispose();
                    return true;
                }
            }
            catch (Exception hata)
            {
                return false;
                MessageBox.Show(hata.Message);
            }

        }
        public bool veriKontrol(string tabloAdi,string alanAdi,object deger)
        {
            DataTable dt;
            try
            {
                dt = veriAl("SELECT COUNT" + "(" + alanAdi + ")" + " from " + tabloAdi + " where " + alanAdi + "=" + "'" + deger + "'");
                if (Convert.ToInt32(dt.Rows[0]["Expr1000"]) > 0)
                {
                   return true;
                }
                else
                   return false;
               
            }
            catch (Exception hata)
            {
                return false;
                MessageBox.Show(hata.Message);
            }
        }

    }
}