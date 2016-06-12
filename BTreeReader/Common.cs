using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace BTreeReader
{
    public static class Common
    {
        public static string SkipTemplate = ConfigurationManager.AppSettings["skipTemplate"];

        public static Tree BuildTree(StreamReader reader)
        {
            try
            {
                Tree resultTree = new Tree();
                List<Tree> tmpTrees = new List<Tree>();
                List<string> keys = new List<string>();
                while (reader.Peek() >= 0)
                {
                    var line = reader.ReadLine();
                    var lineArray = line.Split(',');

                    if (lineArray.Length > 3)
                        throw new ArgumentException("The number of points can not be more than 3.");

                    var parentEl = lineArray[0].Trim();
                    var leftEl = lineArray[1].Trim();
                    var rightEl = lineArray[2].Trim();

                    if (keys.Contains(parentEl))
                        throw new ArgumentException("Duplicate keys.");
                    else keys.Add(parentEl);

                    var newTree = InitTree(parentEl, leftEl, rightEl);
                    if (resultTree.Value == null)
                        resultTree = newTree;
                    else
                        resultTree = MergeTrees(resultTree, newTree, ref tmpTrees);
                }

                foreach (var tree in tmpTrees)
                    resultTree = MergeTrees(resultTree, tree);
                return resultTree;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static Tree InitTree(string parentEl, string leftEl, string rightEl)
        {
            try
            {
                if (parentEl == SkipTemplate || parentEl == leftEl || parentEl == rightEl)
                    throw new ArgumentException("Tree point is not valid.");
                var tree = new Tree();
                if (parentEl != SkipTemplate)
                    tree.Insert(parentEl, DirectionLeaf.none);
                if (leftEl != SkipTemplate)
                    tree.Insert(leftEl, DirectionLeaf.Left);
                if (rightEl != SkipTemplate)
                    tree.Insert(rightEl, DirectionLeaf.Right);
                return tree;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static Tree MergeTrees(Tree firstTree, Tree secondTree, ref List<Tree> tmpTree)
        {
            Tuple<bool, Tree> resultMerge;
            resultMerge = firstTree.Merge(secondTree);
            if (resultMerge.Item1)
                firstTree = resultMerge.Item2;
            else
            {
                resultMerge = secondTree.Merge(firstTree);
                if (resultMerge.Item1)
                    firstTree = resultMerge.Item2;
                else tmpTree.Add(secondTree);
            }
            return firstTree;
        }

        public static Tree MergeTrees(Tree firstTree, Tree secondTree)
        {
            Tuple<bool, Tree> resultMerge;
            resultMerge = firstTree.Merge(secondTree);
            if (resultMerge.Item1)
                firstTree = resultMerge.Item2;
            else
            {
                resultMerge = secondTree.Merge(firstTree);
                if (resultMerge.Item1)
                    firstTree = resultMerge.Item2;
            }
            return firstTree;
        }
    }
}
