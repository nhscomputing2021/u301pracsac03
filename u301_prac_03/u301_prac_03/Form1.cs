using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace u301_prac_03
{
    public partial class Form1 : Form
    {
        string filter;
        List<Sale> sales = new List<Sale>();
        BindingSource bs = new BindingSource();
        
        public Form1()
        {
            InitializeComponent();
            LoadCSV();
            bs.DataSource = sales;
            dataGridView1.DataSource = bs;
        }

        private void LoadCSV()
        {
            string filePath = @"C:\demo\sample data_1.csv";
            List<string> lines = new List<string>();
            lines = File.ReadAllLines(filePath).ToList();
            foreach (string line in lines)
            {
                List<string> fields = line.Split(',').ToList();
                Sale s = new Sale();
                s.ItemName = fields[0];
                s.Category = fields[1];
                s.PurchasedPrice = float.Parse(fields[3]);
                s.SalePrice = fields[5];
                s.Rating = fields[6];
                sales.Add(s);
            }
        }

        private void comFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            filter = comFilter.Text;
            if (filter == "Rating") SortByRating(sales);
            dataGridView1.DataSource = bs;
            bs.ResetBindings(false);
        }
        private void SortByRating(List<Sale> _sales)
        {//adapted version of selection sort
            int  min;
            string temp;
            for (int i = 0; i < _sales.Count - 1; i++)
            {
                min = i;
                for (int j = i + 1; j < _sales.Count; j++)
                {
                    
                    if(int.TryParse(_sales[j].Rating, out int ratingJ))
                    { /*
                        if the current rating is a number and the current smallest 
                        is also a number and is greater than the current rating,
                        then assign the index of current rating to the index of the smallest
                        else if the smallest is NA, which is greater than the number, 
                        so assign the index of current rating to the index of the smallest

                      */
                       
                        if(int.TryParse(_sales[min].Rating, out int ratingMin))
                            { if (ratingJ < ratingMin) min = j; }
                        else
                        {
                            min = j;
                        }
                    }
                }
                //swap the current element with the current smallest one
                temp = _sales[min].Rating;
                _sales[min].Rating = _sales[i].Rating;
                _sales[i].Rating = temp;
            }
        }

        private List<Sale> Search(string target,string filter)
        {
            List<Sale> results = new List<Sale>();
            foreach(Sale s in sales)
            {
                if(filter == "Rating")
                {
                    if (s.Rating.ToLower() == target.ToLower()) results.Add(s);
                }
                if(filter == "Category")
                {
                    if (s.Category.ToLower().Contains(target.ToLower())) { results.Add(s); Console.WriteLine(target); }
                }
            }
            return results;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            List<Sale> r = Search(txtSearch.Text, filter);
            bs.DataSource = r;
            dataGridView1.DataSource = r;
            bs.ResetBindings(false);
        }
    }
}
