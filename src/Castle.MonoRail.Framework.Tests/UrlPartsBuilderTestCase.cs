// Copyright 2004-2010 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Castle.MonoRail.Framework.Tests
{
	using Castle.MonoRail.Framework.Services;
	using NUnit.Framework;
	using Test;

	[TestFixture]
	public class UrlPartsBuilderTestCase
	{
		private StubServerUtility serverUtil;

		[SetUp]
		public void Init()
		{
			serverUtil = new StubServerUtility();
		}

		[Test]
		public void CanBuildPathUrls()
		{
			UrlParts builder = new UrlParts(serverUtil, "controller", "action");
		
			Assert.AreEqual("controller/action", builder.BuildPath());
		}

		[Test]
		public void BuildPathWithPathInfo()
		{
			UrlParts builder = new UrlParts(serverUtil, "controller", "action");
			builder.PathInfo.Add("State").Add("FL");

			Assert.AreEqual("controller/action/State/FL", builder.BuildPath());
		}

		[Test]
		public void BuildPathWithPathInfoDictionary()
		{
			UrlParts builder = new UrlParts(serverUtil, "controller", "action");
			builder.PathInfoDict["State"] ="FL";

			Assert.AreEqual("controller/action/State/FL", builder.BuildPath());
		}

		[Test]
		public void BuildPathWithPathInfoAndQueryString()
		{
			UrlParts builder = new UrlParts(serverUtil, "controller", "action");
			builder.PathInfoDict["State"] = "FL";
			builder.SetQueryString("type=Residential");

			Assert.AreEqual("controller/action/State/FL?type=Residential", builder.BuildPath());
		}

		[Test]
		public void PathInfoDictParse_AcceptsNull()
		{
			UrlParts builder = new UrlParts(serverUtil, "controller", "action");
			builder.PathInfoDict.Parse(null);

			Assert.AreEqual("controller/action", builder.BuildPath());
		}

		[Test]
		public void PathInfoDictParse_AcceptsEmptyString()
		{
			UrlParts builder = new UrlParts(serverUtil, "controller", "action");
			builder.PathInfoDict.Parse("");

			Assert.AreEqual("controller/action", builder.BuildPath());
		}

		[Test]
		public void PathInfoDictParse_CanHandleMissingSlash()
		{
			UrlParts builder = new UrlParts(serverUtil, "controller", "action");
			builder.PathInfoDict.Parse("State/Fl");

			Assert.AreEqual("controller/action/State/Fl", builder.BuildPath());
		}

		[Test]
		public void PathInfoDictParse_CanHandleMultipleEntries()
		{
			UrlParts builder = new UrlParts(serverUtil, "controller", "action");
			builder.PathInfoDict.Parse("/State/FL/Type/Home");

			Assert.AreEqual("controller/action/State/FL/Type/Home", builder.BuildPath());
		}

		[Test]
		public void PathInfoDictParse_CanHandleOddNumberOfEntries()
		{
			UrlParts builder = new UrlParts(serverUtil, "controller", "action");
			builder.PathInfoDict.Parse("/State/FL/Type/");

			Assert.AreEqual("controller/action/State/FL/Type", builder.BuildPath());
		}

		[Test]
		public void PathInfoDictParse_CanHandleOddNumberOfEntries2()
		{
			UrlParts builder = new UrlParts(serverUtil, "controller", "action");
			builder.PathInfoDict.Parse("/State/FL/Type");

			Assert.AreEqual("controller/action/State/FL/Type", builder.BuildPath());
		}

		[Test]
		public void ParseCanHandleAbsolutePaths()
		{
			UrlParts builder = UrlParts.Parse(serverUtil, "http://localhost/home/index.ext");

			Assert.AreEqual(0, builder.PathInfoDict.Count);

			Assert.AreEqual("http://localhost/home/index.ext", builder.BuildPath());
		}

		[Test]
		public void ParseCanHandleAbsolutePathsWithQueryString()
		{
			UrlParts builder = UrlParts.Parse(serverUtil, "http://localhost/home/index.ext?id=1&type=home");

			Assert.AreEqual(0, builder.PathInfoDict.Count);
			Assert.AreEqual("id=1&type=home", builder.QueryStringAsString());

			Assert.AreEqual("http://localhost/home/index.ext?id=1&type=home", builder.BuildPath());
		}

		[Test]
		public void ParseCanHandleAbsolutePathsWithPathInfo()
		{
			UrlParts builder = UrlParts.Parse(serverUtil, "http://localhost/home/index.ext/state/fl/");

			Assert.AreEqual(1, builder.PathInfoDict.Count);
			Assert.AreEqual("fl", builder.PathInfoDict["state"]);
			Assert.AreEqual("", builder.QueryStringAsString());

			Assert.AreEqual("http://localhost/home/index.ext/state/fl", builder.BuildPath());
		}

		[Test]
		public void ParseCanHandleRelativePaths()
		{
			UrlParts builder = UrlParts.Parse(serverUtil, "home/index.ext");

			Assert.AreEqual("home/index.ext", builder.BuildPath());
		}

		[Test]
		public void ParseCanHandleRelativePathsWithEmptyPathInfo()
		{
			UrlParts builder = UrlParts.Parse(serverUtil, "home/index.ext/");

			Assert.AreEqual("home/index.ext", builder.BuildPath());
		}

		[Test]
		public void ParseCanHandleRelativePathsWithPathInfo()
		{
			UrlParts builder = UrlParts.Parse(serverUtil, "home/index.ext/state/fl");

			Assert.AreEqual(1, builder.PathInfoDict.Count);
			Assert.AreEqual("fl", builder.PathInfoDict["state"]);
			Assert.IsNull(builder.QueryStringAsString());

			Assert.AreEqual("home/index.ext/state/fl", builder.BuildPath());
		}

		[Test]
		public void ParseCanHandleRelativePathsWithPathInfo2()
		{
			UrlParts builder = UrlParts.Parse(serverUtil, "home/index.ext/state/fl/");

			Assert.AreEqual(1, builder.PathInfoDict.Count);
			Assert.AreEqual("fl", builder.PathInfoDict["state"]);
			Assert.IsNull(builder.QueryStringAsString());

			Assert.AreEqual("home/index.ext/state/fl", builder.BuildPath());
		}

		[Test]
		public void ParseCanHandleRelativePathsWithQueryString()
		{
			UrlParts builder = UrlParts.Parse(serverUtil, "home/index.ext?id=1&name=john");

			Assert.AreEqual(0, builder.PathInfoDict.Count);
			Assert.AreEqual("id=1&name=john", builder.QueryStringAsString());

			Assert.AreEqual("home/index.ext?id=1&name=john", builder.BuildPath());
		}

		[Test]
		public void ParseCanHandleRelativePathsWithEmptyPathInfoAndQueryString()
		{
			UrlParts builder = UrlParts.Parse(serverUtil, "home/index.ext/?id=1&name=john");

			Assert.AreEqual(0, builder.PathInfoDict.Count);
			Assert.AreEqual("id=1&name=john", builder.QueryStringAsString());

			Assert.AreEqual("home/index.ext?id=1&name=john", builder.BuildPath());
		}

		[Test]
		public void ParseCanHandleRelativePathsWithPathInfoAndQueryString()
		{
			UrlParts builder = UrlParts.Parse(serverUtil, "home/index.ext/state/fl/?id=1&name=john");

			Assert.AreEqual(1, builder.PathInfoDict.Count);
			Assert.AreEqual("fl", builder.PathInfoDict["state"]);
			Assert.AreEqual("id=1&name=john", builder.QueryStringAsString());

			Assert.AreEqual("home/index.ext/state/fl?id=1&name=john", builder.BuildPath());
		}

		[Test]
		public void ParseCanHandleRelativePathsWithoutExtension()
		{
			UrlParts builder = UrlParts.Parse(serverUtil, "home/index/state");

			Assert.AreEqual("home/index/state", builder.BuildPath());
		}

		[Test]
		public void ParseCanHandleRelativePathsWithoutExtensionAndQueryString()
		{
			UrlParts builder = UrlParts.Parse(serverUtil, "home/index/state?id=1");

			Assert.AreEqual(0, builder.PathInfoDict.Count);
			Assert.AreEqual("id=1", builder.QueryStringAsString());
			Assert.AreEqual("1", builder.QueryString["id"]);

			Assert.AreEqual("home/index/state?id=1", builder.BuildPath());
		}

		[Test]
		public void ParseCanHandleRelativePathsWithoutExtensionAndQueryStringContainingDot()
		{
			UrlParts builder = UrlParts.Parse(serverUtil, "home/index/state?file=index.pdf");

			Assert.AreEqual(0, builder.PathInfoDict.Count);
			Assert.AreEqual("file=index.pdf", builder.QueryStringAsString());
			Assert.AreEqual("index.pdf", builder.QueryString["file"]);

			Assert.AreEqual("home/index/state?file=index.pdf", builder.BuildPath());
		}

		[Test]
		public void ParseCanHandleEscapedQueryStringParameters()
		{
			UrlParts builder = UrlParts.Parse(serverUtil, "home/index.ext?date=01%2f01%2f2001");

			Assert.AreEqual("01/01/2001", builder.QueryString["date"]);

			Assert.AreEqual("home/index.ext?date=01%2f01%2f2001", builder.BuildPath());
		}

		[Test]
		public void QueryStringParsesStringCorrectly()
		{
			UrlParts builder = new UrlParts(serverUtil, "home/index.ext");

			builder.QueryString["state"] = "FL";

			Assert.AreEqual("home/index.ext?state=FL", builder.BuildPath());
		}

		[Test]
		public void QueryStringIsExtractedAndParsed()
		{
			UrlParts builder = new UrlParts(serverUtil, "home/index.ext");

			builder.SetQueryString("City=SP&State=MD");

			builder.QueryString["type"] = "home";

			Assert.AreEqual("home/index.ext?City=SP&State=MD&type=home", builder.BuildPath());
		}

		[Test]
		public void QueryStringCanHandleDuplicatedEntries()
		{
			UrlParts builder = new UrlParts(serverUtil, "home/index.ext");

			builder.SetQueryString("City=SP&State=MD&State=NY");

			Assert.AreEqual("home/index.ext?City=SP&State=MD&State=NY", builder.BuildPath());
		}

		[Test]
		public void QueryStringCanReplaceEntries()
		{
			UrlParts builder = new UrlParts(serverUtil, "home/index.ext");

			builder.QueryString["page"] = "1";

			Assert.AreEqual("home/index.ext?page=1", builder.BuildPath());

			builder.QueryString.Set("page", "2");

			Assert.AreEqual("home/index.ext?page=2", builder.BuildPath());

			builder.QueryString.Set("page", "3");

			Assert.AreEqual("home/index.ext?page=3", builder.BuildPath());
		}

		[Test]
		public void InsertFrontPath_ShouldKeepExistingPath()
		{
			UrlParts builder = new UrlParts(serverUtil, "controller", "action");
			builder.PathInfo.Add("State").Add("FL");

			builder.InsertFrontPath("http://something");

			Assert.AreEqual("http://something/controller/action/State/FL", builder.BuildPath());
		}

		[Test]
		public void InsertFrontPath_ShouldHandleTwoPathsWithSlash()
		{
			UrlParts builder = new UrlParts(serverUtil);
			builder.AppendPath("/something");

			builder.InsertFrontPath("http://something/else/");

			Assert.AreEqual("http://something/else/something", builder.BuildPath());
		}
	}
}
