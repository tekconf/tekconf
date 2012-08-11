using System;
using System.Collections.Generic;
using SQLite;
using Catnap;

namespace ConferencesIO.LocalData.iOS
{
	public class EntityGuid : Entity<Guid>
	{
		public override bool IsTransient
		{
			get { return Id == Guid.Empty; }
		}
	}
	
	public class EntityInt : Entity<int>
	{
		public override bool IsTransient
		{
			get { return Id == 0; }
		}
	}

	public class EntityString : Entity<string>
	{
		public override bool IsTransient
		{
			get { return Id == string.Empty; }
		}
	}
	
	public abstract class Entity<T>
	{
		private int? cachedHashCode;
		
		public virtual T Id { get; private set; }
		
		public abstract bool IsTransient { get; }
		
		public bool Equals(Entity<T> x, Entity<T> y)
		{
			if (x == null && y == null)
			{
				return true;
			}
			if (x == null || y == null)
			{
				return true;
			}
			if (x.IsTransient && y.IsTransient)
			{
				return ReferenceEquals(x, y);
			}
			return x.Id.Equals(y.Id);
		}
		
		public int GetHashCode(Entity<T> obj)
		{
			return obj.IsTransient
				? base.GetHashCode()
					: obj.Id.GetHashCode();
		}
		
		public override bool Equals(object obj)
		{
			return Equals(this, obj as Entity<T>);
		}
		
		public override int GetHashCode()
		{
			if (!cachedHashCode.HasValue)
			{
				cachedHashCode = GetHashCode(this);
			}
			return cachedHashCode.Value;
		}
		
		public static bool operator ==(Entity<T> x, Entity<T> y)
		{
			return object.Equals(x, y);
		}
		
		public static bool operator !=(Entity<T> x, Entity<T> y)
		{
			return !(x == y);
		}
		
		public void SetId(T id)
		{
			Id = id;
		}
	}


	public class ConferenceEntity : EntityString
	{
		public string description { get; set; }
		public string facebookUrl { get; set; }
		public string slug { get; set; }
		public string homepageUrl { get; set; }
		public string lanyrdUrl { get; set; }
		public string location { get; set; }
		public string meetupUrl { get; set; }
		public string name { get; set; }
		public DateTime start { get; set; }
		public DateTime end { get; set; }
		public string twitterHashTag { get; set; }
		public string twitterName { get; set; }
		//public List<SessionEntity> sessions { get; set; }
	}

	public class SessionEntity
	{
		public string Id { get; set;}
		
		public string conferenceId {get;set;}
		public string slug { get; set; }
		public string title { get; set; }
		public DateTime start { get; set; }
		public DateTime end { get; set; }
		public string room { get; set; }
		public string difficulty { get; set; }
		public string description { get; set; }
		public string twitterHashTag { get; set; }
		public string sessionType { get; set; }
		//public List<string> links { get; set; }
		//public List<string> tags { get; set; }
		//public List<string> subjects { get; set; }
		//public List<string> resources { get; set; }
		//public List<string> prerequisites { get; set; }
		//public List<FullSpeakerDto> speakers { get; set; }
	}

	public class SpeakerEntity
	{
		public string Id {get;set;}
		public string slug { get; set; }
		public string firstName { get; set; }
		public string lastName { get; set; }
		public string description { get; set; }
		public string blogUrl { get; set; }
		public string twitterName { get; set; }
		public string facebookUrl { get; set; }
		public string linkedInUrl { get; set; }
		public string emailAddress { get; set; }
		public string phoneNumber { get; set; }
	}



	public class LinkEntity
	{
		public string Id {get;set;}
		public string ParentId {get;set;}
		public string Link {get;set;}
	}

	public class TagEntity
	{
		public string Id {get;set;}
		public string ParentId {get;set;}
		public string Tag {get;set;}
	}

	public class SubjectEntity
	{
		public string Id {get;set;}
		public string ParentId {get;set;}
		public string Subject {get;set;}
	}

	public class ResourceEntity
	{
		public string Id {get;set;}
		public string ParentId {get;set;}
		public string Resource {get;set;}
	}

	public class PrerequisiteEntity
	{
		public string Id {get;set;}
		public string ParentId {get;set;}
		public string Prerequisites {get;set;}
	}


}

