using System.Collections.Generic;
using System.IO;
using System.Linq;
using Spark.FileSystem;

namespace PreCompileService
{
	/// <summary>
	/// This custom IViewFolder will treat a virtual path (like: ~\Areas\GiftCertificates\Views\Search\index.spark) 
	/// as a FileSystem path (by stripping the inital ~\ and combining it with the basePath).
	/// 
	/// This is required because the Area views requires the ~\ when running in the web environment, but of course,
	/// that doesn't work when you are not in a web environment - as we are when compiling views in a console app.
	/// </summary>
	public class VirtualFileSystemViewFolder : IViewFolder
	{
		private readonly string _basePath;

		public VirtualFileSystemViewFolder(string basePath)
		{
			_basePath = basePath;
		}

		public string BasePath
		{
			get { return _basePath; }
		}


		public IViewFile GetViewSource(string path)
		{
			var adjustedPath = AdjustPath(path);

			string fullPath = Path.Combine(_basePath, adjustedPath);
			if (!File.Exists(fullPath))
				throw new FileNotFoundException("View source file not found.", fullPath);

			return new FileSystemViewFile(fullPath);
		}

		public IList<string> ListViews(string path)
		{
			if (!path.StartsWith("~"))
				return new string[0];

			var adjustedPath = AdjustPath(path);

			if (!Directory.Exists(Path.Combine(_basePath, adjustedPath)))
				return new string[0];

			var files = Directory.GetFiles(Path.Combine(_basePath, adjustedPath));
			return files.ToList().ConvertAll(viewPath => Path.GetFileName(viewPath));
		}

		public bool HasView(string path)
		{
			if (!path.StartsWith("~"))
				return false;

			var adjustedPath = AdjustPath(path);

			return File.Exists(Path.Combine(_basePath, adjustedPath));
		}

		private string AdjustPath(string path)
		{
			return path.Replace("~\\", "");
		}
	}
}