namespace DoctorApi.Models
{
	public class DoctorModel
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Specialization { get; set; }
		public string? ShortDescription { get; set; }
		public string? LongDescription { get; set; }
		public string? Photo { get; set; }
	}
}

