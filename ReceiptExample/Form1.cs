using PrinterUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReceiptExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PrintReceiptForTransaction();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PrintReceiptForTransaction(true);
        }

        public void PrintReceiptForTransaction(bool print = false)
        {
            PrintDocument recordDoc = new PrintDocument();

            recordDoc.DocumentName = "Customer Receipt";
            recordDoc.PrintPage += new PrintPageEventHandler(PrintReceiptPage); 
            recordDoc.PrintController = new StandardPrintController(); 
                                                                       
            PrinterSettings ps = new PrinterSettings();
            ps.PrinterName = "POS-58";
            recordDoc.PrinterSettings = ps;

            if (print)
                recordDoc.Print();
            else
            {
                printPreviewDialog1.Document = recordDoc;
                printPreviewDialog1.ShowDialog();
            }
            

            recordDoc.Dispose();
        }

        private static void PrintReceiptPage(object sender, PrintPageEventArgs e)
        {
            float x = 10;
            float y = 20;

            Font drawFontArial12Bold = new Font("Arial", 12, FontStyle.Bold);
            Font drawFontArial10Bold = new Font("Arial", 10, FontStyle.Bold);
            Font drawFontArial10Regular = new Font("Arial", 10, FontStyle.Regular);
            Font drawFontArial5Regular = new Font("Arial", 9, FontStyle.Regular);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            StringFormat drawFormatCenter = new StringFormat();
            drawFormatCenter.Alignment = StringAlignment.Center;
            StringFormat drawFormatLeft = new StringFormat();
            drawFormatLeft.Alignment = StringAlignment.Near;
            StringFormat drawFormatRight = new StringFormat();
            drawFormatRight.Alignment = StringAlignment.Far;


            e.Graphics.DrawImage(ReceiptExample.Properties.Resources.logo, new PointF(x, y));
            y += 125;

            string text = "Date       : " + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year;
            e.Graphics.DrawString(text, drawFontArial10Regular, drawBrush, new Point(0, (int)y));
            y += e.Graphics.MeasureString(text, drawFontArial10Regular).Height;

            text = "Time       : " + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
            e.Graphics.DrawString(text, drawFontArial10Regular, drawBrush, new Point(0, (int)y));
            y += e.Graphics.MeasureString(text, drawFontArial10Regular).Height;

            text = "Receipt No : " + 52;
            e.Graphics.DrawString(text, drawFontArial10Regular, drawBrush, new Point(0, (int)y));
            y += e.Graphics.MeasureString(text, drawFontArial10Regular).Height + 10;

            //Ürün bilgileri.
            e.Graphics.DrawLine(Pens.Black, 0, y, 225, y);
            y += 10;

            text = "Product";
            e.Graphics.DrawString(text, drawFontArial10Bold, drawBrush, new Point(0, (int)y));

            text = "Pe.";
            e.Graphics.DrawString(text, drawFontArial10Bold, drawBrush, new Point(115, (int)y));

            text = "Price";
            e.Graphics.DrawString(text, drawFontArial10Bold, drawBrush, new Point(145, (int)y));
            y += e.Graphics.MeasureString(text, drawFontArial10Bold).Height;

            y += 5;

            int productCount = 0;
            foreach (var product in GetProducts())
            {
                if (product.Name.Length > 30)
                {
                    text = product.Name.Substring(0, 15);
                    e.Graphics.DrawString(text, drawFontArial5Regular, drawBrush, new Point(0, (int)y));
                    y += e.Graphics.MeasureString(text, drawFontArial5Regular).Height;

                    text = product.Name.Substring(15, 15) + "...";
                    e.Graphics.DrawString(text, drawFontArial5Regular, drawBrush, new Point(0, (int)y));

                    text = product.Amount.ToString();
                    e.Graphics.DrawString(text, drawFontArial5Regular, drawBrush, new Point(125, (int)y));

                    text = product.Price.ToString();
                    e.Graphics.DrawString(text, drawFontArial5Regular, drawBrush, new Point(150, (int)y));

                    if (productCount != GetProducts().Count - 1)
                        y += e.Graphics.MeasureString(text, drawFontArial5Regular).Height + 2;
                }
                else if (product.Name.Length > 15)
                {
                    text = product.Name.Substring(0, 15);
                    e.Graphics.DrawString(text, drawFontArial5Regular, drawBrush, new Point(0, (int)y));
                    y += e.Graphics.MeasureString(text, drawFontArial5Regular).Height;

                    text = product.Name.Substring(15, product.Name.Length - 15);
                    e.Graphics.DrawString(text, drawFontArial5Regular, drawBrush, new Point(0, (int)y));

                    text = product.Amount.ToString();
                    e.Graphics.DrawString(text, drawFontArial5Regular, drawBrush, new Point(125, (int)y));

                    text = product.Price.ToString();
                    e.Graphics.DrawString(text, drawFontArial5Regular, drawBrush, new Point(150, (int)y));

                    if (productCount != GetProducts().Count - 1)
                        y += e.Graphics.MeasureString(text, drawFontArial5Regular).Height + 2;
                }
                else
                {
                    text = product.Name;
                    e.Graphics.DrawString(text, drawFontArial5Regular, drawBrush, new Point(0, (int)y));

                    text = product.Amount.ToString();
                    e.Graphics.DrawString(text, drawFontArial5Regular, drawBrush, new Point(125, (int)y));

                    text = product.Price.ToString();
                    e.Graphics.DrawString(text, drawFontArial5Regular, drawBrush, new Point(150, (int)y));

                    if (productCount != GetProducts().Count - 1)
                        y += e.Graphics.MeasureString(text, drawFontArial5Regular).Height + 2;
                }
            }

            y += 10;
            e.Graphics.DrawLine(Pens.Black, 0, y, 225, y);
            y += 10;

            text = "Total Price : 125,75";
            e.Graphics.DrawString(text, drawFontArial10Bold, drawBrush, new Point(0, (int)y));
            y += e.Graphics.MeasureString(text, drawFontArial10Bold).Height;

            text = "Payment Type : Cash";
            e.Graphics.DrawString(text, drawFontArial10Bold, drawBrush, new Point(0, (int)y));
            y += e.Graphics.MeasureString(text, drawFontArial10Bold).Height + 5;


            text = "Aydın";
            e.Graphics.DrawString(text, drawFontArial10Regular, drawBrush, new Point(50, (int)y));
            y += e.Graphics.MeasureString(text, drawFontArial10Regular).Height + 5;
        }

        public static List<Product> GetProducts()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Name = "Coca Cola 2.5 L",
                    Amount = 2,
                    Price = Convert.ToDecimal("16,5")
                },
                new Product
                {
                    Name = "Nescafe",
                    Amount = 1,
                    Price = Convert.ToDecimal("5")
                },
                new Product
                {
                    Name = "Türk Kahvesi",
                    Amount = 1,
                    Price = Convert.ToDecimal("8,5")
                },
                new Product
                {
                    Name = "VERNEL YUMUŞATICI  5 KG DENİZ ESİNTİSİ",
                    Amount = 1,
                    Price = Convert.ToDecimal("24,0")
                }
            };

            return products;
        }
    }

    public class Product
    {
        public String Name { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
    }
}
