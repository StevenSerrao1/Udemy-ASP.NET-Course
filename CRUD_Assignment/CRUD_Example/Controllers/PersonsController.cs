using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUD_Example.Controllers
{
    [Route("[controller]")]
    public class PersonsController : Controller
    {
        // private fields to reference services
        private readonly ICountriesService _countriesService;
        private readonly IPersonService _personsService;

        public PersonsController(IPersonService person, ICountriesService country)
        {
            _personsService = person;
            _countriesService = country;
        }

        [Route("[action]")]
        [Route("/")]
        public IActionResult Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderEnum sortOrder = SortOrderEnum.Ascending)
        {
            // Search
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(PersonResponse.PersonName), "Name" },
                { nameof(PersonResponse.PersonEmail), "Email" },
                { nameof(PersonResponse.DOB), "Date of Birth" },
                { nameof(PersonResponse.Gender), "Gender" },
                { nameof(PersonResponse.PersonAddress), "Address" },
                { nameof(PersonResponse.CountryId), "Country ID" },
                { nameof(PersonResponse.ReceivesNewsletters), "Receives Newsletters" },
            };

            List<PersonResponse> allPersons = _personsService.GetFilteredPersons(searchBy, searchString);

            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            // Sort
            List<PersonResponse> sortedPersons = _personsService.GetSortedPersons(allPersons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();

            return View(sortedPersons);
        }

        // Executes when the user clicks on Create Person link, while opening the create view
        [Route("[action]")]
        [HttpGet]
        public IActionResult Create()
        {
            List<CountryResponse> countries = _countriesService.GetAllCountries();
            ViewBag.Countries = countries;

            return View();
        }

        [Route("create")]
        [HttpPost]
        public IActionResult Create(PersonAddRequest personAddRequest)
        {
            if(!ModelState.IsValid)
            {
                List<CountryResponse> countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries;
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }

            // Call the service method
            PersonResponse personResponse = _personsService.AddPerson(personAddRequest);

            // Return to home page
            return RedirectToAction("Index", "Persons");
        }
    }
}
