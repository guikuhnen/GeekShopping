using RestWithASPNET.Model;
using System;

namespace RestWithASPNET.Services.Implementations
{
	public class PersonService : IPersonService
	{
		private volatile int _count;
		private static Random _random = new Random();

		public Person Create(Person person)
		{
			return person;
		}

		public void Delete(int ind)
		{

		}

		public ICollection<Person> GetAll()
		{
			List<Person> listPeople = new List<Person>();

			for (int i = 0; i < 8; i++)
			{
				Person person = MockPerson(i);
				listPeople.Add(person);
			}

			return listPeople;
		}

		public Person GetById(int ind)
		{
			return new Person(IncrementAndGet(), "Guilherme", "Kuhnen", "Blumenau - SC - BR", "Male");
		}

		public Person Update(Person person)
		{
			return person;
		}

		private Person MockPerson(int i)
		{
			return new Person(IncrementAndGet(), RandomString(4), RandomString(8), RandomString(12), "Male");
		}

		public static string RandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[_random.Next(s.Length)]).ToArray());
		}

		private int IncrementAndGet()
		{
			return Interlocked.Increment(ref _count);
		}
	}
}
