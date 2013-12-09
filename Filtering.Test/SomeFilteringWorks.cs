using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;


namespace Filtering.Test
{
    [TestClass]
    public class SomeFilteringWorks
    {
        List<User> users;
        List<Book> books;

        [TestInitialize]
        public void init()
        {

            books = new List<Book>();
            books.Add(new Book
            {
                Title = "C# in a nutshell",
                Description = "Very cool"
            });
            books.Add(new Book
            {
                Title = "Scrum & XP from the trenches",
                Description = "Very usefull!!!11"
            });


            users = new List<User>();
            users.Add(new User
            {
                Id = 1,
                Name = "Just Me",
                RentedBooks = new List<Book>(books)
            });

            users.Add(new User
            {
                Id = 2,
                Name = "Or Him",
                RentedBooks = new List<Book>(books)
            });
        }

        [TestMethod]
        public void CanFilterUsersByName()
        {
            FilterDefinitionForPropertyValue<User, string> nameFilter = new FilterDefinitionForPropertyValue<User, string>
            {
                FieldSelector = u => u.Name,
            };

            var filterJustMe = nameFilter.GetFilterPredicateFor(FilterOperations.Equal, "Just Me");

            var justMeFilteringResults = users.AsQueryable().Where(filterJustMe).ToList();

            Assert.AreEqual(1, justMeFilteringResults.Count, "Should be just one user");

            var justMe = justMeFilteringResults.FirstOrDefault();

            Assert.IsNotNull(justMe, "Can't be null there should be Just Me");
            Assert.IsTrue(justMe.Name.Equals("Just Me"),"Name should be Just Me");

        }
    }

    class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Book> RentedBooks { get; set; }
    }

    class Book
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
