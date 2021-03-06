﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;

namespace Sort.Tests.File {
	public class TestFileSetup {
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private string _testPath;

		public TestFileSetup() {
			_testPath = ConfigurationManager.AppSettings["testPath"];
			log.Info( string.Format( "TestFileSetup - read testPath from appSettings: {0}", _testPath ) );
			try {
				if ( !Directory.Exists( _testPath ) ) {
					Directory.CreateDirectory( _testPath );
				}
			} catch ( Exception ex ) {
				log.Error( string.Format( "TestFileSetup - could not create directory for test file: {0}", _testPath ), ex );
				_testPath = null;
			}
			if ( string.IsNullOrEmpty( _testPath ) ) {
				_testPath = Environment.CurrentDirectory;
			}
			if ( _testPath.EndsWith( @"\" ) ) {
				_testPath = _testPath.TrimEnd( @"\".ToCharArray() );
			}
		}

		public FileInfo CreateNonExistingTestFileInfo() {
			FileInfo file = null;

			var testFilePath = string.Format( @"{0}\testNonExisting.txt", _testPath );
			try {
				file = new FileInfo( testFilePath );
				if ( file.Exists ) {
					file.Delete();
				}
			} catch ( Exception ex ) {
				log.Error( string.Format( "CreateNonExistingTestFileInfo - could not delete test file: {0}", testFilePath ), ex );
			}

			return file;
		}

		public FileInfo CreateEmptyTestFileInfo() {
			FileInfo file = null;

			var testFilePath = string.Format( @"{0}\testEmpty.txt", _testPath );
			try {
				file = new FileInfo( testFilePath );
				using ( var writer = file.CreateText() ) {
					writer.WriteLine( "" );
				}
			} catch ( Exception ex ) {
				log.Error( string.Format( "CreateEmptyTestFileInfo - could not create test file: {0}", testFilePath ), ex );
			}

			return file;
		}

		public FileInfo CreateBadTestFileInfo() {
			FileInfo file = null;

			var testFilePath = string.Format( @"{0}\testBad.txt", _testPath );
			try {
				file = new FileInfo( testFilePath );
				using ( var writer = file.CreateText() ) {
					writer.WriteLine( "\t,\r,^~!$,\f\n,\r\n" );
					writer.WriteLine( "" );
					writer.WriteLine( " ,, " );
				}
			} catch ( Exception ex ) {
				log.Error( string.Format( "CreateBadTestFileInfo - could not create test file: {0}", testFilePath ), ex );
			}

			return file;
		}

		public FileInfo CreateTestFileInfo() {
			FileInfo file = null;

			var testFilePath = string.Format( @"{0}\test.txt", _testPath );
			try {
				file = new FileInfo( testFilePath );
				using ( var writer = file.CreateText() ) {
					writer.WriteLine( "SMITH, FREDRICK" );
					writer.WriteLine( "BAKER, ANDREW" );
					writer.WriteLine( "KENT, MADISON" );
					writer.WriteLine( "SMITH, ANDREW" );
				}
			} catch ( Exception ex ) {
				log.Error( string.Format( "CreateTestFileInfo - could not create test file: {0}", testFilePath ), ex );
			}

			return file;
		}

		public FileInfo CreateNoCommaTestFileInfo() {
			FileInfo file = null;

			var testFilePath = string.Format( @"{0}\testNoComma.txt", _testPath );
			try {
				file = new FileInfo( testFilePath );
				using ( var writer = file.CreateText() ) {
					writer.WriteLine( "SMITH FREDRICK" );
					writer.WriteLine( "BAKER ANDREW" );
					writer.WriteLine( "KENT MADISON" );
					writer.WriteLine( "SMITH ANDREW" );
				}
			} catch ( Exception ex ) {
				log.Error( string.Format( "CreateNoCommaTestFileInfo - could not create test file: {0}", testFilePath ), ex );
			}

			return file;
		}

		public FileInfo CreateLastMiddleFirstTestFileInfo() {
			FileInfo file = null;

			var testFilePath = string.Format( @"{0}\testLastMiddleFirst.txt", _testPath );
			try {
				file = new FileInfo( testFilePath );
				using ( var writer = file.CreateText() ) {
					writer.WriteLine( "SMITH, JOHN, FREDRICK" );
					writer.WriteLine( "BAKER, JAMES, ANDREW" );
					writer.WriteLine( "KENT, RAYMOND, ROBIN, MADISON" );
					writer.WriteLine( "SMITH, GEORGE, ANDREW" );
				}
			} catch ( Exception ex ) {
				log.Error( string.Format( "CreateLastMiddleFirstTestFileInfo - could not create test file: {0}", testFilePath ), ex );
			}

			return file;
		}

		public IList<string> ReadOutputFileInfo( FileInfo outputFile ) {
			IList<string> lines = new List<string>();

			try {
				using ( var reader = outputFile.OpenText() ) {
					while ( !reader.EndOfStream ) {
						lines.Add( reader.ReadLine() );
					}
				}
			} catch ( Exception ex ) {
				log.Error( string.Format( "ReadOutputFileInfo - could not read output file: {0}", outputFile.FullName ), ex );
			}

			return lines;
		}
	}
}
