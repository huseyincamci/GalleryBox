using Galeri.Web.Models;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Galeri.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly GaleriEntities _context = new GaleriEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload()
        {
            return View();
        }

        public ActionResult FileUpload()
        {
            HttpPostedFileBase file = HttpContext.Request.Files[0];
            if (file != null)
            {
                using (BinaryReader reader = new BinaryReader(file.InputStream))
                {
                    byte[] value = reader.ReadBytes(file.ContentLength);
                    if (Session["value"] == null)
                    {
                        Session["value"] = value;
                    }
                    else
                    {
                        Session["value"] = ByteBirlestir((byte[])Session["value"], value);
                    }


                    if (10000 > file.ContentLength)
                    {
                        _context.Dosya.Add(new Dosya()
                        {
                            Deger = (byte[])Session["value"],
                            DosyaAdi = file.FileName,
                            DosyaBoyutu = ((byte[])Session["value"]).Length.ToString(),
                            DosyaTipi = file.ContentType,
                            KayitTarihi = DateTime.Now
                        });
                        _context.SaveChanges();
                    }
                    Session["value"] = null;
                }
            }
            return Json("");
        }

        private static byte[] ByteBirlestir(byte[] previous, byte[] next)
        {
            byte[] output = new byte[previous.Length + next.Length];
            Buffer.BlockCopy(previous, 0, output, 0, previous.Length);
            Buffer.BlockCopy(next, 0, output, previous.Length, next.Length);
            return output;
        }
    }
}