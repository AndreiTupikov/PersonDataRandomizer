using System.Collections.Generic;
using System.Data.Entity;

namespace PersonDataRandomizer.Models
{
    public class PersonDataContext : DbContext
    {
        public DbSet<MaleFirstName> MaleFirstNames { get; set; }
        public DbSet<FemaleFirstName> FemaleFirstNames { get; set; }
        public DbSet<LastName> LastNames { get; set; }
        public DbSet<MiddleName> MiddleNames { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<PhoneCode> PhoneCodes { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Vilage> Vilages { get; set; }
        public DbSet<Road> Roads { get; set; }
        public DbSet<RoadName> RoadNames { get; set; }
        public DbSet<RoadPrefix> RoadPrefixes { get; set; }
    }
    public class MaleFirstName
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }
    }
    public class FemaleFirstName
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }
    }
    public class LastName
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }
    }
    public class MiddleName
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }
    }
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<District> Districts { get; set; }
        public ICollection<Vilage> Vilages { get; set; }
        public ICollection<Road> Roads { get; set; }
        public ICollection<MaleFirstName> MaleFirstNames { get; set; }
        public ICollection<FemaleFirstName> FemaleFirstNames { get; set; }
        public ICollection<LastName> LastNames { get; set; }
        public ICollection<MiddleName> MiddleNames { get; set; }
    }
    public class District
    {
        public int Id { get; set; }
        public ICollection<PhoneCode> PhoneCodes { get; set; }
        public ICollection<Region> Regions { get; set; }
        public ICollection<City> Cities { get; set; }
        public Country Country { get; set; }
    }
    public class PhoneCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public District District { get; set; }
    }
    public class Region
    {
        public int Id { get; set; }
        public string ZipCode { get; set; }
        public string RegionName { get; set; }
        public ICollection<City> Cities { get; set; }
        public bool HasAreas { get; set; }
        public ICollection<Area> Areas { get; set; }
        public District District { get; set; }
    }
    public class City
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public string ZipCode { get; set; }
        public string PhoneCode { get; set; }
        public Region Region { get; set; }
        public District District { get; set; }
    }
    public class Area
    {
        public int Id { get; set; }
        public string AreaName { get; set; }
        public Region Region { get; set; }
    }
    public class Vilage
    {
        public int Id { get; set; }
        public string VilageName { get; set; }
        public Country Country { get; set; }
    }
    public class Road
    {
        public int Id { get; set; }
        public virtual ICollection<RoadPrefix> RoadPrefixes { get; set; }
        public virtual ICollection<RoadName> RoadNames { get; set; }
        public Country Country { get; set; }
    }
    public class RoadPrefix
    {
        public int Id { get; set; }
        public string Prefix { get; set; }
        public Road Road { get; set; }
    }
    public class RoadName
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Road Road { get; set; }
    }
}