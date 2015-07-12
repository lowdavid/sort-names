using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sort.Model {
	public class Name {
		public Name( string firstName, string lastName  ) {
			FirstName = firstName;
			LastName = lastName;
		}

		public string FirstName { get; set; }
		public string LastName { get; set; }
	}
}
