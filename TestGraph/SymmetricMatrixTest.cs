using NUnit.Framework;
using GraphSh;

namespace TestGraph
{
    public class SymmetricMatrixTest
    {
        [Test]
        public void CreateMatrix()
        {
            Assert.DoesNotThrow(() => GetMatrix());
        }
        [TestCase(0, 0)]
        [TestCase(2, 0)]
        public void GetNormalIndex(int x,int y)
        {
            if (x == 0)
                Assert.AreEqual(1, GetMatrix()[x, y]);
            else
                Assert.AreEqual(3, GetMatrix()[x, y]);
        }
        [TestCase(0,2)]
        public void GetSymIndex(int x,int y)
        {
            Assert.AreEqual(3, GetMatrix()[x, y]);
        }
        [Test]
        public void GetDimension()
        {
            Assert.AreEqual(3, GetMatrix().Dimension);
        }
        public SymmetricMatrix GetMatrix()
        {
            return new SymmetricMatrix(new int[,]{ { 1, 2, 3 }, { 2, 5, 6 }, { 3, 6, 9 } });
        }
    }
}
