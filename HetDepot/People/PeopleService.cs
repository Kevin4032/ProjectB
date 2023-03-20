﻿using HetDepot.Persistence;
using HetDepot.People.Model;
using HetDepot.Errorlogging;
using System.Security.Cryptography.X509Certificates;
using System.Collections.ObjectModel;

namespace HetDepot.People
{
    public class PeopleService
	{
		private Repository _repository;
		private List<Person> _people;
		private IDepotErrorLogger _errorLogger;

		public PeopleService(Repository repository, IDepotErrorLogger errorLogger)
		{
			_repository = repository;
			_people = _repository.GetPeople();
			_errorLogger = errorLogger;
		}

		public ReadOnlyCollection<Visitor> GetVisitors()
		{
			var visitors = new List<Visitor>();

			var visitorsInPeople = _people.FindAll(p => p.GetType() == typeof(Visitor)) ?? throw new NullReferenceException("GetVisitors - No Visitors found");

			foreach (var visitor in visitorsInPeople)
			{
				visitors.Add((Visitor)visitor);
			}

			return visitors.AsReadOnly();
		}

		public Person GetById(string id) => _people.FirstOrDefault(p => p.Id == id) ?? throw new NullReferenceException("No Person Found");
		public Visitor GetVisitorById(string id) => _people.FirstOrDefault(p => p.Id == id) as Visitor ?? throw new NullReferenceException("No Visitor Found");
		public Guide GetGuide() => _people.FirstOrDefault(p => p.GetType() == typeof(Guide)) as Guide ?? throw new NullReferenceException("No Guide Found");
		public Manager GetManager() => _people.FirstOrDefault(p => p.GetType() == typeof(Manager)) as Manager ?? throw new NullReferenceException("No Manager Found");


	}
}
