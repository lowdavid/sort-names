﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using NUnit.Core;
using NUnit.Framework;
using Rhino.Mocks;
using Sort.File;
using Sort.Model;

namespace Sort.Tests.File {
	[TestFixture]
	public class NameFileParserTests {
		private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		private TestFileSetup setup;
		private NameFileParser parser;
		private MockRepository mocks;
		private NameFileParser mockedParser;

		[SetUp]
		public void SetupTests() {
			setup = new TestFileSetup();
			parser = new NameFileParser();
			mocks = new MockRepository();
			mockedParser = mocks.PartialMock<NameFileParser>();
		}

		[Test]
		public void CreateSortedNullFileTest() {
			FileInfo outputFile;
			var message = parser.CreateSortedFile( null, out outputFile );
			log.Debug( string.Format( "CreateSortedNullFileTest - sorted null input file path" ) );

			Assert.IsTrue( outputFile == null );
			Assert.IsTrue( message.Equals( "File path not specified" ) );
		}

		[Test]
		public void NonExistingFileNotExists() {
			FileInfo nonExistingFileInfo = setup.CreateNonExistingTestFileInfo();

			log.Debug( string.Format( "NonExistingFileNotExists - test file: {0}", nonExistingFileInfo.FullName ) );

			Assert.IsFalse( nonExistingFileInfo.Exists );
		}

		[Test]
		public void CreateSortedNonExistingFileTest() {
			FileInfo nonExistingFileInfo = setup.CreateNonExistingTestFileInfo();

			FileInfo outputFile;
			var message = parser.CreateSortedFile( nonExistingFileInfo.FullName, out outputFile );
			log.Debug( string.Format( "CreateSortedNonExistingFileTest - sorted test file: {0}", nonExistingFileInfo.FullName ) );

			Assert.IsTrue( outputFile == null );
			Assert.IsTrue( message.StartsWith( "File doesn't exist: " ) );
			Assert.IsTrue( message.EndsWith( nonExistingFileInfo.FullName ) );
		}

		[Test]
		public void EmptyFileExists() {
			FileInfo emptyFileInfo = setup.CreateEmptyTestFileInfo();

			log.Debug( string.Format( "EmptyFileExists - test file: {0}", emptyFileInfo.FullName ) );

			Assert.IsTrue( emptyFileInfo.Exists );
		}

		[Test]
		public void CreateSortedEmptyFileTest() {
			FileInfo emptyFileInfo = setup.CreateEmptyTestFileInfo();

			FileInfo outputFile;
			var message = parser.CreateSortedFile( emptyFileInfo.FullName, out outputFile );
			log.Debug( string.Format( "CreateSortedEmptyFileTest - sorted test file: {0}", emptyFileInfo.FullName ) );

			Assert.IsTrue( outputFile == null );
			Assert.IsTrue( message.StartsWith( "File doesn't contain comma separated last and first names: " ) );
			Assert.IsTrue( message.EndsWith( emptyFileInfo.FullName ) );
		}

		[Test]
		public void CreateSortedFileMockedTryParseFailTest() {
			FileInfo testFileInfo = setup.CreateTestFileInfo();

			IList<Name> outNames = new List<Name>();
			mockedParser.Expect( x => x.TryParseFileNames( Arg<FileInfo>.Is.Anything, out Arg<IList<Name>>.Out( outNames ).Dummy ) ).Return( false );

			mocks.ReplayAll();

			FileInfo outputFile;
			var message = mockedParser.CreateSortedFile( testFileInfo.FullName, out outputFile );
			log.Debug( string.Format( "CreateSortedFileTest - sorted test file: {0}", testFileInfo.FullName ) );

			mocks.VerifyAll();

			Assert.IsTrue( outputFile == null );
			Assert.IsTrue( message.StartsWith( "File doesn't contain comma separated last and first names: " ) );
			Assert.IsTrue( message.EndsWith( testFileInfo.FullName ) );
		}

		[Test]
		public void TestBadFileExists() {
			FileInfo badFileInfo = setup.CreateBadTestFileInfo();

			log.Debug( string.Format( "TestBadFileExists - test file: {0}", badFileInfo.FullName ) );

			Assert.IsTrue( badFileInfo.Exists );
		}

