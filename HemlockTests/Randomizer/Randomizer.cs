using System;
using System.Linq;

namespace HemlockTests.Randomizer
{
    public class Randomizer
    {
        private Random _random = new Random();
        private DateTime _startDate = new DateTime(2009, 12, 1);

        public string RandomEmail()
        {
            return string.Format("fakePerson{0}@REDACTED_EMAIL.com", _random.Next(10000, 99999));
        }

        public string RandomString(int length)
        {
            return new string(Enumerable.Range(1, length).
                Select(x => (char)(_random.Next(97,122))).
                ToArray());
        }

        public DateTime RandomDate()
        {
            var range = (DateTime.Now - _startDate).Days;
            return _startDate.AddDays(_random.Next(range)).
                AddHours(_random.Next(0, 24)).
                AddMinutes(_random.Next(0, 60)).
                AddSeconds(_random.Next(0, 60));
        }

        public int RandomNumber(int min, int max)
        {
            var random = new Random();

            return random.Next(min, max); 
        }
    }
}
