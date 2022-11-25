using Jobsity.Chat.Domain.Helpers;

namespace Jobsity.Chat.Test.xUnit.Helpers
{
    public class UnitTestHashPassword
    {
        internal static Random random = new Random();

        internal static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [Fact]
        public void HashAndVerifyPassword()
        {
            string password = RandomString(10);
            var hash = password.HashPassword();
            Assert.True(password.VerifyHashedPassword(hash));
        }
    }
}
