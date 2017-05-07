namespace GraphSh
{
    public sealed class SymmetricMatrix
    {
        private readonly int[][] _baseMatrix;

        public int Dimension => _baseMatrix.Length;

        public SymmetricMatrix(int[,] matrix)
        {
            int newSize = matrix.GetLength(0);
            _baseMatrix = new int[newSize][];
            for (int i = 0; i < newSize; i++)
            {
                _baseMatrix[i] = new int[i + 1];
            }
            for (int i = 0; i < newSize; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    _baseMatrix[i][j] = matrix[i, j];
                }
            }
        }

        public SymmetricMatrix(int dimension)
        {
            _baseMatrix = new int[dimension][];
            for (int i = 0; i < dimension; i++)
                _baseMatrix[i] = new int[i + 1];
        }

        public int[,] GetMatrix()
        {
            int[,] returnedMatrix = new int[Dimension, Dimension];
            for (int i = 0; i < Dimension; i++)
            {
                for (int j = 0; j < Dimension; j++)
                {
                    returnedMatrix[i, j] = this[i, j];
                }
            }
            return returnedMatrix;
        }


        public int this[int x, int y]
        {
            get
            {
                return y > x ? _baseMatrix[y][x]: _baseMatrix[x][y];
            }
            set
            {
                if (y < x)
                    _baseMatrix[x][y] = value;
                else
                    _baseMatrix[y][x] = value;
            }
        }
    }
}