		[Test]
		public void CreateSortedBadFileTest() {
			FileInfo badFileInfo = setup.CreateBadTestFileInfo();

			FileInfo outputFile;
			var message = parser.CreateSortedFile( badFileInfo.FullName, out outputFile );
			log.Debug( string.Format( "CreateSortedBadFileTest - sorted test file: {0}", badFileInfo.FullName ) );

			Assert.IsTrue( outputFile != null );
			Assert.IsTrue( outputFile.Exists );
			var changedFileName = string.Format( @"{0}\{1}-sorted.txt", badFileInfo.DirectoryName, badFileInfo.Name.Replace( badFileInfo.Extension, "" ) );
			Assert.IsTrue( outputFile.FullName == changedFileName );
			Assert.IsTrue( message.StartsWith( "Finished: created: " ) );
			Assert.IsTrue( message.EndsWith( changedFileName ) );

			// Assert the contents of outputFile
			IList<string> outputLines = setup.ReadOutputFileInfo( outputFile );
			Assert.IsTrue( outputLines.Count == 4 );
			Assert.IsTrue( outputLines[0] == ", " );
			Assert.IsTrue( outputLines[1] == ", " );
			Assert.IsTrue( outputLines[2] == ", " );
			Assert.IsTrue( outputLines[3] == ", " );
		}

		[Test]
		public void TestFileExists() {
			FileInfo testFileInfo = setup.CreateTestFileInfo();

			log.Debug( string.Format( "TestFileExists - test file: {0}", testFileInfo.FullName ) );

			Assert.IsTrue( testFileInfo.Exists );
		}

		[Test]
		public void CreateSortedFileTest() {
			FileInfo testFileInfo = setup.CreateTestFileInfo();

			FileInfo outputFile;
			var message = parser.CreateSortedFile( testFileInfo.FullName, out outputFile );
			log.Debug( string.Format( "CreateSortedFileTest - sorted test file: {0}", testFileInfo.FullName ) );

			Assert.IsTrue( outputFile != null );
			Assert.IsTrue( outputFile.Exists );
			var changedFileName = string.Format( @"{0}\{1}-sorted.txt", testFileInfo.DirectoryName, testFileInfo.Name.Replace( testFileInfo.Extension, "" ) );
			Assert.IsTrue( outputFile.FullName == changedFileName );
			Assert.IsTrue( message.StartsWith( "Finished: created: " ) );
			Assert.IsTrue( message.EndsWith( changedFileName ) );

			// Assert the contents of outputFile
			IList<string> outputLines = setup.ReadOutputFileInfo( outputFile );
			Assert.IsTrue( outputLines.Count == 4 );
			Assert.IsTrue( outputLines[0] == "BAKER, ANDREW" );
			Assert.IsTrue( outputLines[1] == "KENT, MADISON" );
			Assert.IsTrue( outputLines[2] == "SMITH, ANDREW" );
			Assert.IsTrue( outputLines[3] == "SMITH, FREDRICK" );
		}

		[Test]
		public void CreateSortedFileMockedTryParseSuccessTest() {
			FileInfo testFileInfo = setup.CreateTestFileInfo();

			var names = new List<Name>() { new Name( "ANDREW", "BAKER" ), new Name( "MADISON", "KENT" ), new Name( "ANDREW", "SMITH" ), new Name( "FREDRICK", "SMITH" ) };
			mockedParser.Expect( x => x.TryParseFileNames( Arg<FileInfo>.Is.Anything, out Arg<IList<Name>>.Out( names ).Dummy ) ).Return( true );

			mocks.ReplayAll();

			FileInfo outputFile;
			var message = mockedParser.CreateSortedFile( testFileInfo.FullName, out outputFile );
			log.Debug( string.Format( "CreateSortedFileTest - sorted test file: {0}", testFileInfo.FullName ) );

			mocks.VerifyAll();

			Assert.IsTrue( outputFile != null );
			Assert.IsTrue( outputFile.Exists );
			var changedFileName = string.Format( @"{0}\{1}-sorted.txt", testFileInfo.DirectoryName, testFileInfo.Name.Replace( testFileInfo.Extension, "" ) );
			Assert.IsTrue( outputFile.FullName == changedFileName );
			Assert.IsTrue( message.StartsWith( "Finished: created: " ) );
			Assert.IsTrue( message.EndsWith( changedFileName ) );

			// Assert the contents of outputFile
			IList<string> outputLines = setup.ReadOutputFileInfo( outputFile );
			Assert.IsTrue( outputLines.Count == 4 );
			Assert.IsTrue( outputLines[0] == "BAKER, ANDREW" );
			Assert.IsTrue( outputLines[1] == "KENT, MADISON" );
			Assert.IsTrue( outputLines[2] == "SMITH, ANDREW" );
			Assert.IsTrue( outputLines[3] == "SMITH, FREDRICK" );
		}

