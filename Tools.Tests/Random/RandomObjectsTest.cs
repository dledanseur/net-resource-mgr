using System;
using Tools.Random;
using Moq;
using Tools.Random.Impl;
using Xunit;

namespace Tools.Tests.Random
{
    public class RandomObjectsTest
    {
        private Mock<IRawRandom> _raw_random_mock;

        private RandomObjects _random_objects;

        public RandomObjectsTest()
        {
            _raw_random_mock = new Mock<IRawRandom>();
            _random_objects = new RandomObjects(_raw_random_mock.Object);
        }

        [Theory,
         InlineData(3,new byte[] {0xED,0xE2,0x14}, "ede214"),
         InlineData(2,new byte[] {0xE8,0xD3}, "e8d3")
        ]
        public void TestRandomHexString(int length, byte[] randomData, string result) {

            // given
            _raw_random_mock.Setup(c => c.GetBytes(It.IsAny<int>())).Returns(randomData);

            // when
            string randomHex = _random_objects.RandomHexString(length);

            // then
            Assert.Equal(result, randomHex);

            _raw_random_mock.Verify(c => c.GetBytes(length));

        } 

    }
}
