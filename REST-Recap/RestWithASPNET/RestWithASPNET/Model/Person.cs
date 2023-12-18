namespace RestWithASPNET.Model
{
	public class Person
	{
		public int Id { get; private set; }
		public string FirstName { get; private set; }
		public string LastName { get; private set; }
		public string Address { get; private set; }
		public string Gender { get; private set; }

		public Person(int id, string firstName, string lastName, string address, string gender)
		{
			Id = id;
			FirstName = firstName;
			LastName = lastName;
			Address = address;
			Gender = gender;
		}
	}
}
