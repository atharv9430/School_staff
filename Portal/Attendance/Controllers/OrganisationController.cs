using Attendance.Models;
using Attendance.Repository;
using Microsoft.AspNetCore.Mvc;
using static iTextSharp.text.pdf.AcroFields;

namespace Attendance.Controllers
{
    public class OrganisationController : Controller
    { 
        private readonly OrganisationRepository _repository;


        public OrganisationController(OrganisationRepository repository)
        {
            _repository = repository;
        }
        public async Task<ActionResult> Index()
        {
            List<OrganisationList> organisations = await _repository.GetOrganisationsAsync();
            return View(organisations);
        }
        public IActionResult AddOrganisation()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddOrganisationDetail(AddOrganisation obj)
        {
            bool result = _repository.OrganisationRegistration(obj);
            if (result == true)
            {
                return RedirectToAction("Index");

            }
            else
            {
                return RedirectToAction("AddOrganisation");
            }
        }
        public async Task<ActionResult> EditOrganisation(int id)
        {
            OrganisationList organisation = await _repository.GetOrganisationByIdAsync(id);
            if (organisation == null)
                return NotFound(); // Use NotFound() instead of HttpNotFound()

            return View(organisation);
        }

        [HttpPost]  
        public async Task<ActionResult> EditOrganisationDetails(OrganisationList organisation)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("EditOrganisation", new { id = organisation.id });

            bool isUpdated = await _repository.UpdateOrganisationAsync(organisation);

            if (isUpdated)
            {
                TempData["SuccessMessage"] = "Organisation updated successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Failed to update organisation.");
                return RedirectToAction("EditOrganisation", new { id = organisation.id });
            }
        }
    }
}
