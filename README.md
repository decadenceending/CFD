# CFD
CHE477 CFD - SIMPLE Algorithm Project by Orkhan Abdullayev

Computational Fluid Dynamics:
Semi Implicit Method for Pressure Linked Equations

SIMPLE is one of the algorithms used in computational fluid dynamics.
It can help identify appropriate pressure of the system, based on
initial pressure guess and initial velocity field guess.

The algorithm is based on Navier-Stokes and "Stokes Problem."
The general idea is to identify corrected pressure array.

User can enter initial pressure guess, initial velocity guess, momentum
matrix define smallest magnitude eigenvalue matrix (Main Matrix) and
total number of velocity nodes.

The above listed inputs, can be generated using initial and boundary conditions,
from previous computations.

The program can be executed using IDE such as Visual Studio.

To Do:

Math.NET method for solving linear systems of equations,
did not function properly. Algorithm for solving Ax=B, needs to
be defined. One of the algorithms, known to do that is QR.
