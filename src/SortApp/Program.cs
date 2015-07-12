using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common.Logging;
using Sort.File;

namespace Sort.App {
	public class Program {
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private static void Main( string[] args ) {
			log.Info( string.Format( "sort-names - executed with {0} argument: {1}", args.Length, string.Join( ",", args ) ) );

			if ( args == null || args.Length != 1 ) {
				Console.WriteLine( "Usage" );
				Console.WriteLine( "- requires one parameter that represents a text file containing a list of names." );
				Console.WriteLine( "- the text file names are last name then first name, separated by a comma." );
				Console.WriteLine( "- will write a text file containing list of names sorted by last name then first name." );
				Console.WriteLine( "- output text file will be named <input-file-name>-sorted.txt at the same location." );
			} else {
				var parser = new NameFileParser();
				FileInfo outputFile;
				var message = parser.CreateSortedFile( args[0], out outputFile );
				log.Debug( string.Format( "sort-names - message: {0}", message ) );
				Console.WriteLine( message );
			}

			Console.WriteLine();
			Console.WriteLine( "Press any key to exit" );
			Console.ReadKey( true );
		}
	}
}
