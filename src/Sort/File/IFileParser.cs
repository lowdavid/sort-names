using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sort.Model;

namespace Sort.File {
	public interface IFileParser {
		string CreateSortedFile( string inputFilePath, out FileInfo outputFile );
	}
}
