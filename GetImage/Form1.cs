using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetImage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string saveFolder = "D:\\get_pdf_photo\\ProposalPDF";
            if (!Directory.Exists(saveFolder))
            {
                Directory.CreateDirectory(saveFolder);
            }

            string Image = "D:\\get_pdf_photo\\PdfImage\\";
            if (!Directory.Exists(Image))
            {
                Directory.CreateDirectory(Image);
            }
            string[] imageFiles = Directory.GetFiles(saveFolder, "*.pdf");

            for (int i = 0; i < imageFiles.Length; i++)
            {
                string fileName = Path.GetFileName(imageFiles[i]);
                Pdf2ImgConversion( imageFiles[i], Image, fileName);
            }
        }
        public void Pdf2ImgConversion(string filePath, string imagePath,string fileName)
        {
            using (var document = PdfDocument.Load(filePath))
            {
                int lastPageIndex = document.PageCount - 1;

                using (var image = document.Render(lastPageIndex, 300, 300, true))
                {
                    string outputImagePath = Path.Combine(imagePath, fileName);
                    string reqPath = outputImagePath + ".png";

                    image.Save(reqPath, System.Drawing.Imaging.ImageFormat.Png);
                    string NameOnly = fileName.Split('_')[0];
                    CropImage(reqPath, NameOnly);

                }
            }
        }
        public void CropImage(string path, string fileName)
        {
            try
            {
                using (System.Drawing.Image originalImage = System.Drawing.Image.FromFile(path))
                {
                    int x = 430;
                    int y = 46;
                    int width = 120;
                    int height = 120;

                    if (y + height > originalImage.Height)
                    {
                        height = originalImage.Height - y;
                    }

                    Rectangle cropRect = new Rectangle(x, y, width, height);
                    //Bitmap croppedImage = new Bitmap(cropRect.Width, cropRect.Height);
                    Bitmap croppedImage = new Bitmap(300, 300);

                    using (Graphics g = Graphics.FromImage(croppedImage))
                    {
                        g.DrawImage(originalImage, new Rectangle(0, 0, croppedImage.Width, croppedImage.Height), cropRect, GraphicsUnit.Pixel);
                    }

                    string savePath = "D:\\get_pdf_photo\\CroppedImages\\";
                    string cropImageFolder = Path.Combine(savePath, fileName);
                    if (!Directory.Exists(savePath))
                    {
                        Directory.CreateDirectory(savePath);
                    }
                    if (!Directory.Exists(cropImageFolder))
                    {
                        Directory.CreateDirectory(cropImageFolder);
                    }

                    string reqImgName = fileName + "_PHOTO_26";
                    string fullPath = Path.Combine(cropImageFolder, reqImgName);

                    croppedImage.Save(fullPath + ".Jpeg", ImageFormat.Jpeg);

                    return;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return;
            }
        }
    }
}
