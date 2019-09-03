using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Interview.Data;
using Interview.Model;

namespace Interview.Controllers
{
    public class HomeController : Controller
    {
        private InterviewDBEntities entities = new InterviewDBEntities();

        public ActionResult Index()
        {
            List<CharModel> model = new List<CharModel>();
            var list = entities.fortheads.ToList();
            model = list.ToList().Select(x => new CharModel
            {
                ID = x.ID,
                CName = x.CharName,
                CDes = x.CharDesc,
            }).OrderByDescending(x => x.CName).ToList();
            
            return View(model);
        }
        

        public ActionResult MyIndex(int? index)
        {
            int max = 5; // modify based on the actual number of records

            int currentIndex = index.GetValueOrDefault();
            if (currentIndex == 0)
            {
                ViewBag.NextIndex = 1;
            }
            else if (currentIndex >= max)
            {
                currentIndex = max;
                ViewBag.PreviousIndex = currentIndex - 1;
            }
            else
            {
                ViewBag.PreviousIndex = currentIndex - 1;
                ViewBag.NextIndex = currentIndex + 1;
            }

            CharModel objnews = new CharModel();
            CharModel model = entities.fortheads.Select(x=> new CharModel
            {
                ID = x.ID,
                CName=x.CharName,
                CDes = x.CharDesc,
            }).OrderBy(x=>x.CName).Skip(currentIndex).Take(1).FirstOrDefault();
            return View(model);
        }

        public ActionResult Descshow(string id)
        {
           
                #region data display
                int ID = Convert.ToInt32(id);
                CharModel model = new CharModel();
                model = entities.fortheads.Where(x => x.ID == ID).Select(x => new CharModel
                {
                    ID = x.ID,
                    CDes = x.CharDesc,
                }).FirstOrDefault();
                return Json(model.CDes, JsonRequestBehavior.AllowGet);
                #endregion         
        }

        public ActionResult Save()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Save( CharModel model)
        {

            #region check duplicate 
            forthead fhead = entities.fortheads.Where(x => x.CharName == model.CName).FirstOrDefault();
            #endregion
            bool isSave = false;
            if (fhead == null)
            {
                #region Save
                fhead = new forthead();
                entities.fortheads.Add(fhead);
                isSave = true;
                ViewBag.msg = "New Char Is Saved";
                #endregion
            }
            else
            {
                ViewBag.msg = "New Char Is Updated";
            }
            #region Set Value
            fhead.CharName = model.CName;
            fhead.CharDesc = model.CDes;
            #endregion
            entities.SaveChanges();
            return View();
        }

    }
}