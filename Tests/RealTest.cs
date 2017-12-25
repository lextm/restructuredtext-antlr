using System;
using System.IO;
using Xunit;

namespace Lextm.ReStructuredText.Tests
{
    public class RealTest
    {
        [Fact]
        public void TestDockPanelSuite()
        {
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
                        Assert.Equal(path, file);
                    }
                }
            }
        }

        [Fact]
        public void TestJexus()
        {
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
                        Assert.Equal(path, file);
                    }
                }
            }
        }
        
        [Fact]
        public void TestObfuscar()
        {
            var path = "/Users/lextm/obfuscar_docs";
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
                        Assert.Equal(path, file);
                    }
                }
            }
        }
    }
}