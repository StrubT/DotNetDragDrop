using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Storage;

namespace StrubT.BFH.DotNet.DragDrop.Data {

	public class Store {

		static JsonSerializer Serializer { get; } = new JsonSerializer();

		ICollection<Category> Categories { get; }

		ICollection<Appointment> Appointments { get; }

		Store(ICollection<Category> categories, ICollection<Appointment> appointments) {

			Categories = categories;
			Appointments = appointments;
		}

		public Store() : this(new List<Category>(), new List<Appointment>()) { }

		public static async Task<Store> LoadAsync(StorageFile file) {

			using (Stream stream = await file.OpenStreamForReadAsync())
			using (StreamReader streamReader = new StreamReader(stream))
			using (JsonReader jsonReader = new JsonTextReader(streamReader))
				return Serializer.Deserialize<Store>(jsonReader);
		}

		public async Task Save(StorageFile file) {

			using (Stream stream = await file.OpenStreamForWriteAsync())
			using (StreamWriter streamWriter = new StreamWriter(stream))
			using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter))
				Serializer.Serialize(jsonWriter, this);
		}
	}
}
