using RestWithASPNET.Model;

namespace RestWithASPNET.Services
{
	public interface IPersonService
	{
		Person Create(Person person);

		Person Update(Person person);

		Person GetById(int ind);

		ICollection<Person> GetAll();

		void Delete(int ind);
	}
}
