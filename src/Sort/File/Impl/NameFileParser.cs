using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common.Logging;
using Sort.Model;

namespace Sort.File {
	public class NameFileParser : IFileParser {
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		public NameFileParser() {
			log.Debug( "Instance created" );
		}

		public string CreateSortedFile( string inputFilePath, out FileInfo outputFileInfo ) {
			log.Debug( string.Format( "CreateSortedFile - called with path: {0}", inputFilePath ) );

			var message = string.Format( "Failed to create sorted file for: {0}", inputFilePath );
			outputFileInfo = null;

			try {
				if ( string.IsNullOrEmpty( inputFilePath ) ) {
					// Cannot created sorted file if input file path is null or empty

					message = "File path not specified";
					log.Info( string.Format( "CreateSortedFile - {0}", message ) );

				} else {

					// Create FileInfo of input file path and attempt to parse, sort and create output file
					IList<Name> names;
					var inputFileInfo = new FileInfo( inputFilePath );

					if ( !inputFileInfo.Exists ) {
						// Cannot created sorted output file if input file doesn't exist

						message = string.Format( "File doesn't exist: {0}", inputFilePath );
						log.Info( string.Format( "CreateSortedFile - {0}", message ) );

					} else if ( !TryParseFileNames( inputFileInfo, out names ) || names.Count == 0 ) {
						// Cannot created sorted output file if input file cannot be parsed into names list

						message = string.Format( "File doesn't contain comma separated last and first names: {0}", inputFilePath );
						log.Info( string.Format( "CreateSortedFile - {0}", message ) );

					} else {
						// Create output file and write lines for each name sorted by last name, then by first name

						var sortedNames = names.OrderBy( n => n.LastName ).ThenBy( n => n.FirstName ).Select( n => string.Format( "{0}, {1}", n.LastName, n.FirstName ) ).ToList();
						message = WriteLinesToFile( inputFileInfo, sortedNames, out outputFileInfo );
						log.Info( string.Format( "CreateSortedFile - {0}", message ) );
					}
				}

			} catch ( Exception ex ) {
				message = string.Format( "Failed to create sorted file for: {0}", inputFilePath );
				log.Error( string.Format( "CreateSortedFile - {0}", message ), ex );
			}

			return message;
		}

		public virtual bool TryParseFileNames( FileInfo inputFileInfo, out IList<Name> names ) {
			string inputFilePath = inputFileInfo != null ? inputFileInfo.FullName : "<null>";
			log.Debug( m => m( "TryParseFileNames - called with file: {0}", inputFilePath ) );

			return inputFileInfo.TryParseFileNames( out names );
		}

		public virtual string WriteLinesToFile( FileInfo inputFileInfo, IList<string> lines, out FileInfo outputFile ) {
			string inputFilePath = inputFileInfo != null ? inputFileInfo.FullName : "<null>";
			string allLines = lines != null ? string.Join( "\r\n", lines.ToArray() ) : "<null>";
			log.Debug( m => m( "WriteLinesToFile - called with file: {0}, names: \r\n{1}", inputFilePath, allLines ) );

			var message = string.Format( "Failed to create sorted file for: {0}", inputFilePath );
			outputFile = null;

			if ( inputFileInfo != null ) {
				try {
					var outputFilePath = string.Format( @"{0}\{1}-sorted.txt", inputFileInfo.DirectoryName, inputFileInfo.Name.Replace( inputFileInfo.Extension, "" ) );
					outputFile = new FileInfo( outputFilePath );

					using ( var writer = outputFile.CreateText() ) {
						foreach ( var item in lines ) {

							// Write to output file
							writer.WriteLine( item );

							// Write to console
							Console.WriteLine( item );

							// Write debug information to log file
							log.Debug( string.Format( "WriteLinesToFile - wrote line: {0}", item ) );
						}
					}

					// Create appropriate message to display on return
					message = string.Format( "Finished: created: {0}", outputFile.FullName );

				} catch ( Exception ex ) {
					message = string.Format( "Failed to write lines to file for lines: \r\n{0}", allLines );
					log.Error( string.Format( "WriteLinesToFile - {0}", message ), ex );
				}
			}

			return message;
		}
	}
}
