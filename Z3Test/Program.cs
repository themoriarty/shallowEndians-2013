using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Z3; 

namespace Z3Test
{
    class Program
    {
        /// <summary>
        /// Sudoku solving example.
        /// </summary>        
        static void SudokuExample(Context ctx)
        {
            Console.WriteLine("SudokuExample");

            // 9x9 matrix of integer variables
            IntExpr[][] X = new IntExpr[9][];
            for (uint i = 0; i < 9; i++)
            {
                X[i] = new IntExpr[9];
                for (uint j = 0; j < 9; j++)
                    X[i][j] = (IntExpr)ctx.MkConst(ctx.MkSymbol("x_" + (i + 1) + "_" + (j + 1)), ctx.IntSort);
            }

            // each cell contains a value in {1, ..., 9}
            Expr[][] cells_c = new Expr[9][];
            for (uint i = 0; i < 9; i++)
            {
                cells_c[i] = new BoolExpr[9];
                for (uint j = 0; j < 9; j++)
                    cells_c[i][j] = ctx.MkAnd(ctx.MkLe(ctx.MkInt(1), X[i][j]),
                                                ctx.MkLe(X[i][j], ctx.MkInt(9)));
            }

            // each row contains a digit at most once
            BoolExpr[] rows_c = new BoolExpr[9];
            for (uint i = 0; i < 9; i++)
                rows_c[i] = ctx.MkDistinct(X[i]);

            // each column contains a digit at most once
            BoolExpr[] cols_c = new BoolExpr[9];
            for (uint j = 0; j < 9; j++)
                cols_c[j] = ctx.MkDistinct(X[j]);

            // each 3x3 square contains a digit at most once
            BoolExpr[][] sq_c = new BoolExpr[3][];
            for (uint i0 = 0; i0 < 3; i0++)
            {
                sq_c[i0] = new BoolExpr[3];
                for (uint j0 = 0; j0 < 3; j0++)
                {
                    IntExpr[] square = new IntExpr[9];
                    for (uint i = 0; i < 3; i++)
                        for (uint j = 0; j < 3; j++)
                            square[3 * i + j] = X[3 * i0 + i][3 * j0 + j];
                    sq_c[i0][j0] = ctx.MkDistinct(square);
                }
            }

            BoolExpr sudoku_c = ctx.MkTrue();
            foreach (BoolExpr[] t in cells_c)
                sudoku_c = ctx.MkAnd(ctx.MkAnd(t), sudoku_c);
            sudoku_c = ctx.MkAnd(ctx.MkAnd(rows_c), sudoku_c);
            sudoku_c = ctx.MkAnd(ctx.MkAnd(cols_c), sudoku_c);
            foreach (BoolExpr[] t in sq_c)
                sudoku_c = ctx.MkAnd(ctx.MkAnd(t), sudoku_c);

            // sudoku instance, we use '0' for empty cells
            int[,] instance = {{0,0,0,0,9,4,0,3,0},
                               {0,0,0,5,1,0,0,0,7},
                               {0,8,9,0,0,0,0,4,0},
                               {0,0,0,0,0,0,2,0,8},
                               {0,6,0,2,0,1,0,5,0},
                               {1,0,2,0,0,0,0,0,0},
                               {0,7,0,0,0,0,5,2,0},
                               {9,0,0,0,6,5,0,0,0},
                               {0,4,0,9,7,0,0,0,0}};

            BoolExpr instance_c = ctx.MkTrue();
            for (uint i = 0; i < 9; i++)
                for (uint j = 0; j < 9; j++)
                    instance_c = ctx.MkAnd(instance_c,
                        (BoolExpr)
                        ctx.MkITE(ctx.MkEq(ctx.MkInt(instance[i, j]), ctx.MkInt(0)),
                                    ctx.MkTrue(),
                                    ctx.MkEq(X[i][j], ctx.MkInt(instance[i, j]))));

            Solver s = ctx.MkSolver();
            s.Assert(sudoku_c);
            s.Assert(instance_c);

            if (s.Check() == Status.SATISFIABLE)
            {
                Model m = s.Model;
                Expr[,] R = new Expr[9, 9];
                for (uint i = 0; i < 9; i++)
                    for (uint j = 0; j < 9; j++)
                        R[i, j] = m.Evaluate(X[i][j]);
                Console.WriteLine("Sudoku solution:");
                for (uint i = 0; i < 9; i++)
                {
                    for (uint j = 0; j < 9; j++)
                        Console.Write(" " + R[i, j]);
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Failed to solve sudoku");
                throw new Exception();
            }
        }

        static void Disprove(Context ctx, BoolExpr f, bool useMBQI = false, params BoolExpr[] assumptions)
        {
            Console.WriteLine("Disproving: " + f);
            Solver s = ctx.MkSolver();
            Params p = ctx.MkParams();
            p.Add("mbqi", useMBQI);
            s.Parameters = p;
            foreach (BoolExpr a in assumptions)
                s.Assert(a);
            s.Assert(ctx.MkNot(f));
            Status q = s.Check();

            switch (q)
            {
                case Status.UNKNOWN:
                    Console.WriteLine("Unknown because: " + s.ReasonUnknown);
                    break;
                case Status.SATISFIABLE:
                    Console.WriteLine("OK, model: " + s.Model);
                    break;
                case Status.UNSATISFIABLE:
                    throw new Exception("UNSATISFIABLE");
            }
        }

        static Model Check(Context ctx, BoolExpr f, Status sat)
        {
            Solver s = ctx.MkSolver();
            s.Assert(f);
            if (s.Check() != sat)
                throw new Exception();
            if (sat == Status.SATISFIABLE)
                return s.Model;
            else
                return null;
        } 

        static void Test1(Context ctx)
        {
            Console.WriteLine("BitvectorExample2");

            /* construct x ^ y - 103 == x * y */
            Sort bv_type = ctx.MkBitVecSort(64);
            //BitVecExpr x = ctx.MkBVConst("x", 64);
            BitVecExpr y = ctx.MkBVConst("y", 64);
            BitVecExpr z = ctx.MkBVConst("z", 64);
            BitVecExpr i1 = ctx.MkBVConst("i1", 64);
            BitVecExpr i2 = ctx.MkBVConst("i2", 64);
            BitVecExpr i3 = ctx.MkBVConst("i3", 64);

            BitVecExpr cfalse = (BitVecNum)ctx.MkNumeral((0x0000000000000000).ToString(), bv_type);
            BitVecExpr ctrue = (BitVecNum)ctx.MkNumeral((0xffffffffffffffff).ToString(), bv_type);
            BitVecExpr czero = (BitVecNum)ctx.MkNumeral((0x0000000000000000).ToString(), bv_type);
            BitVecExpr cone = (BitVecNum)ctx.MkNumeral((0x0000000000000001).ToString(), bv_type);


            BitVecExpr cinput0 = (BitVecNum)ctx.MkNumeral((0x0000000000000002).ToString(), bv_type);
            BitVecExpr coutput0 = (BitVecNum)ctx.MkNumeral((0x0000000000000003).ToString(), bv_type);
            BitVecExpr cinput1 = (BitVecNum)ctx.MkNumeral((0x000000000000000E).ToString(), bv_type);
            BitVecExpr coutput1 = (BitVecNum)ctx.MkNumeral((0x000000000000000F).ToString(), bv_type);
            BitVecExpr cinput2 = (BitVecNum)ctx.MkNumeral((0x00000000000000FF).ToString(), bv_type);
            BitVecExpr coutput2 = (BitVecNum)ctx.MkNumeral((0x00000000000000FE).ToString(), bv_type);

            Func<BitVecExpr, BitVecExpr> funcz = (x) => ctx.MkBVOR(ctx.MkBVAND(i3, x), ctx.MkBVAND(ctx.MkBVNot(i3), z));
            Func<BitVecExpr, BitVecExpr> funcy = (x) => ctx.MkBVOR(ctx.MkBVAND(i2, x), ctx.MkBVAND(ctx.MkBVNot(i2), y));
            Func<BitVecExpr, BitVecExpr> func = (x) => ctx.MkBVOR(ctx.MkBVAND(i1, ctx.MkBVXOR(funcz(x), funcy(x))), ctx.MkBVAND(ctx.MkBVNot(i1), ctx.MkBVAND(funcz(x), funcy(x))));

            BoolExpr ctr1 = ctx.MkEq(func(cinput0), coutput0);
            BoolExpr ctr2 = ctx.MkEq(func(cinput1), coutput1);
            BoolExpr ctr3 = ctx.MkEq(func(cinput2), coutput2);

            BoolExpr i1ctr = ctx.MkOr(ctx.MkEq(i1, ctrue), ctx.MkEq(i1, cfalse));
            BoolExpr i2ctr = ctx.MkOr(ctx.MkEq(i2, ctrue), ctx.MkEq(i2, cfalse));
            BoolExpr i3ctr = ctx.MkOr(ctx.MkEq(i3, ctrue), ctx.MkEq(i3, cfalse));
            BoolExpr yctr = ctx.MkOr(ctx.MkEq(y, czero), ctx.MkEq(y, cone));
            BoolExpr zctr = ctx.MkOr(ctx.MkEq(z, czero), ctx.MkEq(z, cone));
            BoolExpr ctr = ctx.MkAnd(ctr1, ctr2, ctr3, yctr, zctr, i1ctr, i2ctr, i3ctr);

            Console.WriteLine("ctr = {0}", ctr);

            Console.WriteLine(">>>>>");

            /* find a model (i.e., values for x an y that satisfy the constraint */
            Model m = Check(ctx, ctr, Status.SATISFIABLE);
            Console.WriteLine(m);
            Console.WriteLine(m.Eval(y)); 
        }


        static void AddTreeConstrains(Context ctx, int i, int n, ArrayExpr a, ArrayExpr r, Goal g)
        {
            var asel = ctx.MkSelect(a, ctx.MkInt(i));

            var topLevel = new List<BoolExpr>();

            topLevel.Add(ctx.MkEq(asel, ctx.MkInt(0)));

            if (i < n - 1)
            {
                var rsel = ctx.MkSelect(r, ctx.MkInt(i + 1));

                topLevel.Add(ctx.MkAnd(ctx.MkEq(asel, ctx.MkInt(1)), ctx.MkEq(rsel, ctx.MkInt(i))));

                if (i < n - 2)
                {

                    var secondLevel = new List<BoolExpr>();

                    for (int j = i + 2; j < n; ++j)
                    {
                        secondLevel.Add(ctx.MkEq(ctx.MkSelect(r, ctx.MkInt(j)), ctx.MkInt(i)));
                    }

                    topLevel.Add(ctx.MkAnd(ctx.MkEq(asel, ctx.MkInt(2)), ctx.MkEq(rsel, ctx.MkInt(i)), secondLevel.Count > 1 ? ctx.MkOr(secondLevel.ToArray()) : secondLevel.First()));

                    if (i < n - 3)
                    {

                        var thirdLevel = new List<BoolExpr>();

                        for (int j = i + 2; j < n - 1; ++j)
                        {
                            var part1 = ctx.MkEq(ctx.MkSelect(r, ctx.MkInt(j)), ctx.MkInt(i));
                            var part2 = new List<BoolExpr>();

                            for (int k = j + 1; k < n; ++k)
                            {
                                part2.Add(ctx.MkEq(ctx.MkSelect(r, ctx.MkInt(k)), ctx.MkInt(i)));
                            }

                            thirdLevel.Add(ctx.MkAnd(part1, part2.Count > 1 ? ctx.MkOr(part2.ToArray()) : part2.First()));
                        }

                        topLevel.Add(ctx.MkAnd(ctx.MkEq(asel, ctx.MkInt(3)), ctx.MkEq(rsel, ctx.MkInt(i)), thirdLevel.Count > 1 ? ctx.MkOr(thirdLevel.ToArray()) : thirdLevel.First()));
                    }
                }
            }

            g.Assert(ctx.MkOr(topLevel.ToArray()));
        }

        static void Test2(Context ctx)
        {
            Console.WriteLine("ArrayExample1");

            Goal g = ctx.MkGoal(true);
            ArraySort asort = ctx.MkArraySort(ctx.IntSort, ctx.IntSort);
            ArrayExpr a = (ArrayExpr)ctx.MkConst("a", asort);
            ArrayExpr r = (ArrayExpr)ctx.MkConst("r", asort);

            int n = 16;
            for (int i = 0; i < n; ++i)
            {
                AddTreeConstrains(ctx, i, n, a, r, g);
            }


            //Expr sel = ctx.MkSelect(aex, ctx.MkInt(0));
            //g.Assert(ctx.MkEq(sel, ctx.MkInt(42)));
            //Symbol xs = ctx.MkSymbol("x");
            //IntExpr xc = (IntExpr)ctx.MkConst(xs, ctx.IntSort);

            //Symbol fname = ctx.MkSymbol("f");
            //Sort[] domain = { ctx.IntSort };
            //FuncDecl fd = ctx.MkFuncDecl(fname, domain, ctx.IntSort);
            //Expr[] fargs = { ctx.MkConst(xs, ctx.IntSort) };
            //IntExpr fapp = (IntExpr)ctx.MkApp(fd, fargs);

            //g.Assert(ctx.MkEq(ctx.MkAdd(xc, fapp), ctx.MkInt(123)));

            Solver s = ctx.MkSolver();
            foreach (BoolExpr asr in g.Formulas)
                s.Assert(asr);
            Console.WriteLine("Solver: " + s);

            Status q = s.Check();
            Console.WriteLine("Status: " + q);

            if (q != Status.SATISFIABLE)
                throw new Exception();

            Console.WriteLine("Model = " + s.Model);

            Console.WriteLine("ArgCount:\n" + s.Model.FuncInterp(a.FuncDecl));
            Console.WriteLine("Ref:\n" + s.Model.FuncInterp(r.FuncDecl));

            //var af = s.Model.FuncInterp(a.FuncDecl);
            //var rf = s.Model.FuncInterp(r.FuncDecl);

            //for (int i = 0; i < n; ++i)
            //{
            //    Console.WriteLine("-- {0}", af.Entries[i]);
            //}
            //Console.WriteLine("Interpretation of x:\n" + s.Model.ConstInterp(xc));
            //Console.WriteLine("Interpretation of f:\n" + s.Model.FuncInterp(fd));
            //Console.WriteLine("Interpretation of MyArray as Term:\n" + s.Model.FuncInterp(a.FuncDecl));  
        } 

        static void Main(string[] args)
        {
                            // These examples need model generation turned on.
            using (Context ctx = new Context(new Dictionary<string, string>() { { "model", "true" } }))
            {
                Test2(ctx);
            }
        }
    }
}
