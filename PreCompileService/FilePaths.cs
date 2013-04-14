using System.IO;

namespace PreCompileService
{
	public class FilePaths
	{
		//assuming we are running from: SparkDynamicTheme_root\PreCompileService\bin\Debug

		public static readonly string ThemesFolder =  Path.GetFullPath(@"..\..\..\SparkDynamicTheme\App_Data\");
		public static readonly string ViewsFolder = Path.GetFullPath(@"..\..\..\SparkDynamicTheme\Views\");

		public static readonly string StorageFolder = Path.GetFullPath(@".\PreCompiledBinaries");

		public static void EnsureFoldersExist()
		{
			if (!Directory.Exists(StorageFolder))
			{
				Directory.CreateDirectory(StorageFolder);
			}
		}
	}
}
