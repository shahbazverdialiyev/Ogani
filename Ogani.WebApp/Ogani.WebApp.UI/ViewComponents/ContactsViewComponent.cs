using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Services.Interfaces;

namespace Ogani.WebApp.UI.ViewComponents
{
    public class ContactsViewComponent : ViewComponent
    {
        private readonly IContactService _contactService;

        public ContactsViewComponent(IContactService contactService)
        {
            _contactService = contactService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string viewName = "Default")
        {
            return viewName switch
            {
                "Default" => View(viewName, await _contactService.GetEmailAsync()),
                "HeroPhone" => View(viewName, await _contactService.GetPhoneAsync()),
                _ => View(viewName, await _contactService.GetContactsForUIAsync())
            };
        }
    }
}
