using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            ViewBag.Countries = countries
                .Select(temp => new SelectListItem()
                {
                    Text = temp.CountryName ?? "Unknown", // Fallback if CountryName is null
                    Value = temp.CountryID.ToString() // Guid to string
                })
                .ToList();

            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult Create(PersonAddRequest personAddRequest)
        {
            if(!ModelState.IsValid)
            {
                List<CountryResponse> countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries
                    .Select(temp => new SelectListItem()
                    {
                        Text = temp.CountryName ?? "Unknown", // Fallback if CountryName is null
                        Value = temp.CountryID.ToString() // Guid to string
                    })
                    .ToList();

                ViewBag.Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return View();
            }

            // Call the service method
            PersonResponse personResponse = _personsService.AddPerson(personAddRequest);

            // Return to home page
            return RedirectToAction("Index", "Persons");
        }

        [HttpGet]
        [Route("[action]/{personId}")] // Eg: personId = 1 / "~/edit/1"
        public IActionResult Edit(Guid personId)
        {
            PersonResponse? personResponse = _personsService.GetPersonByPersonId(personId);

            if (personResponse is null)
            {
                return RedirectToAction("Index");
            }

            PersonUpdateRequest personUpdate = personResponse.ToPersonUpdateRequest();

            List<CountryResponse> countries = _countriesService.GetAllCountries();
            ViewBag.Countries = countries
                .Select(temp => new SelectListItem()
                {
                    Text = temp.CountryName ?? "Unknown", // Fallback if CountryName is null
                    Value = temp.CountryID.ToString() // Guid to string
                })
                .ToList();

            return View(personUpdate);
        }

        [HttpPost]
        [Route("[action]/{personId}")]
        public IActionResult Edit(PersonUpdateRequest personUpdateRequest)
        {

            PersonResponse? person = _personsService.GetPersonByPersonId(personUpdateRequest.PersonId);

            if (person is null)
            {
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                List<CountryResponse> countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries
                    .Select(temp => new SelectListItem()
                    {
                        Text = temp.CountryName ?? "Unknown", // Fallback if CountryName is null
                        Value = temp.CountryID.ToString() // Guid to string
                    })
                    .ToList();

                ViewBag.Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return View(person.ToPersonUpdateRequest());
            }

            PersonResponse? updatedPerson = _personsService.UpdatePerson(personUpdateRequest);

            return RedirectToAction("Index");
        }


        [HttpGet]
        [Route("[action]/{personId}")]
        public IActionResult Delete(Guid? personId)
        {
            PersonResponse? personResponse = _personsService.GetPersonByPersonId(personId);
            if(personResponse is null)
            {
                return RedirectToAction("Index");
            }

            return View(personResponse);
        }

        [HttpPost]
        [Route("[action]/{personId}")]
        public IActionResult Delete(PersonUpdateRequest person)
        {
            PersonResponse? personResponse = _personsService.GetPersonByPersonId(person.PersonId);
            if (personResponse is null)
            {
                return RedirectToAction("Index");
            }

            _personsService.DeletePerson(person.PersonId);

            return RedirectToAction("Index");
        }

    }
}
