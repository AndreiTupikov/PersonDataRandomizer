using System;
using System.Text;

namespace PersonDataRandomizer.Models
{
    public class Person
    {
        public string Id { get; set; }
        public int CountryId { get; set; }
        public Gender Gender { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int DistrictId { get; set; }
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public string ZipCode { get; set; }
        public string PhoneCode { get; set; }
        public bool HasAreas { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Village { get; set; }
        public string RoadPrefix { get; set; }
        public string RoadName { get; set; }
        public int House { get; set; }
        public int? Appartment { get; set; }
        public Random random { get; set; }
        public Person(int seed)
        {
            random = new Random(seed);
        }
        public void GetHouseAndAppartment(int k)
        {
            int huoseMax = City == null ? k / 5 : k;
            int appartmentChance = City == null ? 3 : 1;
            int appartmentMax = City == null ? k / 5 : k / 2;
            House = random.Next(huoseMax);
            if (random.Next(5) > appartmentChance)
            {
                Appartment = random.Next(appartmentMax);
            }
        }
        public char RandomLetter()
        {
            return (char)random.Next(65, 91);
        }
        public int RandomNumber()
        {
            return random.Next(0, 10);
        }
        public override string ToString()
        {
            return "Выберите регион";
        }
    }

    public class RussianPerson : Person
    {
        public RussianPerson(int seed) : base(seed)
        {
        }
        public override string ToString()
        {
            StringBuilder resultString = new StringBuilder();
            resultString.Append(WriteFullName()).Append(", ");
            resultString.Append(WriteAddress()).Append(", ");
            resultString.Append(WritePhoneNumber());
            return resultString.ToString();
        }
        public string WriteFullName()
        {
            StringBuilder fullName = new StringBuilder();
            bool startsFromFirstName = random.Next(3) == 1;
            fullName.Append(startsFromFirstName ? FirstName : LastName).Append(" ")
                .Append(startsFromFirstName && random.Next(2) == 1 ? MiddleName + " " : "")
                .Append(startsFromFirstName ? LastName : FirstName)
                .Append(!startsFromFirstName && random.Next(2) == 1 ? " " + MiddleName : "");
            return fullName.ToString();
        }
        public string WriteAddress()
        {
            GetHouseAndAppartment(350);
            StringBuilder address = new StringBuilder();
            address.Append(WriteZipCode()).Append(", ");
            if (Village != null || random.Next(2) == 1)
            {
                string prefixChange = random.Next(2) == 1 ? RegionName.Replace("область", "обл.").Replace("республика", "респ.").Replace("Республика", "Респ.").Replace("автономная область", "АО").Replace("автономный округ", "АО") : RegionName;
                address.Append(prefixChange).Append(", ");
            } 
            if (Village != null) address.Append(Area.Contains("район") && random.Next(2) == 1 ? Area : Area.Replace("район", "р-н")).Append(", ").Append(WriteVillage());
            else address.Append("г. ").Append(City);
            address.Append(", ").Append(RoadPrefix).Append(" ").Append(RoadName).Append(", ");
            if (Appartment != null && random.Next(2) == 1) address.Append(House).Append("-").Append(Appartment);
            else
            {
                address.Append(random.Next(2) == 1 ? "д." : "").Append(House);
                if (Appartment != null && random.Next(4) == 1) address.Append(" корп.").Append(random.Next(1, 6));
                address.Append(Appartment != null ? " кв." + Appartment : "");
            }
            return address.ToString();
        }
        public string WriteZipCode()
        {
            StringBuilder zipCode = new StringBuilder();
            zipCode.Append(ZipCode).Append(random.Next(0, 5)).Append(RandomNumber()).Append(RandomNumber());
            return zipCode.ToString();
        }
        public string WriteVillage()
        {
            StringBuilder village = new StringBuilder();
            string[] prefixes = { "д.", "дер.", "деревня", "пос.", "п.", "поселок", "с.", "село", "пгт."};
            string postfix = Village.Substring(Village.Length - 2);
            if (postfix == "ая" || postfix == "яя") village.Append(prefixes[random.Next(0, 3)]);
            else if (postfix == "ий" || postfix == "ый") village.Append(prefixes[random.Next(3, 6)]);
            else if (postfix == "ее" || postfix == "ое") village.Append(prefixes[random.Next(6, 8)]);
            else village.Append(prefixes[random.Next(9)]);
            village.Append(" ").Append(Village);
            return village.ToString();
        }
        public string WritePhoneNumber()
        {
            StringBuilder phoneNumber = new StringBuilder();
            if (random.Next(2) == 1) phoneNumber.Append(random.Next(2) == 1 ? "т." : "тел.");
            phoneNumber.Append(random.Next(2) == 1 ? "+7" : "8");
            string codeseparator = new string[] { "(", "-", " ", "" }[random.Next(4)];
            phoneNumber.Append(codeseparator).Append(PhoneCode);
            if (codeseparator == "(")
            {
                phoneNumber.Append(")");
                codeseparator = random.Next(2) == 1 ? "-" : " ";
            }
            else phoneNumber.Append(codeseparator);
            for (int i = 0; i < 7; i++)
            {
                phoneNumber.Append(RandomNumber());
                if (i == 2 || i == 4) phoneNumber.Append(codeseparator);
            }
            return phoneNumber.ToString();
        }
    }
    public class MexicanPerson : Person
    {
        public MexicanPerson(int seed) : base(seed)
        {
        }
        public override string ToString()
        {
            
            StringBuilder resultString = new StringBuilder();
            resultString.Append(WriteFullName()).Append(", ");
            resultString.Append(WriteAddress()).Append(", ");
            resultString.Append(WritePhoneNumber());
            return resultString.ToString();
        }
        public string WriteFullName()
        {
            StringBuilder fullName = new StringBuilder();
            if (random.Next(5) == 1) fullName.Append(Gender == Gender.Male ? "Sr. " : "Sra. ");
            fullName.Append(FirstName).Append(" ").Append(LastName);
            return fullName.ToString();
        }
        public string WriteAddress()
        {
            GetHouseAndAppartment(300);
            StringBuilder address = new StringBuilder();
            address.Append($"{RoadName} {RoadPrefix} {House}");
            if (Appartment != null) address.Append(random.Next(2) == 1 ? "-" : random.Next(2) == 1 ? " Apt." : " apt").Append(Appartment);
            address.Append($", {City}, {RegionName}, {WriteZipCode()}");
            if (random.Next(4) == 1) address.Append(", México");
            return address.ToString();
        }
        public string WriteZipCode()
        {
            StringBuilder zipCode = new StringBuilder();
            zipCode.Append(ZipCode).Append($"{RandomNumber()}{RandomNumber()}{RandomNumber()}");
            return zipCode.ToString();
        }
        public string WritePhoneNumber()
        {
            StringBuilder phoneNumber = new StringBuilder();
            string[] codeseparators = { "-", " ", "", "(" };
            string codeseparator = codeseparators[random.Next(4)];
            if (random.Next(2) == 1) phoneNumber.Append("+52").Append(codeseparator == "" ? " " : codeseparator);
            else if (codeseparator == "(") phoneNumber.Append(codeseparator);
            phoneNumber.Append(PhoneCode);
            if (codeseparator == "(")
            {
                phoneNumber.Append(")");
                codeseparator = codeseparators[random.Next(3)];
            }
            else if (codeseparator == "") phoneNumber.Append(" ");
            else phoneNumber.Append(codeseparator);
            for (int i = 0; i < 10 - PhoneCode.Length; i++)
            {
                if (i == (10 - PhoneCode.Length) / 2) phoneNumber.Append(codeseparator);
                phoneNumber.Append(RandomNumber());
            }
            return phoneNumber.ToString();
        }
    }
    public class BritishPerson : Person
    {
        public BritishPerson(int seed) : base(seed)
        {
        }
        public override string ToString()
        {
            StringBuilder resultString = new StringBuilder();
            resultString.Append(WriteFullName()).Append(", ");
            resultString.Append(WriteAddress()).Append(", ");
            resultString.Append(WritePhoneNumber());
            return resultString.ToString();
        }
        public string WriteFullName()
        {
            StringBuilder fullName = new StringBuilder();
            if (random.Next(3) == 1)
            {
                if (Gender == Gender.Male) fullName.Append("Mr. ");
                else fullName.Append(random.Next(2) == 1 ? "Mrs. " : "Ms. ");
            }
            fullName.Append(FirstName).Append(" ").Append(LastName);
            return fullName.ToString();
        }
        public string WriteAddress()
        {
            GetHouseAndAppartment(250);
            StringBuilder address = new StringBuilder();
            address.Append(House).Append(" ").Append(RoadName).Append(" ")
                .Append(RoadPrefix).Append(Appartment != null ? " apt. " + Appartment : "" );
            address.Append(", ").Append(City).Append(", ").Append(WriteZipCode())
                .Append(random.Next(2) == 1 ? ", " + RegionName : "");
            return address.ToString();
        }
        public string WriteZipCode()
        {
            StringBuilder zipCode = new StringBuilder();
            if (ZipCode != null) zipCode.Append(ZipCode);
            else 
            {
                zipCode.Append(City[0]);
                if (random.Next(2) == 1) zipCode.Append(RandomLetter());
            }
            zipCode.Append(RandomNumber());
            if (random.Next(2) == 1)
            {
                if (random.Next(2) == 1) zipCode.Append(RandomLetter());
                else zipCode.Append(RandomNumber());
            }
            zipCode.Append(" ").Append(RandomNumber()).Append(RandomLetter()).Append(RandomLetter());
            return zipCode.ToString();
        }
        public string WritePhoneNumber()
        {
            StringBuilder phoneNumber = new StringBuilder();
            int numberLength = 11;
            if (PhoneCode == null) PhoneCode = $"07{random.Next(100, 1000)}";
            if (random.Next(2) == 1)
            {
                phoneNumber.Append("+44 ");
                PhoneCode = PhoneCode.Substring(1);
            }
            bool brackets = random.Next(2) == 1;
            if (brackets) phoneNumber.Append('(');
            if (PhoneCode.Length > 5) phoneNumber.Append(PhoneCode.Substring(0, PhoneCode.Length - 2) + " " + PhoneCode.Substring(PhoneCode.Length - 2));
            else phoneNumber.Append(PhoneCode);
            numberLength -= PhoneCode.Length;
            if (brackets) phoneNumber.Append(") ");
            else phoneNumber.Append(' ');
            string separator = "";
            if (numberLength > 6) separator = " ";
            for (int i = 0; i < numberLength; i++)
            {
                if (i == numberLength/2) phoneNumber.Append(separator);
                phoneNumber.Append(random.Next(10));
            }
            return phoneNumber.ToString();
        }
    }
    public enum Gender
    {
        Female,
        Male
    }
}