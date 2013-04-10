using System;

namespace TekConf.Common.Entities
{
	public abstract class TinyMessageBase : ITinyMessageBase
	{
		protected TinyMessageBase()
		{
			if (this.Id == default (Guid))
				this.Id = Guid.NewGuid();

			if (this.EventDate == default(DateTime))
				this.EventDate = DateTime.Now;
		}

		public Guid Id { get; set; }
		public object Sender { get; set; }
		public DateTime EventDate { get; set; }

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			var input = obj as TinyMessageBase;
			if (input == null)
				return false;

			return this.Id.Equals(input.Id);
		}

		public override int GetHashCode()
		{
			return this.Id.GetHashCode();
		}
	}
}