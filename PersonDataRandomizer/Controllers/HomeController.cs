using PersonDataRandomizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PersonDataRandomizer.Controllers
{
	public class HomeController : Controller
	{
		private PersonDataContext db = new PersonDataContext();
        private static Random random = new Random();
        private static int country;
        private static int seed = random.Next();

        public ActionResult Index()
        {
            country = Request.QueryString["country"] == null ? 1 : Int32.Parse(Request.QueryString["country"]);
            seed = Request.QueryString["seed"] != null ? Int32.Parse(Request.QueryString["seed"]) : seed;
            int page = Request.QueryString["page"] == null ? 1 : Int32.Parse(Request.QueryString["page"]);
            if (Request.IsAjaxRequest())
            {
                return PartialView("PeopleSet", GetPeopleSet(page));
            }
            return View(GetPeopleSet(page));
        }

        private List<ResultPerson> GetPeopleSet(int page)
        {
            random = new Random(seed + page);
            List<ResultPerson> people = new List<ResultPerson>();
            for (int i = 0; i < 10; i++)
            {
                ResultPerson resultPerson = new ResultPerson();
                switch (country)
                {
                    case 1:
                        RussianPerson rp = (RussianPerson)GetPerson();
                        resultPerson.FullName = rp.WriteFullName();
                        resultPerson.Address = rp.WriteAddress();
                        resultPerson.PhoneNumber = rp.WritePhoneNumber();
                        break;
                    case 2:
                        MexicanPerson mp = (MexicanPerson)GetPerson();
                        resultPerson.FullName = mp.WriteFullName();
                        resultPerson.Address = mp.WriteAddress();
                        resultPerson.PhoneNumber = mp.WritePhoneNumber();
                        break;
                    case 3:
                        BritishPerson bp = (BritishPerson)GetPerson();
                        resultPerson.FullName = bp.WriteFullName();
                        resultPerson.Address = bp.WriteAddress();
                        resultPerson.PhoneNumber = bp.WritePhoneNumber();
                        break;
                }
                people.Add(resultPerson);
            }
            return people;
        }

        private Person GetPerson()
        {
            Person person = new Person(random.Next());
            switch (country)
            {
                case 1:
                    person = new RussianPerson(random.Next());
                    break;
                case 2:
                    person = new MexicanPerson(random.Next());
                    break;
                case 3:
                    person = new BritishPerson(random.Next());
                    break;
            }
            person.CountryId = country;
            person.Gender = GetGender();
            person = GetFullName(person);
            person = GetDistrictData(person);
            person = GetRegionData(person);
            person = GetCityData(person);
            person = GetRoadData(person);
            return person;
        }

        private Person GetFullName(Person person)
        {
            switch (person.CountryId)
            {
                case 2:
                    person.FirstName = GetFirstName(person.Gender, person.CountryId);
                    if (random.Next(2) == 1) person.FirstName += " " + GetFirstName((Gender)random.Next(2), person.CountryId);
                    person.LastName = GetLastName(person.Gender, person.CountryId);
                    if (random.Next(2) == 1) person.LastName += " " + GetLastName(person.Gender, person.CountryId);
                    break;
                case 3:
                    person.FirstName = GetFirstName(person.Gender, person.CountryId);
                    if (random.Next(2) == 1) person.FirstName += " " + (random.Next(2) == 1 ? GetFirstName(person.Gender, person.CountryId) : GetFirstName(person.Gender, person.CountryId).Substring(0, 1));
                    person.LastName = GetLastName(person.Gender, person.CountryId);
                    break;
                default:
                    person.FirstName = GetFirstName(person.Gender, person.CountryId);
                    person.MiddleName = GetMiddleName(person.Gender, person.CountryId);
                    person.LastName = GetLastName(person.Gender, person.CountryId);
                    break;
            }
            return person;
        }

        private string GetFirstName(Gender gender, int countryId)
        {
            if (gender == Gender.Female)
            {
                var femaleFirstNames = db.FemaleFirstNames.Where(l => l.Country.Id == countryId);
                return femaleFirstNames.OrderBy(f => f.Id).Skip(random.Next(femaleFirstNames.Count() / random.Next(1, 21))).First().Name;
            }
            else
            {
                var maleFirstNames = db.MaleFirstNames.Where(l => l.Country.Id == countryId);
                return maleFirstNames.OrderBy(f => f.Id).Skip(random.Next(maleFirstNames.Count() / random.Next(1, 21))).First().Name;
            }
        }

        private string GetMiddleName(Gender gender, int countryId)
        {
            var middleNames = db.MiddleNames.Where(l => l.Country.Id == countryId);
            string middleName = middleNames.OrderBy(m => m.Id).Skip(random.Next(middleNames.Count() / random.Next(1, 21))).First().Name;
            return gender == Gender.Female ? RussianNamePostfixChange(middleName + "mn") : middleName;
        }

        private string GetLastName(Gender gender, int countryId)
        {
            var lastNames = db.LastNames.Where(l => l.Country.Id == countryId);
            LastName lastName = lastNames.OrderBy(l => l.Id).Skip(random.Next(lastNames.Count())).First();
            if (countryId == 1 && gender == Gender.Female) return RussianNamePostfixChange(lastName.Name);
            return lastName.Name;
        }

        private string RussianNamePostfixChange(string name)
        {
            switch (name.Substring(name.Length - 2))
            {
                case "ов":
                case "ин":
                case "ев":
                case "ёв":
                    return name + 'а';
                case "ий":
                case "ый":
                case "ой":
                    return name.Substring(0, name.Length - 2) + "ая";
                case "mn":
                    return name.Substring(0, name.Length - 4) + "на";
                default: return name;
            }
        }

        private Person GetDistrictData(Person person)
        {
            var districts = db.Districts.Where(d => d.Country.Id == person.CountryId).OrderBy(d => d.Id);
            District district = districts.Skip(random.Next(districts.Count())).First();
            person.DistrictId = district.Id;
            if (person.CountryId < 3) person.PhoneCode = GetPhoneCode(person.DistrictId);
            return person;
        }

        private Person GetRegionData(Person person)
        {
            var regions = db.Regions.Where(r => r.District.Id == person.DistrictId).OrderBy(r => r.Id);
            var region = regions.Skip(random.Next(regions.Count())).First();
            person.RegionId = region.Id;
            person.RegionName = region.RegionName;
            if (person.CountryId < 3) person.ZipCode = GetZipCode(person.RegionId);
            return person;
        }

        private string GetZipCode(int regionId)
        {
            var region = db.Regions.First(r => r.Id == regionId);
            string[] codes = region.ZipCode.Split('|');
            return codes.Skip(random.Next(codes.Length)).First();
        }

        private string GetPhoneCode(int districtId)
        {
            var phoneCodes = db.PhoneCodes.Where(p => p.District.Id == districtId).OrderBy(p => p.Id);
            return phoneCodes.Skip(random.Next(phoneCodes.Count())).First().Code;
        }

        private Person GetCityData(Person person)
        {
            if (person.CountryId == 1 && person.HasAreas && random.Next(3) == 0) return GetAreaData(person);
            var cities = db.Cities.Where(c => c.Region.Id == person.RegionId).OrderBy(c => c.Id);
            City city = cities.Skip(random.Next(cities.Count())).First();
            person.City = city.CityName;
            if (person.CountryId == 3)
            {
                person.ZipCode = city.ZipCode;
                person.PhoneCode = city.PhoneCode;
            }
            return person;
        }

        private Person GetAreaData(Person person)
        {
            var areas = db.Areas.Where(a => a.Region.Id == person.RegionId).OrderBy(a => a.Id);
            person.Area = areas.Skip(random.Next(areas.Count())).First().AreaName;
            var villages = db.Vilages.Where(v => v.Country.Id == person.CountryId).OrderBy(v => v.Id);
            person.Village = villages.Skip(random.Next(villages.Count() / random.Next(1, 11))).First().VilageName;
            return person;
        }

        private Person GetRoadData(Person person)
        {
            var roads = db.Roads.Where(r => r.Country.Id == person.CountryId).OrderBy(r => r.Id);
            Road road = roads.Skip(random.Next(roads.Count() / random.Next(1, person.CountryId > 2 ? 2 : 10))).First();
            var roadPrefixes = db.RoadPrefixes.Where(r => r.Road.Id == road.Id).OrderBy(r => r.Id);
            person.RoadPrefix = roadPrefixes.Skip(random.Next(roadPrefixes.Count())).First().Prefix;
            var roadNames = db.RoadNames.Where(r => r.Road.Id == road.Id).OrderBy(r => r.Id);
            person.RoadName = roadNames.Skip(random.Next(roadNames.Count() / random.Next(1, person.CountryId > 2 ? 2 : 10))).First().Name;
            return person;
        }

        private Gender GetGender()
        {
            return (Gender)random.Next(2);
        }
    }
}