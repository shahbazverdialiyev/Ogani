using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Exceptions;
using Ogani.WebApp.Business.Services.Interfaces;
using Ogani.WebApp.DTOs.ContactDTO;

namespace Ogani.WebApp.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public async Task<IActionResult> Index() => View(await _contactService.GetAllAsync());

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                return View(await _contactService.GetByIdAsync(id));
            }
            catch (NotFoundException ex)
            {
                TempData["NotifyError"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(ContactCreateDTO contactDto)
        {
            if (!ModelState.IsValid)
                return View(contactDto);

            try
            {
                await _contactService.AddAsync(contactDto);
                TempData["NotifySuccess"] = $"Contact \"{contactDto.Title}\" was created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (BusinessValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
            catch (BusinessException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(contactDto);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                return View(await _contactService.GetForUpdateAsync(id));
            }
            catch (NotFoundException ex)
            {
                TempData["NotifyError"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Update(ContactUpdateDTO contactDto)
        {
            if (!ModelState.IsValid)
                return View(contactDto);

            try
            {
                await _contactService.UpdateAsync(contactDto);
                TempData["NotifySuccess"] = $"Contact \"{contactDto.Title}\" was updated successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (BusinessValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
            catch (NotFoundException ex)
            {
                TempData["NotifyError"] = ex.Message;

                return RedirectToAction(nameof(Index));
            }
            catch (BusinessException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(contactDto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _contactService.DeleteAsync(id);
                TempData["NotifySuccess"] = "Contact item deleted successfully.";
            }
            catch (NotFoundException ex)
            {
                TempData["NotifyError"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
