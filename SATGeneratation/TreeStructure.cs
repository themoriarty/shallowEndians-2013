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
            ArgumentCount = (ArrayExpr)ctx.MkConst(name + "a", ctx.MkArraySort(ctx.IntSort, ctx.IntSort));
            ReverseLink = (ArrayExpr)ctx.MkConst(name + "r", ctx.MkArraySort(ctx.IntSort, ctx.IntSort));
            PinLink = new IntExpr[treeSize];

            for (int i = 0; i < treeSize; i++)
            {
                PinLink[i] = ctx.MkIntConst(name + "p" + i.ToString());
            }


            FwLink1 = (ArrayExpr)ctx.MkConst(name + "f1", ctx.MkArraySort(ctx.IntSort, ctx.IntSort));
            FwLink2 = (ArrayExpr)ctx.MkConst(name + "f2", ctx.MkArraySort(ctx.IntSort, ctx.IntSort));
            FwLink3 = (ArrayExpr)ctx.MkConst(name + "f3", ctx.MkArraySort(ctx.IntSort, ctx.IntSort));
            SubtreeSize = (ArrayExpr)ctx.MkConst(name = "m", ctx.MkArraySort(ctx.IntSort, ctx.IntSort));
            this.TreeSize = treeSize;
        }

        #endregion

        #region Public Properties

        public ArrayExpr ArgumentCount { get; set; }

        public IntExpr[] PinLink { get; set; }

        public ArrayExpr ReverseLink { get; set; }

        public ArrayExpr FwLink1 { get; set; }
        public ArrayExpr FwLink2 { get; set; }
        public ArrayExpr FwLink3 { get; set; }
        public ArrayExpr SubtreeSize { get; set; }

        public static bool UseFwLinks = true;

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

            //for (int i = 0; i < TreeSize; ++i)
            //{
            //    treeConstrains.Add(ctx.MkGe((ArithExpr)ctx.MkSelect(ReverseLink, ctx.MkInt(i)), ctx.MkInt(0)));
            //    treeConstrains.Add(ctx.MkGe((ArithExpr)ctx.MkSelect(FwLink1, ctx.MkInt(i)), ctx.MkInt(0)));
            //    treeConstrains.Add(ctx.MkGe((ArithExpr)ctx.MkSelect(FwLink2, ctx.MkInt(i)), ctx.MkInt(0)));
            //    treeConstrains.Add(ctx.MkGe((ArithExpr)ctx.MkSelect(FwLink3, ctx.MkInt(i)), ctx.MkInt(0)));
            //    treeConstrains.Add(ctx.MkLt((ArithExpr)ctx.MkSelect(ReverseLink, ctx.MkInt(i)), ctx.MkInt(TreeSize)));
            //    treeConstrains.Add(ctx.MkLt((ArithExpr)ctx.MkSelect(FwLink1, ctx.MkInt(i)), ctx.MkInt(TreeSize)));
            //    treeConstrains.Add(ctx.MkLt((ArithExpr)ctx.MkSelect(FwLink2, ctx.MkInt(i)), ctx.MkInt(TreeSize)));
            //    treeConstrains.Add(ctx.MkLt((ArithExpr)ctx.MkSelect(FwLink3, ctx.MkInt(i)), ctx.MkInt(TreeSize)));
            //}

            // Make sure each node has at least one other one node pointing to it
            var flinkConsraints = new List<BoolExpr>();
            var argConstraints = new List<BoolExpr>();
            for (int i = 1; i < TreeSize; ++i)
            {
                argConstraints.Add(ctx.MkLt((ArithExpr)ctx.MkSelect(ReverseLink, ctx.MkInt(i)), ctx.MkInt(TreeSize)));
                argConstraints.Add(ctx.MkGe((ArithExpr)ctx.MkSelect(ReverseLink, ctx.MkInt(i)), ctx.MkInt(0)));

                var parIndex = ctx.MkSelect(ReverseLink, ctx.MkInt(i));
                var selPin = PinLink[i];
                var selParent1 = ctx.MkSelect(FwLink1, parIndex);
                var selParent2 = ctx.MkSelect(FwLink2, parIndex);
                var selParent3 = ctx.MkSelect(FwLink3, parIndex);
                var selParentArity = ctx.MkSelect(ArgumentCount, parIndex);
                // arity >= 1 And Pin = 0 And FwLink 1 = i
                // arity >= 2 And Pin = 1 And FwLink 2 = i
                // arity >= 3 And Pin = 2 And FwLink 3 = i
                var ar1 = ctx.MkEq(selParentArity, ctx.MkInt(1));
                var ar2 = ctx.MkEq(selParentArity, ctx.MkInt(2));
                var ar3 = ctx.MkEq(selParentArity, ctx.MkInt(3));
                var arge1 = ctx.MkOr(ar1, ar2, ar3);
                var arge2 = ctx.MkOr(ar2, ar3);
                var arge3 = ctx.MkOr(ar3);

                var pin0 = ctx.MkEq(selPin, ctx.MkInt(0));
                var pin1 = ctx.MkEq(selPin, ctx.MkInt(1));
                var pin2 = ctx.MkEq(selPin, ctx.MkInt(2));


                var ourAr = ctx.MkSelect(ArgumentCount, ctx.MkInt(i));
                var ourAr0 = ctx.MkEq(ourAr, ctx.MkInt(0));
                var ourAr1 = ctx.MkEq(ourAr, ctx.MkInt(1));
                var ourAr2 = ctx.MkEq(ourAr, ctx.MkInt(2));
                var ourAr3 = ctx.MkEq(ourAr, ctx.MkInt(3));

                flinkConsraints.Add(ctx.MkOr(
                    ctx.MkAnd(arge1, pin0, ctx.MkEq(selParent1, ctx.MkInt(i))),
                    ctx.MkAnd(arge2, pin1, ctx.MkEq(selParent2, ctx.MkInt(i))),
                    ctx.MkAnd(arge3, pin2, ctx.MkEq(selParent3, ctx.MkInt(i)))
                    ));

                if (!UseFwLinks)
                {
                    continue;
                }

                if (i == 0)
                {
                    flinkConsraints.Add(ctx.MkEq(ctx.MkSelect(SubtreeSize, ctx.MkInt(0)), ctx.MkInt(TreeSize)));
                }

                // Ordering of flink
                var curentFlink1 = ctx.MkSelect(FwLink1, ctx.MkInt(i));
                var curentFlink2 = ctx.MkSelect(FwLink2, ctx.MkInt(i));
                var curentFlink3 = ctx.MkSelect(FwLink3, ctx.MkInt(i));


                var stree = ctx.MkSelect(SubtreeSize, ctx.MkInt(i));
                var stree1size = ctx.MkSelect(SubtreeSize, curentFlink1);
                var stree2size = ctx.MkSelect(SubtreeSize, curentFlink2);
                var stree3size = ctx.MkSelect(SubtreeSize, curentFlink3);
                var stree0 = ctx.MkAnd(ourAr0, ctx.MkEq(stree, ctx.MkInt(1)));
                var stree1 = ctx.MkAnd(ourAr1, ctx.MkEq(stree, ctx.MkAdd((ArithExpr)stree1size, ctx.MkInt(1))));
                var stree2 = ctx.MkAnd(ourAr2, ctx.MkEq(stree, ctx.MkAdd((ArithExpr)stree1size, (ArithExpr)stree2size, ctx.MkInt(1))));
                var stree3 = ctx.MkAnd(ourAr3, ctx.MkEq(stree, ctx.MkAdd((ArithExpr)stree1size, (ArithExpr)stree2size, (ArithExpr)stree3size, ctx.MkInt(1))));

                // second fw link - us - 1== stree1size 
                var link2LengthConst = ctx.MkEq(ctx.MkAdd((ArithExpr)curentFlink2, ctx.MkInt(-i - 1)), stree1size);
                // third fw link - us - 1 = stree1size + stree2size
                var link3LengthCost = ctx.MkEq(ctx.MkAdd((ArithExpr)curentFlink3, ctx.MkInt(-i - 1)), ctx.MkAdd((ArithExpr) stree1size, (ArithExpr) stree2size));

                flinkConsraints.Add(ctx.MkOr(stree0, stree1, stree2, stree3));

                flinkConsraints.Add(ctx.MkOr(ourAr0, ourAr1, ctx.MkAnd(ourAr2, link2LengthConst), ctx.MkAnd(ourAr3, link3LengthCost)));

                flinkConsraints.Add(ctx.MkOr(
                                    ourAr0,
                                    ctx.MkAnd(pin0, ctx.MkGt((ArithExpr)selParent2, (ArithExpr)curentFlink1)),
                                    ctx.MkAnd(pin1, ctx.MkGt((ArithExpr)selParent3, (ArithExpr)curentFlink1)),
                                    pin2));
                flinkConsraints.Add(ctx.MkOr(
                                    ourAr0,
                                    ourAr1,
                                    ctx.MkAnd(pin0, ctx.MkGt((ArithExpr)selParent2, (ArithExpr)curentFlink2)),
                                    ctx.MkAnd(pin1, ctx.MkGt((ArithExpr)selParent3, (ArithExpr)curentFlink2)),
                                    pin2));
                flinkConsraints.Add(ctx.MkOr(
                                    ourAr0,
                                    ourAr1,
                                    ourAr2,
                                    ctx.MkAnd(pin0, ctx.MkGt((ArithExpr)selParent2, (ArithExpr)curentFlink3)),
                                    ctx.MkAnd(pin1, ctx.MkGt((ArithExpr)selParent3, (ArithExpr)curentFlink3)),
                                    pin2));
                
                // All flinks between a child and it's parent point between the child and the parent
            }


            return ctx.MkAnd(ctx.MkAnd(flinkConsraints.ToArray()), ctx.MkAnd(argConstraints.ToArray()), ctx.MkAnd(treeConstrains.ToArray()));
        }

        #endregion

        #region Methods

        private BoolExpr GetTreeLevelConstrains(Context ctx, int i)
        {
            // look at \sattreeconstr.txt for details
            var asel = ctx.MkSelect(ArgumentCount, ctx.MkInt(i));

            var topLevel = new List<BoolExpr>();

            // zero-argument node
            topLevel.Add(ctx.MkEq(asel, ctx.MkInt(0)));



            if (i < TreeSize - 1)
            {
                // one-argument node
                // reverse link of the next argument
                var rsel = ctx.MkSelect(ReverseLink, ctx.MkInt(i + 1));
                // pin of the next argument
                var psel = PinLink[i + 1];
                
                // argsLevel == 1 AND next link is our childer AND next link is our 0th pin
                topLevel.Add(ctx.MkAnd(ctx.MkEq(asel, ctx.MkInt(1)), ctx.MkEq(rsel, ctx.MkInt(i)), ctx.MkEq(psel, ctx.MkInt(0))));

                if (i < TreeSize - 2)
                {
                    // two-arguments node
                    var secondLevel = new List<BoolExpr>();

                    for (int j = i + 2; j < TreeSize; ++j)
                    {
                        var reverseJth = ctx.MkSelect(ReverseLink, ctx.MkInt(j));
                        var pinJth = PinLink[j];
                        // jth node is our childer AND it's our 1st pin [ 1 ]
                        secondLevel.Add(ctx.MkAnd(ctx.MkEq(reverseJth, ctx.MkInt(i)), ctx.MkEq(pinJth, ctx.MkInt(1))));
                    }
                    // this node has 2 arguments AND (i + 1) is our child and (i + 1) is our 0th pin and and of the subsequent node satisfies [1]
                    topLevel.Add(ctx.MkAnd(ctx.MkEq(asel, ctx.MkInt(2)), ctx.MkEq(rsel, ctx.MkInt(i)), ctx.MkEq(psel, ctx.MkInt(0)), secondLevel.Count > 0 ? ctx.MkOr(secondLevel.ToArray()) : ctx.MkFalse()));

                    if (i < TreeSize - 3)
                    {
                        // three-arguments node
                        // (a[0] == 3 && r[1] == 0 && ((r[2] == 0) && (r[3] == 0 || r[3] == 0 || ... || r[n-1] == 0) || ... || (r[n-2] == 0) && r[n-1] == 0))
                        var thirdLevel = new List<BoolExpr>();

                        for (int j = i + 2; j < TreeSize - 1; ++j)
                        {
                            var fourthLevelPart1 = ctx.MkEq(ctx.MkSelect(ReverseLink, ctx.MkInt(j)), ctx.MkInt(i));
                            var fourthLevelPart2 = ctx.MkEq(PinLink[j], ctx.MkInt(1));
                            var fourthLevelPart3 = new List<BoolExpr>();

                            for (int k = j + 1; k < TreeSize; ++k)
                            {
                                fourthLevelPart3.Add(ctx.MkAnd(ctx.MkEq(ctx.MkSelect(ReverseLink, ctx.MkInt(k)), ctx.MkInt(i)), ctx.MkEq(PinLink[k], ctx.MkInt(2))));
                            }
                            thirdLevel.Add(ctx.MkAnd(fourthLevelPart1, fourthLevelPart2, fourthLevelPart3.Count > 1 ? ctx.MkOr(fourthLevelPart3.ToArray()) : fourthLevelPart3.First()));
                        }

                        topLevel.Add(ctx.MkAnd(
                            ctx.MkEq(asel, ctx.MkInt(3)), 
                            ctx.MkEq(rsel, ctx.MkInt(i)),
                            ctx.MkEq(psel, ctx.MkInt(0)), 
                            thirdLevel.Count > 0 ? ctx.MkOr(thirdLevel.ToArray()) : ctx.MkFalse()));
                    }
                }
            }

            return ctx.MkAnd(ctx.MkOr(topLevel.ToArray()));
        }

        #endregion
    }
}