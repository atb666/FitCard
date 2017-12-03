using System;
using System.Globalization;
using System.Diagnostics;

namespace PersonDemo
{
	/// <summary>
	/// A Person...
	/// 
	/// has a social security number,
	/// a name and a date of birth.
	/// </summary>
	public class Person : BusinessObject
	{
		#region Public interface

		public Person()
		{
			isNew = true;
			isEditing = false;
			isDirty = false;
			isDeleted = false;

			// Rules that are known to be initially broken
			RuleBroken("Social Security Number", true);
			RuleBroken("Name", true);
			RuleBroken("Birth Date", true);
		}

		/// <summary>
		/// Social Security Number.
		/// Must be exactly SocialSecurityNumberLength characters long.
		/// 
		/// Setting it requires that the object is editable. 
		/// Use the IsEditing property to test this.
		/// </summary>
		public string SocialSecurityNumber
		{
			get
			{
				return socialSecurityNumber;
			}
			set
			{
				if (!isEditing)
				{
					throw new InvalidOperationException("This property can only be set if editing is enabled.");
				}

				socialSecurityNumber = value;

				// Is it valid?
				RuleBroken("Social Security Number", 
					socialSecurityNumber.Trim().Length != socialSecurityNumberLength);
				
				isDirty = true;
			}
		}

		/// <summary>
		/// The person's name.
		/// 
		/// Setting it requires that the object is editable. 
		/// Use the IsEditing property to test this.
		/// </summary>
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				if (!isEditing)
				{
					throw new InvalidOperationException("This property can only be set if editing is enabled.");
				}

				name = value;

				// Is it valid?
				RuleBroken("Name", name.Trim() == String.Empty);
				
