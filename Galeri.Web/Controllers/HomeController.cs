using Galeri.Web.Models;
using Galeri.Web.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Galeri.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly GaleriEntities _context = new GaleriEntities();

        public ActionResult Index()
        {
            using (GaleriEntities context = new GaleriEntities())
            {
                var dosya = context.Dosya.OrderByDescending(d => d.Id).ToList();
                return View(dosya);
            }
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
                        Session["value"] = UtilityManager.ByteBirlestir((byte[])Session["value"], value);
                    }


                    if (10000 > file.ContentLength)
                    {
                        _context.Dosya.Add(new Dosya()
                        {
                            Deger = (byte[])Session["value"],
                            DosyaAdi = file.FileName,
                            DosyaBoyutu = ((byte[])Session["value"]).Length.ToString(),
                            DosyaTipi = file.ContentType,
                            Ikon = UtilityManager.SetIcon(file.ContentType),
                            BoyutKisaltma = UtilityManager.BytesToString(((byte[])Session["value"]).Length),
                            Renk = UtilityManager.SetClass(file.ContentType),
                            KayitTarihi = DateTime.Now
                        });
                        _context.SaveChanges();
                        Session["value"] = null;
                    }
                }
            }
            return Json("");
        }
    }
}