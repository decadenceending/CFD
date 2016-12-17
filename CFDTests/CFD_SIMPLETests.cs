using Microsoft.VisualStudio.TestTools.UnitTesting;
using CFD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFD.Tests
{
    [TestClass()]
    public class CFD_SIMPLETests
    {
        [TestMethod()]
        public void Vel_Vector_ATest()
        {
            int nodes = 1; int A_Size_Exp=2; bool Expected = true; bool Actual;

            double[,] Main_Mat ={

                {0.1, 0.3, 0.5, 0.7, 0.9 },

                {0.2, 0.4, 0.6, 0.8, 1.0 },

                {0.5, 0.7, 0.9, 1.1, 1.3 },

                {0.5, 0.7, 0.9, 1.1, 1.3 },

                {0.2, 0.4, 0.6, 0.8, 1.0},

                {0.1, 0.3, 0.5, 0.7, 0.9}};

            double[,]A = new CFD_SIMPLE().Vel_Vector_A(Main_Mat,nodes);

            if (A.GetLength(1) == A_Size_Exp)
            {
                Actual = true;
            }
            else
            {
                Actual = false;
            }
            Assert.AreEqual(Actual, Expected);
        }

        [TestMethod()]
        public void Vel_Vector_BTest()
        {
            int nodes = 1; int B_Size_Exp = 3; bool Expected = true; bool Actual;

            double[,] Main_Mat ={

                {0.1, 0.3, 0.5, 0.7, 0.9 },

                {0.2, 0.4, 0.6, 0.8, 1.0 },

                {0.5, 0.7, 0.9, 1.1, 1.3 },

                {0.5, 0.7, 0.9, 1.1, 1.3 },

                {0.2, 0.4, 0.6, 0.8, 1.0},

                {0.1, 0.3, 0.5, 0.7, 0.9}};

            double[,]B = new CFD_SIMPLE().Vel_Vector_B(Main_Mat, nodes);

            if (B.GetLength(1) == B_Size_Exp)
            {
                Actual = true;
            }
            else
            {
                Actual = false;
            }
            Assert.AreEqual(Actual, Expected);
        }

        [TestMethod()]
        public void Vel_Vector_CTest()
        {
            int nodes = 1; int C_Size_Exp = 2; bool Expected = true; bool Actual;

            double[,] Main_Mat ={

                {0.1, 0.3, 0.5, 0.7, 0.9 },

                {0.2, 0.4, 0.6, 0.8, 1.0 },

                {0.5, 0.7, 0.9, 1.1, 1.3 },

                {0.5, 0.7, 0.9, 1.1, 1.3 },

                {0.2, 0.4, 0.6, 0.8, 1.0},

                {0.1, 0.3, 0.5, 0.7, 0.9}};

            double[,]C = new CFD_SIMPLE().Vel_Vector_C(Main_Mat, nodes);

            if (C.GetLength(1) == C_Size_Exp)
            {
                Actual = true;
            }
            else
            {
                Actual = false;
            }
            Assert.AreEqual(Actual, Expected);
        }

        [TestMethod()]
        public void Diag_OperTest()
        {
            double element1 = 10; double element2=2.5; bool Expected = true; bool Actual=false;

            double[,] A ={

                {0.1, 0.3},

                {0.2, 0.4 }};

            double[,]D_Inv = new CFD_SIMPLE().Diag_Oper(A);

            if (element1==D_Inv[0,0] && element2==D_Inv[1,1])
            {
                Actual = true;
            }
            Assert.AreEqual(Actual, Expected);
        }

        [TestMethod()]
        public void Momentum_RedTest()
        {
            int nodes = 1; bool Expected = true; bool Actual=false;

            double element1 = 0.95; double element2 = 1.4;

            double[] Momentum = {0.95, 1.4, 1.85, 2.3, 2.75};

            double[,] M_R = new CFD_SIMPLE().Momentum_Red(Momentum, nodes);

            if (element1 == M_R[0,0] && element2 == M_R[0, 1])
            {
                Actual = true;
            }
            Assert.AreEqual(Actual, Expected);
        }

        [TestMethod()]
        public void Pres_Guess_RedTest()
        {
            int nodes = 1; bool Expected = true;

            double element1 = 1.1; double element2 = 1.35;

            bool Actual=false;

            double[] P_V_guess = {0.6, 0.85, 1.1, 1.35, 1.6 };

            double[,] P_R = new CFD_SIMPLE().Pres_Guess_Red(P_V_guess, nodes);

            if (element1 == P_R[0, 0] && element2 == P_R[0, 1])
            {
                Actual = true;
            }
            Assert.AreEqual(Actual, Expected);
        }

        [TestMethod()]
        public void Matrix_MultiplicationTest()
        {
            double[,] MatA = {
                {2,4},
                {6,10}};

            double[,] MatB = {
                {3,5 },
                {8,11}};

            bool Expected = true; bool Actual;

            double[,] Product = new CFD_SIMPLE().Matrix_Multiplication(MatA, MatB);

            int rowA = MatA.GetLength(0);
            int colB = MatB.GetLength(1);

            if (rowA==Product.GetLength(0) && colB==Product.GetLength(1))
            {
                Actual = true;
            }
            else
            {
                Actual = false;
            }

            Assert.AreEqual(Actual, Expected);
        }

    }
}