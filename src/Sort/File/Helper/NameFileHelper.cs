using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic.FileIO;
using Common.Logging;
using Sort.Model;

namespace Sort.File {
	public static class NameFileHelper {
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		public static bool TryParseFileNames( this FileInfo @this, out IList<Name> names ) {
			var parsed = false;
			names = new List<Name>();

			// out parameter is empty list rather than null if cannot parse and returning false
			if ( @this != null && @this.Exists ) {
				try {
					// Use the built in .NET csv parser to handle file with comma separated fields. For more information
					// go to - https://msdn.microsoft.com/en-us/library/microsoft.visualbasic.fileio.textfieldparser.aspx
					using ( FileStream stream = @this.OpenRead() ) {
						using ( TextFieldParser csv = new TextFieldParser( stream ) ) {
							csv.TextFieldType = FieldType.Delimited;
							csv.TrimWhiteSpace = true;
							csv.HasFieldsEnclosedInQuotes = true;
							csv.Delimiters = new string[] { "," };

							while ( !csv.EndOfData ) {
								var row = csv.ReadFields();
								// Allow for any number of fields, assume last name is first field, first name is last field
								var lastName = row[0];
								var firstName = row[row.Length - 1];
								names.Add( new Name( firstName, lastName ) );
							}
						}
					}
					parsed = true;
				} catch ( Exception ex ) {
					var msg = ex.Message;
					log.Error( m => m( "TryParseFileNames - Failed to parse names from file: {0} \r\nError: {1}", @this, msg ), ex );
				}
			}

			return parsed;
		}
	}
}