				isDirty = true;
			}
		}
		
		/// <summary>
		/// Date of birth in UK format (dd/mm/yyyy).
		/// 
		/// Single digit day or month can
		/// omit leading 0, i.e., both of
		/// the following are valid:
		/// 
		/// 21/7/1959 or 21/07/1959
		/// 
		/// Setting it requires that the object is editable. 
		/// Use the IsEditing property to test this.
		/// </summary>
		public string BirthDate
		{
			get
			{
				return birthDate;
			}
			set
			{
				if (!isEditing)
				{
					throw new InvalidOperationException("This property can only be set if editing is enabled.");
				}
				
				birthDate = value;

				// Is it valid?
				bool isDate = IsDate(birthDate);
				RuleBroken("Birth Date", !isDate);

				if (isDate)
				{
					// Update internal date
					birthDateDt = Convert.ToDateTime(birthDate);
					
					// Calculate age to see whether it's changed
					int age = CalculateAge();
					bool ageChanged = (age != this.age);

					if (ageChanged)
					{
						// Update age and notify clients
						this.age = age;
						RaiseEvent(OnNewAge, EventArgs.Empty);		
					}

				} // End if (isDate)
				
				isDirty = true;
			
			} // End set
		}

		/// <summary>
		/// Age in years.
		/// </summary>
		public int Age
		{
			get
			{
				return age;
			}
		}

		/// <summary>
		/// Required number of characters in Social Security Number.
		/// (Assumed alphanumeric.)
		/// </summary>
		public int SocialSecurityNumberLength
		{
			get
			{
				return socialSecurityNumberLength;
			}
		}

		/// <summary>
		/// Is object new?
		/// For example, only old objects can be deleted.
		/// </summary>
		public bool IsNew
		{
			get
			{
				return isNew;
			}
		}

		/// <summary>
		/// Is object being edited?
		/// </summary>
		public bool IsEditing
		{
			get
			{
				return isEditing;
			}
		}

		/// <summary>
		/// Has object's data been changed?
		/// </summary>
		public bool IsDirty
		{
			get
			{
				return isDirty || isNew;
			}
		}

		/// <summary>
		/// Has object been deleted?
		/// </summary>
		public bool IsDeleted
		{
			get
			{
				return isDeleted;
			}
		}

		/// <summary>
		/// Is Social Security Number valid?
		/// </summary>
		public bool IsValidSocialSecurityNumber
		{
			get
			{
				return socialSecurityNumber.Trim().Length == socialSecurityNumberLength;
			}
		}

		/// <summary>
		/// Is Name valid?
		/// </summary>
		public bool IsValidName
		{
			get
			{
				return name.Trim() != String.Empty;
			}
		}

		/// <summary>
		/// Is Birth Date valid?
		/// </summary>
		public bool IsValidBirthDate
		{
			get
			{
				return IsDate(birthDate);
			}
		}
		/// <summary>
		/// Is Social Security Number valid?
		/// Clients can check this before proceeding with a Load operation.
		/// </summary>
		public static bool CheckValidSocialSecurityNumber(string socialSecurityNumber)
		{
			return socialSecurityNumber.Trim().Length == socialSecurityNumberLength;
		}

		/// <summary>
		/// Enables editing.
		/// Precondition: Must not already be editing.
		/// </summary>
		public void BeginEdit()
		{
			Trace.Assert(!isEditing, "Must not already be editing.");

			// First save state
			if (savedPerson == null)
			{
				savedPerson = new Person();
			}
			
			SaveState();
			
			// Then enable editing
			isEditing = true;
		}

		/// <summary>
		/// Saves or deletes object if appropriate.
		/// Precondition: Must be editing.
		/// Terminates editing.
		/// Clients should immediately call BeginEdit
		/// if they wish to continue editing.
		/// </summary>
		public void ApplyEdit()
		{
			Trace.Assert(isEditing, "Must be editing.");
			
			if (isDeleted && !isNew)
			{
				Delete(socialSecurityNumber);
				isNew = true;
				isDeleted = false;
			}
			else
			{
				if (isDirty || isNew)
				{
					Trace.Assert(IsValid, "Must be valid before a Save.");
					Save();
					isNew = false;
				}
			}

			isEditing = false;
			isDirty = false;
		}

		/// <summary>
		/// Cancels all changes since the last ApplyEdit
		/// or since the object was marked for deletion.
		/// Precondition: Must be editing.
		/// Terminates editing.
		/// </summary>
		public void CancelEdit()
		{
			Trace.Assert(isEditing, "Must be editing.");

			isEditing = false;
			isDeleted = false;
			isDirty = false;

			// Rollback
			RetrieveSavedState();
		}
		
		/// <summary>
		/// Loads a Person from the database.
		/// Precondition: Must not be editing.
		/// Precondition: Must be new.
		/// Postcondition: Object is in a valid state.
		/// </summary>
		public void Load(string socialSecurityNumber)
		{
			Trace.Assert(!isEditing, "Must not be editing.");
			Trace.Assert(isNew, "Must be new.");

			// Load the object
			PersonManager manager = new PersonManager();
			PersonProperties properties = manager.Load(socialSecurityNumber);
			Trace.Assert(properties.IsPopulated, "Person must have all its properties set.");
			this.SetState(properties);

			isNew = false;

			Trace.Assert(IsValid, "Should be valid.");
		}

		/// <summary>
		/// Marks object for deletion.
		/// Precondition: Must be editing.
		/// Can be undone by calling CancelEdit.
		/// Actual deletion is performed by ApplyEdit.
		/// </summary>
		public void Delete()
		{
			Trace.Assert(isEditing, "Must be editing.");
			
			isDeleted = true;
			isDirty = true;
		}

		/// <summary>
		/// Gets the core state fields,
		/// Social Security Number, Name and Birth Date.
		/// Used by PersonManager.
		/// </summary>
		/// <returns></returns>
		public PersonProperties GetState()
		{
			Trace.Assert(IsValid, "Require valid state.");

			PersonProperties properties = new PersonProperties();
			properties.SocialSecurityNumber = this.SocialSecurityNumber;
			properties.Name = this.Name;
			properties.BirthDate = this.BirthDate;

			return properties;
		}
		
		#endregion // Public interface

		#region Events

		/// <summary>
		/// Raised when the age property changes.
		/// </summary>
		public event EventHandler OnNewAge;
		
		#endregion // Events

		#region Implementation

		/// <summary>
		/// Saves object to database.
		/// </summary>
		private void Save()
		{
			PersonManager manager = new PersonManager();
			manager.Save(this);
		}

		/// <summary>
		/// Deletes object from database.
		/// </summary>
		/// <param name="socialSecurityNumber">Social Security Number.</param>
		private void Delete(string socialSecurityNumber)
		{
			PersonManager manager = new PersonManager();
			manager.Delete(socialSecurityNumber);
		}

		/// <summary>
		/// Saves a copy of the object's state fields.
		/// </summary>
		private void SaveState()
		{
			savedPerson.socialSecurityNumber = this.socialSecurityNumber;
			savedPerson.name = this.name;
			savedPerson.birthDate = this.birthDate;
			savedPerson.birthDateDt = this.birthDateDt;
			savedPerson.age = this.age;
		}

		/// <summary>
		/// Restores the object's state fields to the most recent valid values.
		/// </summary>
		private void RetrieveSavedState()
		{
			this.socialSecurityNumber = savedPerson.socialSecurityNumber;
			this.name = savedPerson.name;
			this.birthDate = savedPerson.birthDate;
			this.birthDateDt = savedPerson.birthDateDt;
			this.age = savedPerson.age;
		}

		/// <summary>
		/// Is this a date in UK format dd/mm/yyyy?
		/// 
		/// Single digit day or month can
		/// omit leading 0, e.g., both of
		/// the following are valid:
		/// 
		/// 21/7/1959 or 21/07/1959
		/// 
		/// </summary>
		/// <param name="date">The date - dd/mm/yyyy.</param>
		/// <returns></returns>
		private bool IsDate(string date)
		{
			bool ret = false;
			
			try
			{
				CultureInfo info = new CultureInfo("en-GB");
				DateTime birthDate = DateTime.ParseExact(date, "d/M/yyyy", info);

				ret = true;
			}
			catch (System.FormatException)
			{
				// Ignore - function is called repeatedly until date is valid
			}

			return ret;
		}

		/// <summary>
		/// Calculates age from the date of birth.
		/// </summary>
		/// <returns>Age in years.</returns>
		private int CalculateAge()
		{
			DateTime now = DateTime.Now;

			int birthDay = Convert.ToInt32(birthDateDt.ToString("%d"));
			int birthMonth = Convert.ToInt32(birthDateDt.ToString("%M"));
			int birthYear = Convert.ToInt32(birthDateDt.ToString("yyyy"));

			int day = Convert.ToInt32(now.ToString("%d"));
			int month = Convert.ToInt32(now.ToString("%M"));
			int year = Convert.ToInt32(now.ToString("yyyy"));
			int age = Math.Max(0, year - birthYear);

			// Adjust age depending on which part of the 
			// year the birth date falls
			if (age > 0)
			{
				if (month < birthMonth)
				{
					age--;
				}
				else 
				{
					if (month == birthMonth)
					{
						if (day < birthDay) age--;
					}
				}
			}
			
			return age;
		}

		/// <summary>
		/// Sets the state fields,
		/// Social Security Number, Name and Birth Date.
		private void SetState(PersonProperties properties)
		{
			// Ensure that the properties are valid
			// before setting into object
			RuleBroken("Social Security Number", 
				properties.SocialSecurityNumber.Trim().Length != SocialSecurityNumberLength);
			RuleBroken("Name", properties.Name.Trim() == String.Empty);
			RuleBroken("Birth Date", IsDate(properties.BirthDate) == false);

			Trace.Assert(IsValid, "Must be valid state.");

			this.socialSecurityNumber = properties.SocialSecurityNumber;
			this.name = properties.Name;
			this.birthDate = properties.BirthDate;
			this.birthDateDt = Convert.ToDateTime(this.birthDate);
			this.age = CalculateAge();
		}

		/// <summary>
		/// Do all fields have their default values?
		/// Not used, but keep for reference.
		/// </summary>
		private bool IsDefaultState
		{
			get
			{
				return 
					socialSecurityNumber == null &&
					name == null &&
					birthDate == null &&
					birthDateDt == DateTime.MinValue &&
					age == 0 &&
					isNew == true &&
					isEditing == false;
			}
		}

		private string    socialSecurityNumber;
		private const int socialSecurityNumberLength = 11;
		private string    name;
		private string    birthDate;
		
		// Internal - used for calculation
		private DateTime  birthDateDt; 
		
		// Age in years - calculated
		private int       age;
		
		// A copy of this object's business attributes
		private Person    savedPerson;
		
		// Is object new or has it been loaded from database or saved to it?
		private bool isNew;

		// By keeping track of whether this object is currently being edited or not
		// it can make sure that the object's data is only changed when appropriate
		private bool isEditing;
		
		// Has object's data been changed?
		private bool isDirty;
		
		// Has object been marked for deletion?
		private bool isDeleted;

		#endregion // Implementation
	}

	/// <summary>
	/// Facilitates transfer of Person state. 
	/// See Person.SetState and Person.GetState.
	/// </summary>
	public struct PersonProperties
	{
		public string SocialSecurityNumber;
		public string Name;
		public string BirthDate;

		/// <summary>
		/// Diagnostics: are all fields set?
		/// </summary>
		public bool IsPopulated
		{
			get
			{
				return 
					SocialSecurityNumber.Trim().Length != 0 &&
					Name.Trim().Length != 0 &&
					BirthDate.Trim().Length != 0;
			}
		}
	}
}
