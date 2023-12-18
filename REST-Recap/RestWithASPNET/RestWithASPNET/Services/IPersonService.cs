﻿using RestWithASPNET.Model;

namespace RestWithASPNET.Services
{
	public interface IPersonService
	{
		Person Create(Person person);

		Person Update(Person person);

		ICollection<Person> FindAll();

		Person FindById(int ind);

		void Delete(int ind);
	}
}
