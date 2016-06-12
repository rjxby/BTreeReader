using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BTreeReader.Test
{
    [TestFixture]
    public class CommonTest
    {
        string inputData;
        string a, b, c, d, e, m, f, skip;
        Tree firstTreeForMerge, secondTreeForMerge, thirdTreeForMerge;
        List<Tree> tmpTree, tmpTreeResult;
        [SetUp]
        public void BeforeTest()
        {
            a = "A";
            b = "B";
            c = "C";
            d = "D";
            e = "E";
            m = "M";
            f = "F";
            skip = "#";

            Common.SkipTemplate = skip;
            inputData = @"A, B, C
                          B, #, F
                          C, M, #";

            tmpTree = new List<Tree>();
            firstTreeForMerge = new Tree() { Value = a, LeftLeaf = new Tree { Value = b }, RightLeaf = new Tree() { Value = c } };
            secondTreeForMerge = new Tree() { Value = c, LeftLeaf = new Tree() { Value = d }, RightLeaf = new Tree() { Value = e } };
            thirdTreeForMerge = new Tree() { Value = m, LeftLeaf = new Tree() { Value = f } };
            tmpTreeResult = new List<Tree>() { thirdTreeForMerge };
        }

        [Test]
        public void CommonTest_BuildTree()
        {
            byte[] arrayOfMyString = Encoding.UTF8.GetBytes(inputData);
            MemoryStream stream = new MemoryStream(arrayOfMyString);
            StreamReader reader = new StreamReader(stream);
            Tree tree = Common.BuildTree(reader);

            Assert.AreEqual(tree.Value, "A");
            Assert.AreEqual(tree.LeftLeaf.Value, "B");
            Assert.IsNull(tree.LeftLeaf.LeftLeaf);
            Assert.AreEqual(tree.LeftLeaf.RightLeaf.Value, "F");
            Assert.AreEqual(tree.RightLeaf.Value, "C");
            Assert.AreEqual(tree.RightLeaf.LeftLeaf.Value, "M");
            Assert.IsNull(tree.RightLeaf.RightLeaf);
        }

        [Test]
        public void CommonTest_InitTree()
        {
            var treeWithoutSkip = Common.InitTree(a, b, c);
            Assert.AreEqual(treeWithoutSkip.Value, a);
            Assert.AreEqual(treeWithoutSkip.LeftLeaf.Value, b);
            Assert.AreEqual(treeWithoutSkip.RightLeaf.Value, c);

            var treeWithSkip = Common.InitTree(d, skip, e);
            Assert.AreEqual(treeWithSkip.Value, d);
            Assert.IsNull(treeWithSkip.LeftLeaf);
            Assert.AreEqual(treeWithSkip.RightLeaf.Value, e);
        }
        [Test]
        public void CommonTest_MergeTrees()
        {
            Tree resultTreeMerged = Common.MergeTrees(firstTreeForMerge, secondTreeForMerge, ref tmpTree);
            Assert.AreSame(resultTreeMerged.RightLeaf, secondTreeForMerge);

            Tree resultTreeNotMeged = Common.MergeTrees(firstTreeForMerge, thirdTreeForMerge, ref tmpTree);
            Assert.AreEqual(tmpTree, tmpTreeResult);
        }
    }
}
