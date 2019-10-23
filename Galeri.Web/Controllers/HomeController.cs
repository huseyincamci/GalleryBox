using Galeri.Web.Models;
using Galeri.Web.Utilities;
using System;
using System.Data.Entity;
using System.Diagnostics;
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
            if (Session["value"] != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Galeri()
        {
            using (GaleriEntities context = new GaleriEntities())
            {
                var dosya = context.Dosya.OrderByDescending(d => d.Id).ToList();
                return View(dosya);
            }
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
                        try
                        {
                            _context.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.Message);
                        }
                        Session["value"] = null;
                    }
                }
            }
            return Json("");
        }

        public ActionResult GetFileDetailById(int id)
        {
            var file = _context.Dosya.Where(d => d.Id == id).Select(d => new
            {
                d.Id,
                d.DosyaAdi,
                DosyaBoyutu = d.BoyutKisaltma,
                d.DosyaTipi,
                UrlYolu = "/home/fileview/" + d.Id,
                d.Baslik,
                d.Aciklama
            }).FirstOrDefault();
            return Json(file, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateFile(Models.File file)
        {
            try
            {
                var fileInDb = _context.Dosya.FirstOrDefault(d => d.Id == file.Id);
                if (fileInDb != null)
                {
                    fileInDb.Baslik = file.Baslik;
                    fileInDb.Aciklama = file.Aciklama;
                    _context.Entry(fileInDb).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                return Json("E", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("H", JsonRequestBehavior.AllowGet);
            }
        }

        public FileContentResult FileView(int id)
        {
            var list = _context.Dosya.FirstOrDefault(d => d.Id == id);
            return new FileContentResult(list?.Deger, list?.DosyaTipi);
        }
    }
}