using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using NUnit.Framework;
using Sort.File;
using Sort.Model;

namespace Sort.Tests.File {
	[TestFixture]
	public class NameFileHelperTests {
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private TestFileSetup setup;

		[SetUp]
		public void SetupTests() {
			setup = new TestFileSetup();
		}

		[Test]
		public void TryParseNullFileNamesTest() {
			FileInfo nullFileInfo = null;

			IList<Name> names;
			var success = nullFileInfo.TryParseFileNames( out names );
			log.Debug( string.Format( "TryParseNullFileNamesTest - parsed null file: {0}", nullFileInfo = null ) );

			Assert.IsFalse( success );
			Assert.IsTrue( names.Count == 0 );
		}

		[Test]
		public void TryParseNonExistingFileNamesTest() {
			FileInfo nonExistingFileInfo = setup.CreateNonExistingTestFileInfo();

			IList<Name> names;
			var success = nonExistingFileInfo.TryParseFileNames( out names );
			log.Debug( string.Format( "TryParseNonExistingFileNamesTest - parsed empty file: {0}", nonExistingFileInfo.FullName ) );

			Assert.IsFalse( success );
			Assert.IsTrue( names.Count == 0 );
		}

		[Test]
		public void TryParseEmptyFileNamesTest() {
			FileInfo emptyFileInfo = setup.CreateEmptyTestFileInfo();

			IList<Name> names;
			var success = emptyFileInfo.TryParseFileNames( out names );
			log.Debug( string.Format( "TryParseEmptyFileNamesTest - parsed empty file: {0}", emptyFileInfo.FullName ) );

			Assert.IsTrue( success );
			Assert.IsTrue( names.Count == 0 );
		}

		[Test]
		public void TryParseBadFileNamesTest() {
			FileInfo badFileInfo = setup.CreateBadTestFileInfo();

			IList<Name> names;
			var success = badFileInfo.TryParseFileNames( out names );
			log.Debug( string.Format( "TryParseBadFileNamesTest - parsed empty file: {0}", badFileInfo.FullName ) );

			Assert.IsTrue( success );
			Assert.IsTrue( names.Count == 4 );
			Assert.IsTrue( names[0].FirstName == "" );
			Assert.IsTrue( names[0].LastName == "" );
			Assert.IsTrue( names[1].FirstName == "" );
			Assert.IsTrue( names[1].LastName == "" );
			Assert.IsTrue( names[2].FirstName == "" );
			Assert.IsTrue( names[2].LastName == "" );
			Assert.IsTrue( names[3].FirstName == "" );
			Assert.IsTrue( names[3].LastName == "" );
		}

		[Test]
		public void TryParseFileNamesTest() {
			FileInfo testFileInfo = setup.CreateTestFileInfo();

			IList<Name> names;
			var success = testFileInfo.TryParseFileNames( out names );
			log.Debug( string.Format( "TryParseFileNamesTest - parsed test file: {0}", testFileInfo.FullName ) );

			Assert.IsTrue( success );
			Assert.IsTrue( names.Count == 4 );
			Assert.IsTrue( names[0].FirstName == "FREDRICK" );
			Assert.IsTrue( names[0].LastName == "SMITH" );
			Assert.IsTrue( names[1].FirstName == "ANDREW" );
			Assert.IsTrue( names[1].LastName == "BAKER" );
			Assert.IsTrue( names[2].FirstName == "MADISON" );
			Assert.IsTrue( names[2].LastName == "KENT" );
			Assert.IsTrue( names[3].FirstName == "ANDREW" );
			Assert.IsTrue( names[3].LastName == "SMITH" );
		}

		[Test]
		public void TryParseNoCommaFileNamesTest() {
			FileInfo testFileInfo = setup.CreateNoCommaTestFileInfo();

			IList<Name> names;
			var success = testFileInfo.TryParseFileNames( out names );
			log.Debug( string.Format( "TryParseNoCommaFileNamesTest - parsed test file: {0}", testFileInfo.FullName ) );

			Assert.IsTrue( success );
			Assert.IsTrue( names.Count == 4 );
			Assert.IsTrue( names[0].FirstName == "SMITH FREDRICK" );
			Assert.IsTrue( names[0].LastName == "SMITH FREDRICK" );
			Assert.IsTrue( names[1].FirstName == "BAKER ANDREW" );
			Assert.IsTrue( names[1].LastName == "BAKER ANDREW" );
			Assert.IsTrue( names[2].FirstName == "KENT MADISON" );
			Assert.IsTrue( names[2].LastName == "KENT MADISON" );
			Assert.IsTrue( names[3].FirstName == "SMITH ANDREW" );
			Assert.IsTrue( names[3].LastName == "SMITH ANDREW" );
		}

		[Test]
		public void TryParseLastMiddleFirstFileNamesTest() {
			FileInfo testFileInfo = setup.CreateLastMiddleFirstTestFileInfo();

			IList<Name> names;
			var success = testFileInfo.TryParseFileNames( out names );
			log.Debug( string.Format( "TryParseLastMiddleFirstFileNamesTest - parsed test file: {0}", testFileInfo.FullName ) );

			Assert.IsTrue( success );
			Assert.IsTrue( names.Count == 4 );
			Assert.IsTrue( names[0].FirstName == "FREDRICK" );
			Assert.IsTrue( names[0].LastName == "SMITH" );
			Assert.IsTrue( names[1].FirstName == "ANDREW" );
			Assert.IsTrue( names[1].LastName == "BAKER" );
			Assert.IsTrue( names[2].FirstName == "MADISON" );
			Assert.IsTrue( names[2].LastName == "KENT" );
			Assert.IsTrue( names[3].FirstName == "ANDREW" );
			Assert.IsTrue( names[3].LastName == "SMITH" );
		}
	}
}
