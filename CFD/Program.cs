using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFD
{
    public class CFD_SIMPLE
    {

        ///Method for splitting Main Matrix into A

        public double[,] Vel_Vector_A(double[,] Main_Mat, int nodes)
        {
            ///Initiate a new empty matrix

            int rowA = 2 * nodes;

            int colA = 2 * nodes;

            double[,] A = new double[rowA, colA];

            ///Iterate through Main Matrix based on specified criterias

            for (int Ai=0;Ai<2*nodes;Ai++)
            {
                for(int Aj=0;Aj<2*nodes;Aj++)
                {
                    A[Ai, Aj] = Main_Mat[Ai, Aj];

                }
            }

            return A;
        }

        ///Method for splitting Main Matrix into B

        public double[,] Vel_Vector_B(double[,] Main_Mat, int nodes)
        {
            ///Initiate a new empty matrix

            int rowB = 2 * nodes;

            int colB = Main_Mat.GetLength(1) - (2 * nodes);

            double[,] B = new double[rowB, colB];

            ///Iterate through Main Matrix based on specified criterias, where they depend on the total size of Main Matrix,
            ///second loop contains two statements, to account for reading Main Matrix and writing to new maatrix B

            for (int Bi = 0; Bi < 2 * nodes; Bi++)
            {
                for (int Bj = 2 * nodes, Bj_ind=0; Bj < Main_Mat.GetLength(1); Bj++, Bj_ind++)
                {

                    B[Bi, Bj_ind] = Main_Mat[Bi, Bj];

                }

            }

            return B;
        }

        ///Method for splitting Main Matrix into C

        public double[,] Vel_Vector_C(double[,] Main_Mat, int nodes)
        {
            int rowC = Main_Mat.GetLength(0) - (2 * nodes);

            int colC = 2 * nodes;

            double[,] C = new double[rowC, colC];

            ///Iterate through Main Matrix based on specified criterias, where they depend on the total size of Main Matrix,
            ///and first for loop contains two statements to account for reading Main Matrix and writing to the new matrix C

            for (int Ci = 2 * nodes, Ci_ind=0; Ci < Main_Mat.GetLength(0); Ci++, Ci_ind++)
            {
                for (int Cj = 0; Cj < 2 * nodes; Cj++)
                {
                    C[Ci_ind, Cj] = Main_Mat[Ci, Cj];
                }
            }

            return C;
        }

        ///Method for obtaining Inverse of Diagonal

        public double[,] Diag_Oper(double[,] A)
        {
            int rowD = A.GetLength(0);

            int colD = A.GetLength(1);

            double[,] D_Inv = new double[rowD, colD];

            for (int i = 0; i < A.Rank; i++)
            {
                D_Inv[i, i] = 1 / A[i, i];
            }
            return D_Inv;
        }

        ///Define method for splitting momentum matrix for the problem

        public double[,]Momentum_Red(double[]Momentum,int nodes)
        {

            int dimM_R = 2 * nodes;

            double[,] M_R = new double[1,dimM_R];

            ///Iterate through Momentumn array based on specified criterias

            for (int M_Ri = 0; M_Ri < 2 * nodes; M_Ri++)
            {
                    M_R[0,M_Ri] = Momentum[M_Ri];

            }

            return M_R;

        }
        ///Define method for building matrix for sub matrix of pressure guess 

            public double[,]Pres_Guess_Red(double[]P_V_guess, int nodes)
        {

            int dimP_R = P_V_guess.GetLength(0)-2 * nodes;

            double[,] P_R = new double[1,dimP_R];

            ///Iterate through pressure/velocity array based on specified criterias

            for (int P_Ri = 2*nodes, P_Ri_Ind=0; P_Ri < P_V_guess.Length; P_Ri++, P_Ri_Ind++)
            {
                P_R[0,P_Ri_Ind] = P_V_guess[P_Ri];

            }

            return P_R;
        }

        ///Define Multiplication of Matrices Algorithm Method

        public double[,] Matrix_Multiplication(double[,] Mat_A, double[,] Mat_B)
        {
            if (Object.ReferenceEquals(null, Mat_A))

                throw new ArgumentNullException("Matrix A");

            else if (Object.ReferenceEquals(null, Mat_B))

                throw new ArgumentNullException("Matrix B");

            int rowA = Mat_A.GetLength(0);

            int colA = Mat_A.GetLength(1);

            int rowB = Mat_B.GetLength(0);

            int colB = Mat_B.GetLength(1);

            if (colA != rowB)

                throw new ArgumentOutOfRangeException("B", "Matrix dimensions are not compatible for multiplication");

            double[,] Mat_C = new double[rowA, colB];

            for (int r = 0; r < rowA; ++r)
            {
                for (int c = 0; c < colB; ++c)
                {
                    double sum = 0;

                    for (int i = 0; i < colA; ++i)

                        sum += Mat_A[r, i] * Mat_B[i, c];

                    Mat_C[r, c] = sum;
                }
            }

            return Mat_C;
        }

        ///Define method for SIMPLE algorithm

        public double[,] SIMPLE(double[,] A, double[,] B, double[,] C,double[,] C_DInv_B, double[,]M_R, double[,] P_R, double alpha)
        {
            int row_diff = B.GetLength(0);

            int col_diff = B.GetLength(1);

            double[,] new_vel = new double[row_diff, col_diff];

            int p_inter_length = C_DInv_B.GetLength(1);

            double[,] p_inter = new double[1,p_inter_length];

            double[,] p = new double[1,p_inter_length];

            double[,] diff_M_R_BP_R = new double[row_diff,col_diff];

            for (int count = 1; count <= 1000; count++)
            {

                ///Step 1, find new velocity

                ///Find product of B and P_R

                double[,] BP_R = Matrix_Multiplication(B, P_R);

                ///Find difference of M_R and B*P_R

                for (int g=0;g<row_diff;g++)
                {
                    for(int h=0;h<col_diff; h++)
                    {
                        diff_M_R_BP_R[g, h] = M_R[0, h] - BP_R[g, 0];
                    }
                }

                ///Find new velocity

                ///new_vel = A/(diff_M_R_BP_R); THIS PORTION WAS NOT WORKING IN MATH.NET NEEDS FIXING, SOLUTION TO LINEAR SYSTEM OF EQUATIONS

                ///Step 2, find intermediate pressure

                double[,] C_new_vel = Matrix_Multiplication(C, new_vel);

                ///p_inter = C_DInv_B/C_new_vel; THIS PORTION WAS NOT WORKING IN MATH.NET NEEDS FIXING. SOLUTION TO LINEAR SYSTEM OF EQUATIONS

                ///Step 3, find new pressure

                ///Append intermediate pressure using underrelaxation coefficient

                for (int i = 0; i < p_inter.GetLength(1); i++)
                {
                    p_inter[0,i] = p_inter[0,i] * alpha;
                }

                ///Append pressure array (p) with underrelaxaed pressure array

                for (int j = 0; j < p_inter.Length; j++)
                {
                    p[0,j] = p[0,j] + p_inter[0,j];
                }

                ///Step 4, Reset P_R by setting it equal to P

                P_R = p;
            }

            return p;

        }


        ///Define Main Entry

        static void Main(string[] args)
        {

            ///Define underrelaxation parameter

            double alpha = 0.075;

            ///Define flow matrix

            double[,] Main_Mat ={

                {0.1, 0.3, 0.5, 0.7, 0.9 },

                {0.2, 0.4, 0.6, 0.8, 1.0 },

                {0.5, 0.7, 0.9, 1.1, 1.3 },

                {0.5, 0.7, 0.9, 1.1, 1.3 },

                {0.2, 0.4, 0.6, 0.8, 1.0},

                {0.1, 0.3, 0.5, 0.7, 0.9}};

            ///Define momentum matrix

            double[] Momentum ={0.95, 1.4, 1.85, 2.3, 2.75};

            ///Define initial pressure guess

            double[] P_V_guess = { 0.6, 0.85, 1.1, 1.35, 1.6 };

            ///Prompt user to define number of nodes to be utilized, affects accuracy

            Console.WriteLine("Please enter number of nodes, at least 2 and at least half of the length of the number of rows or columsn of Main Mat, which ever is smaller:");

            int nodes = Convert.ToInt32(Console.ReadLine());

            double[,] A = new CFD_SIMPLE().Vel_Vector_A(Main_Mat, nodes);

            double[,] B = new CFD_SIMPLE().Vel_Vector_B(Main_Mat, nodes);

            double[,] C = new CFD_SIMPLE().Vel_Vector_C(Main_Mat, nodes);

            double[,] D_Inv = new CFD_SIMPLE().Diag_Oper(A);

            double[,] M_R = new CFD_SIMPLE().Momentum_Red(Momentum, nodes);

            double[,] P_R = new CFD_SIMPLE().Pres_Guess_Red(P_V_guess, nodes);

            double[,] DInv_B = new CFD_SIMPLE().Matrix_Multiplication(D_Inv, B);

            double[,]C_DInv_B= new CFD_SIMPLE().Matrix_Multiplication(C, DInv_B);

            double[,] p = new CFD_SIMPLE().SIMPLE(A, B, C, C_DInv_B, M_R, P_R, alpha);

            Console.ReadLine();

        }
    }
}
