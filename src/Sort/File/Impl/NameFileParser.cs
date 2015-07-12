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

		public string CreateSortedFile( string inputFilePath, out FileInfo outputFile ) {
			log.Debug( string.Format( "CreateSortedFile - called with path: {0}", inputFilePath ) );
			outputFile = null;
			var message = "";

			try {
				if ( string.IsNullOrEmpty( inputFilePath ) ) {
					// Cannot created sorted file if input file path is null or empty

					message = "File path not specified";
					log.Info( string.Format( "CreateSortedFile - {0}", message ) );

				} else {
					IList<Name> names;
					var inputFile = new FileInfo( inputFilePath );

					if ( !inputFile.Exists ) {
						// Cannot created sorted file if input file doesn't exist

						message = string.Format( "File doesn't exist: {0}", inputFilePath );
						log.Info( string.Format( "CreateSortedFile - {0}", message ) );

					} else if ( !inputFile.TryParseFileNames( out names ) || names.Count == 0 ) {
						// Cannot created sorted file if input file cannot be parsed into names list

						message = string.Format( "File doesn't contain comma separated last and first names: {0}", inputFilePath );
						log.Info( string.Format( "CreateSortedFile - {0}", message ) );

					} else {
						// Create output file and write lines for each name sorted by last name, then by first name

						var outputFilePath = string.Format( @"{0}\{1}-sorted.txt", inputFile.DirectoryName, inputFile.Name.Replace( inputFile.Extension, "" ) );
						outputFile = new FileInfo( outputFilePath );
						using ( var writer = outputFile.CreateText() ) {
							foreach ( var item in names.OrderBy( n => n.LastName ).ThenBy( n => n.FirstName ) ) {
								writer.WriteLine( string.Format( "{0}, {1}", item.LastName, item.FirstName ) );
							}
						}

						message = string.Format( "Created file of names sorted by last then first names: {0}", outputFile.FullName );
						log.Info( string.Format( "CreateSortedFile - {0}", message ) );
					}
				}

			} catch ( Exception ex ) {
				message = string.Format( "Failed to create sorted file for: {0}", inputFilePath );
				log.Error( string.Format( "CreateSortedFile - {0}", message ), ex );
			}

			return message;
		}
	}
}
