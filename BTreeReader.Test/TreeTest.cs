using NUnit.Framework;

namespace BTreeReader.Test
{
    [TestFixture]
    public class TreeTest
    {
        Tree tree = null;
        [SetUp]
        public void BeforeTest()
        {
            tree = new Tree()
            {
                Value = "Fox",
                LeftLeaf = new Tree()
                {
                    Value = "The"
                }
            };
        }

        [Test]
        public void TreeTest_Insert()
        {
            tree.Insert("Lazy", DirectionLeaf.Right);
            Assert.AreEqual(tree.RightLeaf.Value, "Lazy");
        }

        [Test]
        public void TreeTest_Merge()
        {
            var newTree = new Tree()
            {
                Value = "The",
                RightLeaf = new Tree()
                {
                    Value = "ToThe"
                }
            };

            tree.Merge(newTree);
            Assert.AreEqual(tree.LeftLeaf.RightLeaf.Value, "ToThe");
        }
    }
}
