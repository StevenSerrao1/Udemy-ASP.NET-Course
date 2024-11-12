using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System.Text.Json;

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
        public async Task<IActionResult> Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderEnum sortOrder = SortOrderEnum.Ascending)
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

            List<PersonResponse> allPersons = await _personsService.GetFilteredPersons(searchBy, searchString);

            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            // Sort
            List<PersonResponse> sortedPersons = await _personsService.GetSortedPersons(allPersons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();

            return View(sortedPersons);
        }

        // Executes when the user clicks on Create Person link, while opening the create view
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<CountryResponse> countries = await _countriesService.GetAllCountries();

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
        public async Task<IActionResult> Create(PersonAddRequest personAddRequest)
        {
            if(!ModelState.IsValid)
            {
                List<CountryResponse> countries = await _countriesService.GetAllCountries();
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
            PersonResponse personResponse = await _personsService.AddPerson(personAddRequest);

            // Return to home page
            return RedirectToAction("Index", "Persons");
        }

        [HttpGet]
        [Route("[action]/{personId}")] // Eg: personId = 1 / "~/edit/1"
        public async Task<IActionResult> Edit(Guid personId)
        {
            PersonResponse? personResponse = await _personsService.GetPersonByPersonId(personId)!;

            if (personResponse is null)
            {
                return RedirectToAction("Index");
            }

            PersonUpdateRequest personUpdate = personResponse.ToPersonUpdateRequest();

            List<CountryResponse> countries = await _countriesService.GetAllCountries();
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
        public async Task<IActionResult> Edit(PersonUpdateRequest personUpdateRequest)
        {

            PersonResponse? person = await _personsService.GetPersonByPersonId(personUpdateRequest.PersonId)!;

            if (person is null)
            {
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                List<CountryResponse> countries = await _countriesService.GetAllCountries();
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

            PersonResponse? updatedPerson = await _personsService.UpdatePerson(personUpdateRequest);

            return RedirectToAction("Index");
        }


        [HttpGet]
        [Route("[action]/{personId}")]
        public async Task<IActionResult> Delete(Guid? personId)
        {
            PersonResponse? personResponse = await _personsService.GetPersonByPersonId(personId)!;
            if(personResponse is null)
            {
                return RedirectToAction("Index");
            }

            return View(personResponse);
        }

        [HttpPost]
        [Route("[action]/{personId}")]
        public async Task<IActionResult> Delete(PersonUpdateRequest person)
        {
            PersonResponse? personResponse = await _personsService.GetPersonByPersonId(person.PersonId)!;
            if (personResponse is null)
            {
                return RedirectToAction("Index");
            }

            await _personsService.DeletePerson(person.PersonId);

            return RedirectToAction("Index");
        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsPDF()
        {
            List<PersonResponse> people = await _personsService.GetAllPersons();

            return new ViewAsPdf("PersonsPDF", people)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                {
                    Top = 20,
                    Right = 20,
                    Left = 20,
                    Bottom = 20
                },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

        [Route("[action]")]
        public async Task<IActionResult> AllPeopleCSV()
        {
           MemoryStream ms = await _personsService.GetAllPeopleCSV();

           return File(ms, "application/octet-stream", "allpeople.csv");
        }

    }
}
