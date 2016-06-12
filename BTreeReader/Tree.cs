using System;

namespace BTreeReader
{
    public enum DirectionLeaf
    {
        none,
        Right,
        Left
    }

    public class Tree
    {
        public string Value { get; set; }
        public Tree LeftLeaf { get; set; }
        public Tree RightLeaf { get; set; }

        public void Insert(string value, DirectionLeaf direction)
        {
            if (this.Value == null || this.Value == value)
            {
                this.Value = value;
                return;
            }
            if (direction == DirectionLeaf.Left)
            {
                if (this.LeftLeaf == null) this.LeftLeaf = new Tree();
                this.Insert(value, this.LeftLeaf, direction);
            }
            else if (direction == DirectionLeaf.Right)
            {
                if (this.RightLeaf == null) this.RightLeaf = new Tree();
                this.Insert(value, this.RightLeaf, direction);
            }
        }

        private void Insert(string value, Tree node, DirectionLeaf direction)
        {
            if (node.Value == null || node.Value == value)
            {
                node.Value = value;
                return;
            }
            if (direction == DirectionLeaf.Left)
            {
                if (node.LeftLeaf == null) node.LeftLeaf = new Tree();
                Insert(value, node.LeftLeaf, direction);
            }
            else if (direction == DirectionLeaf.Right)
            {
                if (node.RightLeaf == null) node.RightLeaf = new Tree();
                Insert(value, node.RightLeaf, direction);
            }
        }

        public Tuple<bool, Tree> Merge(Tree newTree)
        {
            if (this.Value == newTree.Value) new Tuple<bool, Tree>(true, newTree);
            var mergeLeftResult = Merge(this.LeftLeaf, newTree);
            if (mergeLeftResult.Item1)
            {
                this.LeftLeaf = mergeLeftResult.Item2;
                return new Tuple<bool, Tree>(true, this);
            }
            else
            {
                var mergeRightResult = Merge(this.RightLeaf, newTree);
                if (mergeRightResult.Item1)
                {
                    this.RightLeaf = mergeRightResult.Item2;
                    return new Tuple<bool, Tree>(true, this);
                }
                else
                    return new Tuple<bool, Tree>(false, newTree);
            }
        }

        private Tuple<bool, Tree> Merge(Tree oldTree, Tree newTree)
        {
            if (oldTree == null) return new Tuple<bool, Tree>(false, newTree);
            if (oldTree.Value == newTree.Value) return new Tuple<bool, Tree>(true, newTree);
            var mergeLeftResult = Merge(oldTree.LeftLeaf, newTree);
            if (mergeLeftResult.Item1)
            {
                oldTree.LeftLeaf = mergeLeftResult.Item2;
                return new Tuple<bool, Tree>(true, oldTree);
            }
            else
            {
                var mergeRightResult = Merge(oldTree.RightLeaf, newTree);
                if (mergeRightResult.Item1)
                {
                    oldTree.RightLeaf = mergeRightResult.Item2;
                    return new Tuple<bool, Tree>(true, oldTree);
                }
                else
                    return new Tuple<bool, Tree>(false, newTree);
            }
        }

        public void Print()
        {
            Print("", true);
        }

        private void Print(String prefix, bool isTail)
        {
            Console.WriteLine(prefix + (isTail ? "└── " : "├── ") + this.Value);
            if (this.LeftLeaf != null)
                this.LeftLeaf.Print(prefix + (isTail ? "    " : "│   "), false);
            if (this.RightLeaf != null)
                this.RightLeaf.Print(prefix + (isTail ? "    " : "│   "), false);
        }
    }
}
