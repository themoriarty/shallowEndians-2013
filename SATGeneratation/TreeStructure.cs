﻿namespace SATGeneratation
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
            PinLink = (ArrayExpr)ctx.MkConst(name + "p", ctx.MkArraySort(ctx.IntSort, ctx.IntSort));
            FwLink1 = (ArrayExpr)ctx.MkConst(name + "f1", ctx.MkArraySort(ctx.IntSort, ctx.IntSort));
            FwLink2 = (ArrayExpr)ctx.MkConst(name + "f2", ctx.MkArraySort(ctx.IntSort, ctx.IntSort));
            FwLink3 = (ArrayExpr)ctx.MkConst(name + "f3", ctx.MkArraySort(ctx.IntSort, ctx.IntSort));
            this.TreeSize = treeSize;
        }

        #endregion

        #region Public Properties

        public ArrayExpr ArgumentCount { get; set; }

        public ArrayExpr PinLink { get; set; }

        public ArrayExpr ReverseLink { get; set; }

        public ArrayExpr FwLink1 { get; set; }
        public ArrayExpr FwLink2 { get; set; }
        public ArrayExpr FwLink3 { get; set; }

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

            // Make sure each node has at least one other one node pointing to it
            var flinkConsraints = new List<BoolExpr>();
            var argConstraints = new List<BoolExpr>();
            for (int i = 1; i < TreeSize; ++i)
            {
                argConstraints.Add(ctx.MkLt((ArithExpr)ctx.MkSelect(ReverseLink, ctx.MkInt(i)), ctx.MkInt(TreeSize)));
                argConstraints.Add(ctx.MkGe((ArithExpr)ctx.MkSelect(ReverseLink, ctx.MkInt(i)), ctx.MkInt(0)));


                var selInRevList = ctx.MkSelect(ReverseLink, ctx.MkInt(i));
                var selPin = ctx.MkSelect(PinLink, ctx.MkInt(i));
                var selParent1 = ctx.MkSelect(FwLink1, selInRevList);
                var selParent2 = ctx.MkSelect(FwLink2, selInRevList);
                var selParent3 = ctx.MkSelect(FwLink3, selInRevList);
                var selParentArity = ctx.MkSelect(ArgumentCount, selInRevList);
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


                flinkConsraints.Add(ctx.MkOr(
                    ctx.MkAnd(arge1, pin0, ctx.MkEq(selParent1, ctx.MkInt(i))),
                    ctx.MkAnd(arge2, pin1, ctx.MkEq(selParent2, ctx.MkInt(i))),
                    ctx.MkAnd(arge3, pin2, ctx.MkEq(selParent3, ctx.MkInt(i)))
                    ));

                // Ordering of flink
                var curentFlink1 = ctx.MkSelect(FwLink1, ctx.MkInt(i));
                var curentFlink2 = ctx.MkSelect(FwLink2, ctx.MkInt(i));
                var curentFlink3 = ctx.MkSelect(FwLink3, ctx.MkInt(i));
                var ourAr = ctx.MkSelect(ArgumentCount, ctx.MkInt(i));
                var ourAr0 = ctx.MkEq(ourAr, ctx.MkInt(0));
                var ourAr1 = ctx.MkEq(ourAr, ctx.MkInt(1));
                var ourAr2 = ctx.MkEq(ourAr, ctx.MkInt(2));
                var ourAr3 = ctx.MkEq(ourAr, ctx.MkInt(3));
                /*flinkConsraints.Add(ctx.MkOr(
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
                */
            }


            return ctx.MkAnd(ctx.MkAnd(flinkConsraints.ToArray()), ctx.MkAnd(argConstraints.ToArray()), ctx.MkAnd(treeConstrains.ToArray()));
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
                    topLevel.Add(ctx.MkAnd(ctx.MkEq(asel, ctx.MkInt(2)), ctx.MkEq(rsel, ctx.MkInt(i)), ctx.MkEq(psel, ctx.MkInt(0)), secondLevel.Count > 0 ? ctx.MkOr(secondLevel.ToArray()) : ctx.MkFalse()));

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