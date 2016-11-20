using Hemlock.DAL;
using Hemlock.Handlers;
using Hemlock.Models;
using Hemlock.Models.Enum;
using Hemlock.Models.Interfaces;
using System.Web.Mvc;
using Hemlock.Controllers.ActionFilters;
using System.Linq;
using Hemlock.Models.Interfaces.Repositories;
using System;
using System.Data.Entity;

namespace Hemlock.Controllers
{
    public class SREDCategoryController : Controller
    {
        private ISREDContext _Context;
        private ISREDCategoryRepository _SREDCategoryRepository;

        public SREDCategoryController()
        {
            _Context = new SREDContext();
            _SREDCategoryRepository = new SREDCategoryRepository(_Context);
        }

        public JsonResult GetCategoryById(Guid id)
        {
            var category = _SREDCategoryRepository.GetSREDCategoryByID(id);

            return Json(new { SREDCategoryID = category.SREDCategoryID, CategoryName = category.CategoryName }, JsonRequestBehavior.AllowGet);
        }
    }
}