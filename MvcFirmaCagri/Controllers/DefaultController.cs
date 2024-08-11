using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcFirmaCagri.Models.Entity;

namespace MvcFirmaCagri.Controllers
{
    [Authorize]
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        DbisTakipEntities db = new DbisTakipEntities();
        public ActionResult AktifCagrilar()
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail.ToString()).Select(y => y.ID).FirstOrDefault();

            var cagrilar = db.TblCagrilar.Where(x => x.Durum == true && x.CagriFirma == id).ToList();
            return View(cagrilar);
        }
        public ActionResult PasifCagrilar()
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail.ToString()).Select(y => y.ID).FirstOrDefault();

            var cagrilar = db.TblCagrilar.Where(x => x.Durum == false && x.CagriFirma == id).ToList();
            return View(cagrilar);
        }

        [HttpGet]
        public ActionResult YeniCagri()
        {
            return View();
        }

        [HttpPost]        
        public ActionResult YeniCagri(TblCagrilar p)
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail.ToString()).Select(y => y.ID).FirstOrDefault();

            p.Durum = true;
            p.Tarih = DateTime.Parse(DateTime.Now.ToShortDateString());
            p.CagriFirma = id;
            db.TblCagrilar.Add(p);
            db.SaveChanges();
            return RedirectToAction("AktifCagrilar");
        }

        public ActionResult CagriDetay(int id)
        {
            var cagri = db.TblCagriDetay.Where(x => x.Cagri == id).ToList();
            return View(cagri);
        }

        public ActionResult CagriGetir(int id)
        {
            var cagri = db.TblCagrilar.Find(id);
            return View("CagriGetir", cagri);
        }

        public ActionResult CagriDuzenle(TblCagrilar p)
        {
            var cagri = db.TblCagrilar.Find(p.ID);
            cagri.Konu = p.Konu;
            cagri.Aciklama = p.Aciklama;
            db.SaveChanges();
            return RedirectToAction("AktifCagrilar");
        }
        [HttpGet]
        public ActionResult ProfilDuzenle()
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail.ToString()).Select(y => y.ID).FirstOrDefault();

            var profil = db.TblFirmalar.Where(x=> x.ID == id).FirstOrDefault();
            return View(profil);
        }

        public ActionResult Anasayfa()
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail.ToString()).Select(y => y.ID).FirstOrDefault();
            var toplamCagri = db.TblCagrilar.Where(x => x.CagriFirma == id).Count();
            ViewBag.cagriTotal = toplamCagri;
            var aktifCagri = db.TblCagrilar.Where(x => x.CagriFirma == id && x.Durum == true).Count();
            ViewBag.cagriAktif = aktifCagri;
            var pasifCagri = db.TblCagrilar.Where(x => x.CagriFirma == id && x.Durum == false).Count();
            ViewBag.cagriPasif = pasifCagri;
            var sirketSektor = db.TblFirmalar.Where(x => x.ID == id).Select(y => y.Sektör).FirstOrDefault();
            ViewBag.sektor = sirketSektor;
            var yetkili = db.TblFirmalar.Where(x => x.ID == id).Select(y => y.Yetkili).FirstOrDefault();
            ViewBag.yetkili = yetkili;

            return View();
        }

        public PartialViewResult Partial1()
        {
            var mail = (string)Session["Mail"];
            var mesajar = db.TblMesajlar.Where(x => x.Alici == mail).ToList();
            return PartialView(mesajar);
        }
    }

}