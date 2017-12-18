using System;
using System.IO;
using Xunit;

namespace ReStructuredText.Tests
{
    public class RealTest
    {
        [Fact]
        public void TestDockPanelSuite()
        {
            //*
            var path = "/Users/lextm/dockpanelsuite_docs";
            if (Directory.Exists(path))
            {
                foreach (string file in Directory.EnumerateFiles(
                    path, "*.rst", SearchOption.AllDirectories))
                {
                    try
                    {
                        var result = ReStructuredTextParser.ParseDocument(file);
                    }
                    catch (Exception ex)
                    {
                        Assert.Equal("", file);
                    }
                }
                //*/

                /*
                var result = ReStructuredTextParser.ParseDocument("/Users/lextm/dockpanelsuite_docs/contribute/index.rst");
                // */
            }
        }

        [Fact]
        public void TestJexus()
        {
            //*
            var path = "/Users/lextm/jexus_docs";
            if (Directory.Exists(path))
            {
                foreach (string file in Directory.EnumerateFiles(
                    path, "*.rst", SearchOption.AllDirectories))
                {
                    try
                    {
                        var result = ReStructuredTextParser.ParseDocument(file);
                    }
                    catch (Exception ex)
                    {
                        Assert.Equal("", file);
                    }
                }
                //*/

                /*
                var result = ReStructuredTextParser.ParseDocument("/Users/lextm/dockpanelsuite_docs/contribute/index.rst");
                // */
            }
        }
    }
}