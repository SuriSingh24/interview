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
                ID=x.ID,
                CName=x.CharName,
                CDes=x.CharDesc,
            }).ToList();
            
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
    }
}