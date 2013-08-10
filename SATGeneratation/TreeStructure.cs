namespace SATGeneratation
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Z3;

    public class TreeStructure
    {
        #region Constructors and Destructors

        public TreeStructure(Context ctx, string name, int treeSize)
        {
            var asort = ctx.MkArraySort(ctx.IntSort, ctx.IntSort);
            ArgumentCount = (ArrayExpr)ctx.MkConst(name + "a", asort);
            ReverseLink = (ArrayExpr)ctx.MkConst(name + "r", asort);
            PinLink = (ArrayExpr)ctx.MkConst(name + "p", asort);
            this.TreeSize = treeSize;
        }

        #endregion

        #region Public Properties

        public ArrayExpr ArgumentCount { get; set; }

        public ArrayExpr PinLink { get; set; }

        public ArrayExpr ReverseLink { get; set; }

        public int TreeSize { get; set; }

        #endregion

        #region Public Methods and Operators

        public BoolExpr GetTreeLevelConstrains(Context ctx)
        {
            var treeConstrains = new List<BoolExpr>();

            for (int i = 0; i < TreeSize; ++i)
            {
                treeConstrains.Add(GetTreeLevelConstrains(ctx, i));
            }

            treeConstrains.Add(
                ctx.MkOr(
                    ctx.MkAnd(ctx.MkEq(ctx.MkInt(TreeSize), ctx.MkInt(1)), ctx.MkEq(ctx.MkSelect(ArgumentCount, ctx.MkInt(0)), ctx.MkInt(0))), 
                    ctx.MkAnd(ctx.MkNot(ctx.MkEq(ctx.MkInt(TreeSize), ctx.MkInt(1))), ctx.MkNot(ctx.MkEq(ctx.MkSelect(ArgumentCount, ctx.MkInt(0)), ctx.MkInt(0))))));

            return ctx.MkAnd(treeConstrains.ToArray());
        }

        #endregion

        #region Methods

        private BoolExpr GetTreeLevelConstrains(Context ctx, int i)
        {
            var asel = ctx.MkSelect(ArgumentCount, ctx.MkInt(i));

            var topLevel = new List<BoolExpr>();

            topLevel.Add(ctx.MkEq(asel, ctx.MkInt(0)));

            if (i < TreeSize - 1)
            {
                var rsel = ctx.MkSelect(ReverseLink, ctx.MkInt(i + 1));
                var psel = ctx.MkSelect(PinLink, ctx.MkInt(i + 1));
                
                topLevel.Add(ctx.MkAnd(ctx.MkEq(asel, ctx.MkInt(1)), ctx.MkEq(rsel, ctx.MkInt(i)), ctx.MkEq(psel, ctx.MkInt(0))));

                if (i < TreeSize - 2)
                {
                    var secondLevel = new List<BoolExpr>();

                    for (int j = i + 2; j < TreeSize; ++j)
                    {
                        secondLevel.Add(ctx.MkAnd(ctx.MkEq(ctx.MkSelect(ReverseLink, ctx.MkInt(j)), ctx.MkInt(i)), ctx.MkEq(ctx.MkSelect(PinLink, ctx.MkInt(j)), ctx.MkInt(1))));
                    }

                    topLevel.Add(ctx.MkAnd(ctx.MkEq(asel, ctx.MkInt(2)), ctx.MkEq(rsel, ctx.MkInt(i)), ctx.MkEq(psel, ctx.MkInt(0)), secondLevel.Count > 1 ? ctx.MkOr(secondLevel.ToArray()) : secondLevel.First()));

                    if (i < TreeSize - 3)
                    {
                        var thirdLevel = new List<BoolExpr>();

                        for (int j = i + 2; j < TreeSize - 1; ++j)
                        {
                            var fourthLevelPart1 = ctx.MkEq(ctx.MkSelect(ReverseLink, ctx.MkInt(j)), ctx.MkInt(i));
                            var fourthLevelPart2 = ctx.MkEq(ctx.MkSelect(PinLink, ctx.MkInt(j)), ctx.MkInt(1));
                            var fourthLevelPart3 = new List<BoolExpr>();

                            for (int k = j + 1; k < TreeSize; ++k)
                            {
                                fourthLevelPart3.Add(ctx.MkAnd(ctx.MkEq(ctx.MkSelect(ReverseLink, ctx.MkInt(k)), ctx.MkInt(i)), ctx.MkEq(ctx.MkSelect(PinLink, ctx.MkInt(k)), ctx.MkInt(2))));
                            }

                            thirdLevel.Add(ctx.MkAnd(fourthLevelPart1, fourthLevelPart2, fourthLevelPart3.Count > 1 ? ctx.MkOr(fourthLevelPart3.ToArray()) : fourthLevelPart3.First()));
                        }

                        topLevel.Add(ctx.MkAnd(ctx.MkEq(asel, ctx.MkInt(3)), ctx.MkEq(rsel, ctx.MkInt(i)), ctx.MkEq(psel, ctx.MkInt(0)), thirdLevel.Count > 1 ? ctx.MkOr(thirdLevel.ToArray()) : thirdLevel.First()));
                    }
                }
            }

            return ctx.MkOr(topLevel.ToArray());
        }

        #endregion
    }
}