		[Test]
		public void CreateSortedFileMockedWriteLinesToFileTest() {
			FileInfo testFileInfo = setup.CreateTestFileInfo();

			FileInfo nonExistingFileInfo = setup.CreateNonExistingTestFileInfo();
			var nonExistingFileName = nonExistingFileInfo.FullName;
			var returnMessage = string.Format( "Finished: created: {0}", nonExistingFileName );
			mockedParser.Expect( x => x.WriteLinesToFile( Arg<FileInfo>.Is.Anything, Arg<IList<string>>.Is.Anything, out Arg<FileInfo>.Out( nonExistingFileInfo ).Dummy ) ).Return( returnMessage );

			mocks.ReplayAll();

			FileInfo outputFile;
			var message = mockedParser.CreateSortedFile( testFileInfo.FullName, out outputFile );
			log.Debug( string.Format( "CreateSortedFileTest - sorted test file: {0}", testFileInfo.FullName ) );

			mocks.VerifyAll();

			Assert.IsTrue( outputFile != null );
			Assert.IsFalse( outputFile.Exists );
			Assert.IsTrue( outputFile.FullName == nonExistingFileName );
			Assert.IsTrue( message.StartsWith( "Finished: created: " ) );
			Assert.IsTrue( message.EndsWith( nonExistingFileName ) );
		}

		[Test]
		public void TestNoCommaFileExists() {
			FileInfo testFileInfo = setup.CreateNoCommaTestFileInfo();

			log.Debug( string.Format( "TestNoCommaFileExists - test file: {0}", testFileInfo.FullName ) );

			Assert.IsTrue( testFileInfo.Exists );
		}

		[Test]
		public void CreateSortedNoCommaFileTest() {
			FileInfo testFileInfo = setup.CreateNoCommaTestFileInfo();

			FileInfo outputFile;
			var message = parser.CreateSortedFile( testFileInfo.FullName, out outputFile );
			log.Debug( string.Format( "CreateSortedNoCommaFileTest - sorted test file: {0}", testFileInfo.FullName ) );

			Assert.IsTrue( outputFile != null );
			Assert.IsTrue( outputFile.Exists );
			var changedFileName = string.Format( @"{0}\{1}-sorted.txt", testFileInfo.DirectoryName, testFileInfo.Name.Replace( testFileInfo.Extension, "" ) );
			Assert.IsTrue( outputFile.FullName == changedFileName );
			Assert.IsTrue( message.StartsWith( "Finished: created: " ) );
			Assert.IsTrue( message.EndsWith( changedFileName ) );

			// Assert the contents of outputFile
			IList<string> outputLines = setup.ReadOutputFileInfo( outputFile );
			Assert.IsTrue( outputLines.Count == 4 );
			Assert.IsTrue( outputLines[0] == "BAKER ANDREW, BAKER ANDREW" );
			Assert.IsTrue( outputLines[1] == "KENT MADISON, KENT MADISON" );
			Assert.IsTrue( outputLines[2] == "SMITH ANDREW, SMITH ANDREW" );
			Assert.IsTrue( outputLines[3] == "SMITH FREDRICK, SMITH FREDRICK" );
		}

		[Test]
		public void TestLastMiddleFirstFileExists() {
			FileInfo testFileInfo = setup.CreateLastMiddleFirstTestFileInfo();

			log.Debug( string.Format( "TestLastMiddleFirstFileExists - test file: {0}", testFileInfo.FullName ) );

			Assert.IsTrue( testFileInfo.Exists );
		}

		[Test]
		public void CreateSortedLastMiddleFirstFileTest() {
			FileInfo testFileInfo = setup.CreateLastMiddleFirstTestFileInfo();

			FileInfo outputFile;
			var message = parser.CreateSortedFile( testFileInfo.FullName, out outputFile );
			log.Debug( string.Format( "CreateSortedLastMiddleFirstFileTest - sorted test file: {0}", testFileInfo.FullName ) );

			Assert.IsTrue( outputFile != null );
			Assert.IsTrue( outputFile.Exists );
			var changedFileName = string.Format( @"{0}\{1}-sorted.txt", testFileInfo.DirectoryName, testFileInfo.Name.Replace( testFileInfo.Extension, "" ) );
			Assert.IsTrue( outputFile.FullName == changedFileName );
			Assert.IsTrue( message.StartsWith( "Finished: created: " ) );
			Assert.IsTrue( message.EndsWith( changedFileName ) );

			// Assert the contents of outputFile
			IList<string> outputLines = setup.ReadOutputFileInfo( outputFile );
			Assert.IsTrue( outputLines.Count == 4 );
			Assert.IsTrue( outputLines[0] == "BAKER, ANDREW" );
			Assert.IsTrue( outputLines[1] == "KENT, MADISON" );
			Assert.IsTrue( outputLines[2] == "SMITH, ANDREW" );
			Assert.IsTrue( outputLines[3] == "SMITH, FREDRICK" );
		}
	}
}
